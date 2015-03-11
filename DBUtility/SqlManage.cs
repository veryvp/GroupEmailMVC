using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using System.Collections;
using Common.Base;

namespace DBUtility
{
    /// <summary>
    ///newsql 的摘要说明
    /// </summary>
    public class SqlManage
    {
        JsonHashtable JsonHashtable1 = new JsonHashtable();
        public SqlManage()
        {

        }

        protected Database db
        {
            get
            {
                return EnterpriseLibraryContainer.Current.GetInstance<Database>("ConnStr");
                //db.ConnectionString=ConfigurationManager.AppSettings["ConnectionString"].ToString();
                //return EnterpriseLibraryContainer.Current.GetInstance<Database>(ConfigurationManager.AppSettings["ConnectionString"].ToString());
            }

        }
        ///////////////////////////////////////////////////////////////////
        /// <summary>
        /// 插入(SQL语句)，返回新记录Id（不能做安全处理）
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>新记录Id</returns>
        public string Ins(string Sql)
        {
            Sql = Sql + " SELECT  SCOPE_IDENTITY()  as  newIDValue";
            DbCommand com = db.GetSqlStringCommand(Sql);
            return db.ExecuteScalar(com).ToString();
        }


        /// <summary>
        /// 插入，表名--哈希表，返回插入结果的JSON
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>JSON</returns>
        public Hashtable Ins(string TableName, Hashtable Ht)
        {
            TableName = DBUtility.Safe.SafeReplace(TableName);
            XMLHandler VerifyHt = new XMLHandler();
            string a = VerifyHt.VerifyHt(Ht, TableName, 0);
            if (a != null) {
                a="{\"Type\":-1,\"Message\":\"" + a + "\"}";
                return JsonHashtable1.EasyDecode(a);
            }


            string ziduan = "";
            string value = "";

            // int  n = 0;
            foreach (DictionaryEntry de in Ht)
            {
                string Key = DBUtility.Safe.SafeReplace(de.Key.ToString());
                ziduan = ziduan + Key + ",";
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

                    if (b)//如果是时间（包含有空格的）
                    {
                        value = value + "'" + Value + "',";
                    }
                    else
                    {
                        value = value + "'" + DBUtility.Safe.SafeReplace(Value) + "',";
                    }

                }
                else
                {
                    value = value + "null,";
                }


            }


            ziduan = ziduan.Trim(',');
            value = value.Trim(',');


            string sql = "insert into " + TableName + "(" + ziduan + ")values(" + value + ")";
            string id = Ins(sql);
            return JsonHashtable1.EasyDecode("{\"Type\":0,\"Message\":\"操作完成\",\"Id\":" + id + "}");

        }


        ////////////////////////////////////////////////////////////////////



        /// <summary>
        /// 删除(SQL语句)，返回处理的数量（不能做安全处理）
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        public int Del(string sql)
        {
            DbCommand com = db.GetSqlStringCommand(sql);
            return db.ExecuteNonQuery(com);
        }

        /// <summary>
        /// 删除(参数)，返回处理的数量
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Column">字段名</param>
        /// <param name="Id">Id</param>
        public int Del(string TableName, string Column, string Id)
        {
            TableName = DBUtility.Safe.SafeReplace(TableName.Trim());
            Column = DBUtility.Safe.SafeReplace(Column.Trim());
            Id = DBUtility.Safe.SafeReplace(Id.Trim());

            DbCommand com = db.GetSqlStringCommand("delete " + TableName + " where " + Column + " in(" + Id + ")");
            return db.ExecuteNonQuery(com);
        }

        /// <summary>
        /// 删除(参数)，返回处理的数量
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Id">Id</param>
        public int Del(string TableName, string Id)
        {
            TableName = DBUtility.Safe.SafeReplace(TableName.Trim());
            Id = DBUtility.Safe.SafeReplace(Id.Trim());

            DbCommand com = db.GetSqlStringCommand("delete " + TableName + " where Id in(" + Id + ")");
            return db.ExecuteNonQuery(com);
        }

