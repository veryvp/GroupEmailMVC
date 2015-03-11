using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Common.Base;
using System.Collections;
using DBUtility;

namespace Web.Components
{
    /// <summary>
    /// 云通讯平台管理  暂只实现手机验证码发送和语音验证码发送
    /// </summary>
    public class RestSharp
    {
        //设置每个IP地址每天最大发送次数
        private int MaxSendTimes = 1000;
        //设置每个手机号每个需要发送短信或语音的功能每天最多发送短信或语音次数
        private int SendTimes = 5;



        /// <summary>
        /// 配置文件key值
        /// </summary>

        public const string config_key_address = "server_address";
        public const string config_key_port = "server_port";
        public const string config_key_mainAccount = "main_account";
        public const string config_key_mainToken = "main_token";
        public const string config_key_subAccount = "sub_account";
        public const string config_key_subToken = "sub_token";
        public const string config_key_voipAccount = "voip_account";
        public const string config_key_voipPwd = "voip_password";
        public const string config_key_appId = "app_id";


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="Phone">接收信息电话</param>
        /// <param name="Message">信息内容</param>
        /// <param name="Operate">操作 注册验证:0 找回密码验证:1 子账号激活:2 子帐号关联验证:3</param>
        public string SendSMS(string Phone, string VerifyCode, string Operate)
        {
            //是否发送短信
            string IfSend = IsSend(Phone, Operate, 0);
            if (IfSend != null)
            {
                return IfSend;
            }

            // 读取配置文件信息
            string accountSid = System.Configuration.ConfigurationManager.AppSettings[config_key_mainAccount];
            string authToken = System.Configuration.ConfigurationManager.AppSettings[config_key_mainToken];
            string subAccountSid = System.Configuration.ConfigurationManager.AppSettings[config_key_subAccount];
            string appId = System.Configuration.ConfigurationManager.AppSettings[config_key_appId];

            // 包体参数默认值
            string msgType = "0";

            //json格式
            RestAPI_Json restApi = new RestAPI_Json();

             // 调用接口
            string Result = restApi.sendSMS(accountSid, authToken, subAccountSid, appId, Phone, VerifyCode, msgType);
            //string Result = "{\"statusCode\":000000}";
            int IsSuccess = 1;
            bool flag = false;
           
            JsonHashtable JsonHashtable1 = new JsonHashtable();
            string statusCode = JsonHashtable1.GetNodeByKey(Result, "statusCode");
            
            if ("000000".Equals(statusCode))
            {
                IsSuccess = 0;
                flag =  true;
            }
            WriteLog(Result, Phone, Operate, VerifyCode, "短信验证", IsSuccess);
            if (flag)
            {
                return null;
                //return "{\"Type\":0,\"Message\":\"短信验证发送成功,请注意查收!\"}";
            }
            return "{\"Type\":-1,\"Message\":\"短信验证发送失败\"}";
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="Phone">接收信息电话</param>
        /// <param name="Message">信息内容</param>
        /// <param name="Operate">操作 注册验证:0 找回密码验证:1 子账号激活:2 子帐号关联验证:3</param>
        public string SendSMSNew(string Phone, string VerifyCode, string Operate)
        {
            //是否发送短信
            string IfSend = IsSend(Phone, Operate, 0);
            if (IfSend != null)
            {
                return IfSend;
            }


            string ret = null;

            CCPRestSDK.CCPRestSDK api = new CCPRestSDK.CCPRestSDK();
            //ip格式如下，不带https://
            //            (开发) Rest URL：https://sandboxapp.cloopen.com:8883
            //(生产)  Rest URL：https://app.cloopen.com:8883
            bool isInit = api.init("app.cloopen.com", "8883");
            api.setAccount("0000000041962d8c014198ec91a50077", "d535fbb107274239a41567e0d80f6311");
            api.setAppId("0000000041962d8c014198f318de0078");



            try
            {
                if (isInit)
                {
                    string[] str = new string[2];
                    str[0] = VerifyCode;
                    str[1] = "20";
                    if (Operate == "注册验证") { Operate = "5218"; }//注册验证，调用短信模板
                    if (Operate == "找回密码验证") { Operate = "5217"; }//找回密码验证，调用短信模板
                    if (Operate == "子账号激活") { Operate = "5303"; }//子账号激活，调用短信模板
                    //if (Operate == "3") { Operate = "5218"; }//子帐号关联验证，调用短信模板
                      if (Operate == "变更手机") { Operate = "8421";
                      str[1] = "1";
                      
                      }

                    Dictionary<string, object> retData = api.SendTemplateSMS(Phone, Operate, str);//13926585334

                    ret = getDictionaryData(retData);
                }
                else
                {
                    ret = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            finally
            {

            }


            int IsSuccess = 1;
            bool flag = false;

            if (ret.IndexOf("000000")>=0)//发送成功
            {
                IsSuccess = 0;
                flag = true;
            }
            ret = Safe.SafeReplace(ret);
            if (ret.Length > 480) { ret = ret.Substring(0,480); }
            WriteLog(ret, Phone, Operate, VerifyCode, "短信验证", IsSuccess);
            if (flag)
            {
                return null;
            }
            return "{\"Type\":-1,\"Message\":\"短信验证发送失败\"}";
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="Phone">接收信息电话</param>
        /// <param name="Message">信息内容</param>
        /// <param name="Operate">操作 注册验证:0 找回密码验证:1 子账号激活:2 子帐号关联验证:3</param>
        public string SendSMSNew(string Phone, string ModelType, string[] ParameterArray)
        {
            
            //是否发送短信
            string IfSend = IsSend(Phone, ModelType, 0);
            if (IfSend != null)
            {
                return IfSend;
            }


            string ret = null;

            CCPRestSDK.CCPRestSDK api = new CCPRestSDK.CCPRestSDK();
            //ip格式如下，不带https://
            //            (开发) Rest URL：https://sandboxapp.cloopen.com:8883
            //(生产)  Rest URL：https://app.cloopen.com:8883
            bool isInit = api.init("app.cloopen.com", "8883");
            api.setAccount("0000000041962d8c014198ec91a50077", "d535fbb107274239a41567e0d80f6311");
            api.setAppId("0000000041962d8c014198f318de0078");



            try
            {
                if (isInit)
                {
                    if (ModelType == "注册验证") { ModelType = "5218"; }//注册验证，调用短信模板
                    if (ModelType == "找回密码验证") { ModelType = "5217"; }//找回密码验证，调用短信模板
                    if (ModelType == "子账号激活") { ModelType = "5303"; }//子账号激活，调用短信模板
                    if (ModelType == "APP注册") { ModelType = "5404"; }//子帐号关联验证，调用短信模板
                    Dictionary<string, object> retData = api.SendTemplateSMS(Phone, ModelType, ParameterArray);//13926585334

                    ret = getDictionaryData(retData);
                }
                else
                {
                    ret = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            finally
            {

            }


            int IsSuccess = 1;
            bool flag = false;

            if (ret.IndexOf("000000") >= 0)//发送成功
            {
                IsSuccess = 0;
                flag = true;
            }
            ret = Safe.SafeReplace(ret);
            if (ret.Length > 480) { ret = ret.Substring(0, 480); }
            WriteLog(ret, Phone, ModelType, ModelType, "短信验证", IsSuccess);
            if (flag)
            {
                return "{\"Type\":0,\"Message\":\"短信验证发送成功\"}";
            }
            return "{\"Type\":-1,\"Message\":\"短信验证发送失败\"}";
        }

        /// <summary>
        /// 语音验证
        /// </summary>
        /// <param name="Phone">被叫电话</param>
        /// <param name="VerifyCode">验证码内容，为数字和英文字母，不区分大小写，长度4-8位</param>
        /// <param name="Operate">操作 注册验证:0 找回密码验证:1 子账号激活:2 子帐号关联验证:3</param>
        public string SendVoice(string Phone, string VerifyCode, string Operate)
        {
            //是否发送语音
            string IfSend = IsSend(Phone, Operate, 1);
            if (IfSend != null)
            {
                return IfSend;
            }


            // 读取配置文件信息
            string AccountSid = System.Configuration.ConfigurationManager.AppSettings[config_key_mainAccount];
            string Token = System.Configuration.ConfigurationManager.AppSettings[config_key_mainToken];
            string voipID = System.Configuration.ConfigurationManager.AppSettings[config_key_voipAccount];
            string appId = System.Configuration.ConfigurationManager.AppSettings[config_key_appId];


            //json格式
            RestAPI_Json restApi = new RestAPI_Json();
        

            // 调用接口
           string Result = restApi.sendVoice(AccountSid, Token, voipID, appId, VerifyCode, Phone, "", "3");
            // string Result = "{\"statusCode\":000000}";
            int IsSuccess = 1;
            bool flag = false;
           
            JsonHashtable JsonHashtable1 = new JsonHashtable();
            string statusCode = JsonHashtable1.GetNodeByKey(Result, "statusCode");
            
            if ("000000".Equals(statusCode))
            {
                IsSuccess = 0;
                flag = true;
            }
            WriteLog(Result, Phone, Operate, VerifyCode, "语音验证", IsSuccess);
            if (flag) {
                return null;
                //return "{\"Type\":0,\"Message\":\"语音验证发送成功,请注意查收!\"}";
            }
            return "{\"Type\":-1,\"Message\":\"语音验证发送失败\"}";
        }


        /// <summary>
        /// 添加调用日志
        /// </summary>
        /// <param name="Result">发送结果返回</param>
        /// <param name="Phone">手机号码</param>
        /// <param name="Operate">操作 注册验证:0 找回密码验证:1 子账号激活:2 子帐号关联验证:3</param>
        /// <param name="VerifyCode">验证码</param>
        /// <param name="VerifyType">验证方式 0:短信验证  1:语音验证</param>
        /// <returns></returns>
        private string WriteLog(string Result, string Phone, string Operate, string VerifyCode, string VerifyType, int IsSuccess)
        {

            //记录调用日志
          
            DBUtility.SqlManage Helper1 = new DBUtility.SqlManage();
           
            string userid = "";
            string parentid = "";
            string ownerid = "";

            if (HttpContext.Current.Session["UserInfo"] != null)
            {
                Model.SessionUser su = (Model.SessionUser)HttpContext.Current.Session["UserInfo"];
                userid = su.UserId + "";
                parentid = su.ParentId + "";
                ownerid = su.OwnerId + "";
            }
            int operate = 0 ;
            if (Operate == "找回密码验证") {
                operate = 1;
            }
            if (Operate == "子账号激活")
            {
                operate = 2;
            }
            if (Operate == "子帐号关联验证")
            {
                operate = 3;
            }
            if (Operate == "APP注册")
            {
                operate = 4;
            }
            if (Operate == "变更手机")
            {
                operate = 5;
            }
            int verifyType = 0;
            if(VerifyType=="语音验证"){
                verifyType = 1;
            }


            return Helper1.Ins("insert into RestSharpLog(UserId,ParentId,OwnerId,Operate,PhoneNo,VerifyCode,IP,IsSuccess,Result,VerifyType) values('" + userid + "','" + parentid + "','" + ownerid + "','" + operate + "','" + Safe.SafeReplace(Phone) + "','" + Safe.SafeReplace(VerifyCode) + "','" + Safe.SafeReplace(IPHelper.GetIPAddress()) + "'," + IsSuccess + ",'" + Safe.SafeReplace(Result) + "','" + verifyType + "')");
        }

        /// <summary>
        /// 判断是否发送语音或短信
        /// </summary>
        /// <param name="Phone">手机号码</param>
        /// <param name="Operate">操作 注册验证:0 找回密码验证:1 子账号激活:2 子帐号关联验证:3</param>
        /// <param name="SendType">0:短信 1:语音</param>
        /// <returns></returns>
        public string IsSend(string Phone,string Operate,int SendType) {
            int operate = 0;
            if (Operate == "找回密码验证")
            {
                operate = 1;
            }
            if (Operate == "子账号激活")
            {
                operate = 2;
            }
            if (Operate == "子帐号关联验证")
            {
                operate = 3;
            }
            if (Operate == "APP注册")
            {
                operate = 4;
            }
            DBUtility.SqlManage Helper1 = new DBUtility.SqlManage();
            string IP = IPHelper.GetIPAddress();
            string IPSentTimes = Helper1.One("select count(Id) from dbo.RestSharpLog where datediff(d,addtime,getdate())=0 and IP= '" + Safe.SafeReplace(IP) + "' and IsSuccess=0");
            if (Convert.ToInt32(IPSentTimes) > MaxSendTimes) {
                return "{\"Type\":-2,\"Message\":\"操作太频繁,请明天再试!\"}";
            }
            string PhoneSendTimes = Helper1.One("select count(Id) from dbo.RestSharpLog where datediff(d,addtime,getdate())=0 and PhoneNo= '" + Safe.SafeReplace(Phone) + "' and Operate='" + operate + "' and VerifyType='" + SendType + "' and IsSuccess=0 ");
            if (Convert.ToInt32(PhoneSendTimes) > SendTimes)
            {
                return "{\"Type\":-2,\"Message\":\"每个手机号每天最多使用" + Operate + "发送" + SendTimes + "次短信或语音!\"}";
            }
            return null;
        }


        /// <summary>
        /// 获取当前IP
        /// </summary>
        /// <returns></returns>
        private string GetIP()
        {
            string IP = "";
            System.Net.IPAddress[] arrIPAddresses = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());
            foreach (System.Net.IPAddress ip in arrIPAddresses)
            {
                if (ip.AddressFamily.Equals(System.Net.Sockets.AddressFamily.InterNetwork))
                {
                    IP = ip.ToString();
                }
            }
            return IP;
        }


        private string getDictionaryData(Dictionary<string, object> data)
        {
            string ret = null;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    ret += item.Key.ToString() + "={";
                    ret += getDictionaryData((Dictionary<string, object>)item.Value);
                    ret += "};";
                }
                else
                {
                    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                }
            }
            return ret;
        }

    }


    public class myUntil
    {
     

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source">原内容</param>
        /// <returns>加密后内容</returns>
        public static string MD5Encrypt(string source)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(source));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        /// <summary>
        /// 设置服务器证书验证回调
        /// </summary>
        public void setCertificateValidationCallBack()
        {
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationResult;
        }

        /// <summary>
        ///  证书验证回调函数  
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cer"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool CertificateValidationResult(object obj, X509Certificate cer, X509Chain chain, System.Net.Security.SslPolicyErrors error)
        {
            return true;
        }
    }


    class RestAPI_Json
    {

        /// <summary>
        /// 服务器api版本
        /// </summary>
        const string softVer = "2013-12-26";




        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="accountSid">主账号</param>
        /// <param name="authToken">主账号令牌</param>
        /// <param name="appendCode">子账号</param>
        /// <param name="appId">应用id</param>
        /// <param name="to">接收短信电话</param>
        /// <param name="body">短信内容</param>
        /// <param name="msgType">信息类型</param>
        /// <returns>包体内容</returns>
        public string sendSMS(string accountSid, string authToken, string subAccountSid, string appId, string to, string body, string msgType)
        {
            string date = DateTime.Now.ToString("yyyyMMddhhmmss");
           
        
            // 构建URL内容
            string sigstr = myUntil.MD5Encrypt(accountSid + authToken + date);
            string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/SMS/Messages?sig={4}", System.Configuration.ConfigurationManager.AppSettings[RestSharp.config_key_address], System.Configuration.ConfigurationManager.AppSettings[RestSharp.config_key_port], softVer, accountSid, sigstr);
            
            Uri address = new Uri(uriStr);

            // 创建网络请求  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            myUntil util = new myUntil();
            util.setCertificateValidationCallBack();

            // 构建Head
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "application/json;charset=utf-8";

            Encoding myEncoding = Encoding.GetEncoding("utf-8");
            byte[] myByte = myEncoding.GetBytes(accountSid + ":" + date);
            string authStr = Convert.ToBase64String(myByte);
            request.Headers.Add("Authorization", authStr);


            // 构建Body
            StringBuilder data = new StringBuilder();
            data.Append("{");
            data.Append("\"to\":\"").Append(to).Append("\"");
            data.Append(",\"body\":\"").Append(body).Append("\"");
            data.Append(",\"msgType\":\"").Append(msgType).Append("\"");
            data.Append(",\"appId\":\"").Append(appId).Append("\"");
            data.Append(",\"subAccountSid\":\"").Append(subAccountSid).Append("\"");
            data.Append("}");
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());


            // 开始请求
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            // 获取请求
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseStr = reader.ReadToEnd();

                return responseStr;
            }
        }


