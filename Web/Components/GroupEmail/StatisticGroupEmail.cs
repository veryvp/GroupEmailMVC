using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Threading;

namespace Web.Components
{
    /// <summary>
    /// 统计群发邮件
    /// </summary>
    public class StatisticGroupEmail : BasePage
    {
        private static DBUtility.SqlManage SqlHelper = new DBUtility.SqlManage();
        private static Comm100Manage Comm100Manage1 = new Comm100Manage();
        private static MailGunManage MailGunManage1 = new MailGunManage();
        private string GStatisticIntervalTime = System.Configuration.ConfigurationManager.AppSettings["GStatisticIntervalTime"];


        /// <summary>
        /// 统计发送结果
        /// </summary>
        /// <param name="QueueId">队列Id</param>
        /// <param name="IsScore">是否计分</param>
        /// <returns></returns>
        public string StatisticsResult(long Id, int IsScore = 0)
        {

            if (Id != 0)
            {
                string SId = GetStatisticTask();
                if (SId != null && SId != "")
                {
                    return "{\"Type\":-1,\"Message\":\"数据正在统计中,请稍后再试!\",\"Id\":" + SId + "}";
                }
                int count = SqlHelper.Upd("update [GroupEmail].[dbo].[Email] set Status=1 where UserId = " + base.UserId + " and Id = " + Id);
                if (count < 1) return "{\"Type\":-1,\"Message\":\"该任务不存在\"}";
                ManualResetEvent eventX = new ManualResetEvent(false);
                ThreadPool.SetMaxThreads(3, 3);
                thr t = new thr(1, eventX);
                string[] o = new string[3] { Id.ToString(), base.UserId.ToString(), IsScore.ToString() };
                ThreadPool.QueueUserWorkItem(new WaitCallback(t.ThreadProc), o);
                return "{\"Type\": 0,\"Message\":\"任务已提交\"}";
            }

            return "{\"Type\":-1,\"Message\":\"该任务不存在\"}";
        }



        /// <summary>
        /// 统计发送结果并计分
        /// </summary>
        /// <returns></returns>
        public string StatisticsScoreResult(long QId=0)
        {
            ManualResetEvent eventX = new ManualResetEvent(false);
            ThreadPool.SetMaxThreads(3, 3);
            thr t = new thr(1, eventX);
            string where = "";
            if (QId != 0) {
                where += " and Q.Id =" + QId;
            }
            DataTable dt = SqlHelper.Sel("select Q.Id,Q.UserId  from [iTradeEDM].[dbo].[GroupEmailStatistic] S left join [iTradeEDM].[dbo].[GroupEmailQueue] Q on s.QueueId=Q.Id where Q.IsComplete=1 and isnull(S.[Status],0)=0 and datediff(HH,Q.SendTime,getdate())>23 and isnull(Q.IsScored,0)=0 and Q.SendTime>'2015-1-17 12:30:00' " + where);
            if (dt.Rows.Count > 0) {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string QueueId = dt.Rows[i]["Id"].ToString();
                    string UserId = dt.Rows[i]["UserId"].ToString();
                    int count = SqlHelper.Upd("update [iTradeEDM].[dbo].[GroupEmailStatistic] set Status=1,AddTime=getdate() where UserId = " + UserId + " and QueueId = " + QueueId);
                    if (count < 1) return "{\"Type\":-1,\"Message\":\"该任务不存在\"}";
                    string[] o = new string[3] { QueueId.ToString(), UserId.ToString(), "1" };
                    ThreadPool.QueueUserWorkItem(new WaitCallback(t.ThreadProc), o);
                    SqlHelper.Upd("update [iTradeEDM].[dbo].[GroupEmailQueue] set IsScored=1 where UserId = " + UserId + " and Id = " + QueueId);
                }
                return "{\"Type\": -1,\"Message\":\"任务上传失败\"}";
            }
            return "{\"Type\": 0,\"Message\":\"Ok\"}";
        }

        public class thr
        {

            private DBUtility.SqlManage SqlHelper = new DBUtility.SqlManage();

            public thr(int count, ManualResetEvent mre)
            {
                iMaxCount = count;
                eventX = mre;
            }


