using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections;
using System.Data;
using DBUtility;
using Common.Base;
using Model;

namespace Web.Components
{
    public class SqlManageHelper:DBUtility.SqlManage
    {
        //SqlManage SqlManage1 = new SqlManage();
        JsonHashtable JsonHashtable1 = new JsonHashtable();
        public SqlManageWhere Where = new SqlManageWhere();
        public SqlManageOrder Order = new SqlManageOrder();


        /// <summary>
        /// 获取当前登录用户Id
        /// </summary>
        private int UserId
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
        /// 获取当前登录用户ParentId
        /// </summary>
        private int ParentId
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
        /// 总记录数
        /// </summary>
        public int RecordCount;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount;

        /// <summary>
        /// 搜索（参数），返回DataTable结果集
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Column">字段名</param>
        /// <param name="Order">排序方式</param>
        /// <param name="Where">搜索条件</param>
        /// <param name="PageSize">每页几条数据</param>
        /// <param name="Page">当前第几页</param>
        /// <param name="RecordCount">返回的总记录数</param>
        /// <param name="PageCount">返回的总页数</param>
        /// <param name="Params">函数参数</param>
        /// <param name="ConParams">全文索引参数</param>
        /// <returns>DataTable结果集</returns>
        public DataTable PagingFu(string TableName, string Column, SqlManageWhere WhereParameters, SqlManageOrder Order, int PageSize, int Page, string[] Params, string[] ConParams = null)
        {
            DataTable dt = base.PagingFu(TableName, Column, GetWhere(WhereParameters), Order, PageSize, Page, ref  RecordCount, ref  PageCount, Params, ConParams);
            clear();
            return dt;
        }



        /// <summary>
        /// 置顶和取消置顶(参数)，返回处理的结果
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Id">Id</param>
        /// 
        public string SendTop(string TableName, int Id)
        {
            string OrderBy = One(TableName, "OrderBy", Id);
            int V = 0;
            if (OrderBy == "0")
            {
                V = int.Parse(One(TableName, "max(OrderBy)+1"));
            }

            base.Upd("update " + TableName + " set OrderBy=" + V + " where id=" + Id + " and userid=" + UserId + " ");
            return ("{\"Type\":0,\"Message\":\"操作成功\"}");
        }


        /// <summary>
        /// 搜索（参数），返回DataTable结果集
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Column">字段名</param>
        /// <param name="Order">排序方式</param>
        /// <param name="Where">搜索条件</param>
        /// <param name="PageSize">每页几条数据</param>
        /// <param name="Page">当前第几页</param>
        /// <param name="RecordCount">返回的总记录数</param>
        /// <param name="PageCount">返回的总页数</param>
        /// <returns>DataTable结果集</returns>
        public DataTable Paging(string TableName, string Column, SqlManageWhere WhereParameters, SqlManageOrder Order, int PageSize, int Page)
        {
            DataTable dt = base.Paging(TableName, Column, GetWhere(WhereParameters), Order, PageSize, Page, ref  RecordCount, ref  PageCount);
            clear();
            return dt;
        }


        /// <summary>
        /// 将后台拼接好的行转换成分页时前台的Json格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Json</returns>
        public string PagingReturn(string str)
        {
            str = Microsoft.JScript.GlobalObject.escape(str);

            return ("{ \"Message\": \"" + str + "\", \"PageCount\":\"" + PageCount + "\", \"RecordCount\":\"" + RecordCount + "\" }");
        }

        private void clear()
        {
            Where = new SqlManageWhere();
            Order = new SqlManageOrder();
        }

        /// <summary>
        /// 搜索（参数），返回DataTable结果集不分页
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Column">字段名</param>
        /// <param name="WhereParameters">搜索条件(Where集合)</param>
        /// <param name="OrderParameters">排序方式(Order集合)</param>
        /// <returns>DataTable结果集</returns>
        public DataTable Sel(string TableName, string Column, SqlManageWhere WhereParameters, SqlManageOrder OrderParameters)//
        {
            DataTable ta = base.Sel(TableName, Column, GetWhere(WhereParameters), OrderParameters);
            clear();
            return ta;
        }