        ////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 修改(SQL语句)，返回处理的数量（不能做安全处理）
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        public int Upd(string Sql)
        {
            DbCommand com = db.GetSqlStringCommand(Sql);
            return db.ExecuteNonQuery(com);
        }



        /// <summary>
        /// 修改，表名--哈希表，返回插入结果的JSON
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        public Hashtable Upd(string TableName, Hashtable Ht, string zhujian)
        {
            TableName = DBUtility.Safe.SafeReplace(TableName);
            XMLHandler VerifyHt = new XMLHandler();
            string a = VerifyHt.VerifyHt(Ht, TableName, 1);
            if (a != null)
            {
                a = "{\"Type\":-1,\"Message\":\"" + a + "\"}";
                return JsonHashtable1.EasyDecode(a);
            }


            string key_value = Ht[zhujian].ToString().Trim();

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

                    if (Key != zhujian)
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

            string sql = "update " + TableName + " set " + sql1 + " where " + zhujian + "='" + key_value + "'";
            int Num = Upd(sql);
            return JsonHashtable1.EasyDecode("{\"Type\":0,\"Message\":\"操作完成\",\"Num\":" + Num + "}");
        }


        /// <summary>
        /// 修改，表名--哈希表，返回插入结果的Hashtable
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        public Hashtable Upd(string TableName, Hashtable Ht)
        {
            TableName = DBUtility.Safe.SafeReplace(TableName);
            XMLHandler VerifyHt = new XMLHandler();
            string a = VerifyHt.VerifyHt(Ht, TableName, 1);
            if (a != null)
            {
                a = "{\"Type\":-1,\"Message\":\"" + a + "\"}";
                return JsonHashtable1.EasyDecode(a);
            }
            if (a != null) { return JsonHashtable1.EasyDecode(a); }

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
            int Num = Upd(sql);
            return JsonHashtable1.EasyDecode("{\"Type\":0,\"Message\":\"操作完成\",\"Num\":" + Num + "}");
        }
        ////////////////////////////////////////////////////////////////////



        /// <summary>
        /// 搜索（SQL语句），返回DataTable结果集不分页（不能做安全处理）
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>DataTable结果集</returns>
        public DataTable Sel(string Sql)
        {
            DbCommand com = db.GetSqlStringCommand(Sql);
            //执行并返回数据集
            return db.ExecuteDataSet(com).Tables[0];
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
            SqlManageWhere SqlManageWhere1 = new SqlManageWhere();
            SqlManageOrder SqlManageOrder1 = new SqlManageOrder();
            TableName = DBUtility.Safe.SafeReplace(TableName);
            Column = DBUtility.Safe.SafeReplace(Column);
            string Sql = "select " + Column + " from " + TableName + "";
            Sql = Sql + " where " + SqlManageWhere1.getstring(WhereParameters);
            string order = SqlManageOrder1.getstring(OrderParameters);
            if (order == "")
            {
                order = "id desc";
            }
            Sql = Sql + " order by  " + order;

            DbCommand com = db.GetSqlStringCommand(Sql);
            //执行并返回数据集
            return db.ExecuteDataSet(com).Tables[0];
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
            TableName = DBUtility.Safe.SafeReplace(TableName);
            Column = DBUtility.Safe.SafeReplace(Column);
            string Sql = "select " + Column + " from " + TableName + " where Id=" + Id;
            DbCommand com = db.GetSqlStringCommand(Sql);
            //执行并返回数据集
            return db.ExecuteDataSet(com).Tables[0];
        }


        /// <summary>
        /// 搜索（参数），返回指定的一条记录的对应信息
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="WhereParameters">搜索条件(指定的Id)</param>
        /// <returns>DataTable结果集</returns>
        public DataTable Sel(string TableName, int Id)//
        {
            TableName = DBUtility.Safe.SafeReplace(TableName);
            string Sql = "select * from " + TableName + " where Id=" + Id;
            DbCommand com = db.GetSqlStringCommand(Sql);
            //执行并返回数据集
            return db.ExecuteDataSet(com).Tables[0];
        }


