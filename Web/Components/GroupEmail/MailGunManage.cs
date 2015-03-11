using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using System.IO;
using Common.Base;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;

namespace Web.Components
{
    /// <summary>
    /// MailGun管理
    /// </summary>
    public class MailGunManage
    {
        //邮件配置信息
        private static string ApiKey = System.Configuration.ConfigurationManager.AppSettings["Gun_Key"];
        private static string Domain = System.Configuration.ConfigurationManager.AppSettings["Gun_DOMAIN"];
        private static string SmtpDomain = System.Configuration.ConfigurationManager.AppSettings["Gun_Smtp_DOMAIN"];
        //群发配置信息
        private static string GApiKey = System.Configuration.ConfigurationManager.AppSettings["GGun_Key"];
        private static string GDomain = System.Configuration.ConfigurationManager.AppSettings["GGun_DOMAIN"];
        private string FileRoot = System.Configuration.ConfigurationManager.AppSettings["FileRoot"];
        private JsonHashtable JsonHashtable1 = new JsonHashtable();
        private DBUtility.SqlManage SqlHelper = new DBUtility.SqlManage();
        private ErrorLog Log = new ErrorLog();
        private Web.Components.Base.FileDownLoad FileDownLoad1 = new Web.Components.Base.FileDownLoad();



        /// <summary>
        /// 添加七牛附件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Html"></param>
        /// <param name="Size"></param>
        /// <param name="UserId"></param>
        /// <param name="ParentId"></param>
        private void QiniuAttach( int UserId, int ParentId,ref RestRequest request, ref string Html, ref long Size)
        {

            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?/Ajax/Att.aspx[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串   
            MatchCollection matches = regImg.Matches(Html);

            if (matches.Count > 0)
            {
                Hashtable imght = new Hashtable();
                //string STbCidstr = ""; //发件箱插入到正文附件
                //string TbCidstr = ""; //收件箱插入到正文附件
                string SEidstr = ""; //发件箱邮件Ids
                string Eidstr = ""; //收件箱邮件Ids
                string GEidstr = ""; //群发邮件Ids
                // 取得匹配项列表   
                foreach (Match match in matches)
                {
 
                    string url = (match.Groups["imgUrl"].Value + "").Trim();
                    string[] Params = url.Split('=');
                    if (Params.Length == 3) {
                        string aid = Params[2];
                        string t = Params[1].Substring(0,1);
                        //string FilePath = Params[1].Substring(0, Params[1].Length-6);
                        if (!imght.Contains(t + "---" + aid))
                        {
                            if (t == "0")
                            {
                                //TbCidstr += "'" + DBUtility.Safe.SafeReplace(FilePath) + "',";
                                long Aid;
                                if (Int64.TryParse(aid, out Aid))
                                {
                                    Eidstr += Aid + ",";
                                }
                            }
                            if (t == "1")
                            {
                                //STbCidstr += "'" + DBUtility.Safe.SafeReplace(FilePath) + "',";
                                long Aid;
                                if (Int64.TryParse(aid, out Aid))
                                {
                                    SEidstr += Aid + ",";
                                }
                            }
                            if (t == "2")
                            {
                                //STbCidstr += "'" + DBUtility.Safe.SafeReplace(FilePath) + "',";
                                long Aid;
                                if (Int64.TryParse(aid, out Aid))
                                {
                                    GEidstr += Aid + ",";
                                }
                            }
                            imght.Add(t + "---" + aid, url);
                        }
                    }
                }
                if (Eidstr != "" || SEidstr != "" || GEidstr != "")
                {
                    DataTable CAttachDt = new DataTable();
                    if (SEidstr != "")
                    {
                        CAttachDt = SqlHelper.Sel("select Id,Name,FileName,FilePath,Size,ContentId,EmailId from [iTradeEM].[dbo].[SAttachment] where UserId=" + UserId + " and Id in (" + SEidstr.Trim(',') + ")");
                        if (CAttachDt.Rows.Count > 0)
                        {
                            for (int i = 0; i < CAttachDt.Rows.Count; i++){

                                string FilePath =  DBUtility.Safe.safereturn(CAttachDt.Rows[i]["FilePath"].ToString());
                                string Name = DBUtility.Safe.safereturn(CAttachDt.Rows[i]["Name"].ToString());
                                string Aid = CAttachDt.Rows[i]["Id"].ToString();
                                if (imght.Contains("1---" + Aid))
                                {

                                    Html = Html.Replace("src=\"/Ajax/Att.aspx" + imght["1---" + Aid].ToString() + "\"", "src=\"cid:" + Name + "\"");
                                    AddInline(ref request, ref Size, Name, FilePath);
                                }
                                
                            }
                        }
                    }
                    if (Eidstr != "")
                    {
                        CAttachDt = SqlHelper.Sel("select Id,Name,FileName,FilePath,Size,ContentId,EmailId from [iTradeEM].[dbo].[Attachment] where UserId=" + UserId + " and  Id in (" + Eidstr.Trim(',') + ")");
                        if (CAttachDt.Rows.Count > 0)
                        {
                            for (int i = 0; i < CAttachDt.Rows.Count; i++)
                            {
                                string FilePath = DBUtility.Safe.safereturn(CAttachDt.Rows[i]["FilePath"].ToString());
                                string Name = DBUtility.Safe.safereturn(CAttachDt.Rows[i]["Name"].ToString());
                                string Aid = CAttachDt.Rows[i]["Id"].ToString();
                                if (imght.Contains("0---" + Aid))
                                {
                                    Html = Html.Replace("src=\"/Ajax/Att.aspx" + imght["0---" + Aid].ToString() + "\"", "src=\"cid:" + Name + "\"");
                                    AddInline(ref request, ref Size, Name, FilePath);
                                }

                            }
                        }
                    }
                    if (GEidstr != "")
                    {
                        CAttachDt = SqlHelper.Sel("select Id,Name,FileName,FilePath,Size from [iTradeEDM].[dbo].[GAttachment] where UserId=" + UserId + " and  Id in (" + GEidstr.Trim(',') + ")");
                        if (CAttachDt.Rows.Count > 0)
                        {
                            for (int i = 0; i < CAttachDt.Rows.Count; i++)
                            {
                                string FilePath = DBUtility.Safe.safereturn(CAttachDt.Rows[i]["FilePath"].ToString());
                                string Name = DBUtility.Safe.safereturn(CAttachDt.Rows[i]["Name"].ToString());
                                string Aid = CAttachDt.Rows[i]["Id"].ToString();
                                if (imght.Contains("2---" + Aid))
                                {
                                    Html = Html.Replace("src=\"/Ajax/Att.aspx" + imght["2---" + Aid].ToString() + "\"", "src=\"cid:" + Name + "\"");
                                    AddInline(ref request, ref Size, Name, FilePath);
                                }

                            }
                        }
                    }
                }

            }
        }
        /// <summary>
        /// 添加嵌入正文附件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Size"></param>
        /// <param name="Name"></param>
        /// <param name="FilePath"></param>
        private void AddInline(ref RestRequest request, ref long Size, string Name, string FilePath) {
            FileDownLoad1.FileSyn(Name, FilePath);

            FileInfo fi = new FileInfo(FileRoot + FilePath);
            long len = fi.Length;
            Size += len;
            FileStream fs = new FileStream(FileRoot + FilePath, FileMode.Open);

            byte[] buffer = new byte[len];

            fs.Read(buffer, 0, (int)len);

            fs.Close();
            request.AddFile("inline", buffer, Name);
        }

        /// <summary>
        /// 附加附件(本地文件)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="request"></param>
        /// <param name="Htmls"></param>
        /// <param name="imght"></param>
        /// <param name="isinline"></param>
        public void AttachAdd1(DataTable dt, ref RestRequest request, ref string Html, ref long Size, Hashtable imght = null, bool isinline = false)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                //FileInfo fi = new FileInfo(@System.Web.HttpContext.Current.Server.MapPath(dt.Rows[i]["FilePath"].ToString()));
                FileInfo fi = new FileInfo(FileRoot + dt.Rows[i]["FilePath"].ToString());
                long len = fi.Length;
                Size += len;
                //FileStream fs = new FileStream(@System.Web.HttpContext.Current.Server.MapPath(dt.Rows[i]["FilePath"].ToString()), FileMode.Open);
                FileStream fs = new FileStream(FileRoot + dt.Rows[i]["FilePath"].ToString(), FileMode.Open);

                byte[] buffer = new byte[len];

                fs.Read(buffer, 0, (int)len);

                fs.Close();

                if (isinline)
                {
                    string ContentId = dt.Rows[i]["ContentId"].ToString();
                    if (imght.Contains(ContentId + "eid--a--" + dt.Rows[i]["EmailId"].ToString()))
                    {
                        Html = Html.Replace("src=\"/Ajax/ViewFile.aspx" + imght[ContentId + "eid--a--" + dt.Rows[i]["EmailId"].ToString()].ToString() + "\"", "src=\"cid:" + dt.Rows[i]["FileName"].ToString() + "\"");
                        request.AddFile("inline", buffer, DBUtility.Safe.safereturn(dt.Rows[i]["FileName"].ToString()));
                    }
                }
                else
                {
                    request.AddFile("attachment", buffer, DBUtility.Safe.safereturn(dt.Rows[i]["FileName"].ToString()));
                }

            }
        }

