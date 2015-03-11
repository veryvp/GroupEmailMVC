using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using CodeScales.Http;
using CodeScales.Http.Entity;
using CodeScales.Http.Entity.Mime;
using CodeScales.Http.Methods;
using System.Net;
using System.Web;
using System.Xml;
using System.Diagnostics;
using System.Configuration;

namespace VeryVP.Components
{
    public class SendCloudMail
    {
        //private static string ApiUser =  ConfigurationManager.AppSettings["SendCloudUser"];
        //private static string ApiKey =  ConfigurationManager.AppSettings["SendCloudKey"];
        
        /// <summary>
        /// 发送邮件
        /// <param name="From">发件地址</param>
        /// <param name="FromName">发件人</param>
        /// <param name="To">收件人地址</param>
        /// <param name="Subject">主题</param>
        /// <param name="Body">正文</param>
        ///  </summary>
        public void WebAPISendMail(string From, string FromName, string To, string Subject, string Body)
        {
            HttpClient Client = new HttpClient();
            HttpPost PostMethod = new HttpPost(new Uri("http://sendcloud.sohu.com/webapi/mail.send.xml"));

            MultipartEntity MultipartEntity = new MultipartEntity();
            PostMethod.Entity = MultipartEntity;

            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_user", "postmaster@action.sendcloud.org"));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_key", "Wnp5CSalW02D388R"));

            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "use_maillist", "false "));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "from", From));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "fromname", FromName));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "to", To));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", Subject));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "html", Body));


            CodeScales.Http.Methods.HttpResponse Response = Client.Execute(PostMethod);
            string aa = EntityUtils.ToString(Response.Entity);
        }


        /// <summary>
        /// 发送邮件模板
        /// <param name="From">发件地址</param>
        /// <param name="FromName">发件人</param>
        /// <param name="To">收件人地址</param>
        /// <param name="Subject">主题</param>
        /// <param name="Body">正文</param>
        ///  </summary>
        public void WebAPISendTemplate(string From, string FromName, string To, string Subject)
        {
            HttpClient Client = new HttpClient();
            HttpPost PostMethod = new HttpPost(new Uri("http://sendcloud.sohu.com/webapi/mail.send_template.xml"));

            MultipartEntity MultipartEntity = new MultipartEntity();
            PostMethod.Entity = MultipartEntity;

            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_user", "postmaster@action.sendcloud.org"));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "api_key", "Wnp5CSalW02D388R"));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "template_invoke_name", "test_template"));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "substitution_vars", "{\"to\": [\"525389685@qq.com\", \"13480131350@163.com\"], \"sub\" : { \"%name%\" : [\"约翰\", \"林肯\"], \"%money%\" : [\"1000\", \"200\"]} }"));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "use_maillist", "false "));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "subject", Subject));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "from", From));
            MultipartEntity.AddBody(new StringBody(Encoding.UTF8, "fromname", FromName));

            CodeScales.Http.Methods.HttpResponse Response = Client.Execute(PostMethod);
            string aa = EntityUtils.ToString(Response.Entity);
        }
        



        /// <summary>
        /// 注册获取正文
        /// <param name="UserName">用户名</param>
        /// <param name="PasswordMing">明文密码</param>
        ///  </summary>
        public string GetRegistBody(string UserName, string PasswordMing)
        {

            string SBody = "尊敬的客户：</b>";
            SBody += "<p>  您好！非常感谢您的注册。</p>";
            SBody += "<p>  您的会员登录信息如下：</p>";
            SBody += "<p>" + "账号：" + UserName + "</p>";
            SBody += "<p>" + "密码：" + PasswordMing + "</p>";
            SBody += "<p>  如果您已有UKey，可以在设置“子账号”的同时进行绑定。绑定UKey后，仅能通过UKey打开链接并登陆。单个账号可以绑定多个UKey。</p>";
            SBody += "<p>  优诗力科技</p>";
            return SBody;

        }


        /// <summary>
        /// 找回密码获取正文
        /// <param name="UserName">用户名</param>
        /// <param name="PasswordMing">明文密码</param>
        ///  </summary>
        public string GetRetirePswBody(string UserName, string PasswordMing)
        {

            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            url = url.Substring(0, url.LastIndexOf("/")) + "/changepsw.aspx?username=" + UserName + "&password=" + StrHelper.EncryptPassword(PasswordMing, StrHelper.PasswordType.MD5);
            string sBody = "尊敬的客户：</br>";
            sBody += "<p>  您的会员登录信息如下：</p>";
            sBody += "<p>" + "  账号：" + UserName + "</p>";
            sBody += "<p>" + "  您可以点击链接<a href='" + url + "'>" + url + "</a>重新设定密码</p>";
            sBody += "<p>  优诗力科技</p>";
            return sBody;

        }

     }

    
}