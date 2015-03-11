using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.Base
{
    public class IPHelper
    {

        /// <summary>
        /// 获取当前IP 当前项目正在使用获取IP的方法
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {

            try
            {
                if (HttpContext.Current == null
                    || HttpContext.Current.Request == null
                    || HttpContext.Current.Request.ServerVariables == null)
                    return "";
                string customerIP = "";
                //CDN加速后取到的IP simone 090805
                customerIP = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(customerIP))
                {
                    return SafeReplace(customerIP);
                }
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    customerIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (customerIP == null)
                        customerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    customerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.Compare(customerIP, "unknown", true) == 0)
                    return SafeReplace(HttpContext.Current.Request.UserHostAddress);
                return SafeReplace(customerIP);
                /** lcq
                关键就在HTTP_X_FORWARDED_FOR
                使用不同种类代理服务器，上面的信息会有所不同：
                一、没有使用代理服务器的情况：
                REMOTE_ADDR = 您的 IP
                HTTP_VIA = 没数值或不显示
                HTTP_X_FORWARDED_FOR = 没数值或不显示
                二、使用透明代理服务器的情况：Transparent Proxies
                REMOTE_ADDR = 代理服务器 IP 
                HTTP_VIA = 代理服务器 IP
                HTTP_X_FORWARDED_FOR = 您的真实 IP
                这类代理服务器还是将您的信息转发给您的访问对象，无法达到隐藏真实身份的目的。
                三、使用普通匿名代理服务器的情况：Anonymous Proxies
                REMOTE_ADDR = 代理服务器 IP 
                HTTP_VIA = 代理服务器 IP
                HTTP_X_FORWARDED_FOR = 代理服务器 IP
                隐藏了您的真实IP，但是向访问对象透露了您是使用代理服务器访问他们的。
                四、使用欺骗性代理服务器的情况：Distorting Proxies
                REMOTE_ADDR = 代理服务器 IP 
                HTTP_VIA = 代理服务器 IP 
                HTTP_X_FORWARDED_FOR = 随机的 IP
                告诉了访问对象您使用了代理服务器，但编造了一个虚假的随机IP代替您的真实IP欺骗它。
                五、使用高匿名代理服务器的情况：High Anonymity Proxies (Elite proxies)
                REMOTE_ADDR = 代理服务器 IP
                HTTP_VIA = 没数值或不显示
                HTTP_X_FORWARDED_FOR = 没数值或不显示 
                **/
            }
            catch
            {
                return "";
            }

        }


        public static string SafeReplace(string Html)
        {
            if (Html == null || Html == "") return "";
            System.Text.StringBuilder S = new System.Text.StringBuilder();
            S.Append(Html);
            S = S.Replace("<", "&lt;");
            S = S.Replace(">", "&gt;");
            S = S.Replace("\n", "<br>");
            S = S.Replace("\"", "&quot;");
            S = S.Replace("'", "&#39;");
            S = S.Replace("\t", " ");
            S = S.Replace(" ", "&nbsp;");//半角
            S = S.Replace(" ", "&nbsp;");//全角

            S = S.Replace("\r", "");


            return S.ToString();
        }
    }
}