        /// <summary>
        /// 搜索（参数），返回指定的一条记录的对应信息
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Column">字段名</param>
        /// <param name="WhereParameters">搜索条件(指定的Id)</param>
        /// <returns>DataTable结果集</returns>
        public DataTable Sel(string TableName, string Column, int Id)//
        {
            SqlManageWhere SqlManageWhere1 = new SqlManageWhere();
            SqlManageWhere1.Add("Id", Id.ToString());
            DataTable ta = base.Sel(TableName, Column, GetWhere(SqlManageWhere1), null);
            clear();
            return ta;
        }

        /// <summary>
        /// 搜索（参数），返回指定的一条记录的对应信息
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Column">字段名</param>
        /// <param name="WhereParameters">搜索条件(指定的Id)</param>
        /// <returns>DataTable结果集</returns>
        public DataTable Sel(string TableName, int Id)//
        {
            SqlManageWhere SqlManageWhere1 =new SqlManageWhere();
            SqlManageWhere1.Add("Id", Id.ToString());
            DataTable dt = base.Sel(TableName, "*", GetWhere(SqlManageWhere1), null);
            clear();
            return dt;

        }

        /// <summary>
        /// 搜索（参数），返回指定的一条记录的对应信息
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Column">字段名</param>
        /// <param name="WhereParameters">搜索条件(指定的Id)</param>
        /// <returns>DataTable结果集</returns>
        public string Sel2Json(string TableName, int Id)//
        {
            SqlManageWhere SqlManageWhere1 = new SqlManageWhere();
            SqlManageWhere1.Add("Id", Id.ToString());
            DataTable ta = base.Sel(TableName, "*", GetWhere(SqlManageWhere1), null);
            string a = JsonHashtable1.DataTableToJson(ta);
            return a;
        }


        /// <summary>
        /// 搜索（参数），返回指定的一条记录的一列信息
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>有返回，无返回null</returns>
        public string One(string TableName, string Column, int Id)
        {
            SqlManageWhere SqlManageWhere1 = new SqlManageWhere();
            SqlManageWhere1.Add("Id", Id.ToString());
            string a = base.One(TableName, Column, GetWhere(SqlManageWhere1));
            clear();
            return a;
        }

        /// <summary>
        /// 搜索（参数），如max(id),返回指定的一条记录的一列信息
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>有返回，无返回null</returns>
        public string One(string TableName, string Column)
        {
            string a = base.One(TableName, Column,GetWhere(null));
            clear();
            return a;
        }


        /// <summary>
        /// 搜索（参数），返回指定的一条记录的一列信息
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>有返回，无返回null</returns>
        public string One(string TableName, string Column, SqlManageWhere WhereParameters, SqlManageOrder OrderParameters = null)
        {
            string a = base.One(TableName, Column, GetWhere(WhereParameters), OrderParameters);
            clear();
            return a;
        }



        /// <summary>
        /// 搜索（参数），返回指定记录的一列信息,逗号隔开
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>有返回，无返回null</returns>
        public string OneList(string TableName, string Column, SqlManageWhere WhereParameters)
        {
            string a = base.OneList(TableName, Column, GetWhere(WhereParameters));
            clear();
            return a;
        }

        /// <summary>
        /// 搜索（参数），返回指定记录的一列信息,以ArrayList的格式返回
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>有返回，无返回null</returns>
        public ArrayList OneArrayList(string TableName, string Column, SqlManageWhere WhereParameters)
        {
            ArrayList a = base.OneArrayList(TableName, Column, GetWhere(WhereParameters));
            clear();
            return a;
        }



        private SqlManageWhere GetWhere(SqlManageWhere OldWhereParameters)
        {
            if (OldWhereParameters == null)
            {
                OldWhereParameters = new SqlManageWhere();
                OldWhereParameters.Add("DelState","0");
                OldWhereParameters.Add("UserId", this.UserId.ToString());
                return OldWhereParameters;
            }
            else
            {
                string State = OldWhereParameters.Contains("DelState");

                if (State == null)
                {
                      OldWhereParameters.Add("DelState", "0");
                }
                if (State == "-1")
                {
                      OldWhereParameters.Remove("DelState");
                }


                string SelType = OldWhereParameters.Contains("SelType");
                if (SelType == null)
                {
                    OldWhereParameters.Add("UserId", this.UserId.ToString());
                }
                else
                {
                    OldWhereParameters.Remove("SelType");
                    if (SelType == "0")
                    {
                        OldWhereParameters.Add("UserId", this.UserId.ToString());
                    }
                    if (SelType == "1")
                    {
                        OldWhereParameters.Add("ParentId", this.ParentId.ToString());
                    }
                    if (SelType == "2")
                    {
                        OldWhereParameters.Add("(UserId", this.UserId.ToString());
                        OldWhereParameters.Or("ParentId", this.UserId + ")");
                    }
                    if (SelType == "3")
                    {
                        OldWhereParameters.Add("ParentId", this.UserId.ToString());
                    }
                }

            }
            return OldWhereParameters;
        }



