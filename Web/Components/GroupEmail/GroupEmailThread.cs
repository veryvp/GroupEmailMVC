using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Data;
using Common.Base;
using System.Text.RegularExpressions;
using System.Collections;



namespace Web.Components
{
    /// <summary>
    /// 线程发送邮件
    /// </summary>
    public class GroupEmailThread
    {
        private static DBUtility.SqlManage SqlHelper = new DBUtility.SqlManage();
        private static Comm100Manage Comm100Manage1 = new Comm100Manage();
        private static MailGunManage MailGunManage1 = new MailGunManage();
        private static string GDomain = System.Configuration.ConfigurationManager.AppSettings["GGun_DOMAIN"];



        public static void SendEmail(int EmailId)
        {
            //新建ManualResetEvent对象并且初始化为无信号状态
            ManualResetEvent eventX = new ManualResetEvent(false);
            ThreadPool.SetMaxThreads(3, 3);
            if (EmailId == 0) return;
            string Sql = "select Id,DataTaskId from Email where DelState=0 and IsError = 0 and Id=" + EmailId;//直接发送
            DataTable dt = SqlHelper.Sel(Sql);
            if (dt.Rows.Count > 0) {


                //2:发送邮件
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                  
                    thr t = new thr(dt.Rows.Count, eventX);
                    string[] o = new string[2] { dt.Rows[i]["Id"].ToString(), dt.Rows[i]["DataTaskId"].ToString() };
                    ThreadPool.QueueUserWorkItem(new WaitCallback(t.ThreadProc), o);

                }
            }
           
        }
 