            public static int iCount = 0;
            public static int iMaxCount = 0;
            public ManualResetEvent eventX;
            public void ThreadProc(object o)
            {
                string[] strs = (string[])o;

                int EmailId = Convert.ToInt32(strs[0]);
                int UserId = Convert.ToInt32(strs[1]);
                bool IsScore = Convert.ToInt32(strs[2]) == 1;

                //查询要统计的发送队列
                string Sql = "select DataTaskId,UserId,ParentId,EmailTaskId,SendTime,CampainId,Domain,CEmailId,FromEmailAddress from [Email] where DataTaskId>0 and UserId = " + UserId;
                if (EmailId != 0)
                {
                    Sql = Sql + " and Id=" + EmailId;
                }
                DataTable dt = SqlHelper.Sel(Sql);
                if (dt.Rows.Count > 0)
                {

                        int CEmailId = Convert.ToInt32(dt.Rows[0]["CEmailId"]);
                        string CampainId = dt.Rows[0]["CampainId"].ToString();
                        int DataTaskId = Convert.ToInt32(dt.Rows[0]["DataTaskId"]);
                        int EmailTaskId = Convert.ToInt32(dt.Rows[0]["EmailTaskId"]);
                        //int UserId = Convert.ToInt32(dt.Rows[i]["UserId"]);
                        int ParentId = Convert.ToInt32(dt.Rows[0]["ParentId"]);
                        string FromEmailAddress = dt.Rows[0]["FromEmailAddress"].ToString();


                        SqlHelper.Del(" delete from [ClickDetail] where EmailId =" + CEmailId);
                        SqlHelper.Del(" delete from [OpenDetail] where EmailId =" + CEmailId);


                        if (EmailTaskId > 0)
                        {

                            if (CEmailId == 0)
                            {
                                string SendResult = Comm100Manage1.EmailSendResult(EmailTaskId, EmailId);
                                if (SendResult == "Failed")
                                {
                                    SqlHelper.Upd("update CustDataReport set FailureTime=getdate() where UserId=" + UserId + " and  CustDataTaskId = " + DataTaskId);
                                }
                                if (SendResult == "Finished")
                                {
                                    string EmailIdStr = SqlHelper.One("select CEmailId from Email where UserId=" + UserId + " and Id = " + EmailId);
                                    if (EmailIdStr != null)
                                    {
                                        CEmailId = Convert.ToInt32(EmailIdStr);
                                    }
                                }
                                else
                                {
                                    CEmailId = 0;
                                }

                            }
                            if (CEmailId > 0)
                            {
                                //更新总体发送情况
                                //Comm100Manage1.UpdateEmailStatistic(EmailId, ReportId);
                                //更新打开情况
                                Comm100Manage1.UpdateEmailOpen(CEmailId, EmailId, DataTaskId, UserId, ParentId, 1);
                                //更新点击
                                Comm100Manage1.UpdateEmailClick(CEmailId, EmailId, DataTaskId, UserId, ParentId, 1);
                                //更新退回
                                Comm100Manage1.UpdateEmailBounced(CEmailId, DataTaskId, UserId, 1);
                                //更新发送、送达 、退订详情
                                Comm100Manage1.UpdateEmailSentDetail(CEmailId, EmailId, DataTaskId, UserId, FromEmailAddress, 1);
                            }
                        }
                        if (CampainId != "")
                        {
                            //更新总体发送详情
                            //MailGunManage1.UpdCampaignNum(QId, ReportId);
                            //更新送达详情
                            int DNum = 100;
                            int DPage = 1;
                            while (DNum == 100)
                            {
                                DNum = MailGunManage1.UpdCampaignsDelivered(CampainId, DPage, DataTaskId);
                                DPage++;
                            }
                            //更新打开详情
                            int ONum = 100;
                            int OPage = 1;
                            while (ONum == 100)
                            {
                                ONum = MailGunManage1.UpdCampaignsOpens(CampainId, OPage, UserId, ParentId, EmailId, DataTaskId);
                                OPage++;
                            }

                            //更新点击详情
                            int CNum = 100;
                            int CPage = 1;
                            while (CNum == 100)
                            {
                                CNum = MailGunManage1.UpdCampaignsClick(CampainId, CPage, UserId, ParentId, EmailId, DataTaskId);
                                CPage++;
                            }
                            //更新退回详情
                            int BNum = 100;
                            int BPage = 1;
                            while (BNum == 100)
                            {
                                BNum = MailGunManage1.UpdCampaignsBounced(CampainId, BPage, DataTaskId);
                                BPage++;
                            }
                            //更新退订详情
                            int UNum = 100;
                            int UPage = 1;
                            while (UNum == 100)
                            {
                                UNum = MailGunManage1.UpdCampaignsUnsubscribed(CampainId, UPage, UserId, ParentId, FromEmailAddress, EmailId, DataTaskId);
                                UPage++;
                            }
                            //更新发送失败信息
                            int DrNum = 100;
                            int DrPage = 1;
                            while (DrNum == 100)
                            {
                                DrNum = MailGunManage1.UpdCampaignsDropped(CampainId, DrPage, DataTaskId);
                                DrPage++;
                            }

                        }

                        //邮箱计分
                        if (IsScore)
                        {
                            //统计白名单
                            SqlHelper.Upd("update [EmailScore] set EmailType=1,[Score]=0,LatestRecordTime=getdate() where id in (select S.Id from [CustDataReport] R left join EmailScore S on  R.Email=S.Email where R.UserId= " + UserId + " and R.DeliveryTime>0 and UnsubscribedTime is null and CustDataTaskId=" + DataTaskId + ")  ");
                            //统计Comm100退回
                            //统计Comm100硬退回和不存在地址加三分
                            SqlHelper.Upd("update [EmailScore] set EmailType=0,[Score]=[Score]+3,LatestRecordTime=getdate() where id in (select S.Id from [CustDataReport] R left join EmailScore S on  R.Email=S.Email where R.UserId= " + UserId + " and  UnsubscribedTime >0 and BouncedType in (1,3) and CustDataTaskId=" + DataTaskId + ") and EmailType in(0,1) and datediff(HH,LatestRecordTime,getdate())>23 ");
                            //统计Comm100软退回加1分
                            SqlHelper.Upd("update [EmailScore] set EmailType=0,[Score]=[Score]+3,LatestRecordTime=getdate() where id in (select S.Id from [CustDataReport] R left join EmailScore S on  R.Email=S.Email where R.UserId= " + UserId + " and  UnsubscribedTime >0 and BouncedType in (0,2) and CustDataTaskId=" + DataTaskId + ") and EmailType in(0,1) and datediff(HH,LatestRecordTime,getdate())>23 ");
                            //未送达(除去退回)的邮箱加三分
                            SqlHelper.Upd("update [EmailScore] set EmailType=0,[Score]=[Score]+3,LatestRecordTime=getdate() where id in (select S.Id from [CustDataReport] R left join EmailScore S on  R.Email=S.Email where R.UserId= " + UserId + " and DeliveryTime is null and UnsubscribedTime is null and CustDataTaskId=" + DataTaskId + ") and EmailType in(0,1) and datediff(HH,LatestRecordTime,getdate())>23");
                            //更新邮箱黑名单
                            //邮箱分数超出10分的加入邮箱黑名单
                            SqlHelper.Upd("update [EmailScore] set EmailType=2,LatestRecordTime=getdate() where id in (select S.Id from [CustDataReport] R left join EmailScore S on  R.Email=S.Email where R.UserId= " + UserId + " and CustDataTaskId=" + DataTaskId + ") and [Score]>9");
                        }
                    }
                    eventX.Set();
                }

            }



