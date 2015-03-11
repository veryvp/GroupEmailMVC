using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Components.Base
{
    public class YanzhengBianhao
    {
        /// <summary>
        /// 验证Id是否存在，并判断Id是否为数字
        /// </summary>
        /// <param name="TypeId"></param>
        internal void Yanzhen(string TypeId, string TableName)
        {
            if (TypeId != "")
            {
                Common.Base.Check Check1 = new Common.Base.Check();
                if (!Check1.IsIntZheng(TypeId)) { HttpContext.Current.Response.End(); }
                if (TypeId == "0") { HttpContext.Current.Response.End(); }
                if (BoolExists(TypeId, TableName) == null) { HttpContext.Current.Response.End(); }
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
            return SqlManage1.One("select top 1  Id from " + TableName + " where userid='" + BasePage1.UserId + "' and id='" + ThisId + "'");
        }


        /// <summary>
        /// 判断是否存在该Id
        /// </summary>
        /// <returns></returns>
        internal string BoolExists2(string TableName, string Condition)
        {
            DBUtility.SqlManage SqlManage1 = new DBUtility.SqlManage();
            BasePage BasePage1 = new BasePage();
            return SqlManage1.One("select top 1  Id from " + TableName + " where " + Condition);
        }

        /// <summary>
        /// 验证Id是否存在，并判断Id是否为数字
        /// </summary>
        /// <param name="TypeId"></param>
        internal void Yanzhen(string TypeId, string TableName, string Condition)
        {
            Common.Base.Check CCheck = new Common.Base.Check();
            if (!CCheck.IsIntZheng(TypeId) && TypeId != "0") { HttpContext.Current.Response.End(); }
            if (BoolExists2(TableName, Condition) == null) { HttpContext.Current.Response.End(); }
        }

    }
}