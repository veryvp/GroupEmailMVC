using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUtility
{
    public static class Safe
    {
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

        /// <summary>
        /// 全文索引替换(必须先执行SafeReplace在执行该方法)
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        public  static string FuSafeReplace(string Html)
        {
            if (Html == null || Html == "") return "";
            Html = SafeReplace(Html);
            System.Text.StringBuilder S = new System.Text.StringBuilder();
            S.Append(Html);
            S = S.Replace("&", "_");
            S = S.Replace("|", "_");
            S = S.Replace("[", "_");
            S = S.Replace("]", "_");
            S = S.Replace("(", "_");
            S = S.Replace(")", "_");
            S = S.Replace("*", "_");
            S = S.Replace("~", "_");
            S = S.Replace("!", "_");
            S = S.Replace(",", "_");

            return S.ToString();
        }


        public static string safereturn(string Html)
        {
            if (Html == null || Html == "") return "";
            System.Text.StringBuilder S = new System.Text.StringBuilder();
            S.Append(Html);
            S = S.Replace("&lt;", "<");
            S = S.Replace("&gt;", ">");
            S = S.Replace("&quot;", "\"");
            S = S.Replace("&#39;", "'");
            S = S.Replace("&nbsp;", " ");

            return S.ToString();
        }



    }
}
