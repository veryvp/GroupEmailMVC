using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Comm100.Api;
using Comm100.Api.Request;
using Comm100.Api.Response;
using System.Collections;
using System.Text;
using Common.Base;

namespace Web.Components
{
    //Comm100管理
    public class Comm100Manage : BasePage
    {
        private DBUtility.SqlManage SqlHelper = new DBUtility.SqlManage();
        private Web.Components.ErrorLog ErrorLog1 = new Web.Components.ErrorLog();
        private Common.FileStreamEncode.CodeTrans CodeTrans1 = new Common.FileStreamEncode.CodeTrans();
        private Web.Components.Base.FileDownLoad FileDownLoad1 = new Web.Components.Base.FileDownLoad();
        private GroupEmailAPICallsRecord GroupEmailAPICallsRecord1 = new GroupEmailAPICallsRecord();

        private static string Url = "http://solution.comm100.cn/emailmarketingapi";
        private static string AppKey = System.Configuration.ConfigurationManager.AppSettings["EDM_APIKEY"];
        private string FileRoot = System.Configuration.ConfigurationManager.AppSettings["FileRoot"];
        private static string Format = "xml";

        
        //返回的列表Id
        private int ListId = 0;
        //收件人Id
        private int ContactID = 0;
        //用户Id
        public int UserId = 0;

        //收件人账号
        private static string SfromAddress = "veryvp@qq.com";
        //添加联系人
        private const string AddContact = "添加联系人";
        //批量导入收件人列表
        private const string ImportContact = "批量导入收件人列表";
        //更新联系人
        private const string UpdContact = "更新联系人";
        //添加发件/回复邮箱API
        private const string AddEmailVerified = "添加发件/回复邮箱";
        //发送邮件
        private const string SendEmail = "发送邮件";
        //添加收件人列表
        private const string AddMailingList = "添加收件人列表";
        //清空收件人列表
        private const string ClearMailingList = "清空收件人列表";
        //收件人列表添加成员
        private const string AddMembersMailingList = "收件人列表添加成员";
        //更新收件人列表
        private const string UpdMailingList = "更新收件人列表";
        //添加邮件
        private const string AddEmailList = "添加邮件";
        //更新邮件
        private const string UpdEmailList = "更新邮件";
        //发送预定发送计划
        private const string ScheduleEmail = "发送预定发送计划";
        //生成Csv文件
        private const string CreateCsvFile = "生成Csv文件";

        /// <summary>
        /// 添加发件/回复邮箱
        /// </summary>
        /// <param name="Email">添加的发送回复邮箱</param>
        /// <returns></returns>
        public string EmailVerifiedAdd(string Email)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);

                EmailVerifiedRequest req = new EmailVerifiedRequest();
                req.Email = Email;

