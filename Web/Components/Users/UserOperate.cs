using System;
using System.Web;
using System.Collections;
using Common.Base;
using DBUtility;
using System.Data;
using Common.SystemCacheConfig;
using Model;
using DAL;

namespace Web.Components.Users
{

    public class UserOperate : BasePage
    {
        SqlManage SqlManage1 = new SqlManage();
        Check Check1 = new Check();
        DAL.DALUsers DALUsers1 = new DAL.DALUsers();
        Model.SessionUser SessionUser1 = new Model.SessionUser();
      
        RestSharp RestSharp1 = new RestSharp();
        Web.Components.ErrorLog ErrorLog1 = new Web.Components.ErrorLog();
       /// Comm100Transaction c100 = new Comm100Transaction();
        private string CookiesValidate = System.Configuration.ConfigurationManager.AppSettings["CookiesValidate"];

        /// <summary>
        /// 注册前的验证,获取、发送验证码
        /// </summary>
        /// <param name="phoneno">手机号</param>
        /// <returns></returns>
        public string GetAndSendRegistCode(string message)
        {
            string SharpResult = null;
            Hashtable Ht = new Hashtable();
            Ht = JsonHashtable1.EasyDecode(message);
            string phoneno = Ht["PhoneNo"].ToString();
            string option = Ht["CheckMobileType"].ToString();
            if (phoneno == "")
            {
                return "{\"Type\":-1,\"Message\":\"手机不能为空\"}";
            }
            if (!Check1.IsMobile(phoneno))
            {
                return "{\"Type\":-1,\"Message\":\"手机格式错误\"}";
            }
            phoneno = Safe.SafeReplace(phoneno);
            DataTable ta = SqlManage1.Sel("select * from Users where PhoneNo='" + phoneno + "'");
            if (ta.Rows.Count != 0)
            {
                return "{\"Type\":-1,\"Message\":\"该号码已注册\"}";
            }
            string Code = StrHelper.GetNumRandom(6);//随机生成的验证码
            //短信发送
            if (option == "SMS")
            {
                SharpResult = RestSharp1.SendSMSNew(phoneno, Code, "注册验证");

            }
            //语音发送
            if (option == "Voice")
            {
                SharpResult = RestSharp1.SendVoice(phoneno, Code, "注册验证");
            }
            if (SharpResult==null)
            {
                HttpContext.Current.Session["SharpCount"] = null;
                HttpContext.Current.Session["RegistCode"] = Code;
                HttpContext.Current.Session["RegistMobile"] = phoneno;
                phoneno = phoneno.Substring(0, 3) + "XXXX" + phoneno.Substring(7, 4);
                return "{\"Type\":0,\"Message\":\"验证码已发送至" + phoneno + "\"}";
            }
            else
            {
                return SharpResult;
            }
        }

