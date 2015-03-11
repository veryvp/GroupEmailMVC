using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Web.Components
{
    //发送邮件
    public class SendGroupEmail : BasePage
    {
        private DBUtility.SqlManage SqlHelper = new DBUtility.SqlManage();
        private System.Collections.Hashtable ht = new System.Collections.Hashtable();
        private Comm100Manage Comm100Manage1 = new Comm100Manage();
        private VmoneyInterface VmoneyInterface1 = new VmoneyInterface();


        /// <summary>
        /// 拆分任务
        /// </summary>
        /// <param name="EmailId">邮件Id</param>
        /// <param name="SentTime">发送时间</param>
        /// <param name="UserId">用户Id</param>
        /// <returns></returns>
        public string SplitTask(int EmailId,int DataTaskId, DateTime SentTime, int UserId = 0)
        {
            
            if (UserId == 0) {
                UserId = base.UserId;
            }

            string SAccount = SqlHelper.One("select top 1 SAccount from Email Where UserId=" + UserId + " and Id=" + EmailId);
            if (SAccount == null)
            {
                return "{\"Type\":-1,\"Message\":\"收件人不存在\"}";
            }
            ht.Clear();

            //获取收件人总数
            int TotalSentAmount = GetRecipientTotal(EmailId, UserId);
            if (TotalSentAmount == 0)
            {
                return "{\"Type\":-1,\"Message\":\"收件人不存在\"}";
            }


            string sql1 = "insert into [EmailScore](Email) select [Email] from CustDataReport where  UserId=" + UserId + " and  CustDataTaskId=" + DataTaskId + " and not exists(select Id from [EmailScore] where [CustDataReport].[Email]=[EmailScore].[Email])";
            SqlHelper.Ins(sql1);


            string sql2 = "update CustDataReport set Platform=0 where id in (select Id from CustDataReport D left join  [EmailScore] S on D.Email=S.Email left join Unsubscribed U on D.Email=U.Email and U.SAccount='" + SAccount + "' where S.UserId=" + UserId + " and S.EmailType=0  and U.Id is null and D.CustDataTaskId=" + DataTaskId + " )";
            SqlHelper.Upd(sql2);

            string sql3 = "update CustDataReport set Platform=1 where id in (select Id from CustDataReport D left join  [EmailScore] S on D.Email=S.Email left join Unsubscribed U on D.Email=U.Email and U.SAccount='" + SAccount + "' where S.UserId=" + UserId + " and S.EmailType=1  and U.Id is null and D.CustDataTaskId=" + DataTaskId + " )";
            SqlHelper.Upd(sql3);



            //测试阶段直接发送邮件
            GroupEmailThread.SendEmail(EmailId);
            return "{\"Type\":0,\"Message\":\"发送成功\"}"; 
        }

        /// <summary>
        /// 获取收件人总数
        /// </summary>
        /// <param name="EmailRecipientListId">收件人列表Id</param>
        /// <param name="UserId">用户Id</param>
        /// <param name="IsSentUnsubscribe">发送退订 0:不发送 1:发送</param>
        /// <param name="IsEliminate">是否添加排除条件</param>
        /// <returns></returns>
        public int GetRecipientTotal(int EmailId,int UserId)
        {
            DataTable Dt = SqlHelper.Sel("select FromEmailAddress,DataTaskId from Email where UserId=" + UserId + " and Id = " + EmailId);
            if (Dt.Rows.Count == 1)
            {
                string SAccount = Dt.Rows[0]["FromEmailAddress"].ToString();
                int DataTaskId = Convert.ToInt32(Dt.Rows[0]["DataTaskId"]);
                string Total = SqlHelper.One("select count(*) from [GroupEmail].[dbo].[CustDataReport] D left join Unsubscribed U on D.Email = U.Email and U.SAccount='" + SAccount + "' where D.UserId = " + UserId + " and D.DataTaskId=" + DataTaskId + " U.Id is null ");
                return Convert.ToInt32(Total);
            }
            return 0;
        }

        /// <summary>
        /// 获取收件人总数,可用V币数和消耗V币数
        /// </summary>
        /// <param name="EmailRecipientListId">收件人列表Id</param>
        /// <param name="IsSentUnsubscribe">发送退订 0:不发送 1:发送</param>
        /// <returns></returns>
        public string GetSendVDetail(int EmailId)
        {
            int RecipientsCount = GetRecipientTotal(EmailId,base.UserId);
            double AvailableV = GetV();

            int ConsumeV = RecipientsCount;
            return "{\"RecipientsCount\":" + RecipientsCount + ",\"AvailableV\":" + AvailableV + ",\"ConsumeV\":" + ConsumeV + "}";
        }

        /// <summary>
        /// 获取收件人总数,可用V币数和消耗V币数
        /// </summary>
        /// <param name="EmailRecipientListId">收件人列表Id</param>
        /// <param name="IsSentUnsubscribe">发送退订 0:不发送 1:发送</param>
        /// <returns></returns>
        public double GetV()
        {
            return VmoneyInterface1.GetVmoney();
        }




        /// <summary>
        /// 插入发送队列
        /// </summary>
        /// <param name="SendGroupEmailId">邮件Id</param>
        /// <param name="SendRecipientId">收件人Id</param>
        /// <param name="QName">发送任务名称</param>
        /// <param name="QDesc">发送任务描述</param>
        /// <param name="SentTime">发送时间</param>
        /// <param name="IsEliminate">是否排除例外</param>
        /// <returns></returns>
        public string InsertIntoQueue(int EmailId, int DataTaskId, string SentTime)
        {

            if (EmailId == 0 || DataTaskId == 0)
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }
            int UserId = base.UserId;
            //string EId = SqlHelper.One("select top 1 Id from Email where UserId=" + UserId + " and DataTaskId=" + DataTaskId);
            //if (EId != null)
            //{
            //    return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            //}

            string CDId = SqlHelper.One("select top 1 Id from CustDataTask where UserId=" + UserId + " and Id=" + DataTaskId);
            if (CDId == null)
            {
                return "{\"Type\":-1,\"Message\":\"收件人不存在\"}";
            }
            int num = SqlHelper.Upd("update email set DataTaskId=" + DataTaskId + " where  UserId=" + UserId + " and Id=" + EmailId);
            if (num != 1) {
                return "{\"Type\":-1,\"Message\":\"邮件不存在\"}";
            }
            int SentAmount = GetRecipientTotal(EmailId, UserId);
            if (SentAmount == 0)
            {
                return "{\"Type\":-1,\"Message\":\"请添加收件人\"}";
            }
            if (SentAmount > 100000)
            {
                return "{\"Type\":-1,\"Message\":\"每次群发数量不能大于100000个\"}";
            }

            string SAccount = SqlHelper.One("select FromEmailAddress from  Email where Id=" + EmailId);

            int VerifiedResult = EmailVerified(SAccount);
            if (VerifiedResult == -1)
            {
                return "{\"Type\":-1,\"Message\":\"请确认群发邮箱地址是否存在\"}";
            }
            if (VerifiedResult == 0)
            {
                return "{\"Type\":-1,\"Message\":\"系统繁忙请稍后重试\"}";
            }

            //群发扣除V币
            string DeductVResult = VmoneyInterface1.DeductVmoney(base.UserId, 0, SentAmount, "群发邮件");
            string DeductVType = JsonHashtable1.GetNodeByKey(DeductVResult, "Type");
            if (DeductVType == "0") {
                SqlHelper.Upd("update email set VDetailId=" + DBUtility.Safe.SafeReplace(JsonHashtable1.GetNodeByKey(DeductVResult, "DetailId")) + " where  UserId=" + UserId + " and Id=" + EmailId);
            }
            return DeductVResult;
        }



        /// <summary>
        /// 验证邮件地址
        /// </summary>
        /// <param name="SendGroupEmailId">邮件Id</param>
        /// <returns>-1:不存在地址0:等待1:验证成功</returns>
        public int EmailVerified(string SAccount)
        {
            string Id = SqlHelper.One("select top 1 id from EmailSetting where UserId=" + UserId + " and DelState=0 and Account='" + DBUtility.Safe.SafeReplace(SAccount) + "' and CVerify=1");
            if (Id != null)
            {
                return 1;
            }
            int Result = 0;
            if (SAccount != null && SAccount != "")
            {
                //添加发件/回复邮箱
                //Comm100Manage1.EmailVerifiedAdd(FromEmailAddress);
                //查询邮箱验证结果
                Result = Comm100Manage1.EmailVerifiedResult(SAccount);
                if (Result == 1) {
                    SqlHelper.Upd("update EmailSetting set CVerify=1 where UserId=" + UserId + " and DelState=0 and Account='" + DBUtility.Safe.SafeReplace(SAccount) + "' and CVerify=0");
                }
                
            }
            else {
                return -1;
            }
            return Result;
        }



    }
}