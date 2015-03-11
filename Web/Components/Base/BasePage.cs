using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Web.Components;
using System.Collections;
using Model;
using DBUtility;

public class BasePage : System.Web.UI.Page
{
    public SqlManageHelper Helper1 = new SqlManageHelper();
    public Common.Base.JsonHashtable JsonHashtable1 = new Common.Base.JsonHashtable();

    /// <summary>
    /// 管理员才能访问的页面要加载ManageLogin方法(AJAX请求前要加上去)
    /// </summary>
    public void ManageLogin()
    {
        if (ParentId != UserId)
        {
            System.Web.HttpContext.Current.Response.Write("您不是管理员，没有权限访问");
            System.Web.HttpContext.Current.Response.End();
        }
    }


    /// <summary>
    /// 获取当前登录用户Id
    /// </summary>
    public int UserId
    {
        get
        {
            SessionUser suser = (SessionUser)HttpContext.Current.Session["UserInfo"];
            if (suser == null)
            {
                return 0;
            }
            return suser.UserId;
        }
        set
        {
            SessionUser su = (SessionUser)HttpContext.Current.Session["UserInfo"];
            su.UserId = value;
        }
    }

    /// <summary>
    /// 获取当前登录用户名
    /// </summary>
    public string UserNo
    {
        get
        {
            SessionUser suser = (SessionUser)HttpContext.Current.Session["UserInfo"];
            if (suser == null)
            {
                return null;
            }
            return suser.UserNo;
        }
        set
        {
            SessionUser su = (SessionUser)HttpContext.Current.Session["UserInfo"];
            su.UserName = value;
        }
    }

    /// <summary>
    /// 获取当前登录用户名
    /// </summary>
    public string UserName
    {
        get
        {
            SessionUser suser = (SessionUser)HttpContext.Current.Session["UserInfo"];
            if (suser == null)
            {
                return null;
            }
            return suser.UserName;
        }
        set
        {
            SessionUser su = (SessionUser)HttpContext.Current.Session["UserInfo"];
            su.UserName = value;
        }
    }


    /// <summary>
    /// 获取当前登录用户ParentId
    /// </summary>
    public int ParentId
    {
        get
        {
            SessionUser suser = (SessionUser)HttpContext.Current.Session["UserInfo"];
            if (suser == null)
            {
                return 0;
            }
            return suser.ParentId;
        }
        set
        {
            SessionUser su = (SessionUser)HttpContext.Current.Session["UserInfo"];
            su.ParentId = value;
        }
    }

    /// <summary>
    /// 获取当前登录用户是否在职
    /// </summary>
    public int Status
    {
        get
        {
            SessionUser suser = (SessionUser)HttpContext.Current.Session["UserInfo"];
            if (suser == null)
            {
                return 0;
            }
            return suser.Status;
        }
        set
        {
            SessionUser su = (SessionUser)HttpContext.Current.Session["UserInfo"];
            su.Status = value;
        }
    }

    /// <summary>
    /// 获取当前登录用户的公司Id
    /// </summary>
    public int CompanyId
    {
        get
        {
            SessionUser suser = (SessionUser)HttpContext.Current.Session["UserInfo"];
            if (suser == null)
            {
                return 0;
            }
            return suser.CompanyId;
        }
        set
        {
            SessionUser su = (SessionUser)HttpContext.Current.Session["UserInfo"];
            su.CompanyId = value;
        }
    }

    /// <summary>
    /// 获取当前登录用户的公司名称
    /// </summary>
    public string CompanyName
    {
        get
        {
            SessionUser suser = (SessionUser)HttpContext.Current.Session["UserInfo"];
            if (suser == null)
            {
                return null;
            }
            return suser.CompanyName;
        }
        set
        {
            SessionUser su = (SessionUser)HttpContext.Current.Session["UserInfo"];
            su.CompanyName = value;
        }
    }


    /// <summary>
    /// 获取当前登录用户对象
    /// </summary>
    public SessionUser UserInfo
    {
        get
        {
            if (HttpContext.Current.Session["UserInfo"] == null)
            {
                return null;
            }
            return (SessionUser)HttpContext.Current.Session["UserInfo"];
        }
        set
        {
            HttpContext.Current.Session["UserInfo"] = value;
        }
    }