        /// <summary>
        /// 插入，表名--哈希表，返回插入结果的Hashtable
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>JSON</returns>
        public Hashtable Ins(string TableName, Hashtable Ht)
        {
            if (!Ht.Contains("UserId"))
            {
                Ht.Add("UserId", this.UserId.ToString());
            }
            else
            {
                Ht["UserId"]=this.UserId.ToString();
            }
            if (!Ht.Contains("ParentId"))
            {
                Ht.Add("ParentId", this.UserId.ToString());
            }
            else
            {
                Ht["ParentId"] = this.UserId.ToString();
            }
            return base.Ins(TableName, Ht);
       }




        /// <summary>
        /// 修改，表名--哈希表，返回插入结果的Hashtable
        /// </summary>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <param name="TableName">表名</param>
        /// <param name="Ht">要修改的参数和值</param>
        public Hashtable Upd(string TableName, Hashtable Ht, int Permissions=0)
        {
            TableName = DBUtility.Safe.SafeReplace(TableName);
            XMLHandler VerifyHt = new XMLHandler();
            string a = VerifyHt.VerifyHt(Ht, TableName, 1);
            if (a != null) { return JsonHashtable1.EasyDecode(a); }
            if (a != null)
            {
                a = "{\"Type\":-1,\"Message\":\"" + a + "\"}";
                return JsonHashtable1.EasyDecode(a);
            }

            string key_name = "", key_value = "";
            if (Ht.Contains("id")) { key_name = "id"; key_value = DBUtility.Safe.SafeReplace(Ht["id"].ToString()); }
            if (Ht.Contains("Id")) { key_name = "Id"; key_value = DBUtility.Safe.SafeReplace(Ht["Id"].ToString()); }
            if (Ht.Contains("ID")) { key_name = "ID"; key_value = DBUtility.Safe.SafeReplace(Ht["ID"].ToString()); }
            if (key_name == "") { return JsonHashtable1.EasyDecode("{\"Type\":-1,\"Message\":\"未知主键\"}"); }
            string sql1 = "";

            foreach (DictionaryEntry de in Ht)
            {
                string Key = DBUtility.Safe.SafeReplace(de.Key.ToString());

                if (de.Value != null)
                {
                    string Value = de.Value.ToString();
                    bool b = false;
                    try
                    {
                        DateTime t = Convert.ToDateTime(Value);
                        b = true;

                    }
                    catch (Exception)
                    {

                    }

                    if (Key != key_name)
                    {
                        if (b)//如果是时间（包含有空格的）
                        {
                            sql1 = sql1 + Key + "='" + Value + "',";
                        }
                        else
                        {
                            sql1 = sql1 + Key + "='" + DBUtility.Safe.SafeReplace(Value) + "',";
                        }

                    }
                }
                else
                {
                    sql1 = sql1 + Key + "=null,";
                }

            }
            sql1 = sql1.Trim(',');

            string sql = "update " + TableName + " set " + sql1 + " where " + key_name + "='" + key_value + "'";
            if (Permissions == 0)
            {
                sql = sql + "and UserId = '" + UserId + "'";
            }
            if (Permissions == 1)
            {
                sql = sql + "and ParentId = '" + ParentId + "'";
            }
            if (Permissions == 2)
            {
                sql = sql + "and (UserId = '" + UserId + "' and ParentId = '" + ParentId + "')";
            }
            if (Permissions == 3)
            {
                sql = sql + "and (UserId = '" + UserId + "' or ParentId = '" + ParentId + "')";
            }
            int Num = Upd(sql);
            return JsonHashtable1.EasyDecode("{\"Type\":0,\"Message\":\"操作完成\",\"Num\":" + Num + "}");
        }
    }
}