        /// <summary>
        /// 注册前的验证,检测验证码
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public string VerificationCode(string code)
        {
            string result = SharpCount();
            if (result != null)
            {
                return result;
            }
            if (HttpContext.Current.Session["RegistCode"] == null)
            {
                return "{\"Type\":-1,\"Message\":\"验证失败\"}";
            }
            string OldCode = HttpContext.Current.Session["RegistCode"].ToString();
            if (code != OldCode)
            {
                ErrorLog1.InsertLogs("Error", "验证失败", "验证失败");
                if (CheckCode("验证失败"))
                    return "{\"Type\":-10,\"Message\":\"验证失败，请核对\"}";

                return "{\"Type\":-1,\"Message\":\"验证失败\"}";
            }
            HttpContext.Current.Session["RegistCode"] = "OK";
            HttpContext.Current.Session["SharpCount"] = null;
            return "{\"Type\":0,\"Message\":\"验证通过\"}";
        }
        /// <summary>
        /// 注册前的验证,检测验证码
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public string PhoneVerificationCode(string code,string phone)
        {
            string result = SharpCount();
            if (result != null)
            {
                return  result ;  
            }
            if (HttpContext.Current.Session["RegistCode"] == null)
            {
                return "{\"Type\":-1,\"Message\":\"验证失败\"}";
            }
            string OldCode = HttpContext.Current.Session["RegistCode"].ToString();
            if (code != OldCode)
            {
                ErrorLog1.InsertLogs("Error", "验证失败", "验证失败");

                if (CheckCode("验证失败"))
                    return "{\"Type\":-10,\"Message\":\"验证失败，请核对\"}";
                return "{\"Type\":-1,\"Message\":\"验证失败\"}";
            }
            SqlManage1.Upd("update Users set PhoneNo='" + phone + "'  where id=" + HttpContext.Current.Session["ChildId"] );
            return "{\"Type\":0,\"Message\":\"验证通过\"}";
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string RegistUser(string message)
        {
            Hashtable Ht = new Hashtable();
            Ht = JsonHashtable1.EasyDecode(message);
            string mobile = Ht["PhoneNo"].ToString();
            string username = Ht["UserName"].ToString();
            string password = Ht["Password"].ToString();
            string passwords = Ht["Passwords"].ToString();
            SessionUser1.UserId = 0;
            SessionUser1.ParentId = 0;
            HttpContext.Current.Session["UserInfo"] = SessionUser1;
            DALUsers1.Verify(Ht, 0);
            if (Check1.IsMatch(username))
            {
                return "{\"Type\":-1,\"Message\":\"帐号为至少6位数字、字母组合\"}";
            }
            if (!passwords.Equals(password))
            {
                return "{\"Type\":-1,\"Message\":\"确认密码与密码不一致\"}";
            }
            string code = HttpContext.Current.Session["RegistCode"].ToString();
            string phoneno = HttpContext.Current.Session["RegistMobile"].ToString();
            if (code != "OK")
            {
                return "{\"Type\":-1,\"Message\":\"注册失败\"}";
            }
            if (!mobile.Equals(phoneno))
            {
                return "{\"Type\":-1,\"Message\":\"注册失败\"}";
            }
           
            //System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][a-zA-Z0-9\u4e00-\u9fa5]*$");
            //return reg1.IsMatch(username);


            string Str = SqlManage1.One("select top 1 Id from Users where UserName='" + Safe.SafeReplace(username) + "' ");
            if (Str != null)
            {
                return "{\"Type\":-1,\"Message\":\"该账号已被注册\"}";
            }
            Ht["Password"] = StrHelper.EncryptPassword(password, StrHelper.PasswordType.MD5);
            Ht["UserNo"] = StrHelper.GetNumRandom(6);
            string Result = DALUsers1.Add(Ht);
            string Type = JsonHashtable1.GetNodeByKey(Result, "Type");
            if (Type != "0")
            {
                return Result;
            }
            else
            {
                string NewId = JsonHashtable1.GetNodeByKey(Result, "Id");
                SqlManage1.Upd("update Users set UserId=Id,ParentId=Id where Id=" + NewId);

                SessionUser1.UserName = username;
                SessionUser1.UserId = int.Parse(NewId);
                SessionUser1.ParentId = int.Parse(NewId);
                HttpContext.Current.Session["UserInfo"] = SessionUser1;
                HttpContext.Current.Session["LoginUserInfo"] = SessionUser1;
                
                HttpContext.Current.Session["RegistCode"] = null;
                HttpContext.Current.Session["RegistMobile"] = null;
                //
                string encrystr = "";
                if (Ht.Contains("inviteby"))
                {
                    try
                    {
                        encrystr = Common.StringHtmlJscript.CEntrypt.Decrypt(Ht["inviteby"].ToString());
                        string[] i = encrystr.Split(',');
                        string id = SqlManage1.One("select id from  Users where  userid=" + int.Parse(i[0]) );
                        if (id != null)
                        {
                            //SqlManage1.Upd(" insert inviteby( Addtime,inviteid, UserId, ParentId, DelState) values(getdate()," + int.Parse(i[1]) + "," + NewId + "," + NewId + ",0) ");
                            SqlManage1.Upd("update Users set referrer=" + id + " where Id=" + NewId);
                        }
                    }
                    catch (Exception ce) { }
                    finally { }
                }
                VmoneyInterface VmoneyInterface1 = new VmoneyInterface();
                VmoneyInterface1.DeductVmoney(0, int.Parse(NewId), 300, "注册赠送");//注册成功后赠送V币


                //string cookie = Php3Des.Encrypt3DES(Common.StringHtmlJscript.CEntrypt.Encrypt(Safe.SafeReplace(Safe.SafeReplace(IPHelper.GetIPAddress())) + "," + SessionUser1.UserId + "," + System.Web.HttpContext.Current.Session.SessionID), "MzI0OGNkemR5NzNxNjNxNzcookieency");
                //string cookie = GetEncryValue(Safe.SafeReplace(Safe.SafeReplace(IPHelper.GetIPAddress())) + "," + SessionUser1.UserId + "," + System.Web.HttpContext.Current.Session.SessionID + "," + GetLoginId());
                //CookiesHelper.WriteCookie("atlg", cookie);
               // WriteCookie("用户注册");

                return "{\"Type\":0,\"Message\":\"账户注册成功\"}";
            }
        }






        public  string RegistTaoBaoUser(string message, int taobaotype = 0)
        {


            //{ "w2_valid": "1422430544276", "sp": "icbu", "r1_valid": "1422430544276", "r2_valid": "1422430544276", "w1_valid": "1422430544276", "user_id": "17379944775", "expire_time": "1422430544276", "user_nick": "cnoriphe", "access_token": "50002401920dns9xrcYfiwAMzXEolLg213afa881mRTolzjwvcLuaohsVzJeuY" }

            //          "taobao_user_id": "3650780011","taobao_user_nick": "sandbox_veryvp2015", "token_type": "Bearer"
            string Type = "", Result = "", strEmail = "";
            Hashtable Ht = new Hashtable();
            Hashtable Htusres = new Hashtable();
            Ht = JsonHashtable1.EasyDecode(message);
            string mobile = "";
            string username = Safe.SafeReplace(Ht[(taobaotype == 1 ? "user_nick" : "taobao_user_nick")].ToString());
            string password = StrHelper.GetNumRandom(6);
            string taobaoid = Safe.SafeReplace(Ht[(taobaotype == 1 ? "user_id" : "taobao_user_id")].ToString().TrimEnd().TrimStart());
            if (Ht.Contains("Email"))
                strEmail = Ht["Email"].ToString();
            SessionUser1.UserId = 1;
            SessionUser1.ParentId = 1;
            HttpContext.Current.Session["UserInfo"] = SessionUser1;
            HttpContext.Current.Session["LoginUserInfo"] = SessionUser1;



            Htusres.Add("TaobaoID", Ht[(taobaotype == 1 ? "user_id" : "taobao_user_id")].ToString().TrimEnd().TrimStart());

            Htusres.Add("taobao_user_nick", username);












            //mobile = Safe.SafeReplace(mobile.Trim());
            //strEmail = Safe.SafeReplace(strEmail.Trim());

            int i = 0;

            string id = SqlManage1.One("select top 1 Id from Users where TaobaoID='" + taobaoid + "'");
            if (id != null)
            {
                Htusres.Add("Id", id);
                i = 1;
            }
            else { Htusres.Add("Vmoney", "300"); }


            //string a = DALUsers1.Verify(Htusres, i);
            //if (a != null)
            //{
            //    return a;
            //}






            //if (strEmail != "" || mobile != "")
            //{
            //    string e = "", p = "";
            //    if (strEmail != "")
            //        e = "  Email='" + Safe.SafeReplace(strEmail) + "'";

            //    if (mobile != "")
            //        p = "  PhoneNo='" + Safe.SafeReplace(mobile) + "'";
            //    string Str = SqlManage1.One("select top 1 Id from Users where " + e + (e != "" ? " or " : "") + p);
            //    if (Str != null)
            //    {
            //        return "{\"Type\":-1,\"Message\":\"该账号已被注册\"}";
            //    }
            //}
            Htusres["Password"] = StrHelper.EncryptPassword(password, StrHelper.PasswordType.MD5);
            Htusres["UserNo"] = StrHelper.GetNumRandom(6);




            Result = DALUsers1.AddOrUpdate(Htusres, 4, false);
            Type = JsonHashtable1.GetNodeByKey(Result, "Type");
            if (Type != "0")
            {
                return Result;
            }



            string NewId = JsonHashtable1.GetNodeByKey(Result, "Id");
            //string NewId = "1";
            SqlManage1.Upd("update Users set UserId=Id,parentid=id where Id=" + NewId);
            //SqlManage1.Ins("insert into UserVmoney(UserId,HaveId,Vmoney,Description,VMoneyfreeze,ParentId,DelState,Addtime,UseType)values(" + NewId + "," + NewId + ",300,null,0," + NewId + ",0,getdate(),0)");
            //SqlManage1.Ins("insert into VmoneyDetailNew (UserId,ParentdId,Type,Money,FromUserId,ToUserId)values(" + NewId + "," + NewId + ",'注册赠V币300',300,'1'," + NewId + "");
            //VmoneyInterface VmoneyInterface1 = new VmoneyInterface();
            //VmoneyInterface1.DeductVmoney(0, int.Parse(NewId), 300, "注册赠送");//注册成功后赠送V币

            SessionUser1.UserId = int.Parse(NewId);
            SessionUser1.ParentId = int.Parse(NewId);




            SessionUser1.UserName = username;
            SessionUser1.HeadPortrait = "";
            HttpContext.Current.Session["UserInfo"] = SessionUser1;
            HttpContext.Current.Session["LoginUserInfo"] = SessionUser1;

            //string cookie = Php3Des.Encrypt3DES(Common.StringHtmlJscript.CEntrypt.Encrypt(Safe.SafeReplace(Safe.SafeReplace(IPHelper.GetIPAddress())) + "," + SessionUser1.UserId + "," + System.Web.HttpContext.Current.Session.SessionID), "MzI0OGNkemR5NzNxNjNxNzcookieency");
            //string cookie = user.GetEncryValue(Safe.SafeReplace(Safe.SafeReplace(IPHelper.GetIPAddress())) + "," + SessionUser1.UserId + "," + System.Web.HttpContext.Current.Session.SessionID + "," + user.GetLoginId());
            //CookiesHelper.WriteCookie("atlg", cookie);
            WriteCookie("账户注册成功");
            return "{\"Type\":0,\"Account\":\"" + username + "\",\"Message\":\"账户注册成功\",\"PS\":\"" + password + "\"}";
        }





        /// <summary>
        /// 帐号登录
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string LoginUser(string message)
        {
            Hashtable Ht = new Hashtable();
            Ht = JsonHashtable1.EasyDecode(message);
            string username = Ht["UserName"].ToString();
            string pwd = StrHelper.EncryptPassword(Ht["Password"].ToString(), StrHelper.PasswordType.MD5);
            if (username == "")
            {
                return "{\"Type\":-1,\"Message\":\"请输入账号\"}";
            }

            if (username.Length > 20 || username.Length < 3)
            {
                return "{\"Type\":-1,\"Message\":\"账号长度只能在3-20位字符之间\"}";
            }
            string psw = Ht["Password"].ToString();
            if (psw == "")
            {
                return "{\"Type\":-1,\"Message\":\"请输入密码\"}";
            }
            if (psw.Length > 16 || psw.Length < 6)
            {
                return "{\"Type\":-1,\"Message\":\"密码长度只能在6-16位字符之间\"}";
            }
            username = Safe.SafeReplace(username);


            if (Check1.IsNum(username))
            {
                if (!Check1.IsMobile(username))
                    return "{\"Type\":-1,\"Message\":\"手机格式错误\"}";
            }else   if (username.IndexOf('@') != -1)
            {
                if (!Check1.IsEmail(username))
                {
                    return "{\"Type\":-1,\"Message\":\"邮箱格式错误\"}";

                }
            }else if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z\u4e00-\u9fa5][a-zA-Z0-9\u4e00-\u9fa5]*$"))
            {
                return "{\"Type\":-1,\"Message\":\"用户名只能输入中英文或者数字,第一个不能是数字,\"}";
            }

           


            //DataTable ta = SqlManage1.Sel("select top 1 * from Users where (UserName='" + username + "' or PhoneNo='" + username + "' or Email='" + username + "')");
            DataTable ta = SqlManage1.Sel("select top 1 * from Users where (UserName='" + username + "' or PhoneNo='" + username + "' )");
            if (ta.Rows.Count == 0)
            {

                ErrorLog1.InsertLogs("Error", "登录错误", "账号错误");

                if (CheckCode("登录错误"))
                    return "{\"Type\":-10,\"Message\":\"账号密码错误，请核对\"}";


                return "{\"Type\":-1,\"Message\":\"账号密码错误，请核对\"}";
            }
            else
            {
              

                string password = ta.Rows[0]["Password"].ToString();
                if (pwd != password)
                {
                    ErrorLog1.InsertLogs("Error", "登录错误", "密码错误");

                    if (CheckCode("登录错误"))
                       return "{\"Type\":-10,\"Message\":\"账号密码错误，请核对\"}";

                    return "{\"Type\":-1,\"Message\":\"账号密码错误，请核对\"}";
                }
                else
                {
                    if (ta.Rows[0]["PhoneNo"].ToString() == "")
                    {
                        HttpContext.Current.Session["ChildId"] = ta.Rows[0]["Id"].ToString();
                        return "{\"Type\":-2,\"Message\":\"帐号未验证，请进行验证\"}";
                    }
                    if (ta.Rows[0]["Status"].ToString() == "0")
                    {
                        DataTable dt = GetCompanyInfoByUserId(Convert.ToInt32(ta.Rows[0]["ParentId"]));
                        if (dt.Rows.Count > 0)
                        {
                            SessionUser1.CompanyId = Convert.ToInt32(dt.Rows[0]["Id"]);
                            SessionUser1.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                        }
                    }
                    SessionUser1.UserName = username;
                    SessionUser1.UserId = Convert.ToInt32(ta.Rows[0]["Id"]);
                    SessionUser1.UserNo = ta.Rows[0]["UserNo"].ToString();
                    //SessionUser1.TypeId = Convert.ToInt32(ta.Rows[0]["TypeId"]);
                    //SessionUser1.Email = ta.Rows[0]["Email"].ToString();
                    SessionUser1.ParentId = Convert.ToInt32(ta.Rows[0]["ParentId"]);
                    //SessionUser1.EnglishName = ta.Rows[0]["EnglishName"].ToString();
                    //SessionUser1.NickName = ta.Rows[0]["NickName"].ToString();
                    //SessionUser1.Realname = ta.Rows[0]["Realname"].ToString();
                  //  SessionUser1.Status = Convert.ToInt32(ta.Rows[0]["Status"]);
                    SessionUser1.HeadPortrait = ta.Rows[0]["HeadPortrait"].ToString();
                    HttpContext.Current.Session["UserInfo"] = SessionUser1;
                    HttpContext.Current.Session["LoginUserInfo"] = SessionUser1;
                    //登陆成功 则记住账号
                    //string RemendState = Ht["RemendUser"].ToString();
                    //if (RemendState == "on")
                    //{
                    //    Common.SystemCacheConfig.CookiesHelper.WriteCookie("VervpUserName", username);
                    //}
                    //if (RemendState == "")
                    //{
                    //    Common.SystemCacheConfig.CookiesHelper.RemoveCookie("VervpUserName");
                    //}
                    if (Session["OldUserInfo"] != null)
                    {
                        Session["OldUserInfo"] = null;
                        Session.Remove("OldUserInfo");
                    }
                    string strway = "LoginWay = null";
                    if (Ht.Contains("Way"))
                    {
                        strway = "LoginWay='" + Ht["Way"] + "'";
                        ErrorLog1.InsertLogs("Info", "登录", "手机登录成功");
                    }
                    else
                    {
                        ErrorLog1.InsertLogs("Info", "登录", "网页登录成功");
                    }
                    SqlManage1.Upd("update Users set logincount=isnull(logincount,0)+1, LatelyLogin=getdate(), " + strway + " ,LatestLoginIP='" + Safe.SafeReplace(Safe.SafeReplace(IPHelper.GetIPAddress())) + "' where id="+ SessionUser1.UserId);


                   // string cookie = Php3Des.Encrypt3DES(Common.StringHtmlJscript.CEntrypt.Encrypt(Safe.SafeReplace(Safe.SafeReplace(IPHelper.GetIPAddress())) + "," + SessionUser1.UserId + "," + System.Web.HttpContext.Current.Session.SessionID), "MzI0OGNkemR5NzNxNjNxNzcookieency");
                  //  string cookie = GetEncryValue(Safe.SafeReplace(Safe.SafeReplace(IPHelper.GetIPAddress())) + "," + SessionUser1.UserId + "," + System.Web.HttpContext.Current.Session.SessionID + "," + GetLoginId());
                  //CookiesHelper.WriteCookie("atlg", cookie);
                    WriteCookie("登录成功");
   

                    return "{\"Type\":0,\"Message\":\"登录成功\"}";
                }
            }
        }