    /// <summary>
    ///快速的获取？后面的一个参数的值，注意没有就是“”
    /// </summary>
    /// <param name="a">参数名</param>
    public string RequestQueryString(string a)
    {
        string b = "";
        if (HttpContext.Current.Request.QueryString[a] != null)
        {
            b = Server.UrlDecode(HttpContext.Current.Request.QueryString[a].ToString().Trim());
        }
        return b;
    }

    /// <summary>
    ///快速的获取？后面的一个参数的值，注意没有就是“”
    /// </summary>
    /// <param name="a">参数名</param>
    public string RequestForm(string a)
    {
        string b = "";
        if (HttpContext.Current.Request.Form[a] != null)
        {
            b = HttpContext.Current.Request.Form[a].ToString().Trim();
        }
        return b;
    }


    /// <summary>
    ///字符串分割，主要用于分割前端传递来的JSON字符串
    /// </summary>
    /// <param name="Str">字符串</param>
    /// <param name="Key">分隔符</param>
    /// <returns>返回分割后的数组</returns>
    public string[] SplitMore(string Str, string Key = "--A--")
    {
        //string[] sArr = null;
        //sArr = Common.StringHtmlJscript.StringPlus.SplitMore(string.Empty + Str + string.Empty, "--A--");
        //string[] sArr = Str.Split(new string[] { "--A--" }, StringSplitOptions.RemoveEmptyEntries);
        string[] sArr = Str.Split(new string[] { Key }, StringSplitOptions.None);
        return sArr;

    }

    /// <summary>
    ///字符串数组转成string
    /// </summary>
    /// <param name="Str">字符串数组</param>
    /// <returns>string</returns>
    public string ArrayToString(string[] Str)
    {
        if (Str == null) { return ""; }
        if (Str.Length == 0) { return ""; }
        else
        {
            string a = "";
            for (int i = 0; i < Str.Length; i++)
            {
                 if( Str[i]!="")
                a = a + Str[i] + ",";
            }
            a = Safe.SafeReplace(a);
            return a.Trim(',');
        }
    }


    /// <summary>
    ///字符串数组转成int数组，字符串为空就赋0
    /// </summary>
    /// <param name="Str">字符串数组</param>
    /// <returns>int数组</returns>
    public int[] ArrayStringToArrayInt(string[] Str)
    {
        if (Str == null) { return null; }
        int[] a = new int[Str.Length];
        for (int i = 0; i < Str.Length; i++)
        {
            if (Str[i] == "") { Str[i] = "0"; }
            a[i] = int.Parse(Str[i]);
        }
        return a;
    }


