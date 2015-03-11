using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


public class MyHtmlHelper
{
    public static string Label(string target, string text)
    {
        return String.Format("<label id='{0}'>{1}</label>", target, text);
    }

}


public static class MyHtmlHelper1
{
    public static HtmlString MyLabel(this HtmlHelper helper, string label)
    {
        string a=" id='a' name='a' ";

        return (new HtmlString(a));
    }

    public static  string Span(this HtmlHelper helper, string id, string text)
    {
        return String.Format(@"<span id=""{0}"">{1}</span>", id, text);
    }
}