        public class thr
        {

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
                int DataTaskId = Convert.ToInt32(strs[1]);
                string FileRoot = System.Configuration.ConfigurationManager.AppSettings["FileRoot"];
                DataTable EmailDt = SqlHelper.Sel("select top 1 Subject,Body,FromEmailAddress,FromName,UserId,ParentId,SendTime from Email where id = " + EmailId);
                if (EmailDt.Rows.Count > 0 && EmailDt.Rows.Count == 1)
                {
                    int UserId = Convert.ToInt32(EmailDt.Rows[0]["UserId"].ToString());
                    int ParentId = Convert.ToInt32(EmailDt.Rows[0]["ParentId"].ToString());
                    string Subject = DBUtility.Safe.safereturn(EmailDt.Rows[0]["Subject"].ToString());
                    string Body = DBUtility.Safe.safereturn(EmailDt.Rows[0]["Body"].ToString());
                    string FromEmailAddress = DBUtility.Safe.safereturn(EmailDt.Rows[0]["FromEmailAddress"].ToString());
                    string FromName = DBUtility.Safe.safereturn(EmailDt.Rows[0]["FromName"].ToString());


                    //发送时间
                    DateTime SentTime = Convert.ToDateTime(EmailDt.Rows[0]["SendTime"].ToString());

                    //是否发送失败
                    bool IsFailure = false;

                    string FileName = Comm100Manage1.CreateCsv(DataTaskId);
                    if (FileName != null)
                    {
                        SentTime = SentTime.AddHours(-8);
                        int EmailTaskId = Comm100Manage1.EmailSend(FileName, Subject, Body, FromEmailAddress, FromEmailAddress, FromName, SentTime.ToString(), UserId, ParentId);
                        //EmailTaskId为零时表示发送失败
                        if (EmailTaskId != 0)
                        {
                            string SendResult = Comm100Manage1.EmailSendResult(EmailTaskId, EmailId);

                            if (SendResult == "Failed")
                            {
                                IsFailure = true;
                            }
                        }
                        else
                        {
                            SqlHelper.Upd("Update Email set IsError = 1,ErrorMessage='Comm100群发邮件失败',FileName='" + DBUtility.Safe.SafeReplace(FileName.Replace(FileRoot, "")) + "' where Id = " + EmailId);
                            IsFailure = true;
                        }
                    }
                    else
                    {
                        IsFailure = true;
                    }
                    if (IsFailure)
                    {
                        SqlHelper.Upd("update [GroupEmail].[dbo].[CustDataReport] set FailureTime=getdate() where Platform=0 and CustDataTaskId = " + DataTaskId);
                    }

                    IsFailure = false;
                    string MailGunAmount = SqlHelper.One("select count(*) from [GroupEmail].[dbo].[CustDataReport] where  Platform=1 and CustDataTaskId = " + DataTaskId);
                    if (MailGunAmount != null)
                    {
                        int SentAmount = Convert.ToInt32(MailGunAmount);
                        string MailGunDelayTime = SqlHelper.One("select top 1 [State] from [RMDB].[dbo].[Config] where Code='MailGun_Delay_Time'");
                        int MDelayTime = 0;
                        if (MailGunDelayTime != null && MailGunDelayTime != "")
                        {
                            MDelayTime = Convert.ToInt32(MailGunDelayTime);
                        }

                        string CampainId = DateTime.Now.ToString("yyyyMMddHHmmssffff") + EmailId;

                        //创建campaign
                        if (MailGunManage1.CreateCampaign("c" + CampainId + "@" + GDomain, CampainId))
                        {
                            //创建收件人列表
                            string ListName = MailGunManage1.CreateMailingList("m" + CampainId + "@" + GDomain, EmailId);
                            if (ListName != null)
                            {
                                //上传收件人列表
                                if (MailGunManage1.AddListMember(ListName, DataTaskId, SentAmount))
                                {
                                    //发送收件人列表
                                    if (FromName != "")
                                    {
                                        FromEmailAddress = FromName + " <" + FromEmailAddress + ">";
                                    }
                                    if (SentTime < DateTime.Now.AddMinutes(MDelayTime))
                                    {
                                        SentTime = DateTime.Now.AddMinutes(1);
                                    }
                                    if (SentAmount > 100)
                                    {
                                        SentTime = SentTime.AddHours(-8).AddMinutes(MDelayTime);
                                    }
                                    else
                                    {
                                        SentTime = SentTime.AddHours(-8);
                                    }
                                    //发送邮件
                                    bool IsSuccess = MailGunManage1.SendScheduledMessagelist(ListName, FromEmailAddress, Subject, Body, SentTime, CampainId, UserId, ParentId);
                                    if (IsSuccess)
                                    {
                                        //更新任务
                                        string Sql2 = "Update Email set  CampainId=" + DBUtility.Safe.SafeReplace(CampainId) + ",Domain='" + DBUtility.Safe.SafeReplace(GDomain) + "' where Id = " + EmailId;

                                        SqlHelper.Upd(Sql2);
                                    }
                                    else
                                    {
                                        IsFailure = true;
                                    }
                                }
                                else
                                {
                                    IsFailure = true;
                                }
                            }
                            else
                            {
                                IsFailure = true;
                            }

                        }
                        else
                        {
                            IsFailure = true;
                        }
                    }


                    //若发送失败则插入发送失败列表
                    if (IsFailure)
                    {
                        SqlHelper.Upd("update [GroupEmail].[dbo].[CustDataReport] set FailureTime=getdate() where Platform=1 and CustDataTaskId = " + DataTaskId);
                    }

                    ////将发送的邮件包含的Url插入表中
                    ////Regex reg = new Regex(@"<a\s*href=(""|')(?<url>[\s\S.]*?)(""|').*?>");
                    //Regex reg = new Regex(@"<a[^>]*href=[""|'](?<url>[^""|']*?)[""|'][^>]*>(?<text>[\w\W]*?)</a>");
                    //MatchCollection mc = reg.Matches(Body);
                    //ArrayList UrlList = new ArrayList();
                    //foreach (Match m in mc)
                    //{
                    //    //排除重复Url
                    //    if (!UrlList.Contains(DBUtility.Safe.SafeReplace((m.Groups["url"].Value + "").Trim())))
                    //    {
                    //        string url = DBUtility.Safe.SafeReplace((m.Groups["url"].Value + "").Trim());
                    //        UrlList.Add(url);
                    //        string InsertClickUrlSql = "insert into GroupEmailUrlClick(QueueId,Url,UserId,ParentId) values('" + QueueId + "','" + url + "', '" + UserId + "' ,'" + ParentId + "')";
                    //        SqlHelper.Ins(InsertClickUrlSql);
                    //    }
                    //}

                    
                }
                //Interlocked.Increment(ref iCount);
                eventX.Set();

            }
        }

    }
}