using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace VeryVP.Web.Components
{
    public class CompileString
    {
        //BLL.Users ubll = new BLL.Users();

        //private Model.Users _umodel;

        //private Model.Users Umodel
        //{
        //    get { return _umodel; }
        //    set { _umodel = value; }
        //}

        ///// <summary>
        ///// 编译模板插入字段数据
        ///// </summary>
        ///// <param name="str">待编译的字符串</param>
        ///// <param name="option">操作标识(0:发送邮件，1:回复邮件)</param>
        ///// <param name="userId">用户Id</param>
        ///// <param name="emailId">邮件Id</param>
        ///// <returns></returns>
        //public string GetCompileString(string str, int option, int userId, int emailId)
        //{
        //    if (!string.IsNullOrEmpty(str)&&userId != 0)
        //    {
        //        if (Umodel==null)
        //        {
        //            Umodel = ubll.GetModel(userId);
        //        }
        //        if (option == 0)
        //        {
        //            str = str.Replace("{用户昵称}", Umodel.nickName);
        //            str = str.Replace("{用户中文名}", Umodel.RealName);
        //            str = str.Replace("{用户英文名}", Umodel.EnglishName);
        //            str = str.Replace("{用户邮件地址}", Umodel.UserName);
        //            str = str.Replace("{用户电话}", Umodel.Tel);
        //            str = str.Replace("{用户传真}", Umodel.FaxNo);
        //            str = str.Replace("{用户手机}", Umodel.PhoneNo);
        //            str = str.Replace("{用户QQ}", Umodel.QQ);
        //        }
        //    }
        //    return str;
        //}

        ///// <summary>
        ///// 编译模板插入字段数据重载
        ///// </summary>
        ///// <param name="str">待编译的字符串</param>
        ///// <param name="option">操作标识(0:发送邮件，1:回复邮件)</param>
        ///// <param name="userId">用户Id</param>
        ///// <returns></returns>
        //public string GetCompileString(string str, int option, int userId)
        //{
        //    return GetCompileString(str, option, userId, 0);
        //}

        ///// <summary>
        ///// 编译模板插入字段数据重载
        ///// </summary>
        ///// <param name="str">待编译的字符串</param>
        ///// <param name="option">操作标识(0:发送邮件，1:回复邮件)</param>
        ///// <returns></returns>
        //public string GetCompileString(string str, int option)
        //{
        //    return GetCompileString(str, option, 0, 0);
        //}

        /// <summary>
        /// 编译模板插入字段数据
        /// </summary>
        /// <param name="str">待编译的字符串</param>
        /// <returns></returns>
        public string GetCompileString(string str)
        {   
            if(!string.IsNullOrEmpty(str))
            str = str.Replace("{收件人昵称}", "{名}");
            return str;
        }
    }
}