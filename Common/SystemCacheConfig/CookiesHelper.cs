using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Common.SystemCacheConfig
{
    /// <summary>
    ///CookiesHelper 的摘要说明
    /// </summary>
    public class CookiesHelper
    {


        /// <summary>
        /// 获得Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static HttpCookie GetCookie(string cookieName)
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
                return request.Cookies[cookieName];
            return null;
        }


        /// <summary>
        /// 直接获得Cookie值，没有返回空
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookieString(string cookieName)
        {
            string a = null;
            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
            {
                if (request.Cookies[cookieName] != null)
                {
                    a = request.Cookies[cookieName].Value.ToString();
                }
            }
            return a;
        }


        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void RemoveCookie(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                //RemoveCookie(cookieName, null);
                HttpCookie myCookie = new HttpCookie(cookieName);
                //myCookie.Domain = ".veryvp.com";
                myCookie.Path = "/";
                myCookie.Expires = DateTime.Now.AddDays(-3d);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }

        }
        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public static void AddCookie(HttpCookie cookie)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                //指定客户端脚本是否可以访问[默认为false]
                cookie.HttpOnly = true;
                //指定统一的Path，比便能通存通取
                cookie.Path = "/";
                //设置跨域,这样在其它二级域名下就都可以访问到了
                //cookie.Domain = ".veryvp.com";
                response.AppendCookie(cookie);
            }
        }

        /// <summary>
        /// 写Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public static void WriteCookie(string cookiename, string cookvalue)
        {
            HttpCookie aCookie = CookiesHelper.GetCookie(cookiename);
            if (aCookie != null)
            {
                CookiesHelper.RemoveCookie(cookiename);
            }
            aCookie = new HttpCookie(cookiename);
            //    //指定客户端脚本是否可以访问[默认为false]
            aCookie.HttpOnly = true;
            //    //指定统一的Path，比便能通存通取
            aCookie.Path = "/";
            //    //设置跨域,这样在其它二级域名下就都可以访问到了
            //aCookie.Domain = ".veryvp.com";
            aCookie.Expires = DateTime.Now.AddDays(1);
            aCookie.Value = cookvalue;
            HttpContext.Current.Response.Cookies.Add(aCookie);


        }
    }
}