    /// <summary>
    ///检查字符串数组中是否有相同值，有就返回True
    /// </summary>
    /// <param name="Str">字符串数组</param>
    public string ArrayStringSame(string[] Str, string Title = "", string Name = "", string Box = "")
    {
        if (Str == null) { return null; }
        for (int i = 0; i < Str.Length; i++)
        {
            if (Str[i].Length > 100)
            {
                return "{\"Type\":-1,\"Message\":\"" + Title + "的输入信息过长，请检查后重新输入\",\"Name\":\"" + Name + "\",\"Box\":\"" + Box + "\",\"Num\":" + i + "}";
            }
        }

        for (int i = 0; i < Str.Length; i++)
        {
            for (int j = 0; j < Str.Length; j++)
            {
                if (i != j)
                {
                    if ((Str[i] == Str[j]) && (Str[j] != ""))
                    {
                        return "{\"Type\":-1,\"Message\":\"" + Title + "不能存在相同项，请检查后重新输入\",\"Name\":\"" + Name + "\",\"Box\":\"" + Box + "\",\"Num\":" + j + "}";
                        //return "{\"Type\":-1,\"Message\":\"公司名称不能为空\",\"Box\":\"add\",\"Name\":\"CName\",\"Num\":"+j+"}";
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    ///检查字符串数组中是否有相同值，有就返回True
    /// </summary>
    /// <param name="Str">字符串数组</param>
    public string ArrayStringSame(string Str, string Title = "", string Name = "", string Box = "")
    {
        string[] Tel = SplitMore(Str);
        return ArrayStringSame(Tel, Title, Name, Box);
    }


    /// <summary>
    ///检查Hashtable中每一个KEY，是否存在，或者对应值是否是Null,Empty,0
    /// </summary>
    /// <param name="Ht">Hashtable</param>
    /// <param name="Key">Key</param>
    public bool HashtableKeyIsNull(Hashtable Ht, string Key, string Value = "Null,Empty,0")
    {

        if (!Ht.Contains(Key))
        {
            return true;
        }
        else
        {
            string[] a = Value.Split(',');
            for (int i = 0; i < a.Length; i++)
            {
                string a1 = a[i];
                if (a1 == "Null")
                {
                    if (Ht[Key] == null) { return true; }
                }
                if (a1 == "Empty")
                {
                    if (Ht[Key] != null)
                    {
                        string a2 = Ht[Key].ToString();
                        if (a2 == "")
                        {
                            return true;
                        }
                    }
                }
                if (a1 == "0")
                {
                    if (Ht[Key] != null)
                    {
                        string a2 = Ht[Key].ToString();
                        if (a2 == "0")
                        {
                            return true;
                        }
                    }
                }

            }
        }
        return false;

    }

    /// <summary>
    /// 判断用户是否登录（不做页面跳转）
    /// </summary>
    public bool IsLogin()
    {
        if (UserId != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 判断用户是否退出
    /// </summary>
    public string IsLoginExit()
    {
        if (Session["OldUserInfo"] != null)
        {
            Session["OldUserInfo"] = null;
            Session.Remove("OldUserInfo");
        }
        if (Session["UserInfo"] != null)
        {
            Session["UserInfo"] = null;
			Session.Remove("UserInfo");
            return "0";
        }
        else
        {
            return "1";
        }
    }


    /// <summary>
    /// 验证Id是否存在，并判断Id是否为数字
    /// </summary>
    /// <param name="TableName">检查改数据在的表</param>
    /// <param name="CheckType">类型，0是自己的，1以后的再扩展</param>
    internal void CheckTableId(string TableName,int CheckType=0)
    {
        string Type = RequestQueryString("Type").ToLower();
        if ((Type != "") && (Type != "copy") && (Type != "model"))
        { HttpContext.Current.Response.End(); }

        string  Id = RequestQueryString("Id");
        if ((Type != "")&&(Id==""))
        { HttpContext.Current.Response.End(); }

        if (Id != "")
        {
            Id = Safe.SafeReplace(Id);
            TableName = Safe.SafeReplace(TableName);
            Common.Base.Check Check1 = new Common.Base.Check();
            if (!Check1.IsIntZheng(Id)) { HttpContext.Current.Response.End(); }
            if (Id == "0") { HttpContext.Current.Response.End(); }
            if (CheckType == 0)
            {
                if (BoolExists(Id, TableName) == null) { HttpContext.Current.Response.End(); }
            }
        }

    }


    /// <summary>
    /// 判断是否存在该Id
    /// </summary>
    /// <returns></returns>
    private string BoolExists(string ThisId, string TableName)
    {
        DBUtility.SqlManage SqlManage1 = new DBUtility.SqlManage();
        BasePage BasePage1 = new BasePage();
        return SqlManage1.One("select top 1  Id from " + TableName + " where userid='" + UserId + "' and id='" + ThisId + "'");
    }

    /// <summary>
    /// 根据代号获取系统配置参数状态
    /// </summary>
    /// <param name="Code">代号</param>
    public string GetConfigState(string Code)
    {
        Code = Safe.SafeReplace(Code);
        if (Code != "")
        {
            DBUtility.SqlManage SqlManage1 = new DBUtility.SqlManage();
            return SqlManage1.One("select top 1 [State] from [RMDB].[dbo].[Config] where Code='" + Code + "'");
        }
        return null;
    }
}