                EmailVerifiedResponse response = client.Execute(req);
                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "添加发件/回复邮箱");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", AddEmailVerified, response.Error.ErrorMessage, "", "@Email:"+ Email);
                    return response.Error.ErrorMessage;
                }
                
                return response.ErrCode;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(AddEmailVerified, ex);
                return null;
            }
        }

        /// <summary>
        /// 查询邮箱验证结果
        /// </summary>
        /// <param name="Email">需要查询验证结果的邮箱</param>
        /// <returns></returns>
        public int EmailVerifiedResult(string Email)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);

                EmailVerifiedResultRequest req = new EmailVerifiedResultRequest();
                req.Email = Email;

                EmailVerifiedResultResponse response = client.Execute(req);
                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "查询邮箱验证结果");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", "查询邮箱验证结果" , response.Error.ErrorMessage, "", "@Email:" + Email);
                    return 0;
                }
               
                return  response.Status;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(AddEmailVerified, ex);
                return 0;
            }
        }


            
        
        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <param name="Email">联系人邮箱</param>
        /// <param name="Name">联系人名称</param>
        /// <returns></returns>
        public int ContactAdd(string Email, string Name)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);

                ContactAddRequest req = new ContactAddRequest();

                req.Email = Email;
                req.FirstName = Name;//姓
                //req.LastName = Name;//名
                ContactAddResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "添加联系人");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", AddContact, response.Error.ErrorMessage);
                    return 0;
                }

                return response.Id;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(AddContact, ex);
                return 0;
            }
        }


        /// <summary>
        /// 导入联系人
        /// </summary>
        /// <param name="FileName">导入的收件人</param>
        /// <param name="ListIds">导入到的收件人列表(可不填,列表间用都好分隔)</param>
        /// <returns>导入任务的编号，此编号用于查询该任务的导入情况（具体用在contact_import_result方法中）</returns>
        public int ContactImport( string FileName,string ListIds="")
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);

                ContactImportRequest req = new ContactImportRequest();
                if (ListIds != "") {
                    req.ListIds = ListIds;
                }


                req.Contacts = new Comm100.Api.Util.FileItem(FileName);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "导入联系人");

                ContactImportResponse response = client.Execute(req);

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", ImportContact, response.Error.ErrorMessage);
                    return 0;
                }

                return response.Id;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(ImportContact, ex);
                return 0;
            }
        }   


        /// <summary>
        /// 更新联系人
        /// </summary>
        /// <param name="Email">联系人邮箱</param>
        /// <param name="Name">联系人名称</param>
        /// <returns></returns>
        public string ContactUpdate(int ContactId, string Email, string Name)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);

                ContactUpdateRequest req = new ContactUpdateRequest();
                req.ContactId = ContactId;
                req.Email = Email;
                req.FirstName = Name;//姓
                //req.LastName = Name;//名
                ContactUpdateResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "更新联系人");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", UpdContact, response.Error.ErrorMessage);
                    return response.Error.ErrorMessage;
                }

                return response.ErrCode;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(UpdContact, ex);
                return null;
            }
        }


        /// <summary>
        /// 调用API生成用户绑定的收件人列表Id 返回值为COMM100新建的收件人列表Id
        /// </summary>
        /// <returns>发送平台收件人列表Id</returns>
        public int MailingListAdd()
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);

                MailingLilistAddRequest req = new MailingLilistAddRequest();
                //生成唯一性的邮件列表NAME。
                req.Name = "MailList" + StrHelper.GetRamCode();
                req.Description = "MailLstDesc" + StrHelper.GetRamCode();
                MailingLilistAddResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "调用API生成用户绑定的收件人列表Id");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", AddMailingList, response.Error.ErrorMessage);
                    return 0;
                }

                return response.Id;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(AddMailingList, ex);
                return 0;
            }
        }

        /// <summary>
        /// 清空收件人列表
        /// </summary>
        /// <param name="Subject">邮件标题</param>
        /// <returns>清空结果</returns>
        public string MailingListClear(int ListId)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);

                MailingListClearRequest req = new MailingListClearRequest();
                req.ListId = ListId;

                MailingListClearResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "清空收件人列表");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", ClearMailingList, response.Error.ErrorMessage);
                    return response.Error.ErrorMessage;
                }

                return response.ErrCode;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(ClearMailingList, ex);
                return null;
            }
        }
        
        /// <summary>
        /// 收件人列表添加收件人
        /// </summary>
        /// <param name="ListId">收件人列表Id</param>
        /// <param name="ContactIds">成员Id</param>
        /// <returns>清空结果</returns>
        public string MailingListAddMultipleMembers(int ListId, string ContactIds)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);
                MailingListAddMultipleMembersRequest req = new MailingListAddMultipleMembersRequest();
                req.ListId = ListId;
                req.ContactIds = ContactIds;

                MailingListAddMultipleMembersResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "收件人列表添加收件人");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", ClearMailingList, response.Error.ErrorMessage);
                    return response.Error.ErrorMessage;
                }

                return response.ErrCode;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(ClearMailingList, ex);
                return null;
            }
        }



        /// <summary>
        /// 获取绑定的发送平台发送计划Id 返回值为COMM100新建的发送计划Id
        /// </summary>
        /// <param name="Subject">邮件标题</param>
        /// <param name="Body">邮件内容</param>
        /// <param name="FromEmailAddress">发件地址</param>
        /// <param name="ReplyEmailAddress">回复地址</param>
        /// <param name="FromName">发件人</param>
        /// <returns>新增加的发送计划Id</returns>
        public int EmailListAdd( string Subject, string Body,  string FromEmailAddress, string ReplyEmailAddress, string FromName)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);

                EmailAddRequest req = new EmailAddRequest();
                req.Name = "Email" + StrHelper.GetRamCode();
                req.Subject = Subject;
                req.Body = Body;
                req.FromEmailAddress = FromEmailAddress;
                req.ReplyEmailAddress = ReplyEmailAddress;
                req.FromName = FromName;
                req.IfForwardLink = true;
                req.IfHtmlBody = true;
                req.IfPrivacyPolicyLink = false;
                req.IfReminderinfor = true;
                req.IfUpdateLink = true;
                req.IfWebVersion = true;
                req.SubjectADPrefix = "163.com,sina.com";

                EmailAddResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "获取绑定的发送平台发送计划Id");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", AddEmailList, response.Error.ErrorMessage, "",  "@Subject:" + Subject + "@Body:" + Body + "@FromEmailAddress:" + FromEmailAddress + "@ReplyEmailAddress:" + ReplyEmailAddress + "@FromName:" + FromName);
                    return 0;
                }
                
                return response.Id;
                
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(AddEmailList, ex);
                return 0;
            }
        }


        /// <summary>
        /// 更新发送平台发送计划 返回值为COMM100新建的发送计划Id
        /// </summary>
        /// <param name="EmailId">需要更新的邮件Id</param>
        /// <param name="Subject">邮件标题</param>
        /// <param name="Body">邮件内容</param>
        /// <param name="FromEmailAddress">发件地址</param>
        /// <param name="ReplyEmailAddress">回复地址</param>
        /// <param name="FromName">发件人</param>
        /// <returns>修改结果</returns>
        public string EmailListUpdate(int EmailId, string Subject, string Body,string FromEmailAddress, string ReplyEmailAddress, string FromName)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);

                EmailUpdateRequest req = new EmailUpdateRequest();
                req.EmailId = EmailId;
                req.Name = "Email" + StrHelper.GetRamCode();
                req.Subject = Subject;
                req.Body = Body;
                req.FromEmailAddress = FromEmailAddress;
                req.ReplyEmailAddress = ReplyEmailAddress;
                req.FromName = FromName;
                req.IfForwardLink = true;
                req.IfHtmlBody = true;
                req.IfPrivacyPolicyLink = false;
                req.IfReminderinfor = true;
                req.IfUpdateLink = true;
                req.IfWebVersion = true;
                req.SubjectADPrefix = "163.com,sina.com";

                EmailUpdateResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "更新发送平台发送计划");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", AddEmailList, response.Error.ErrorMessage, "", "@EmailId:" + EmailId + "@Subject:" + Subject + "@Body:" + Body + "@FromEmailAddress:" + FromEmailAddress + "@ReplyEmailAddress:" + ReplyEmailAddress + "@FromName:" + FromName);
                    return response.Error.ErrorMessage;
                }

                return response.ErrCode;

            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(AddEmailList, ex);
                return null;
            }
        }


        /// <summary>
        /// 直接批量发送邮件
        /// </summary>
        /// <param name="File">生成的收件人列表Csv文件名</param>
        /// <param name="Subject">邮件标题</param>
        /// <param name="Body">邮件内容</param>
        /// <param name="FromEmailAddress">发件地址</param>
        /// <param name="ReplyEmailAddress">回复地址</param>
        /// <param name="FromName">发件人</param>
        /// <param name="ScheduleTime">发送时间(小于当前时间则立即发送)</param>
        /// <returns>发送计划Id</returns>
        public int EmailSend(string File, string Subject, string Body, string FromEmailAddress, string ReplyEmailAddress, string FromName, string ScheduleTime,int UserId,int ParentId)
        {
            try
            {

                QiniuAttach(UserId, ParentId, ref Body);

                ITopClient client = new DefaultTopClient(Url, AppKey, Format);
                VeryVP.Web.Components.CompileString CompileString1 = new VeryVP.Web.Components.CompileString();
               
                EmailSendRequest req = new EmailSendRequest();
                req.Contacts = new Comm100.Api.Util.FileItem(File);// new Comm100.Api.Util.FileItem("D:/uploads/edm2013102514372342.xlsx");

                req.Name = "Email" + StrHelper.GetRamCode();
                req.Subject = CompileString1.GetCompileString(Subject);
                req.Body = CompileString1.GetCompileString(Body);
                req.FromEmailAddress = FromEmailAddress;
                req.ReplyEmailAddress = ReplyEmailAddress;
                req.FromName = FromName;
                req.IfForwardLink = true;
                req.IfHtmlBody = true;
                req.IfPrivacyPolicyLink = false;
                req.IfReminderinfor = true;
                req.IfUpdateLink = true;
                req.IfWebVersion = true;
                req.SubjectADPrefix = "163.com,sina.com";
                req.ScheduleTime = ScheduleTime;

                EmailSendResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "直接批量发送邮件");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", SendEmail, response.Error.ErrorMessage, "", "@File" + File + "@Subject:" + Subject + "@Body:" + Body + "@FromEmailAddress:" + FromEmailAddress + "@ReplyEmailAddress:" + ReplyEmailAddress + "@FromName:" + FromName + "@ScheduleTime" + ScheduleTime);
                    return 0;
                }

                return response.Id;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(SendEmail, ex);
                return 0;
            }
        }

        /// <summary>
        /// 添加七牛附件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Html"></param>
        /// <param name="Size"></param>
        /// <param name="UserId"></param>
        /// <param name="ParentId"></param>
        private void QiniuAttach(int UserId, int ParentId, ref string Html)
        {

            System.Text.RegularExpressions.Regex regImg = new System.Text.RegularExpressions.Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?/Ajax/Att.aspx[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // 搜索匹配的字符串   
            System.Text.RegularExpressions.MatchCollection matches = regImg.Matches(Html);

            if (matches.Count > 0)
            {
                Hashtable imght = new Hashtable();
                //string STbCidstr = ""; //发件箱插入到正文附件
                //string TbCidstr = ""; //收件箱插入到正文附件
                string SEidstr = ""; //发件箱邮件Ids
                string Eidstr = ""; //收件箱邮件Ids
                string GEidstr = ""; //群发邮件Ids
                // 取得匹配项列表   
                foreach (System.Text.RegularExpressions.Match match in matches)
                {

                    string url = (match.Groups["imgUrl"].Value + "").Trim();
                    string[] Params = url.Split('=');
                    if (Params.Length == 3)
                    {
                        string aid = Params[2];
                        string t = Params[1].Substring(0, 1);
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
                    System.Data.DataTable CAttachDt = new System.Data.DataTable();
                    if (SEidstr != "")
                    {
                        CAttachDt = SqlHelper.Sel("select Id,Name,FileName,FilePath,Size,ContentId,EmailId from [iTradeEM].[dbo].[SAttachment] where UserId=" + UserId + " and Id in (" + SEidstr.Trim(',') + ")");
                        if (CAttachDt.Rows.Count > 0)
                        {
                            for (int i = 0; i < CAttachDt.Rows.Count; i++)
                            {

                                string FilePath = DBUtility.Safe.safereturn(CAttachDt.Rows[i]["FilePath"].ToString());
                                string Name = DBUtility.Safe.safereturn(CAttachDt.Rows[i]["Name"].ToString());
                                string Aid = CAttachDt.Rows[i]["Id"].ToString();
                                if (imght.Contains("1---" + Aid))
                                {
                                    FileDownLoad1.FileSyn(Name, FilePath);
                                    string FileExt = "jpeg";
                                    if (Name.IndexOf(".") > 0) {
                                        FileExt = Name.Substring(Name.IndexOf(".") + 1).ToLower();
                                        if (!FileExt.Equals("png") && !FileExt.Equals("gif") && !FileExt.Equals("jpeg") && !FileExt.Equals("jpg")) {
                                            FileExt = "jpeg";
                                        }
                                    }
                                    Html = Html.Replace("src=\"/Ajax/Att.aspx" + imght["1---" + Aid].ToString() + "\"", "src=\"data:image/" + FileExt + ";base64," + CodeTrans1.GetBase64FromImage(FileRoot + FilePath, FileExt) + "\"");
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
                                    FileDownLoad1.FileSyn(Name, FilePath);
                                    string FileExt = "jpeg";
                                    if (Name.IndexOf(".") > 0)
                                    {
                                        FileExt = Name.Substring(Name.IndexOf(".") + 1).ToLower();
                                        if (!FileExt.Equals("png") && !FileExt.Equals("gif") && !FileExt.Equals("jpeg") && !FileExt.Equals("jpg"))
                                        {
                                            FileExt = "jpeg";
                                        }
                                    }
                                    Html = Html.Replace("src=\"/Ajax/Att.aspx" + imght["0---" + Aid].ToString() + "\"", "src=\"data:image/" + FileExt + ";base64," + CodeTrans1.GetBase64FromImage(FileRoot + FilePath, FileExt) + "\"");

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
                                    FileDownLoad1.FileSyn(Name, FilePath);
                                    string FileExt = "jpeg";
                                    if (Name.IndexOf(".") > 0)
                                    {
                                        FileExt = Name.Substring(Name.IndexOf(".") + 1).ToLower();
                                        if (!FileExt.Equals("png") && !FileExt.Equals("gif") && !FileExt.Equals("jpeg") && !FileExt.Equals("jpg"))
                                        {
                                            FileExt = "jpeg";
                                        }
                                    }
                                    Html = Html.Replace("src=\"/Ajax/Att.aspx" + imght["2---" + Aid].ToString() + "\"", "src=\"data:image/" + FileExt + ";base64," + CodeTrans1.GetBase64FromImage(FileRoot + FilePath, FileExt) + "\"");

                                }

                            }
                        }
                    }
                }

            }
        }


         /// <summary>
        /// 查询发邮件的结果信息
        /// </summary>
        /// <param name="TaskId">需要查询的任务编号</param>
        /// <returns>发送计划Id</returns>
        public string EmailSendResult(int EmailTaskId, int EmailId)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);

                EmailSendResultRequest req = new EmailSendResultRequest();
                req.TaskId = EmailTaskId;

                EmailSendResultResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "查询发邮件的结果信息");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", "查询发邮件的结果信息", response.Error.ErrorMessage, "", "@EmailTaskId:" + EmailTaskId + "@EmailId:" + EmailId);
                    return response.Error.ErrorMessage;
                }
                //发送失败时记录错误信息
                string ErrorStr = "";
                if (response.Status == "Failed") {
                    ErrorStr = " ,IsError=1,ErrorMessage='" + DBUtility.Safe.SafeReplace(response.Msg) + "'";
                }

                SqlHelper.Upd("Update GroupEmailQueueTask set EmailTaskId=" + EmailTaskId + ",CEmailId=" + DBUtility.Safe.SafeReplace(response.EmailId) + ErrorStr + " where Id = " + EmailId);

                return response.Status;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs("查询发邮件的结果信息", ex);
                return null;
            }
        }

            

        /// <summary>
        /// 发送预定发送计划
        /// </summary>
        /// <param name="EmailId">邮件Id</param>
        /// <param name="ListIds">发送的收件人列表</param>
        /// <param name="ScheduleTime">发送时间(小于当前时间则立即发送)</param>
        /// <returns>发送计划Id</returns>
        public string EmailSchedule(int EmailId, string ListIds, string ScheduleTime)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);

                EmailScheduleRequest req = new EmailScheduleRequest();
                req.EmailId = EmailId;
                req.ListIds = ListIds;
                req.ScheduleTime = ScheduleTime;

                EmailScheduleResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "发送预定发送计划");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", ScheduleEmail, response.Error.ErrorMessage);
                    return response.Error.ErrorMessage;
                }

                return response.ErrCode;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(ScheduleEmail, ex);
                return null;
            }
        }


        /// <summary>
        /// 更新总体发送情况
        /// </summary>
        /// <param name="EmailId">发送计划Id</param>
        /// <returns></returns>
        public string UpdateEmailStatistic(int EmailId,long ReportId)
        {

            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);
                EmailReportGetRequest req = new EmailReportGetRequest();
                req.EmailId = EmailId;//5387;

                EmailReportGetResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "更新总体发送情况");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", "更新总体发送情况", response.Error.ErrorMessage, "", "@EmailId:" + EmailId + "@ReportId:" + ReportId);
                    return response.Error.ErrorMessage;
                }

                //保存统计
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("update GroupEmailReport set TotalSent={0},TotalBounced={1},Delivered={2},HardBounced={3},SoftBounced={4},TotalOpened={5},UniqueOpened={6},TotalClicked={7},UniqueClicked={8},SpamReported={9},Unsubscribed={10} where Id={11}",
                        response.EmailReport.TotalSent,
                        response.EmailReport.TotalBounced,
                        response.EmailReport.Delivered,
                        response.EmailReport.HardBounced,
                        response.EmailReport.SoftBounced,
                        response.EmailReport.TotalOpened,
                        response.EmailReport.UniqueOpened,
                        response.EmailReport.TotalClicked,
                        response.EmailReport.UniqueClicked,
                        response.EmailReport.SpamReported,
                        response.EmailReport.Unsubscribed,
                        ReportId
                );
                SqlHelper.Upd(sb.ToString());

                return response.ErrCode;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs("更新总体发送情况", ex);
                return null;
            }
        }



        /// <summary>
        /// 更新打开详细
        /// </summary>
        /// <param name="CEmailId">发送计划Id</param>
        /// <param name="EmailId">邮件Id</param>
        /// <param name="DataTaskId">收件人批次Id</param>
        /// <param name="UserId">用户Id</param>
        /// <param name="ParentId">用户父Id</param>
        /// <param name="Page">页码</param>
        /// <returns></returns>
        public string UpdateEmailOpen(int CEmailId, int EmailId, int DataTaskId, int UserId, int ParentId, int Page)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);
                EmailReportOpenDetailsGetRequest req = new EmailReportOpenDetailsGetRequest();

                req.EmailId = CEmailId;
                req.Start = (Page - 1) * 5000 + 1;  //返回数据的开始位置，最小值和默认值为1
                req.Limit = 5000 * Page;  //返回数量，默认为1000，上限为5000。
                EmailReportOpenDetailsGetResponse response = client.Execute(req);
                
                //GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "更新打开详细");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", "更新打开详细", response.Error.ErrorMessage, "", "@CEmailId:" + CEmailId);
                    return response.Error.ErrorMessage;
                }

                    //删除统计前数据
                    //SqlHelper.Del(" delete from GroupEmailReportOpen where UserId=" + UserId + " and EmailId =" + EmailId);
                    //SqlHelper.Del(" update GroupEmailReportOpen set DelState=1 where ReportId =" + ReportId);

                foreach (Comm100.Api.Domain.EmailReportDetail detail in response.EmailReportDetails)
                {
                    //保存统计
                    StringBuilder sb = new StringBuilder();

                    SqlHelper.Upd("update [GroupEmail].[dbo].[CustDataReport] set OpenTime='" + Convert.ToDateTime(detail.Date).AddHours(8) + "' where UserId=" + UserId + " and Email='" + DBUtility.Safe.SafeReplace(detail.Email) + "' and CustDataTaskId=" + DataTaskId);
                    //sb.Append("update [GroupEmail].[dbo].[CustDataReport] set OpenTime='" + Convert.ToDateTime(detail.Date).AddHours(8) + "' where UserId=" + UserId + " and Email='" + DBUtility.Safe.SafeReplace(detail.Email) + "' and CustDataTaskId=" + DataTaskId);

                    sb.AppendFormat("insert into GroupEmailReportOpen(City,Country,Date,Email,IPAddress,Province,UserId,ParentId,EmailId,DataTasktId) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                          DBUtility.Safe.SafeReplace(detail.City),
                          DBUtility.Safe.SafeReplace(detail.Country),
                          Convert.ToDateTime(detail.Date).AddHours(8),
                          DBUtility.Safe.SafeReplace(detail.Email),
                          DBUtility.Safe.SafeReplace(detail.IPAddress),
                          DBUtility.Safe.SafeReplace(detail.State),
                          UserId,
                          ParentId,
                          EmailId,
                          DataTaskId
                    );
                    SqlHelper.Ins(sb.ToString());
                }
                while (response.EmailReportDetails.Count == 5000)
                {
                    UpdateEmailOpen(CEmailId, EmailId, DataTaskId, UserId, ParentId, Page + 1);
                }

                return response.ErrCode;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs("更新打开详细", ex);
                return null;
            }
        }


        /// <summary>
        /// 更新送达 、退订详情
        /// </summary>
        /// <param name="CEmailId">发送计划Id</param>
        /// <param name="EmailId">邮件Id</param>
        /// <param name="DataTaskId">收件人批次Id</param>
        /// <param name="UserId">用户Id</param>
        /// <param name="SAccount">发件邮箱</param>
        /// <param name="Page">页码</param>
        /// <returns></returns>
        public string UpdateEmailSentDetail(int CEmailId, int EmailId, int DataTaskId, int UserId, string SAccount, int Page)
        {
            Hashtable ht = new Hashtable();
            try
            {

                //更新发送详情
                //ITopClient client1 = new DefaultTopClient(Url, AppKey, Format);
                //EmailReportSentDetailsGetRequest req1 = new EmailReportSentDetailsGetRequest();
                //req1.EmailId = EmailTaskId;
                //req1.Start = (Page - 1) * 5000 + 1;  //返回数据的开始位置，最小值和默认值为1
                //req1.Limit = 5000 * Page;  //返回数量，默认为1000，上限为5000。

                //EmailReportSentDetailsGetResponse response1 = client1.Execute(req1);

                //GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "更新发送详情");

                //if (response1.Error.ErrorCode != 0)
                //{
                //    ErrorLog1.InsertLogs("Error", "更新发送详情", response1.Error.ErrorMessage, "", "@EmailTaskId:" + EmailTaskId );
                //    return response1.Error.ErrorMessage;
                //}
           

                //foreach (Comm100.Api.Domain.ReportDetail detail in response1.ReportDetails)
                //{
                //    Hashtable DetailHt = new Hashtable();
                //    DetailHt.Add("SendTime", detail.Date);
                //    ht.Add(detail.Email, DetailHt);
                //}


                //更新送达
                ITopClient client2 = new DefaultTopClient(Url, AppKey, Format);
                EmailReportDeliveredDetailsGetRequest req2 = new EmailReportDeliveredDetailsGetRequest();
                req2.EmailId = CEmailId;
                req2.Start = (Page - 1) * 5000 + 1;  //返回数据的开始位置，最小值和默认值为1
                req2.Limit = 5000 * Page;  //返回数量，默认为1000，上限为5000。

                EmailReportDeliveredDetailsGetResponse response2 = client2.Execute(req2);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "更新送达详情");

                if (response2.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", "更新送达详情", response2.Error.ErrorMessage, "", "@CEmailId:" + CEmailId);
                    return response2.Error.ErrorMessage;
                }


                foreach (Comm100.Api.Domain.ReportDetail detail in response2.ReportDetails)
                {
                    Hashtable DetailHt = new Hashtable();
                    DetailHt.Add("DeliveredTime", detail.Date);
                    ht.Add(detail.Email, DetailHt);
                }
                
                //更新退订
                ITopClient client3 = new DefaultTopClient(Url, AppKey, Format);
                EmailReportUnsubscribedDetailsGetRequest req3 = new EmailReportUnsubscribedDetailsGetRequest();
                req3.EmailId = CEmailId;
                req3.Start = (Page - 1) * 5000 + 1;  //返回数据的开始位置，最小值和默认值为1
                req3.Limit = 5000 * Page;  //返回数量，默认为1000，上限为5000。
                EmailReportUnsubscribedDetailsGetResponse response3 = client3.Execute(req3);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "更新退订详情");

                if (response3.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", "更新退订详情", response3.Error.ErrorMessage, "", "@CEmailId:" + CEmailId);
                    return response3.Error.ErrorMessage;
                }

                foreach (Comm100.Api.Domain.ReportDetail detail in response3.ReportDetails)
                {
                    Hashtable DetailHt = (Hashtable)ht[detail.Email];
                    DetailHt.Add("UnsubscribedTime", detail.Date);
                    string Email = DBUtility.Safe.SafeReplace(detail.Email);
                    SAccount = DBUtility.Safe.SafeReplace(SAccount);

                    string IsUnsubscribed = SqlHelper.One("select top 1 id from Unsubscribed where UesrId=" + UserId + " SAccount='" + SAccount + "' and Email='" + Email + "' ");
                    if (IsUnsubscribed == null)
                    {
                        //记录退订的邮件信息
                        StringBuilder SbUnsubscribedList = new StringBuilder();

                        SbUnsubscribedList.AppendFormat("insert into Unsubscribed(Date,SAccount,Email,UserId,ParentId,EmailId) values('{0}','{1}','{2}','{3}','{4}','{5}')",
                                Convert.ToDateTime(detail.Date).AddHours(8),
                                SAccount,
                                Email,
                                UserId,
                                ParentId,
                                EmailId
                         );
                        SqlHelper.Ins(SbUnsubscribedList.ToString());
                    }
                }

                //更新送达 、退订详情
                foreach(string key in ht.Keys){
                    Hashtable Detailht = (Hashtable)ht[key];
                    //保存统计
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("update CustDataReport set DeliveredTime={0},UnsubscribedTime={1} where UserId={2} and DataTaskId={3} and Email={4}",
                        Detailht.ContainsKey("DeliveredTime") ? ("'" + Convert.ToDateTime(Detailht["DeliveredTime"]).AddHours(8) + "'").ToString() : "null",
                        Detailht.ContainsKey("UnsubscribedTime") ? ("'" + Convert.ToDateTime(Detailht["UnsubscribedTime"]).AddHours(8) + "'").ToString() : "null",
                        UserId,
                        DataTaskId,
                        DBUtility.Safe.SafeReplace(key)
                    );
                    SqlHelper.Ins(sb.ToString());
                }
                while (response2.ReportDetails.Count == 5000) {
                    UpdateEmailSentDetail(CEmailId, EmailId, DataTaskId, UserId, SAccount, Page + 1);
                }

                return response2.ErrCode;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs("更新发送、送达 、退订详情", ex);
                return null;
            }
        }



        /// <summary>
        /// 更新点击详细
        /// </summary>
        /// <param name="CEmailId">发送计划Id</param>
        /// <param name="EmailId">邮件Id</param>
        /// <param name="DataTaskId">收件人批次Id</param>
        /// <param name="UserId">用户Id</param>
        /// <param name="ParentId">父Id</param>
        /// <param name="Page">页码</param>
        /// <returns></returns>
        public string UpdateEmailClick(int CEmailId, int EmailId, int DataTaskId, int UserId, int ParentId, int Page)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);
                EmailReportClickDetailsGetRequest req = new EmailReportClickDetailsGetRequest();

                req.EmailId = CEmailId;
                req.Start = (Page - 1) * 5000 + 1;  //返回数据的开始位置，最小值和默认值为1
                req.Limit = 5000 * Page;  //返回数量，默认为1000，上限为5000。
                EmailReportClickDetailsGetResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "更新点击详细");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", "更新点击详细", response.Error.ErrorMessage, "", "@CEmailId:" + CEmailId);
                    return response.Error.ErrorMessage;
                }

                //删除统计前数据
                //SqlHelper.Del(" delete from GroupEmailReportClickDetail where  UserId=" + UserId + " and  ReportId =" + ReportId);

                foreach (Comm100.Api.Domain.EmailReportClickDetail detail in response.EmailReportClickDetails)
                {
                    //保存统计
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("insert into GroupEmailReportClickDetail(City,Country,Date,Email,IPAddress,Province,URL,UserId,ParentId,EmailId,DataTaskId) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                          DBUtility.Safe.SafeReplace(detail.City),
                          DBUtility.Safe.SafeReplace(detail.Country),
                          Convert.ToDateTime(detail.Date).AddHours(8),
                          DBUtility.Safe.SafeReplace(detail.Email),
                          DBUtility.Safe.SafeReplace(detail.IPAddress),
                          DBUtility.Safe.SafeReplace(detail.State),
                          DBUtility.Safe.SafeReplace(detail.Url),
                          UserId,
                          ParentId,
                          EmailId,
                          DataTaskId
                    );
                    SqlHelper.Ins(sb.ToString());
                }
                while(response.EmailReportClickDetails.Count==5000){
                    UpdateEmailClick(CEmailId, EmailId, DataTaskId, UserId, ParentId, Page + 1);
                }

                return response.ErrCode;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs("更新点击详细", ex);
                return null;
            }
        }



        /// <summary>
        /// 更新退回详细
        /// </summary>
        /// <param name="CEmailId">发送计划Id</param>
        /// <param name="EmailId">邮件Id</param>
        /// <param name="DataTaskId">收件人批次Id</param>
        /// <param name="UserId">用户Id</param>
        /// <param name="Page">页码</param>
        /// <returns></returns>
        public string UpdateEmailBounced(int CEmailId, int DataTaskId, int UserId, int Page)
        {
            try
            {
                ITopClient client = new DefaultTopClient(Url, AppKey, Format);
                EmailReportBouncedDetailsGetRequest req = new EmailReportBouncedDetailsGetRequest();

                req.EmailId = CEmailId;
                req.Start = (Page - 1) * 5000 + 1;  //返回数据的开始位置，最小值和默认值为1
                req.Limit = 5000 * Page;  //返回数量，默认为1000，上限为5000。
                EmailReportBouncedDetailsGetResponse response = client.Execute(req);

                GroupEmailAPICallsRecord1.RecordAPI(1, "Comm100", "更新退回详细");

                if (response.Error.ErrorCode != 0)
                {
                    ErrorLog1.InsertLogs("Error", "更新退回详细", response.Error.ErrorMessage, "", "@CEmailId:" + CEmailId);
                    return response.Error.ErrorMessage;
                }

                foreach (Comm100.Api.Domain.BouncedDetail detail in response.BouncedDetails)
                {
                    int BouncedType = 3;
                    if (detail.BouncedType == "软退回") { 
                        BouncedType = 2;
                    }
                    if(detail.BouncedType == "硬退回"){
                        BouncedType = 1;
                    }
                    //保存统计
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("update CustDataReport set FailureTime={0},BouncedTime={0},BouncedType={1} where UserId={2} and DataTaskId={4} and Email={3} ",
                          "'" + Convert.ToDateTime(detail.Date).AddHours(8) + "'",
                          BouncedType,
                          UserId,
                          DataTaskId,
                          "'" + DBUtility.Safe.SafeReplace(detail.Email) + "'"
                    );
                    SqlHelper.Ins(sb.ToString());
                }
                while (response.BouncedDetails.Count == 5000) {
                    UpdateEmailBounced(CEmailId, DataTaskId, UserId, Page);
                }

                return response.ErrCode;
            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs("更新退回详细", ex);
                return null;
            }
        }




        /// <summary>   
        /// 根据QueueTaskId生成收件人文件
        /// </summary>
        /// <param name="QueueTaskId">邮件队列Id</param>
        public string CreateCsv(int DataTaskId)
        {

            //生成文件名
            string FileName = FileRoot + "/groupemailcsv/" + "edm" + StrHelper.GetRamCode() + DataTaskId + ".csv";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //定义表头
            sb.AppendLine("邮箱,名");
            try
            {

                string Sql = "Select Name,Email from [GroupEmail].[dbo].[CustDataReport]  where Platform=0 and CustDataTaskId = " + DataTaskId;
                System.Data.DataTable dt = SqlHelper.Sel(Sql);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sb.AppendLine(DBUtility.Safe.safereturn(dt.Rows[i]["Email"].ToString()) + "," + DBUtility.Safe.safereturn(dt.Rows[i]["Name"].ToString()));
                    }
                }
                else {
                    return null;
                }
                bool IsSeccess = Common.Office.CsvHelper.DtToCsv(sb.ToString(), FileName);
                if (IsSeccess)
                {
                    return FileName;
                }


            }
            catch (Exception ex)
            {
                ErrorLog1.InsertLogs(CreateCsvFile, ex);
            }

            return null;

        }







    }
}