        /// <summary>
        /// 附加附件(七牛文件)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="request"></param>
        /// <param name="Htmls"></param>
        /// <param name="imght"></param>
        /// <param name="isinline"></param>
        public void AttachAdd(DataTable dt, ref RestRequest request, ref string Html, ref long Size, Hashtable imght = null, bool isinline = false)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string Name = DBUtility.Safe.safereturn(dt.Rows[i]["Name"].ToString());
                string FileName = DBUtility.Safe.safereturn(dt.Rows[i]["FileName"].ToString());
                string FilePath = DBUtility.Safe.safereturn(dt.Rows[i]["FilePath"].ToString());
                FileDownLoad1.FileSyn(Name, FilePath);
                //FileInfo fi = new FileInfo(@System.Web.HttpContext.Current.Server.MapPath(dt.Rows[i]["FilePath"].ToString()));
                FileInfo fi = new FileInfo(FileRoot + dt.Rows[i]["FilePath"].ToString());
                long len = fi.Length;
                Size += len;
                //FileStream fs = new FileStream(@System.Web.HttpContext.Current.Server.MapPath(dt.Rows[i]["FilePath"].ToString()), FileMode.Open);
                FileStream fs = new FileStream(FileRoot + dt.Rows[i]["FilePath"].ToString(), FileMode.Open);

                byte[] buffer = new byte[len];

                fs.Read(buffer, 0, (int)len);

                fs.Close();
                