        /// <summary>
        /// 搜索（SQL语句），返回指定的一条记录的一列信息（不能做安全处理）
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>有返回，无返回null</returns>
        public string One(string Sql)
        {
            DbCommand com = db.GetSqlStringCommand(Sql);
            DataTable ta = db.ExecuteDataSet(com).Tables[0];
            if (ta.Rows.Count >= 1)
            {
                return ta.Rows[0][0].ToString();
            }
            return null;
        }


        /// <summary>
        /// 搜索（参数），返回指定的一条记录的一列信息
        /// </summary>
        /// <param name="WhereParameters">查询条件</param>
        /// <returns>有返回，无返回null</returns>
        public string One(string TableName, string Column, SqlManageWhere WhereParameters, SqlManageOrder OrderParameters=null)
        {
            TableName = DBUtility.Safe.SafeReplace(TableName.Trim());
            Column = DBUtility.Safe.SafeReplace(Column.Trim());
            SqlManageWhere SqlManageWhere1 = new SqlManageWhere();
            string Sql = "select top 1 " + Column + " from " + TableName + "";
            Sql = Sql + " where " + SqlManageWhere1.getstring(WhereParameters);

            if (OrderParameters != null)
            {
                SqlManageOrder SqlManageOrder1 = new SqlManageOrder();
                string order = SqlManageOrder1.getstring(OrderParameters);
                if (order == "")
                {
                    order = "id desc";
                }
                Sql = Sql + " order by  " + order;
            }

            return One(Sql);
        }

        /// <summary>
        /// 搜索（参数），返回指定的一条记录的一列信息
        /// </summary>
        /// <param name="Sql">Id</param>
        /// <returns>有返回，无返回null</returns>
        public string One(string TableName, string Column, int Id)
        {
            TableName = DBUtility.Safe.SafeReplace(TableName.Trim());
            Column = DBUtility.Safe.SafeReplace(Column.Trim());
            SqlManageWhere SqlManageWhere1 = new SqlManageWhere();
            string Sql = "select top 1 " + Column + " from " + TableName + " where Id=" + Id;
            return One(Sql);
        }


        /// <summary>
        /// 搜索（SQL语句），返回指定记录的一列信息,逗号隔开（不能做安全处理）
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>有返回，无返回null</returns>
        public string OneList(string Sql)
        {

            DbCommand com = db.GetSqlStringCommand(Sql);
            DataTable ta = db.ExecuteDataSet(com).Tables[0];
            string str = "";
            if (ta.Rows.Count >= 1)
            {
                for (int i = 0; i < ta.Rows.Count; i++)
                {
                    string a = ta.Rows[i][0].ToString();
                    str = str + a + ",";
                }
                str = str.Trim(',');
                return str;
            }
            return "";
        }

        /// <summary>
        /// 搜索（参数），返回指定记录的一列信息,逗号隔开
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>有返回，无返回null</returns>
        public string OneList(string TableName, string Column, SqlManageWhere WhereParameters)
        {
            TableName = DBUtility.Safe.SafeReplace(TableName.Trim());
            Column = DBUtility.Safe.SafeReplace(Column.Trim());
            SqlManageWhere SqlManageWhere1 = new SqlManageWhere();
            string Sql = "select " + Column + " from " + TableName + "";
            Sql = Sql + " where " + SqlManageWhere1.getstring(WhereParameters);
            return OneList(Sql);
        }