        /// <summary>
        /// Cookie值加密，秘钥MzI0OGNkemR5NzNxNjNxNzcookieency固定位数
        /// </summary>
        public string GetEncryValue(string s)
        {
            try
            {
                string cookie = Php3Des.Encrypt3DES(Common.StringHtmlJscript.CEntrypt.Encrypt(s), "MzI0OGNkemR5NzNxNjNxNzcookieency");
                return cookie;
            }
            catch (Exception ee) { }

            return "";
        }


        /// <summary>
        /// 写入Cookie
        /// </summary>
        public void WriteCookie(string s)
        {
            try
            {
                int uid = ((SessionUser)HttpContext.Current.Session["UserInfo"]).UserId;
                string testkey = Safe.SafeReplace(Safe.SafeReplace(IPHelper.GetIPAddress())) + "," + uid + "," + System.Web.HttpContext.Current.Session.SessionID + "," +int.Parse(GetLoginId());
                string key = Safe.SafeReplace(Safe.SafeReplace(IPHelper.GetIPAddress())) + "," + Common.StringHtmlJscript.CEntrypt.EncryptUId(uid) + "," + System.Web.HttpContext.Current.Session.SessionID + "," + Common.StringHtmlJscript.CEntrypt.EncryptUId(int.Parse(GetLoginId()));
                string t = GetDecodeValue(GetEncryValue(key));
                string[] value = t.Split(',');
                value[1] = Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[1]));
                value[3] = Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[3]));
                string values = "";
                foreach (string r in value)
                {
                    values += (r + ",");
                }
                values = values.TrimEnd(',');
                SqlManage1.Upd("insert cookie([operation]      ,[encrykey]      ,[decodekey]      ,[userid]      ,[parentid]      ,[key]      ,[ip]      ,[sessionid],addtime) values('" + s + "','" + GetEncryValue(key) + "','" + values + "'," + UserId + "," + ParentId + ",'" + testkey + "','" + Safe.SafeReplace(Safe.SafeReplace(IPHelper.GetIPAddress())) + "','" + System.Web.HttpContext.Current.Session.SessionID + "',getdate())");
                string cookie = GetEncryValue(key);
                CookiesHelper.WriteCookie("atlg", cookie);
            }
            catch (Exception ee) { }
        }



        /// <summary>
        /// Cookie值解密，秘钥MzI0OGNkemR5NzNxNjNxNzcookieency固定位数
        /// </summary>
        public string GetDecodeValue(string s)
        {
            try
            {

                s = Common.StringHtmlJscript.CEntrypt.Decrypt(Php3Des.Decrypt3DES(s, "MzI0OGNkemR5NzNxNjNxNzcookieency"));

                return s;

            }
            catch (Exception d) { }

            return "";
        }
        public void cookievalue(string s)
        {
             try
            {
            string t = GetDecodeValue(s);
            string[] value = t.Split(',');
            value[1] = Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[1]));
            value[3] = Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[3]));
            string values = "";
            foreach (string r in value)
            {
                values += (r + ",");
            }
            values = values.TrimEnd(',');
            SqlManage1.Upd("insert cookie([operation]      ,[encrykey]      ,[decodekey]      ,[userid]            ,[ip]      ,[sessionid],addtime) values('超时','" + s + "','" + values + "'," + value[1] + ",'" + Safe.SafeReplace(Safe.SafeReplace(IPHelper.GetIPAddress())) + "','" + System.Web.HttpContext.Current.Session.SessionID + "',getdate())");
            }
             catch (Exception d) { }

           
        }


        public string test(string s)
        {

            if (s.Split(',').Length == 4)
            {
                try
                {
                    SessionUser su1 = new SessionUser();
                    string[] value = s.Split(',');

                    if (System.Web.HttpContext.Current.Session.SessionID != Safe.SafeReplace(value[2]))
                        return null;

                    if (Safe.SafeReplace(IPHelper.GetIPAddress()) != Safe.SafeReplace(value[0]))
                        return null;

                    int uid = int.Parse(Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[1])));
                    int loginuid = int.Parse(Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[3])));

                   
                    value[1] = Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[1]));
                    value[3] = Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[3]));
                    string values = "";
                    foreach (string r in value)
                    {
                        values += (r + ",");
                    }
                    values = values.TrimEnd(',');
                    SqlManage1.Upd("update cookie set custcookie='" + values + "',uptime=getdate() where (ip='" + Safe.SafeReplace(value[0]) + "' and userid=" + uid + " and id=(select max(id) from cookie where userid=" + uid + "))");

                }
                catch (Exception dd) { }
              

                return "";
            }
            else
                return "";

            return "";

        }

      
        public string SetSession2(string s)
        {

            if (s.Split(',').Length == 4)
            {
                SessionUser su1 = new SessionUser();
                string[] value = s.Split(',');

                if (System.Web.HttpContext.Current.Session.SessionID != Safe.SafeReplace(value[2]))
                    return null;

                if (Safe.SafeReplace(IPHelper.GetIPAddress()) != Safe.SafeReplace(value[0]))
                    return null;

                int uid = int.Parse(Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[1])));
                int loginuid = int.Parse(Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[3])));

                DataTable ta = SqlManage1.Sel("select top 1 * from Users where (LatestLoginIP='" + Safe.SafeReplace(value[0]) + "' and id=" + uid + ")");
                if (ta.Rows.Count == 0)
                    return null;


                su1.UserName = ta.Rows[0]["UserName"].ToString();
                su1.UserId = Convert.ToInt32(ta.Rows[0]["Id"]);
                su1.UserNo = ta.Rows[0]["UserNo"].ToString();
                //SessionUser1.TypeId = Convert.ToInt32(ta.Rows[0]["TypeId"]);
                //SessionUser1.Email = ta.Rows[0]["Email"].ToString();
                su1.ParentId = Convert.ToInt32(ta.Rows[0]["ParentId"]);
                //SessionUser1.EnglishName = ta.Rows[0]["EnglishName"].ToString();
                //SessionUser1.NickName = ta.Rows[0]["NickName"].ToString();
                //SessionUser1.Realname = ta.Rows[0]["Realname"].ToString();
                su1.Status = Convert.ToInt32(ta.Rows[0]["Status"]);
                su1.HeadPortrait = ta.Rows[0]["HeadPortrait"].ToString();



                if (uid != loginuid)
                {
                    SessionUser su2 = new SessionUser();
                    DataTable ta2 = SqlManage1.Sel("select top 1 * from Users where  id=" + loginuid);
                    if (ta2.Rows.Count == 0)
                    {
                        //HttpContext.Current.Session["LoginUserInfo"] = null;

                        //HttpContext.Current.Session["UserInfo"] = null;
                    }
                    else
                    {
                        su2.UserName = ta2.Rows[0]["UserName"].ToString();
                        su2.UserId = Convert.ToInt32(ta2.Rows[0]["Id"]);
                        su2.UserNo = ta2.Rows[0]["UserNo"].ToString();
                        //SessionUser1.TypeId = Convert.ToInt32(ta.Rows[0]["TypeId"]);
                        //SessionUser1.Email = ta.Rows[0]["Email"].ToString();
                        su2.ParentId = Convert.ToInt32(ta2.Rows[0]["ParentId"]);
                        //SessionUser1.EnglishName = ta.Rows[0]["EnglishName"].ToString();
                        //SessionUser1.NickName = ta.Rows[0]["NickName"].ToString();
                        //SessionUser1.Realname = ta.Rows[0]["Realname"].ToString();
                        su2.Status = Convert.ToInt32(ta2.Rows[0]["Status"]);
                        su2.HeadPortrait = ta2.Rows[0]["HeadPortrait"].ToString();
                        SqlManage1.Upd("update cookie set loginuid=" + su2.UserId + ",currentuid=" + ta.Rows[0]["Id"] + ",cookietime=getdate() where  userid=" + uid + " and id=(select max(id) from cookie where userid=" + uid + ")");

                    }
                }
                else
                {


                    SqlManage1.Upd("update cookie set loginuid=" + su1.UserId + ",currentuid=" + su1.UserId + ",cookietime=getdate() where  userid=" + uid + " and id=(select max(id) from cookie where userid=" + uid + ")");

                
                }

                return "";











               
            }
            else
                return null;

            return null;

        }

        /// <summary>
        /// Cookie转为Session
        /// </summary>
        public string  SetSession(string s)
        {

            if (s.Split(',').Length == 4)
            {
                SessionUser su1 = new SessionUser();
                string[] value = s.Split(',');
                if (CookiesValidate == "0")
                {
                    if (System.Web.HttpContext.Current.Session.SessionID != Safe.SafeReplace(value[2]))
                        return null;

                    if (Safe.SafeReplace(IPHelper.GetIPAddress()) != Safe.SafeReplace(value[0]))
                        return null;
                }
                int uid = int.Parse(Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[1])));
                int loginuid=int.Parse(Common.StringHtmlJscript.CEntrypt.DecodeUId(Safe.SafeReplace(value[3])));

                DataTable ta = null;
                if (CookiesValidate == "0")
                {

                    ta = SqlManage1.Sel("select top 1 * from Users where (LatestLoginIP='" + Safe.SafeReplace(value[0]) + "' and id=" + uid + ")");
                  
                }
                else
                {
                     ta = SqlManage1.Sel("select top 1 * from Users where   id=" + uid );
                   
                }

                if (ta.Rows.Count == 0)
                    return null;


                   su1.UserName = ta.Rows[0]["UserName"].ToString();
                   su1.UserId = Convert.ToInt32(ta.Rows[0]["Id"]);
                   su1.UserNo = ta.Rows[0]["UserNo"].ToString();
                   //SessionUser1.TypeId = Convert.ToInt32(ta.Rows[0]["TypeId"]);
                   //SessionUser1.Email = ta.Rows[0]["Email"].ToString();
                   su1.ParentId = Convert.ToInt32(ta.Rows[0]["ParentId"]);
                   //SessionUser1.EnglishName = ta.Rows[0]["EnglishName"].ToString();
                   //SessionUser1.NickName = ta.Rows[0]["NickName"].ToString();
                   //SessionUser1.Realname = ta.Rows[0]["Realname"].ToString();
                   su1.Status = Convert.ToInt32(ta.Rows[0]["Status"]);
                   su1.HeadPortrait = ta.Rows[0]["HeadPortrait"].ToString();

                   HttpContext.Current.Session["UserInfo"] = su1;


                   if (uid != loginuid)
                   {
                       SessionUser su2 = new SessionUser();
                       DataTable ta2 = SqlManage1.Sel("select top 1 * from Users where  id=" + loginuid);
                       if (ta2.Rows.Count == 0)
                       { 
                       HttpContext.Current.Session["LoginUserInfo"] = null;

                       HttpContext.Current.Session["UserInfo"] = null;
                       }
                       else
                       {
                           su2.UserName = ta2.Rows[0]["UserName"].ToString();
                           su2.UserId = Convert.ToInt32(ta2.Rows[0]["Id"]);
                           su2.UserNo = ta2.Rows[0]["UserNo"].ToString();
                           //SessionUser1.TypeId = Convert.ToInt32(ta.Rows[0]["TypeId"]);
                           //SessionUser1.Email = ta.Rows[0]["Email"].ToString();
                           su2.ParentId = Convert.ToInt32(ta2.Rows[0]["ParentId"]);
                           //SessionUser1.EnglishName = ta.Rows[0]["EnglishName"].ToString();
                           //SessionUser1.NickName = ta.Rows[0]["NickName"].ToString();
                           //SessionUser1.Realname = ta.Rows[0]["Realname"].ToString();
                           su2.Status = Convert.ToInt32(ta2.Rows[0]["Status"]);
                           su2.HeadPortrait = ta2.Rows[0]["HeadPortrait"].ToString();
                           HttpContext.Current.Session["LoginUserInfo"] = su2;
                       }
                   }
                   else
                   { HttpContext.Current.Session["LoginUserInfo"] = su1; }

                   return "";
            }
            else
                return null;

            return null;

        }


        /// <summary>
        /// 得到首页登录id
        /// </summary>
        public string GetLoginId()
        {
            SessionUser suser = (SessionUser)HttpContext.Current.Session["LoginUserInfo"];
            return suser.UserId.ToString();
        }


        /// <summary>
        /// 通过用户Id获取公司信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        private DataTable GetCompanyInfoByUserId(int userid)
        {
            DataTable ta = SqlManage1.Sel("select top 1 Id,CompanyName from CompanyInfo where UserId=" + userid);
            return ta;
        }


        /// <summary>
        /// 3次验证出错，出验证码
        /// </summary>
        public bool CheckCode(string code, string type, int i = 0, int minute=1, int times=3)
        {

            if (i == 0)
            {
                DataTable ta = SqlManage1.Sel("select id  from dbo.UserLogs  where DATEDIFF(MI, Addtime, getdate())<=" + minute + " and ip='" + Safe.SafeReplace(IPHelper.GetIPAddress()) + "' and  Operate='" + type + "'");
                if (ta.Rows.Count >= times)
                {
                    object validateNum = Session["ValidateNum"];
                    string ValidateNum = code.Trim().ToUpper();
                    if (!ValidateNum.Equals(validateNum))
                    {

                        return false;
                    }
                }

                return true;
            }
            else
            {
                object validateNum = Session["ValidateNum"];
                string ValidateNum = code.Trim().ToUpper();
                if (!ValidateNum.Equals(validateNum))
                {

                    return false;
                }
                return true;
            }
            


            return false;

        }
        /// <summary>
        /// 是否超出验证次数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckCode( string type)
        {
            DataTable ta = SqlManage1.Sel("select id  from dbo.UserLogs  where DATEDIFF(MI, Addtime, getdate())<=1 and ip='" + Safe.SafeReplace(IPHelper.GetIPAddress()) + "' and  Operate='" + type + "'");
            if (ta.Rows.Count>=3)
                return true;

           

                    
             

            return false;
        }

      
        /// <summary>
        /// 子帐号登录验证,获取、发送验证码
        /// </summary>
        /// <param name="phoneno">手机号</param>
        /// <returns></returns>
        public string GetAndSendChildLoginCode(string message)
        {
            string SharpResult = null;
            Hashtable Ht = new Hashtable();
            Ht = JsonHashtable1.EasyDecode(message);
            string phoneno = Ht["PhoneNo"].ToString();
            string option = Ht["CheckMobileType"].ToString();
            if (phoneno == "")
            {
                return "{\"Type\":-1,\"Message\":\"手机不能为空\"}";
            }

            if (!Check1.IsMobile(phoneno))
            {
                return "{\"Type\":-1,\"Message\":\"手机格式错误\"}";
            }
            phoneno = Safe.SafeReplace(phoneno);
            DataTable ta = SqlManage1.Sel("select * from Users where PhoneNo='" + phoneno + "'");
            if (ta.Rows.Count != 0)
            {
                return "{\"Type\":-1,\"Message\":\"该号码已注册\"}";
            }
            string Code = StrHelper.GetNumRandom(6);//随机生成的验证码
            //短信发送
            if (option == "SMS")
            {
                SharpResult = RestSharp1.SendSMSNew(phoneno, Code, "子账号激活");
            }
            //语音发送
            if (option == "Voice")
            {
                SharpResult = RestSharp1.SendVoice(phoneno, Code, "子账号激活");
            }
            if (SharpResult==null)
            {
                HttpContext.Current.Session["SharpCount"] = null;
                HttpContext.Current.Session["ChildLoginCode"] = Code;
                HttpContext.Current.Session["ChildLoginMobile"] = phoneno;
                phoneno = phoneno.Substring(0, 3) + "XXXX" + phoneno.Substring(7, 4);
                return "{\"Type\":0,\"Message\":\"验证码已发送至" + phoneno + "\"}";
            }
            else
            {
                return SharpResult;
            }
        }

        /// <summary>
        /// 子帐号登录验证,检测验证码
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public string ChildLoginVerificationCode(string code)
        {
            string results = SharpCount();
            if (results != null)
            {
                return results;
            }
            if (code == "")
            {
                return "{\"Type\":-1,\"Message\":\"验证码不能为空\"}";
            }

            if (HttpContext.Current.Session["ChildLoginCode"] == null)
            {
                return "{\"Type\":-1,\"Message\":\"验证失败\"}";
            }
            string OldCode = HttpContext.Current.Session["ChildLoginCode"].ToString();
            if (code != OldCode)
            {
                return "{\"Type\":-1,\"Message\":\"验证失败\"}";
            }
            HttpContext.Current.Session["ChildLoginCode"] = "OK";
            string Id = HttpContext.Current.Session["ChildId"].ToString();
            string phoneno = HttpContext.Current.Session["ChildLoginMobile"].ToString();
            if (Id != null)
            {
                int result = SqlManage1.Upd("update Users set PhoneNo='" + Safe.SafeReplace(phoneno) + "' where Id='" + Safe.SafeReplace(Id) + "'");
                if (result <= 0)
                {
                    return "{\"Type\":-1,\"Message\":\"验证未通过\"}";
                }
            }
            HttpContext.Current.Session["ChildLoginCode"] = null;
            HttpContext.Current.Session["ChildLoginMobile"] = null;
            HttpContext.Current.Session["SharpCount"] = null;
            return "{\"Type\":0,\"Message\":\"验证通过\"}";
        }

        /// <summary>
        /// 找回密码前验证帐号或手机号是否存并发送验证码
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string RetrievePasswordVerification(string message)
        {
            string SharpResult = null;
           
            Hashtable Ht = new Hashtable();
            Ht = JsonHashtable1.EasyDecode(message);
            string username = Ht["UserName"].ToString();
            string option = Ht["CheckMobileType"].ToString();
            if (username == "")
            {
                return "{\"Type\":-1,\"Message\":\"手机不能为空\"}";
            }
            username = Safe.SafeReplace(username);
            string phoneno = SqlManage1.One("select top 1 PhoneNo from Users where UserName='" + username + "' or PhoneNo='" + username + "'");
            if (phoneno == null)
            {
                return "{\"Type\":-1,\"Message\":\"手机不存在，请核对\"}";
            }
            else
            {
                string code = StrHelper.GetNumRandom(6);//随机生成的验证码
                //短信发送
                if (option == "SMS")
                {
                    SharpResult = RestSharp1.SendSMSNew(phoneno, code, "找回密码验证");
                }
                //语音发送
                if (option == "Voice")
                {
                    SharpResult = RestSharp1.SendVoice(phoneno, code, "找回密码验证");
                }
                if (SharpResult==null)
                {
                 
                    HttpContext.Current.Session["SharpCount"] = null;
                    HttpContext.Current.Session["RetrievePasswordCode"] = code;
                    HttpContext.Current.Session["RetrievePasswordMobile"] = phoneno;
                    phoneno = phoneno.Substring(0, 3) + "XXXX" + phoneno.Substring(7, 4);
                    return "{\"Type\":0,\"Message\":\"验证码已发送至" + phoneno + "\"}";
                }
                else
                {
                    return SharpResult;
                }
            }
        }

       /// <summary>
       /// 验证手机
       /// </summary>
       /// <param name="message"></param>
       /// <returns></returns>
        public string SetPhoneVerification(string message)
        {
            string SharpResult = null;

            Hashtable Ht = new Hashtable();
            Ht = JsonHashtable1.EasyDecode(message);
            string username = Ht["UserName"].ToString();
            string option = Ht["CheckMobileType"].ToString();
            if (username == "")
            {
                return "{\"Type\":-1,\"Message\":\"手机不能为空\"}";
            }
            username = Safe.SafeReplace(username);
            string phoneno = SqlManage1.One("select top 1 PhoneNo from Users where  PhoneNo='" + username + "'");
            if (phoneno != null)
            {
                return "{\"Type\":-1,\"Message\":\"手机已存在，请核对\"}";
            }
            else
            {
                string code = StrHelper.GetNumRandom(6);//随机生成的验证码
                //短信发送
                if (option == "SMS")
                {
                    SharpResult = RestSharp1.SendSMSNew(username, code, "变更手机");
                }
                //语音发送
                if (option == "Voice")
                {
                    SharpResult = RestSharp1.SendVoice(username, code, "变更手机");
                }
                if (SharpResult == null)
                {
                    HttpContext.Current.Session["SharpCount"] = null;
                    HttpContext.Current.Session["RetrievePasswordCode"] = code;
                    HttpContext.Current.Session["RetrievePasswordMobile"] = username;
                    phoneno = username.Substring(0, 3) + "XXXX" + username.Substring(7, 4);
                    return "{\"Type\":0,\"Message\":\"验证码已发送至" + phoneno + "\"}";
                }
                else
                {
                    return SharpResult;
                }
            }
        }


        /// <summary>
        /// 验证email
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string SetEmailVerification(string message)
        {
            string SharpResult = null;

            Hashtable Ht = new Hashtable();
            Ht = JsonHashtable1.EasyDecode(message);
            string username = Ht["UserName"].ToString();
            string option = Ht["CheckMobileType"].ToString();
            if (username == "")
            {
                return "{\"Type\":-1,\"Message\":\"邮箱不能为空\"}";
            }


            username = Safe.SafeReplace(username.Trim());






            if (!Check1.IsEmail(username))
            {
                return "{\"Type\":-1,\"Message\":\"邮箱格式错误\"}";

            }


            string phoneno = SqlManage1.One("select top 1 email from Users where  email='" + username + "'");
            string un = SqlManage1.One("select top 1 UserName from Users where  userid='" + UserId + "'");
            

            if (phoneno != null)
            {
                return "{\"Type\":-1,\"Message\":\"邮箱已存在，请核对\"}";
            }
            else
            {
                phoneno = username;
                string code = StrHelper.GetNumRandom(6);//随机生成的验证码

                if (option == "Email")
                {
                    string str = "";
                   if (JsonHashtable1.GetNodeByKey(str, "Type") != "0")
                   {
                       SharpResult = str;
                   }
                }
              
                if (SharpResult == null)
                {
                    ErrorLog1.InsertLogs("Info", "Email验证码发送", "验证码发送成功");
                    HttpContext.Current.Session["SharpCount"] = null;
                    HttpContext.Current.Session["RetrievePasswordCode"] = code;
                    HttpContext.Current.Session["RetrievePasswordMobile"] = phoneno;
                    phoneno = phoneno.Substring(0, 3) + "XXXX" + phoneno.Substring(7, 4);
                    return "{\"Type\":0,\"Message\":\"验证码已发送至" + phoneno + "\"}";
                }
                else
                {
                    return SharpResult;
                }
            }
        }



        /// <summary>
        /// 找回密码前的验证,检测验证码
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public string RetrievePasswordVerificationCode(string code)
        {
            string result = SharpCount();
            if (result != null)
            {
                return result;
            }
            if (code == "")
            {
                return "{\"Type\":-1,\"Message\":\"验证码不能为空\"}";
            }

            if (HttpContext.Current.Session["RetrievePasswordCode"] == null)
            {
                return "{\"Type\":-1,\"Message\":\"验证失败\"}";
            }
            string OldCode = HttpContext.Current.Session["RetrievePasswordCode"].ToString();
            if (code != OldCode)
            {
                ErrorLog1.InsertLogs("Error", "验证失败", "验证失败");

                if (CheckCode("验证失败"))
                    return "{\"Type\":-10,\"Message\":\"验证失败，请核对\"}";
                return "{\"Type\":-1,\"Message\":\"验证失败\"}";
            }
            HttpContext.Current.Session["RetrievePasswordCode"] = "OK";
            HttpContext.Current.Session["SharpCount"] = null;
            return "{\"Type\":0,\"Message\":\"验证通过\"}";
        }


   


        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string RetrievePassword(string message)
        {
            Hashtable Ht = new Hashtable();
            Ht = JsonHashtable1.EasyDecode(message);
            string password = Ht["Password"].ToString();
            string passwords = Ht["Passwords"].ToString();
            if (password == "")
            {
                return "{\"Type\":-1,\"Message\":\"新密码不能为空\"}";
            }
            if (!passwords.Equals(password))
            {
                return "{\"Type\":-1,\"Message\":\"确认密码与密码不一致\"}";
            }
            string code = HttpContext.Current.Session["RetrievePasswordCode"].ToString();
            if (code != "OK")
            {
                return "{\"Type\":-1,\"Message\":\"密码重置失败\"}";
            }
            string pwd = StrHelper.EncryptPassword(password, StrHelper.PasswordType.MD5);
            string phoneno = HttpContext.Current.Session["RetrievePasswordMobile"].ToString();
            phoneno = Safe.SafeReplace(phoneno);
            int Result = SqlManage1.Upd("update Users set Password='" + pwd + "' where PhoneNo='" + phoneno + "'");
            if (Result < 0)
            {
                return "{\"Type\":-1,\"Message\":\"请检查手机号是否存在\"}";
            }
            else
            {
                HttpContext.Current.Session["RetrievePasswordCode"] = null;
                HttpContext.Current.Session["RetrievePasswordMobile"] = null;
                return "{\"Type\":0,\"Message\":\"密码已重置\"}";
            }
        }


       /// <summary>
       /// 手机更改
       /// </summary>
       /// <param name="message"></param>
       /// <returns></returns>
        public string Phoneset(string message)
        {
            Hashtable Ht = new Hashtable();
            Ht = JsonHashtable1.EasyDecode(message);
            string ph = Ht["PhoneNo"].ToString();
            if (!Check1.IsMobile(ph))
            {
                return "{\"Type\":-1,\"Message\":\"手机格式错误\"}";
            }
            string code = HttpContext.Current.Session["RetrievePasswordCode"].ToString();
            if (code != "OK")
            {
                return "{\"Type\":-1,\"Message\":\"手机重置失败\"}";
            }
         
            string phoneno = HttpContext.Current.Session["RetrievePasswordMobile"].ToString();
            phoneno = Safe.SafeReplace(phoneno);
            int Result = SqlManage1.Upd("update Users set PhoneNo='" + phoneno + "' where userid='" + UserId + "'");
            if (Result < 0)
            {
                return "{\"Type\":-1,\"Message\":\"请检查手机号是否存在\"}";
            }
            else
            {
                HttpContext.Current.Session["RetrievePasswordCode"] = null;
                HttpContext.Current.Session["RetrievePasswordMobile"] = null;
                return "{\"Type\":0,\"Message\":\"手机已重置\"}";
            }
        }



        /// <summary>
        /// Email更改
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string Emailset(string message)
        {
            Hashtable Ht = new Hashtable();
            Ht = JsonHashtable1.EasyDecode(message);
            string email = Ht["Email"].ToString();
            if (!Check1.IsEmail(email))
            {
                return "{\"Type\":-1,\"Message\":\"邮箱错误\"}";
            }
            string code = HttpContext.Current.Session["RetrievePasswordCode"].ToString();
            if (code != "OK")
            {
                return "{\"Type\":-1,\"Message\":\"邮箱重置失败\"}";
            }

            string phoneno = HttpContext.Current.Session["RetrievePasswordMobile"].ToString();
            phoneno = Safe.SafeReplace(phoneno);
            int Result = SqlManage1.Upd("update Users set email='" + phoneno + "' where userid='" + UserId + "'");
            if (Result < 0)
            {
                return "{\"Type\":-1,\"Message\":\"请检查用户是否存在\"}";
            }
            else
            {
                HttpContext.Current.Session["RetrievePasswordCode"] = null;
                HttpContext.Current.Session["RetrievePasswordMobile"] = null;
                return "{\"Type\":0,\"Message\":\"邮箱已重置\"}";
            }
        }



        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public string LogOut()
        {
            HttpContext.Current.Session["UserInfo"] = null;
            HttpContext.Current.Session["LoginUserInfo"] = null;
            
            return "{\"Type\":0,\"Message\":\"账户已退出\"}";
        }
        /// <summary>
        /// 通过用户Id获取公司名称
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetCompanyNameByUserId(string userid)
        {
           // Helper1.Where = Helper1.WhereCreate(1);
            Helper1.Where.Add("UserId", userid);
            string CompanyName = Helper1.One("CompanyInfo", "CompanyName", Helper1.Where);
            return CompanyName;
        }

        /// <summary>
        /// 判断验证码发送次数
        /// </summary>
        /// <returns></returns>
        protected string SharpCount()
        {
            //验证码提交次数
            int SharpCount = 0;
            if (HttpContext.Current.Session["SharpCount"] != null)
            {
                SharpCount = Convert.ToInt32(HttpContext.Current.Session["SharpCount"]);
            }
            SharpCount = SharpCount + 1;
            HttpContext.Current.Session["SharpCount"] = SharpCount;
            if (SharpCount > 5)
            {
                HttpContext.Current.Session["RegistCode"] = null;
                HttpContext.Current.Session["ChildLoginCode"] = null;
                HttpContext.Current.Session["RetrievePasswordCode"] = null;
                return "{\"Type\":-1,\"Message\":\"验证码失效请重新输入!\"}";
            }
            return null;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string ChangePassword(string oldpassword, string newpassword)
        {
            oldpassword = StrHelper.EncryptPassword(oldpassword, StrHelper.PasswordType.MD5);
            string password = SqlManage1.One("select top 1 Password from Users where Id=" + UserId);
            if (!oldpassword.Equals(password))
            {
                return "{\"Type\":-1,\"Message\":\"当前密码输入错误\"}";
            }
            newpassword = StrHelper.EncryptPassword(newpassword, StrHelper.PasswordType.MD5);
            int Result = SqlManage1.Upd("update Users set Password='" + newpassword + "' where Id=" + UserId);
            if (Result < 0)
            {
                return "{\"Type\":-1,\"Message\":\"密码修改失败\"}";
            }
            else
            {
                return "{\"Type\":0,\"Message\":\"密码修改成功\"}";
            }
        }

        /// <summary>
        /// 获取当前登录用户的子帐号
        /// </summary>
        /// <param name="State">离职状态</param>
        /// <returns></returns>
        public DataTable ChildUserList()
        {
            return SqlManage1.Sel("select Id,UserName from Users where UserId<>ParentId and ParentId=" + UserId + " and Status=0");
        }

        /// <summary>
        /// 获取当前登录用户的所有公司员工
        /// </summary>
        /// <param name="State">离职状态</param>
        /// <returns></returns>
        public DataTable EmployeeList()
        {
            return SqlManage1.Sel("select Id,UserName from Users where ParentId=" + ParentId + " and UserId<>" + UserId + " and Status=0 and DelState=0");
        }

        /// <summary>
        /// 获取当前登录用户的所有信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserInfo(string ColumnName = "*")
        {
            int UserId = 0;
            Model.SessionUser suser = (Model.SessionUser)HttpContext.Current.Session["UserInfo"];
            ColumnName = Safe.SafeReplace(ColumnName);
            if (suser != null)
            {
                UserId = suser.UserId;
                return SqlManage1.Sel("select " + ColumnName + " from Users where   UserId=" + UserId + " and Status=0");
            }
            return null;
        }

    }
}