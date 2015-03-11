using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Common.Base;
using DBUtility;

namespace DAL
{
    public partial class BaseDAL
    {

        public BaseDAL()
		{}
		#region  BasicMethod
        //protected Model.SessionUser User = (Model.SessionUser)System.Web.HttpContext.Current.Session["UserInfo"];
        protected Model.SessionUser User
        {
            get
            {
                if (HttpContext.Current.Session["UserInfo"] == null)
                {
                    return null;
                }
                return (Model.SessionUser)HttpContext.Current.Session["UserInfo"];
            }
            set
            {
                HttpContext.Current.Session["UserInfo"] = value;
            }
        }


        protected XMLHandler VerifyHt = new XMLHandler();
        protected string TableName = "";
        /// <summary>
        /// 查询假删除数据的权限 0:不能查询假删除的数据 1:只能查询假删除的数据 -1:查询所有数据
        /// </summary>
        public int DelState = 0;


        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="Sql">需要执行的Sql语句</param>
        /// <returns></returns>
        protected string ExcuseSql(string Sql)
        {
            DBUtility.SqlManage SqlManage = new DBUtility.SqlManage();
            int result = SqlManage.Del(Sql);
            if (result > 0)
            {
                return "{\"Type\":0,\"Message\":\"操作成功\"}";
            }
            else
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }
        }



        /// <summary>
        /// 根据Id获取所需字段并转换为Json
        /// </summary>
        /// <param name="Id">Id</param>
        /// <param name="Clomns">需要获取的字段字段间用逗号分隔  不填或者"*"表示全部字段</param>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <returns>查询的Json</returns>
        public string GetJson(int Id, string Clomns = "*", int Permissions = 0)
        {
            string Sql = "select top 1 " + Safe.SafeReplace(Clomns) + " from " + Safe.SafeReplace(TableName) + " where id = '" + Id + "'" + SetPermissions(Permissions);
            DBUtility.SqlManage SqlManage = new DBUtility.SqlManage();
            System.Text.StringBuilder JsonSb = new System.Text.StringBuilder("{");
            if (DelState != -1)
            {
                Sql += " and isnull(DelState,0) = " + DelState;
            }
            System.Data.DataTable dt = SqlManage.Sel(Sql);
            if (dt.Rows.Count == 1)
            {
                foreach (System.Data.DataColumn DC in dt.Columns)
                {
                    JsonSb.Append("\"" + DC.ColumnName + "\":\"" + dt.Rows[0][DC.ColumnName] + "\",");
                }

                JsonSb.Remove(JsonSb.Length - 1, 1);
            }
            JsonSb.Append("}");

            return JsonSb.ToString();
        }

        /// <summary>
        /// Sql拼接权限Where
        /// </summary>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <param name="Where">Linq的Where条件</param>
        /// <returns></returns>
        public virtual string SetPermissions(int Permissions)
        {

            return "";
        }


        /////////////////////////////////////////////删除//////////////////////////////////////////////////////////////////



        /// <summary>
        /// 假删除
        /// </summary>
        /// <param name="Id">删除的Id</param>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <returns>删除结果</returns>
        public string Del(int Id, int Permissions = 0)
        {
            string Sql = "Update [" + Safe.SafeReplace(TableName) + "] set DelState = 1 where id= '" + Id + "'" + SetPermissions(Permissions);
            return ExcuseSql(Sql);
        }


        /// <summary>
        /// 批量假删除
        /// </summary>
        /// <param name="Id">删除的Id</param>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <returns>删除结果</returns>
        public string Del(string Ids, int Permissions = 0)
        {
            Ids = Safe.SafeReplace(Ids);
            string Sql = "Update [" + Safe.SafeReplace(TableName) + "] set DelState = 1 where Id in (" + Ids + ")" + SetPermissions(Permissions);
            return ExcuseSql(Sql);
        }


        /// <summary>
        /// 假删除传入Ids之外的数据
        /// </summary>
        /// <param name="Id">不删除的Id</param>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <returns>删除结果</returns>
        public string DelNotIn(string Ids, int Permissions = 0)
        {
            Ids = Safe.SafeReplace(Ids);
            string Sql = "Update [" + Safe.SafeReplace(TableName) + "] set DelState = 1 where Id not in (" + Ids + ")" + SetPermissions(Permissions);
            return ExcuseSql(Sql);
        }


        /// <summary>
        /// 真删除
        /// </summary>
        /// <param name="Id">删除的Id</param>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <returns>删除结果</returns>
        public string DelTrue(int Id, int Permissions = 0)
        {
            string Sql = "Delete from [" + Safe.SafeReplace(TableName) + "] where id= '" + Id + "'" + SetPermissions(Permissions);
            return ExcuseSql(Sql);
        }


        /// <summary>
        /// 批量真删除
        /// </summary>
        /// <param name="Id">删除的Id</param>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <returns>删除结果</returns>
        public string DelTrue(string Ids, int Permissions = 0)
        {
            Ids = Safe.SafeReplace(Ids);
            string Sql = "Delete from [" + Safe.SafeReplace(TableName) + "] where id in (" + Ids + ")" + SetPermissions(Permissions);
            return ExcuseSql(Sql);
        }


        /// <summary>
        /// 删除传入Ids之外的数据
        /// </summary>
        /// <param name="Id">不删除的Id</param>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <returns>删除结果</returns>
        public string DelNotInTrue(string Ids, int Permissions = 0)
        {
            Ids = Safe.SafeReplace(Ids);
            string Sql = "Delete from [" + Safe.SafeReplace(TableName) + "] where id not in (" + Ids + ")" + SetPermissions(Permissions);
            return ExcuseSql(Sql);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 验证输入的数据是否符合规范
        /// </summary>
        /// <param name="Ht">需要被验证的Hashtable</param>
        /// <param name="MethodType">0:添加  1:修改</param>
        public string Verify(System.Collections.Hashtable Ht, int MethodType)
        {
            Ht["UserId"] = User.UserId;
            Ht["ParentId"] = User.ParentId;
            string a = VerifyHt.VerifyHt(Ht, Safe.SafeReplace(TableName), MethodType);
            if (a != null)
            {
                return "{\"Type\":-1,\"Message\":\"" + a + "\"}";
            }
            return a;
        }

        /// <summary>
        /// 验证输入的数据是否符合规范(userid和parentid从前一个页面传过来)
        /// </summary>
        /// <param name="Ht">需要被验证的Hashtable</param>
        /// <param name="MethodType">0:添加  1:修改</param>
        public string Verify1(System.Collections.Hashtable Ht, int MethodType)
        {
            string a = VerifyHt.VerifyHt(Ht, Safe.SafeReplace(TableName), MethodType);
            if (a != null)
            {
                return "{\"Type\":-1,\"Message\":\"" + a + "\"}";
            }
            return a;
        }
        #endregion  BasicMethod

    }

}