        /// <summary>
        /// 搜索（参数），返回指定记录的一列信息,逗号隔开
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>有返回，无返回null</returns>
        public ArrayList OneArrayList(string TableName, string Column, SqlManageWhere WhereParameters)
        {
            TableName = DBUtility.Safe.SafeReplace(TableName.Trim());
            Column = DBUtility.Safe.SafeReplace(Column.Trim());
            SqlManageWhere SqlManageWhere1 = new SqlManageWhere();
            string Sql = "select " + Column + " from " + TableName + "";
            Sql = Sql + " where " + SqlManageWhere1.getstring(WhereParameters);
            DataTable ta = Sel(Sql);
            ArrayList a = new ArrayList();
            for (int i = 0; i < ta.Rows.Count; i++)
            {
                a.Add(ta.Rows[i][0].ToString());                
            }
            return a;
        }


        /// <summary>
        /// 搜索（SQL语句），返回DataTable结果集（Where条件不能做安全处理）
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
        public DataTable Paging(string TableName, string Column, string Where, string Order, int PageSize, int Page, ref int RecordCount, ref int PageCount)
        {
            TableName = DBUtility.Safe.SafeReplace(TableName.Trim());
            Column = DBUtility.Safe.SafeReplace(Column.Trim());
            Order = Order.Trim() + ",";
            Order = Order.Replace(" ", "");
            Order = Order.Replace("desc,", " desc,");
            Order = Order.Replace("asc,", " asc,");
            Order = Order.Trim(',');

            int n1 = (Page - 1) * PageSize;
            int n2 = Page * PageSize;
            if (Where == "") { Where = "1=1"; }
            if (Order == "")
            {
                Order = " id desc";
            }
            if (Column == "") { Column = "*"; }
            string sql = "SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY  " + Order + " ) AS RowNo," + Column + "  FROM " + TableName + " where  (" + Where + ") ) AS A  where  RowNo >" + n1 + " and RowNo <= " + n2 + "";

            DataTable dt = Sel(sql);
            dt.Columns.Remove("RowNo");
            string sql1 = "select count(*) from " + TableName + " where  " + Where;
            RecordCount = int.Parse(One(sql1));
            PageCount = RecordCount % PageSize != 0 ? RecordCount / PageSize + 1 : RecordCount / PageSize;

            return dt;
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
        public DataTable Paging(string TableName, string Column, SqlManageWhere WhereParameters, SqlManageOrder OrderParameters, int PageSize, int Page, ref int RecordCount, ref int PageCount)
        {
            SqlManageWhere SqlManageWhere1 = new SqlManageWhere();
            SqlManageOrder SqlManageOrder1 = new SqlManageOrder();
            string Where = SqlManageWhere1.getstring(WhereParameters);
            string Order = SqlManageOrder1.getstring(OrderParameters);


            TableName = DBUtility.Safe.SafeReplace(TableName.Trim());
            Column = DBUtility.Safe.SafeReplace(Column.Trim());

            int n1 = (Page - 1) * PageSize;
            int n2 = Page * PageSize;
            if (Where == "") { Where = " 1=1 "; }
            if (Order == "")
            {
                Order = " id desc";
            }
            if (Column == "") { Column = "*"; }

            string sql = "SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY  " + Order + " ) AS RowNo," + Column + "  FROM " + TableName + " where  (" + Where + ") ) AS A  where  RowNo >" + n1 + " and RowNo <= " + n2 + "";

            DataTable dt = Sel(sql);
            dt.Columns.Remove("RowNo");
            string sql1 = "select count(*) from " + TableName + " where  " + Where;
            RecordCount = int.Parse(One(sql1));
            PageCount = RecordCount % PageSize != 0 ? RecordCount / PageSize + 1 : RecordCount / PageSize;

            return dt;
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
        /// <param name="Params">方法参数</param>
        /// <param name="FuParams">全文索引参数</param>
        /// <returns>DataTable结果集</returns>
        public DataTable PagingFu(string TableName, string Column, SqlManageWhere WhereParameters, SqlManageOrder OrderParameters, int PageSize, int Page, ref int RecordCount, ref int PageCount, string[] Params, string[] ConParams = null)
        {
            SqlManageWhere SqlManageWhere1 = new SqlManageWhere();
            SqlManageOrder SqlManageOrder1 = new SqlManageOrder();
            string Where = SqlManageWhere1.getstring(WhereParameters);
            string Order = SqlManageOrder1.getstring(OrderParameters);



            TableName = DBUtility.Safe.SafeReplace(TableName.Trim());
            if (Params.Length > 0)
            {
                TableName += "(";
                for (int i = 0; i < Params.Length; i++)
                {
                    TableName += "'" + DBUtility.Safe.SafeReplace(Params[i].Trim()) + "',";
                }
                if (ConParams.Length > 0)
                {
                    for (int i = 0; i < ConParams.Length; i++)
                    {
                        TableName += "'*" + DBUtility.Safe.FuSafeReplace(ConParams[i].Trim()) + "*',";
                    }
                }
                TableName = TableName.TrimEnd(',');
                TableName += ")";
            }

            //System.Text.StringBuilder TableNameSb = new System.Text.StringBuilder();
            //if (ConParams.Length > 0)
            //{
            //    for (int i = 0; i < ConParams.Length; i++)
            //    {
            //        ConParams[i] = "*" + DBUtility.Safe.FuSafeReplace(ConParams[i].Trim()) + "*";
            //    }
            //    TableNameSb.AppendFormat(TableName, ConParams);
            //    TableName = TableNameSb.ToString();
            //}
            Column = DBUtility.Safe.SafeReplace(Column.Trim());

            int n1 = (Page - 1) * PageSize;
            int n2 = Page * PageSize;
            if (Where == "") { Where = " 1=1 "; }
            if (Order == "")
            {
                Order = " id desc";
            }
            if (Column == "") { Column = "*"; }

            string sql = "SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY  " + Order + " ) AS RowNo," + Column + "  FROM " + TableName + " where  (" + Where + ") ) AS A  where  RowNo >" + n1 + " and RowNo <= " + n2 + "";

            DataTable dt = Sel(sql);
            dt.Columns.Remove("RowNo");
            string sql1 = "select count(*) from " + TableName + " where  " + Where;
            RecordCount = int.Parse(One(sql1));
            PageCount = RecordCount % PageSize != 0 ? RecordCount / PageSize + 1 : RecordCount / PageSize;

            return dt;
        }



        /// <summary>
        /// 执行存储过程，除了存储过程名称外，不传入传出参数（能做安全处理）
        /// </summary>
        /// <param name="SpName">存储过程名</param>
        /// <returns>不返回结果</returns>
        public void Sp(string SpName)
        {
            SpName = DBUtility.Safe.SafeReplace(SpName);
            string spstr = "exec " + SpName;
            Upd(spstr);
        }


        /// <summary>
        /// 执行存储过程，除了存储过程名称外，不传入传出参数（能做安全处理）
        /// </summary>
        /// <param name="SpName">存储过程名</param>
        /// <returns>不返回结果</returns>
        public DataTable SpReturn(string SpName)
        {
            SpName = DBUtility.Safe.SafeReplace(SpName);
            string spstr = "exec " + SpName;
            return Sel(spstr);
        }


        /// <summary>
        /// 执行存储过程，除了存储过程名称外，传入传出参数（不能做安全处理）
        /// </summary>
        /// <param name="SpName">存储过程名</param>
        /// <param name="Parameters">参数</param>
        /// <returns>不返回结果</returns>
        public void Sp(string SpName, ArrayList Parameters)
        {
            SpName = DBUtility.Safe.SafeReplace(SpName);
            //Parameters = DBUtility.Safe.SafeReplace(Parameters);
            string Parameters1 = SpParametersReplace(Parameters);
            string sql = "exec " + SpName + " " + Parameters1 + "";
            Upd(sql);
        }





        /// <summary>
        /// 执行存储过程，除了存储过程名称外，传入传出参数（不能做安全处理）
        /// </summary>
        /// <param name="SpName">存储过程名</param>
        /// <param name="Parameters">参数</param>
        /// <returns>不返回结果</returns>
        public DataTable SpReturn(string SpName, ArrayList Parameters)
        {
            SpName = DBUtility.Safe.SafeReplace(SpName);
            string Parameters1 = SpParametersReplace(Parameters);
            string sql = "exec " + SpName + " " + Parameters1 + "";
            return Sel(sql);
        }

     

        private string SpParametersReplace(ArrayList Parameters)
        {
            string b = "";
            for (int i = 0; i < Parameters.Count; i++)
            {
                string type = Parameters[i].GetType().Name.ToString();
                //if (type.IndexOf("[]") >= 0)//数组，要用in
                //{
                //    if (type == "String[]")
                //    {
                //        string b1 = "";
                //        String[] str = (String[])Parameters[i];
                //        for (int j = 0; j < str.Length; j++)
                //        {
                //            string a = str[j];
                //            a = DBUtility.Safe.SafeReplace(a);
                //            a = "''" + a + "''";
                //            b1 = b1 + a + ",";
                //        }
                //        b1 = b1.Trim(',');
                //        b = b +"'"+b1+"',";
                //    }
                //    if ((type == "Int16[]") || (type == "Int32[]"))
                //    {
                //        string b1 = "";
                //        var aaa = Parameters[i];
                //        int[] str = (int[])Parameters[i];
                //        for (int j = 0; j < str.Length; j++)
                //        {
                //            b1 = b1 + str[j].ToString() + ",";
                //        }
                //        b1 = b1.Trim(',');
                //        b = b + "'" + b1 + "',";
                //    }
                //    if ((type == "Int64[]") || (type == "Long[]"))
                //    {
                //        string b1 = "";
                //        Int64[] str = (Int64[])Parameters[i];
                //        for (int j = 0; j < str.Length; j++)
                //        {
                //            b1 = b1 + str[j] + ",";
                //        }
                //        b1 = b1.Trim(',');
                //        b = b + "'" + b1 + "',";
                //    }
                //    if ((type == "Double[]"))
                //    {
                //        string b1 = "";
                //        double[] str = (double[])Parameters[i];
                //        for (int j = 0; j < str.Length; j++)
                //        {
                //            b1 = b1 + str[j] + ",";
                //        }
                //        b1 = b1.Trim(',');
                //        b = b + "'" + b1 + "',";
                //    }
                //    if (type == "DateTime[]")
                //    {
                //        string b1 = "";
                //        DateTime[] str = (DateTime[])Parameters[i];
                //        for (int j = 0; j < str.Length; j++)
                //        {
                //            string a = str[j].ToString();
                //            a = "''" + a + "''";
                //            b1 = b1 + a + ",";
                //        }
                //        b1 = b1.Trim(',');
                //        b = b +"'"+b1+"',";
                //    }
                //}
                //else
                //{
                //    //string a = Parameters[i].ToString();
                //    //a = DBUtility.Safe.SafeReplace(a);
                //    //a = "'" + a + "'";
                //    //b = b + a + ",";
                    if (type == "String")
                    {
                        string a = Parameters[i].ToString();
                        a = DBUtility.Safe.SafeReplace(a);
                        a = "'" + a + "'";
                        b = b + a + ",";
                    }
                    if ((type == "Int16") || (type == "Int32") || (type == "Int64") || (type == "Double") || (type == "Long"))
                    {
                        string a = DBUtility.Safe.SafeReplace(Parameters[i].ToString());
                        a = Parameters[i].ToString();
                        b = b + a + ",";
                    }
                    if (type == "DateTime")
                    {
                        string a = Parameters[i].ToString();
                        a = "'" + a + "'";
                        b = b + a + ",";
                    }
                }
            //}
            b = b.Trim(',');
            return b;
        }



        

    }
}