        /// <summary>
        /// 语音验证
        /// </summary>
        /// <param name="AccoutSid">主账号</param>
        /// <param name="subToken">主账号令牌</param>
        /// <param name="voipAccount">VoIP账号</param>
        /// <param name="appId">应用id</param>
        /// <param name="verifyCode">验证码内容，为数字和英文字母，不区分大小写，长度4-8位</param>
        /// <param name="to">被叫电话</param>
        /// <param name="displayNum">显示主叫号码，显示权限由服务侧控制。 </param>
        /// <param name="playTimes">循环播放次数，1－3次，默认播放1次。 </param>
        /// <param name="respUrl">语音验证码状态通知回调地址，云通讯平台将向该Url地址发送呼叫结果通知。</param>
        /// <returns>包体内容</returns>
        public string sendVoice(string AccoutSid, string Token, string voipAccount, string appId, string verifyCode, string to, string displayNum, string playTimes = "3", string respUrl = "")
        {
            string date = DateTime.Now.ToString("yyyyMMddhhmmss");
            // 构建URL内容
            string sigstr = myUntil.MD5Encrypt(AccoutSid + Token + date);
            string uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/Calls/VoiceVerify?sig={4}", System.Configuration.ConfigurationManager.AppSettings[RestSharp.config_key_address],System.Configuration.ConfigurationManager.AppSettings[RestSharp.config_key_port], softVer, AccoutSid, sigstr);
            Uri address = new Uri(uriStr);


            // 创建网络请求  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            myUntil util = new myUntil();
            util.setCertificateValidationCallBack();

            // 构建Head
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "application/json;charset=utf-8";

            Encoding myEncoding = Encoding.GetEncoding("utf-8");
            byte[] myByte = myEncoding.GetBytes(AccoutSid + ":" + date);
            string authStr = Convert.ToBase64String(myByte);
            request.Headers.Add("Authorization", authStr);


            // 构建Body
            StringBuilder data = new StringBuilder();
            data.Append("{");
            data.Append("\"appId\":\"").Append(appId).Append("\"");
            data.Append(",\"to\":\"").Append(to).Append("\"");
            data.Append(",\"verifyCode\":\"").Append(verifyCode).Append("\"");
            if (displayNum != null && displayNum != "")
            {
                data.Append("\"displayNum\":\"").Append(displayNum).Append("\"");
            }
            if (playTimes != null && playTimes != "")
            {
                data.Append(",\"playTimes\":\"").Append(playTimes).Append("\"");
            }
            if (respUrl != null && respUrl != "")
            {
                data.Append(",\"respUrl\":\"").Append(respUrl).Append("\"");
            }
            data.Append("}");
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());


            // 开始请求
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            // 获取请求
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseStr = reader.ReadToEnd();

                return responseStr;
            }
        }
    }
}