                if (isinline)
                {
                    string ContentId = dt.Rows[i]["ContentId"].ToString();

                    if (imght.Contains(ContentId + "eid--a--" + dt.Rows[i]["EmailId"].ToString()))
                    {
                        Html = Html.Replace("src=\"/Ajax/ViewFile.aspx" + imght[ContentId + "eid--a--" + dt.Rows[i]["EmailId"].ToString()].ToString() + "\"", "src=\"cid:" + Name + "\"");
                        request.AddFile("inline", buffer, Name);
                        //request.AddFile("inline", QiniuUpload1.FileUrl(QiniuUpload1.FileUrl(dt.Rows[i]["FilePath"].ToString(), 1, 1, dt.Rows[i]["FileName"].ToString())), DBUtility.Safe.safereturn(dt.Rows[i]["FileName"].ToString()));
                        //request.AddFile("inline", QiniuUpload1.FileUrl(dt.Rows[i]["FilePath"].ToString()), DBUtility.Safe.safereturn(dt.Rows[i]["FileName"].ToString()));
                    }
                }
                else
                {
                    request.AddFile("attachment", buffer, FileName);
                    //request.AddFile("attachment", QiniuUpload1.FileUrl(QiniuUpload1.FileUrl(dt.Rows[i]["FilePath"].ToString(), 1, 1, dt.Rows[i]["FileName"].ToString())), DBUtility.Safe.safereturn(dt.Rows[i]["FileName"].ToString()));
                    //request.AddFile("attachment", QiniuUpload1.FileUrl(dt.Rows[i]["FilePath"].ToString()), DBUtility.Safe.safereturn(dt.Rows[i]["FileName"].ToString()));
                }

            }
        }



        /// <summary>
        /// 发送邮件至某收件人
        /// </summary>
        /// <param name="From">发件人</param>
        /// <param name="To">收件人</param>
        /// <param name="Subject">主题</param>
        /// <param name="Html">邮件内容</param>
        /// <param name="FileIds">附件</param>
        /// <returns></returns>
        public string SendEmail(string From, string To, string Subject, string Html)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               ApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 Domain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", DBUtility.Safe.safereturn(From));

            request.AddParameter("to", DBUtility.Safe.safereturn(To));
            request.AddParameter("subject", DBUtility.Safe.safereturn(Subject));
            request.AddParameter("html", DBUtility.Safe.safereturn(Html));

            request.AddParameter("o:campaign", "bv1ug");


            request.Method = Method.POST;

            IRestResponse res = client.Execute(request);
            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                return "{\"Type\":0,\"Message\":\"操作成功\"}";
            }
            else
            {
                return "{\"Type\":-1,\"Message\":\"发送失败\"}";
            }
        }
        


        ///// <summary>
        ///// 发送邮件至多个收件人
        ///// </summary>
        ///// <param name="From">发件人</param>
        ///// <param name="To">收件人</param>
        ///// <param name="Subject">主题</param>
        ///// <param name="Html">邮件内容</param>
        ///// <param name="Files">附件</param>
        ///// <param name="SentBoxId">发件箱Id</param>
        ///// <returns></returns>
        //public string ReplyEmail(string From, string[] To, string Subject, string Html, string[] Files,string SentBoxId)
        //{
        //    RestClient client = new RestClient();
        //    client.BaseUrl = "https://api.mailgun.net/v2";
        //    client.Authenticator =
        //            new HttpBasicAuthenticator("api",
        //                                       ApiKey);
        //    RestRequest request = new RestRequest();
        //    request.AddParameter("domain",
        //                         "veryvp.co", ParameterType.UrlSegment);
        //    request.Resource = "{domain}/messages";
        //    request.AddParameter("from", From);
        //    if (To.Length < 1) return null;
        //    foreach (string str in To)
        //    {
        //        request.AddParameter("to", str);
        //        //request.AddParameter("h:Reply-To", str);
        //    }
            
              
        //    request.AddParameter("subject", Subject);
        //    request.AddParameter("html", Html);

        //    request.AddParameter("o:campaign", "bv1ug");
        //    if (Files != null)
        //    {
        //        foreach (string File in Files)
        //        {
        //            request.AddFile("attachment", Path.Combine("files", File));//发送附件
        //        }
        //    }

        //    //request.AddFile("attachment", Path.Combine("files", "D:/VeryVP/Web/Images/close_x.png"));//发送附件
        //    //request.AddParameter("o:deliverytime", Convert.ToDateTime("2014-4-11 14:37:00").GetDateTimeFormats('r')[0].ToString());
        //    request.Method = Method.POST;
            
        //    IRestResponse res = client.Execute(request);
        //    if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode)) {
        //        UpdateSentStatu(SentBoxId);
        //        return "{\"Type\":0,\"Message\":\"操作成功\"}"; 
        //    }
        //    return "{\"Type\":-1,\"Message\":\"操作失败\"}";
        //}

        /// <summary>
        /// 更新发送状态(消耗V币)
        /// </summary>
        /// <param name="SentBoxId"></param>
        public void UpdateSentStatu1(string SentBoxId, string VDetailId, string MessageId, int SentAmount)
        {
            Model.SessionUser suser = (Model.SessionUser)HttpContext.Current.Session["UserInfo"];
            SentBoxId = DBUtility.Safe.SafeReplace(SentBoxId);
            MessageId = DBUtility.Safe.SafeReplace(MessageId);
            SqlHelper.Upd("Update [iTradeEM].[dbo].[SentBox] set [SentStatu]=2,VDetailId='" + VDetailId + "',RAmount='" + SentAmount + "',Delivery=1 where UserId=" + suser.UserId + " and Id = " + SentBoxId);
            //SqlHelper.Upd("Update [iTradeEM].[dbo].[TSentBox] set [SentStatu]=2 ,Delivery=1 where UserId=" + suser.UserId + " and SentBoxId = " + SentBoxId);
        }



        /// <summary>
        /// 更新发送状态(不消耗V币)
        /// </summary>
        /// <param name="SentBoxId"></param>
        public void UpdateSentStatu(string SentBoxId, string MessageId, int SentAmount)
        {
            Model.SessionUser suser = (Model.SessionUser)HttpContext.Current.Session["UserInfo"];
            SentBoxId = DBUtility.Safe.SafeReplace(SentBoxId);
            MessageId = DBUtility.Safe.SafeReplace(MessageId);
            SqlHelper.Upd("Update [iTradeEM].[dbo].[SentBox] set [SentStatu]=2,MessageId='" + MessageId + "',RAmount='" + SentAmount + "',Delivery=1 where UserId=" + suser.UserId + " and Id = " + SentBoxId);
            //SqlHelper.Upd("Update [iTradeEM].[dbo].[TSentBox] set [SentStatu]=2,Delivery=1 where UserId=" + suser.UserId + " and SentBoxId = " + SentBoxId);
        }
        /// <summary>
        /// 更新发送失败状态
        /// </summary>
        /// <param name="SentBoxId"></param>
        public void UpdateSentFailed(string SentBoxId,IRestResponse res ,string Message)
        {
            if (res != null)
            {
                Log.InsertLogs("Error", "发送邮件", res.Content, "", " 邮件Id:" + SentBoxId);
            }
            else {
                Log.InsertLogs("Error", "发送邮件", Message);
            }
            Model.SessionUser suser = (Model.SessionUser)HttpContext.Current.Session["UserInfo"];
            SqlHelper.Upd("Update [iTradeEM].[dbo].[SentBox] set [SentStatu]=0 where UserId=" + suser.UserId + " and Id = " + DBUtility.Safe.SafeReplace(SentBoxId));
            //SqlHelper.Upd("Update [iTradeEM].[dbo].[TSentBox] set [SentStatu]=0 where UserId=" + suser.UserId + " and SentBoxId = " + DBUtility.Safe.SafeReplace(SentBoxId));
        }



        ////发送至收件人列表并且参数替换
        //public IRestResponse SendScheduledMessagelist()
        //{
        //    RestClient client = new RestClient();
        //    client.BaseUrl = "https://api.mailgun.net/v2";
        //    client.Authenticator =
        //            new HttpBasicAuthenticator("api",ApiKey);
        //    RestRequest request = new RestRequest();
        //    request.Resource = "lists";
        //    request.AddParameter("domain",
        //                    "veryvp.co", ParameterType.UrlSegment);
        //    request.Resource = "{domain}/messages";
        //    request.AddParameter("from", "wangmeng4918@gmail.com");
        //    request.AddParameter("to", "maillist201312191150345913@veryvp.co");
        //    request.AddParameter("subject", "Dear %recipient.name%!");
        //    request.AddParameter("html", "Good Afternoon ! Your name is %recipient.name%  ! Your age is %recipient.age% ");
        //    //request.AddParameter("recipient-variables", "{\"wangmeng4918@sina.com\": {\"name\":\"Bob\", \"age\":1}, \"wangmeng4918@gmail.com\": {\"name\":\"Alice\", \"age\": 2}}");
        //    request.AddParameter("o:campaign", "bv1ug");
        //    request.Method = Method.POST;
        //    return client.Execute(request);
        //}



        ////收件人列表添加成员
        //public IRestResponse AddListMember()
        //{
        //    RestClient client = new RestClient();
        //    client.BaseUrl = "https://api.mailgun.net/v2";
        //    client.Authenticator =
        //            new HttpBasicAuthenticator("api",ApiKey);
        //    RestRequest request = new RestRequest();
        //    request.Resource = "lists/{list}/members.json";
        //    request.AddParameter("list", "maillist201312191150345913@veryvp.co", ParameterType.UrlSegment);
        //    request.AddParameter("members", "[{\"name\":\"w\",\"address\": \"Alex <wangmeng4918@sina.com>\", \"vars\": {\"age\": 26}},{\"name\": \"meng\", \"address\": \"wang<wangmeng1988@gmail.com>\", \"vars\": {\"age\": 21}}]");
        //    request.AddParameter("subscribed", true);
        //    request.Method = Method.POST;
        //    return client.Execute(request);
        //}

        ////获得某个campaigns打开数
        //public string GetCampaignsOpens()
        //{
        //    RestClient client = new RestClient();
        //    client.BaseUrl = "https://api.mailgun.net/v2";
        //    client.Authenticator =
        //       new HttpBasicAuthenticator("api", ApiKey);
        //    RestRequest request = new RestRequest();
        //    request.AddParameter("domain",
        //                         "veryvp.co", ParameterType.UrlSegment);
        //    request.Resource = "/{domain}/campaigns/bv1ug/events";
        //    //request.AddParameter("groupby", "recipient");
        //    //request.Resource = "/{domain}/campaigns/bv1ug/opens";
        //    //request.AddParameter("groupby", "recipient");

        //    //request.AddParameter("limit", 2);
        //    IRestResponse res = client.Execute(request);
        //    JsonHashtable JsonHashtable1 = new JsonHashtable();
        //    Hashtable ht = new Hashtable();
        //    ht = JsonHashtable1.Decode(res.Content);

        //    return res.Content;
        //}


        /// <summary>
        /// 创建收件人列表
        /// </summary>
        /// <param name="Name">列表名称</param>
        /// <param name="QueueTaskId">Id</param>
        /// <returns></returns>
        public string CreateMailingList(string Name, int EmailId)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                    new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.Resource = "lists";
            string ListName = Name;
            request.AddParameter("address", ListName);
            //request.AddParameter("description", "Mailgun test developers list");

            request.Method = Method.POST;
            IRestResponse res = client.Execute(request);
            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                SqlHelper.Upd("update Email set FileName='" + DBUtility.Safe.SafeReplace(ListName) + "' where  Id= " + EmailId);

                return ListName;
            }
            else
            {
                if (res != null)
                {
                    Log.InsertLogs("Error", "MailGun创建收件人列表", res.Content, "");
                }
                else
                {
                    Log.InsertLogs("Error", "MailGun创建收件人列表", "MailGun创建收件人列表");
                }
            }
            return null;
        }


        /// <summary>
        /// MailGun删除收件人列表
        /// </summary>
        /// <param name="Name">列表名称</param>
        public bool DelMailingList(string Name)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                    new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.Resource = "lists/{list}";
            request.AddParameter("list", Name , ParameterType.UrlSegment);
            request.Method = Method.DELETE;

            IRestResponse res = client.Execute(request);
            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                return true;
            }
            else
            {
                if (res != null)
                {
                    Log.InsertLogs("Error", "MailGun删除收件人列表", res.Content, "");
                }
                else
                {
                    Log.InsertLogs("Error", "MailGun删除收件人列表", "MailGun删除收件人列表");
                }
                return false;
            }
        }


        /// <summary>
        /// 添加成员到收件人列表
        /// </summary>
        /// <param name="ListName">列表名称</param>
        /// <param name="dt">数据源</param>
        /// <returns></returns>
        public bool AddListMember(string ListName, long DataTaskId, int Amount)
        {
            if (Amount > 0)
            {
                int Batch = Amount / 1000;
                for (int j = 0; j <= Batch; j++)
                {
                    int Begin = j * 1000;
                    int End = (j+1)*1000;
                    if(j==Batch){
                        End = Amount;
                    }
                    string Sql = " select  top " + (End - Begin) + "  * from (Select ROW_NUMBER() OVER (ORDER BY  c.Id desc) as RowNo ,Name,Email from CustDataReport where Platform=1 and CustDataTaskId = " + DataTaskId + ") as A where RowNo>" + Begin + " and RowNo<=" + End;
                    System.Data.DataTable dt = SqlHelper.Sel(Sql);

                    if (ListName != "" && dt.Rows.Count > 0)
                    {
                        RestClient client = new RestClient();
                        client.BaseUrl = "https://api.mailgun.net/v2";
                        client.Authenticator =
                                new HttpBasicAuthenticator("api", GApiKey);
                        RestRequest request = new RestRequest();
                        request.Resource = "lists/{list}/members.json";
                        request.AddParameter("list", ListName, ParameterType.UrlSegment);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder("[");

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sb.Append("{\"vars\": {\"name\":\"" + DBUtility.Safe.safereturn(JsonCharFilter(dt.Rows[i]["Name"].ToString().Replace("&quot;", ""))) + "\"},\"address\": \"" + DBUtility.Safe.safereturn(dt.Rows[i]["Email"].ToString()) + "\"},");
                        }
                        request.AddParameter("members", sb.ToString().TrimEnd(',') + "]");
                        request.AddParameter("subscribed", true);
                        request.Method = Method.POST;

                        IRestResponse res = client.Execute(request);
                        if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
                        {
                            continue;
                        }
                        else
                        {
                            if (res != null)
                            {
                                Log.InsertLogs("Error", "MailGun上传收件人", res.Content, "");
                            }
                            else
                            {
                                Log.InsertLogs("Error", "MailGun上传收件人", "MailGun上传收件人至收件人列表");
                            }
                            return false;
                        }

                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Json特符字符过滤，参见http://www.json.org/
        /// </summary>
        /// <param name="sourceStr">要过滤的源字符串</param>
        /// <returns>返回过滤的字符串</returns>
        public string JsonCharFilter(string sourceStr)
        {
            sourceStr = sourceStr.Replace("\\", "");
            sourceStr = sourceStr.Replace("\b", "");
            sourceStr = sourceStr.Replace("\t", "");
            sourceStr = sourceStr.Replace("\n", "");
            sourceStr = sourceStr.Replace("\f", "");
            return sourceStr.Replace("\r", "");
        }

        /// <summary>
        /// 发送至收件人列表并且参数替换
        /// </summary>
        /// <param name="ListName">收件人列表名称</param>
        /// <param name="from">收件人</param>
        /// <param name="Subject">主题</param>
        /// <param name="html">正文</param>
        /// <param name="DeliveryTime">发送时间</param>
        /// <param name="CampaignId">CampaignId</param>
        /// <returns></returns>
        public bool SendScheduledMessagelist(string ListName, string from, string Subject, string html, DateTime DeliveryTime, string CampaignId, int UserId, int ParentId)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                    new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.Resource = "lists";
            request.AddParameter("domain",
                            GDomain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", from);
            request.AddParameter("to", ListName);
            request.AddParameter("subject", Subject.Replace("{收件人昵称}", "%recipient.name%"));

            long Size = 0;

            QiniuAttach(UserId, ParentId, ref request, ref html, ref Size);

            if (Size > (20 * 1024 * 1024))
            {
                return false;
            }

            request.AddParameter("html", html.Replace("{收件人昵称}", "%recipient.name%"));
            request.AddParameter("o:campaign", CampaignId);

            request.AddParameter("o:deliverytime", DeliveryTime.GetDateTimeFormats('r')[0].ToString());

            request.AddParameter("o:tracking", "yes");
            //request.AddParameter("o:tracking-opens", "yes");
            //request.AddParameter("o:tracking-clicks", "no");



            request.Method = Method.POST;
            IRestResponse res = client.Execute(request);
            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                return true;
            }
            else
            {
                if (res != null)
                {
                    Log.InsertLogs("Error", "MailGun群发邮件", res.Content, "");
                }
                else
                {
                    Log.InsertLogs("Error", "MailGun群发邮件", "MailGun群发邮件");
                }
                return false;
            }
        }

        /// <summary>
        /// 创建Campaign
        /// </summary>
        /// <param name="Name">名称</param>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        public bool CreateCampaign(string Name, string Id)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               GApiKey);
            RestRequest request = new RestRequest();
            request.Resource = "{domain}/campaigns";
            request.AddParameter("domain", GDomain,
                                 ParameterType.UrlSegment);
            request.AddParameter("name", Name);
            request.AddParameter("id", Id);
            request.Method = Method.POST;
            IRestResponse res = client.Execute(request);
            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                return true;
            }
            else
            {
                if (res != null)
                {
                    Log.InsertLogs("Error", "MailGun创建Campaign", res.Content, "");
                }
                else
                {
                    Log.InsertLogs("Error", "MailGun创建Campaign", "MailGun创建Campaign");
                }
                return false;
            }
        }

        /// <summary>
        /// 删除Campaign
        /// </summary>
        /// <param name="Id">CampaignId</param>
        public bool DelCampaign(string CampaignId)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";

            client.Authenticator =
                    new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.Resource = "{domain}/campaigns/" + CampaignId;
            request.AddParameter("domain", GDomain,
                                 ParameterType.UrlSegment);
            request.Method = Method.DELETE;
            IRestResponse res = client.Execute(request);
            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                return true;
            }
            else
            {
                if (res != null)
                {
                    Log.InsertLogs("Error", "MailGun删除Campaign", res.Content, "");
                }
                else
                {
                    Log.InsertLogs("Error", "MailGun删除Campaign", "MailGun删除Campaign");
                }
                return false;
            }
        }



        /// <summary>
        /// 获取Campaign总体发送情况
        /// </summary>
        /// <param name="Name">名称</param>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        public void UpdCampaignNum(long campaignId, long ReportId)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
               new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 GDomain, ParameterType.UrlSegment);
            request.Resource = "/{domain}/campaigns/" + campaignId;
            request.AddParameter("limit", 1);
            IRestResponse res = client.Execute(request);
            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                //return res.Content;
                Hashtable Cht = JsonHashtable1.Decode(res.Content);
                
