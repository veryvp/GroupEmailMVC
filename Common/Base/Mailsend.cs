using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Web.Mail;
namespace Common
{
    public static class Mailsend
    {

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to">收件人邮件地址</param>
        /// <param name="from">发件人邮件地址</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="username">登录smtp主机时用到的用户名,注意是邮件地址'@'以前的部分</param>
        /// <param name="password">登录smtp主机时用到的用户密码</param>
        /// <param name="smtpHost">发送邮件用到的smtp主机</param>
        public static int Send(string to, string from, string subject, string body, string userName, string password, string smtpHost ,int port)
        {

            int ret = 0;
            MailAddress from1 = new MailAddress(from);
            MailAddress to1 = new MailAddress(to);
             
            System.Net.Mail.MailMessage message1 = new System.Net.Mail.MailMessage();
            message1.From = from1;
            message1.To.Add(to);
            message1.Subject = subject;//设置邮件主题
            message1.IsBodyHtml = true; //MailFormat.HTML;//设置邮件正文为html格式 
            message1.Body = body;//设置邮件内容
            message1.BodyEncoding = Encoding.UTF8;
            message1.Priority =System.Net.Mail.MailPriority.High; 
            SmtpClient client = new SmtpClient();

            client.Credentials = new NetworkCredential(userName, password);
            client.Port = 25;
            client.Host = smtpHost;
            client.EnableSsl = false;

            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.Host = "smtp.163.com";
            //client.Credentials = new System.Net.NetworkCredential("mailforljf", "ljfblog");
        
            //设置发送邮件身份验证方式
            //注意如果发件人地址是abc@def.com，则用户名是abc而不是abc@def.com
            try
            {
                client.Send(message1);
                ret = 1;
                return ret;
            }
            catch {
                return ret;
            }

            
        }


        public static string getBody(string userid, string username, string pass)
        {

            string sBody = "尊敬的客户：</b>";
            sBody += "<p>  您好！非常感谢您的注册。</p>";
            sBody += "<p>  您的会员登录信息如下：</p>";
            sBody += "<p>" + "账号：" + username + "</p>";
            sBody += "<p>" + "密码：" + pass + "</p>";
            sBody += "<p>  如果您已有UKey，可以在设置“子账号”的同时进行绑定。绑定UKey后，仅能通过UKey打开链接并登陆。单个账号可以绑定多个UKey。</p>";
            sBody += "<p>  优诗力科技</p>";
            return sBody;

        }







    }





}