        /// <summary>
        /// 获取正在统计的任务
        /// </summary>
        /// <returns></returns>
        public string GetStatisticTask()
        {
            DataTable dt = SqlHelper.Sel("select top 1 Id,Datediff(mi,AddTime,GETDATE()) as TimeBetween from [iTradeEDM].[dbo].[GroupEmailStatistic] where UserId = " + base.UserId + " and Status = 1");
            if (dt.Rows.Count == 1) {
                return dt.Rows[0]["Id"].ToString();
            }
            return "";
        }

        /// <summary>
        /// 获取统计的任务的结果
        /// </summary>
        /// <param name="QueueId">队列Id</param>
        /// <returns></returns>
        public string GetStatisticTaskResult(long SId)
        {
            DataTable dt = SqlHelper.Sel("select top 1 Id,Datediff(mi,AddTime,GETDATE()) as TimeBetween,TotalSent,Delivered,TotalBounced,HardBounced,SoftBounced,TotalOpened,UniqueOpened,TotalClicked,UniqueClicked,Unsubscribed,Status from [iTradeEDM].[dbo].[GroupEmailStatistic] where UserId = " + base.UserId + " and Id=" + SId);
            if (dt.Rows.Count == 1)
            {
                //若请求超过设定时间还未统计完成则超时
                if (Convert.ToInt32(dt.Rows[0]["TimeBetween"]) >= Convert.ToInt32(GStatisticIntervalTime))
                {
                    SqlHelper.Upd("update [iTradeEDM].[dbo].[GroupEmailStatistic] set Status=0 where UserId = " + base.UserId + " and Status = 1 and Id=" + SId);
                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                if (dt.Rows[0]["Status"].ToString() == "0")
                {
                    sb.Append("{\"Id\":\"" + SId + "\",");
                    sb.Append("\"TotalSent\":\"" + dt.Rows[0]["TotalSent"].ToString() + "\",");
                    sb.Append("\"Delivered\":\"" + dt.Rows[0]["Delivered"].ToString() + "\",");
                    sb.Append("\"TotalBounced\":\"" + dt.Rows[0]["TotalBounced"].ToString() + "\",");
                    sb.Append("\"HardBounced\":\"" + dt.Rows[0]["HardBounced"].ToString() + "\",");
                    sb.Append("\"SoftBounced\":\"" + dt.Rows[0]["SoftBounced"].ToString() + "\",");
                    sb.Append("\"TotalOpened\":\"" + dt.Rows[0]["TotalOpened"].ToString() + "\",");
                    sb.Append("\"UniqueOpened\":\"" + dt.Rows[0]["UniqueOpened"].ToString() + "\",");
                    sb.Append("\"TotalClicked\":\"" + dt.Rows[0]["TotalClicked"].ToString() + "\",");
                    sb.Append("\"UniqueClicked\":\"" + dt.Rows[0]["UniqueClicked"].ToString() + "\",");
                    sb.Append("\"Unsubscribed\":\"" + dt.Rows[0]["Unsubscribed"].ToString() + "\"}");
                    return sb.ToString();
                }
                else {
                    return "{}";
                }
                //string Json = DALGroupEmailStatistic1.GetJson(SId, "Id,TotalSent,Delivered,TotalBounced,HardBounced,SoftBounced,TotalOpened,UniqueOpened,TotalClicked,UniqueClicked,Unsubscribed,Status");
                //if (JsonHashtable1.GetNodeByKey(Json, "Status") == "0")
                //{
                //    return Json;
                //}
            }
            if (SId > 0) {
                return "{\"Id\":\"" + SId + "\"}";
            }
            return "{}";
        }


        /// <summary>
        /// 获取某个统计结果
        /// </summary>
        /// <param name="QueueId">队列Id</param>
        /// <returns></returns>
        public DataTable GetStatistic(long SId)
        {
            return SqlHelper.Sel("select top 1 * from [iTradeEDM].[dbo].[GroupEmailStatistic] where UserId = " + base.UserId + " and QueueId=" + SId);
        }


        /// <summary>
        /// 将统计结果更新到群发邮件统计报表
        /// </summary>
        /// <param name="QueueId">队列Id</param>
        /// <returns></returns>
        public static void UpdateStatictis(long QueueId = 0)
        {
            string Sql = "Update GroupEmailStatistic set TotalSent=(select Sum(TotalSent) from GroupEmailReport where GroupEmailStatistic.QueueId=GroupEmailReport.QueueId),Delivered=(select Sum(Delivered) from GroupEmailReport where GroupEmailStatistic.QueueId=GroupEmailReport.QueueId),TotalBounced=(select Sum(TotalBounced) from GroupEmailReport where GroupEmailStatistic.QueueId=GroupEmailReport.QueueId),HardBounced=(select Sum(HardBounced) from GroupEmailReport where GroupEmailStatistic.QueueId=GroupEmailReport.QueueId),SoftBounced=(select Sum(SoftBounced) from GroupEmailReport where GroupEmailStatistic.QueueId=GroupEmailReport.QueueId),TotalOpened=(select Sum(TotalOpened) from GroupEmailReport where GroupEmailStatistic.QueueId=GroupEmailReport.QueueId),UniqueOpened=(select Sum(UniqueOpened) from GroupEmailReport where GroupEmailStatistic.QueueId=GroupEmailReport.QueueId),TotalClicked=(select Sum(TotalClicked) from GroupEmailReport where GroupEmailStatistic.QueueId=GroupEmailReport.QueueId),UniqueClicked=(select Sum(UniqueClicked) from GroupEmailReport where GroupEmailStatistic.QueueId=GroupEmailReport.QueueId),Unsubscribed=(select Sum(Unsubscribed) from GroupEmailReport where GroupEmailStatistic.QueueId=GroupEmailReport.QueueId),SpamReported=(select Sum(SpamReported) from GroupEmailReport where GroupEmailStatistic.QueueId=GroupEmailReport.QueueId) ";
            if (QueueId != 0) {
                Sql = Sql + " where GroupEmailStatistic.QueueId=" + QueueId;
            }
            SqlHelper.Upd(Sql);
        }


        /// <summary>
        /// 计算Url点击率并将结果更新到Url点击率报表
        /// </summary>
        /// <param name="QueueId">队列Id</param>
        /// <param name="UserId">用户Id</param>
        /// <param name="ParentId">用户父Id</param>
        /// <returns></returns>
        public static void UpdateUrlClick(long QueueId = 0)
        {
            //统计结果
            //string Sql = " update GroupEmailUrlClick set TotalClick = (select COUNT(*) from GroupEmailReportClickDetail where GroupEmailUrlClick.QueueId=GroupEmailReportClickDetail.QueueId and GroupEmailUrlClick.Url=GroupEmailReportClickDetail.URL  and GroupEmailReportClickDetail.DelState=0),TotalClickPercent = (CONVERT(float,(select COUNT(*) from GroupEmailReportClickDetail where GroupEmailUrlClick.QueueId=GroupEmailReportClickDetail.QueueId and GroupEmailUrlClick.Url=GroupEmailReportClickDetail.URL and GroupEmailReportClickDetail.DelState=0))/CONVERT(float,(select COUNT(*) from GroupEmailReportClickDetail where GroupEmailUrlClick.QueueId=GroupEmailReportClickDetail.QueueId and GroupEmailReportClickDetail.DelState=0 ))),UniqueClick= (select COUNT(Distinct(IPAddress)) from GroupEmailReportClickDetail where GroupEmailUrlClick.QueueId=GroupEmailReportClickDetail.QueueId and GroupEmailUrlClick.Url=GroupEmailReportClickDetail.URL and GroupEmailReportClickDetail.DelState=0 ) ,UniqueClickPercent = (CONVERT(float,(select COUNT(Distinct(IPAddress)) from GroupEmailReportClickDetail where GroupEmailUrlClick.QueueId=GroupEmailReportClickDetail.QueueId and GroupEmailUrlClick.Url=GroupEmailReportClickDetail.URL and GroupEmailReportClickDetail.DelState=0))/CONVERT(float, (select sum(A.UniqueAmount) from (select COUNT(Distinct(IPAddress)) UniqueAmount from GroupEmailReportClickDetail where GroupEmailReportClickDetail.QueueId=GroupEmailUrlClick.QueueId  and GroupEmailReportClickDetail.DelState=0 group by GroupEmailReportClickDetail.URL) as A))) where (select COUNT(*) from GroupEmailReportClickDetail where GroupEmailReportClickDetail.QueueId=GroupEmailUrlClick.QueueId and GroupEmailReportClickDetail.URL=GroupEmailUrlClick.Url and GroupEmailReportClickDetail.DelState=0)>0 ";
            string Sql = " update GroupEmailUrlClick set TotalClick = (select COUNT(*) from GroupEmailReportClickDetail where GroupEmailUrlClick.QueueId=GroupEmailReportClickDetail.QueueId and GroupEmailUrlClick.Url=GroupEmailReportClickDetail.URL  and GroupEmailReportClickDetail.DelState=0),TotalClickPercent = (CONVERT(float,(select COUNT(*) from GroupEmailReportClickDetail where GroupEmailUrlClick.QueueId=GroupEmailReportClickDetail.QueueId and GroupEmailUrlClick.Url=GroupEmailReportClickDetail.URL and GroupEmailReportClickDetail.DelState=0))/CONVERT(float,(select COUNT(*) from GroupEmailReportClickDetail where GroupEmailUrlClick.QueueId=GroupEmailReportClickDetail.QueueId and GroupEmailReportClickDetail.DelState=0 ))),UniqueClick= (select COUNT(Distinct(IPAddress)) from GroupEmailReportClickDetail where GroupEmailUrlClick.QueueId=GroupEmailReportClickDetail.QueueId and GroupEmailUrlClick.Url=GroupEmailReportClickDetail.URL and GroupEmailReportClickDetail.DelState=0 ) ,UniqueClickPercent = (CONVERT(float,(select COUNT(Distinct(IPAddress)) from GroupEmailReportClickDetail where GroupEmailUrlClick.QueueId=GroupEmailReportClickDetail.QueueId and GroupEmailUrlClick.Url=GroupEmailReportClickDetail.URL and GroupEmailReportClickDetail.DelState=0))/CONVERT(float,(select COUNT(*) from GroupEmailReportClickDetail where GroupEmailUrlClick.QueueId=GroupEmailReportClickDetail.QueueId and GroupEmailReportClickDetail.DelState=0 ))) where (select COUNT(*) from GroupEmailReportClickDetail where GroupEmailReportClickDetail.QueueId=GroupEmailUrlClick.QueueId and GroupEmailReportClickDetail.URL=GroupEmailUrlClick.Url and GroupEmailReportClickDetail.DelState=0)>0 ";
            if (QueueId != 0)
            {
                Sql = Sql + " and GroupEmailUrlClick.QueueId=" + QueueId;
            }
            SqlHelper.Upd(Sql);
        }


    }
}