//{"clicked_count": 0, "opened_count": 4, "submitted_count": 2, "unsubscribed_count": 0, "bounced_count": 0, "id": "cam", "name": "firstCampaign", "created_at": "Fri, 12 Dec 2014 05:54:24 GMT", 
//"delivered_count": 2, "complained_count": 0, "dropped_count": 0}

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //sb.AppendFormat("update GroupEmailReport set TotalSent={0},TotalBounced={1},Delivered={2},HardBounced={3},SoftBounced={4},TotalOpened={5},UniqueOpened={6},TotalClicked={7},UniqueClicked={8},SpamReported={9},Unsubscribed={10} where Id={11}",
                sb.AppendFormat("update GroupEmailReport set TotalBounced={1},Delivered={2},HardBounced={3},SoftBounced={4},TotalOpened={5},UniqueOpened={6},TotalClicked={7},UniqueClicked={8},SpamReported={9},Unsubscribed={10} where Id={11}",
                        //Convert.ToInt32(Cht["submitted_count"])>0?Convert.ToInt32(Cht["submitted_count"]) - 1 : 0 ,
                        "",
                        Convert.ToInt32(Cht["bounced_count"]),
                        Convert.ToInt32(Cht["delivered_count"]),0,0,
                        //response.EmailReport.HardBounced,
                        //response.EmailReport.SoftBounced,
                        Convert.ToInt32(Cht["opened_count"]),0,
                        //response.EmailReport.UniqueOpened,
                        Convert.ToInt32(Cht["clicked_count"]),0,
                        //response.EmailReport.UniqueClicked,
                        Convert.ToInt32(Cht["complained_count"]),
                        Convert.ToInt32(Cht["unsubscribed_count"]),
                        ReportId
                );
                SqlHelper.Upd(sb.ToString());
            }
            else
            {
                if (res != null)
                {
                    Log.InsertLogs("Error", "MailGun获取Campaign总体发送情况", res.Content, "");
                }
                else
                {
                    Log.InsertLogs("Error", "MailGun获取Campaign总体发送情况", "MailGun获取Campaign发送情况");
                }
            }
        }


        /// <summary>
        /// 获得某个campaign送达数
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public int UpdCampaignsDelivered(string campaignId, int page, int DataTaskId)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
               new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 GDomain, ParameterType.UrlSegment);
            request.Resource = "/{domain}/campaigns/" + campaignId + "/events";
            request.AddParameter("event", "delivered");
            request.AddParameter("limit", 100);
            request.AddParameter("page", page);

            IRestResponse res = client.Execute(request);

            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                //return res.Content;

                Hashtable DeliveredHt = JsonHashtable1.Decode(res.Content);
                Hashtable VHt = new Hashtable();
                for (int i = 0; i < DeliveredHt.Count; i++)
                {
                    VHt = (Hashtable)DeliveredHt[i];

                    //保存统计
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat("update CustDataReport set DeliveryTime={0} where  CustDataTaskId={1} and Email={2}",
                            "'" + Convert.ToDateTime(VHt["timestamp"]) + "'",
                            DataTaskId,
                            "'" + DBUtility.Safe.SafeReplace(VHt["recipient"].ToString()) + "'"
                     );
                    SqlHelper.Upd(sb.ToString());
                }
                return DeliveredHt.Count;
            }
            else
            {
                Log.InsertLogs("Error", "MailGun获取群发邮件送达数", res.Content, "");
                return -1;
            }
        }


        /// <summary>
        /// 获得某个campaign打开数
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public int UpdCampaignsOpens(string campaignId, int page,int UserId,int ParentId, int EmailId,int DataTaskId)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
               new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 GDomain, ParameterType.UrlSegment);
            request.Resource = "/{domain}/campaigns/" + campaignId + "/events";
            
            request.AddParameter("event", "opened");
            request.AddParameter("limit", 100);
            request.AddParameter("page", page);

            IRestResponse res = client.Execute(request);

            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                //return res.Content;

                Hashtable OpenHt = JsonHashtable1.Decode(res.Content);
                Hashtable VHt = new Hashtable();
                for (int i = 0; i < OpenHt.Count; i++)
                {
                    VHt = (Hashtable)OpenHt[i];
                    //保存统计
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat("insert into GroupEmailReportOpen(City,Country,Date,Email,IPAddress,Province,UserId,ParentId,EmailId,DataTaskId) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",

                          DBUtility.Safe.SafeReplace(VHt["city"].ToString()),
                          DBUtility.Safe.SafeReplace(VHt["country"].ToString()),
                          Convert.ToDateTime(VHt["timestamp"].ToString()),
                          DBUtility.Safe.SafeReplace(VHt["recipient"].ToString()),
                          DBUtility.Safe.SafeReplace(VHt["ip"].ToString()),
                          "",
                          UserId,
                          ParentId,
                          EmailId,
                          DataTaskId
                    );
                    SqlHelper.Ins(sb.ToString());


                    //保存统计
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.AppendFormat("update CustDataReport set OpenTime={0} where  CustDataTaskId={1} and Email={2}",
                            "'" + Convert.ToDateTime(VHt["timestamp"]) + "'",
                            DataTaskId,
                            "'" + DBUtility.Safe.SafeReplace(VHt["recipient"].ToString()) + "'"
                     );
                    SqlHelper.Upd(sb1.ToString());
                }
                return OpenHt.Count;
            }
            else
            {
                Log.InsertLogs("Error", "MailGun获取群发邮件打开数", res.Content, "");
                return -1;
            }
        }


        /// <summary>
        /// 获得某个campaign点击数
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public int UpdCampaignsClick(string campaignId, int page, int UserId, int ParentId, int EmailId, int DataTaskId)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
               new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 GDomain, ParameterType.UrlSegment);
            request.Resource = "/{domain}/campaigns/" + campaignId + "/events";
            
            request.AddParameter("event", "clicked");
            request.AddParameter("limit", 100);
            request.AddParameter("page", page);

            IRestResponse res = client.Execute(request);

            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                //return res.Content;

                Hashtable ClickHt = JsonHashtable1.Decode(res.Content);
                Hashtable VHt = new Hashtable();
                for (int i = 0; i < ClickHt.Count; i++)
                {
                    VHt = (Hashtable)ClickHt[i];


                    //保存统计
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat("insert into ClickDetail(City,Country,Date,Email,IPAddress,Province,URL,UserId,ParentId,EmailId,DataTaskId) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                          DBUtility.Safe.SafeReplace(VHt["city"].ToString()),
                          DBUtility.Safe.SafeReplace(VHt["country"].ToString()),
                          Convert.ToDateTime(VHt["timestamp"].ToString()),
                          DBUtility.Safe.SafeReplace(VHt["recipient"].ToString()),
                          DBUtility.Safe.SafeReplace(VHt["ip"].ToString()),
                          "",
                          DBUtility.Safe.SafeReplace(VHt["link"].ToString()),
                          UserId,
                          ParentId,
                          EmailId,
                          DataTaskId
                    );
                    SqlHelper.Ins(sb.ToString());

                    //保存统计
                    System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                    sb1.AppendFormat("update CustDataReport set ClickTime={0} where  CustDataTaskId={1} and Email={2}",
                            "'" + Convert.ToDateTime(VHt["timestamp"]) + "'",
                            DataTaskId,
                            "'" + DBUtility.Safe.SafeReplace(VHt["recipient"].ToString()) + "'"
                     );
                    SqlHelper.Upd(sb1.ToString());
                }
                return ClickHt.Count;
            }
            else
            {
                Log.InsertLogs("Error", "MailGun获取群发邮件点击数", res.Content, "");
                return -1;
            }
        }


        /// <summary>
        /// 获得某个campaign退订数
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public int UpdCampaignsUnsubscribed(string campaignId, int page,int UserId,int ParentId,string FromAdress,int EmailId, int DataTaskId)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
               new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 GDomain, ParameterType.UrlSegment);
            request.Resource = "/{domain}/campaigns/" + campaignId + "/events";
            request.AddParameter("event", "unsubscribed");
            request.AddParameter("limit", 100);
            request.AddParameter("page", page);

            IRestResponse res = client.Execute(request);

            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                //return res.Content;

                Hashtable UnsubscribedHt = JsonHashtable1.Decode(res.Content);
                Hashtable VHt = new Hashtable();

                for (int i = 0; i < UnsubscribedHt.Count; i++)
                {
                    VHt = (Hashtable)UnsubscribedHt[i];
                    FromAdress = DBUtility.Safe.SafeReplace(FromAdress);
                    string Email = DBUtility.Safe.SafeReplace(VHt["recipient"].ToString());

                    //保存统计
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat("update CustDataReport set UnsubscribedTime={0} where  CustDataTaskId={1} and Email={2}",
                            "'" + Convert.ToDateTime(VHt["timestamp"]).AddHours(8) + "'",
                            DataTaskId,
                            "'" + Email + "'"
                     );
                    SqlHelper.Upd(sb.ToString());



                    string IsUnsubscribed = SqlHelper.One("select top 1 id from Unsubscribed where UesrId=" + UserId + " SAccount='" + FromAdress + "' and Email='" + Email + "' ");
                    if (IsUnsubscribed == null)
                    {
                        //保存统计
                        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                        sb1.AppendFormat("insert into Unsubscribed(UserId,ParentId,SAccount,Email,Date,EmailId) values({0},{1},{2},{3},{4},{5}) ",
                            UserId,
                            ParentId,
                            "'" + FromAdress + "'",
                            "'" + Email + "'",
                            "'" + Convert.ToDateTime(VHt["timestamp"]).AddHours(8) + "'",
                            EmailId
                         );
                        SqlHelper.Upd(sb1.ToString());
                    }
                }
                return UnsubscribedHt.Count;

            }
            else
            {
                Log.InsertLogs("Error", "MailGun获取群发邮件退订数", res.Content, "");
                return -1;
            }
        }


        /// <summary>
        /// 获得某个campaign退回数
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public int UpdCampaignsBounced(string campaignId, int page, int DataTaskId)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
               new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 GDomain, ParameterType.UrlSegment);
            request.Resource = "/{domain}/campaigns/" + campaignId + "/events";
            request.AddParameter("event", "bounced");
            request.AddParameter("limit", 100);
            request.AddParameter("page", page);

            IRestResponse res = client.Execute(request);

            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                Hashtable Bounced = JsonHashtable1.Decode(res.Content);
                Hashtable VHt = new Hashtable();


                for (int i = 0; i < Bounced.Count; i++)
                {
                    VHt = (Hashtable)Bounced[i];
                    //保存统计
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat("update CustDataReport set BouncedTime={0},BouncedType=0 where  CustDataTaskId={1} and Email={2}",
                            "'" + Convert.ToDateTime(VHt["timestamp"]).AddHours(8) + "'",
                            DataTaskId,
                            "'" + DBUtility.Safe.SafeReplace(VHt["recipient"].ToString()) + "'"
                     );
                    SqlHelper.Upd(sb.ToString());

                }

                return Bounced.Count;

            }
            else
            {
                Log.InsertLogs("Error", "MailGun获取群发邮件退回数", res.Content, "");
                return -1;
            }
        }



        /// <summary>
        /// 获得某个campaign发送失败邮件数
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public int UpdCampaignsDropped(string campaignId, int page, int DataTaskId)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
               new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 GDomain, ParameterType.UrlSegment);
            request.Resource = "/{domain}/campaigns/" + campaignId + "/events";
            request.AddParameter("event", "dropped");
            request.AddParameter("limit", 100);
            request.AddParameter("page", page);

            IRestResponse res = client.Execute(request);

            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                //return res.Content;

                Hashtable DroppedHt = JsonHashtable1.Decode(res.Content);
                Hashtable VHt = new Hashtable();

                for (int i = 0; i < DroppedHt.Count; i++)
                {
                    VHt = (Hashtable)DroppedHt[i];

                    //保存统计
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat("update CustDataReport set FailureTime={0} where  CustDataTaskId={1} and Email={2}",
                            "'" + Convert.ToDateTime(VHt["timestamp"]).AddHours(8) + "'",
                            DataTaskId,
                            "'" + DBUtility.Safe.SafeReplace(VHt["recipient"].ToString()) + "'"
                     );
                    SqlHelper.Upd(sb.ToString());

                }
                return DroppedHt.Count;

            }
            else
            {
                Log.InsertLogs("Error", "MailGun发送失败邮件数", res.Content, "");
                return -1;
            }
        }



        /// <summary>
        /// 获得某个campaign垃圾邮件数
        /// </summary>
        /// <param name="campaignId">campaignId</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public string UpdCampaignsComplained(string campaignId, int page)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
               new HttpBasicAuthenticator("api", GApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 GDomain, ParameterType.UrlSegment);
            request.Resource = "/{domain}/campaigns/" + campaignId + "/events";
            request.AddParameter("event", "complained");
            request.AddParameter("limit", 100);
            request.AddParameter("page", page);

            IRestResponse res = client.Execute(request);

            if (System.Net.HttpStatusCode.OK.Equals(res.StatusCode))
            {
                return res.Content;
            }
            else
            {
                Log.InsertLogs("Error", "MailGun获取群发邮件垃圾邮件数", res.Content, "");
                return "[]";
            }
        }

            //client.Authenticator =
            //   new HttpBasicAuthenticator("api", NewApiKey);
            //RestRequest request = new RestRequest();
            //request.AddParameter("domain",
            //                     "veryvp.net", ParameterType.UrlSegment);

    }
}