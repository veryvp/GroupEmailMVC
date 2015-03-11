using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBUtility;
using System.Data;
using System.Collections;

namespace Web.Components.Base
{
    /// <summary>
    /// 树操作基类
    /// </summary>
    public class TreeOperate:BasePage
    {
        SqlManage helper = new SqlManage();

        private int RecordCount;
        private int PageCount;
        


        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataSource(Hashtable ht) {

            //获取表名
            string TableName = Safe.SafeReplace(ht["TableName"].ToString());
            if (TableName == "") {
                return null;
            }

            ////节点标题
            //string Title = Safe.SafeReplace(ht["Title"].ToString());
            //if (Title == "")
            //{
            //    return null;
            //}

            ////节点主键
            //string NodeKey = Safe.SafeReplace(ht["NodeKey"].ToString());
            //if (NodeKey == "")
            //{
            //    return null;
            //}

            //上级节点
            string SuperKey = Safe.SafeReplace(ht["SuperKey"].ToString());
            if (SuperKey == "")
            {
                return null;
            }

            if (ht["FullyLoad"].ToString() == "") {
                return null;
            }
            //全部加载
            bool FullyLoad = Convert.ToBoolean(ht["FullyLoad"]);         

            if (FullyLoad)
            {
              return helper.Sel("select * from " + TableName + " where UserId =" + UserId);
            }
            else {
                return helper.Sel("select * from " + TableName + " where UserId =" + UserId + " and  " + SuperKey + "=0 ");
            }
        }


        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <returns></returns>
        public DataTable GetChildNode(Hashtable ht)
        {

            //获取表名
            string TableName = Safe.SafeReplace(ht["TableName"].ToString());
            if (TableName == "")
            {
                return null;
            }

            //节点主键
            string SuperId = Safe.SafeReplace(ht["SuperId"].ToString());
            if (SuperId == "")
            {
                return null;
            }

            //上级节点
            string SuperKey = Safe.SafeReplace(ht["SuperKey"].ToString());
            if (SuperKey == "")
            {
                return null;
            }


            return helper.Sel("select * from " + TableName + " where UserId =" + UserId + " and  " + SuperKey + "= " + SuperId);

            //if (ht["FullyLoad"].ToString() == "")
            //{
            //    return null;
            //}
            ////全部加载
            //bool FullyLoad = Convert.ToBoolean(ht["FullyLoad"]);



            //if (FullyLoad)
            //{
            //    return helper.Sel("select * from " + TableName + " where UserId =" + UserId);
            //}
            //else
            //{
            //    return helper.Sel("select * from " + TableName + " where UserId =" + UserId + " and  " + SuperKey + "=0 ");
            //}
        }


        /// <summary>
        /// 获取子节点并分页
        /// </summary>
        /// <returns></returns>
        public DataTable GetChildNodePaging(Hashtable ht)
        {

            //获取表名
            string TableName = Safe.SafeReplace(ht["TableName"].ToString());
            if (TableName == "")
            {
                return null;
            }

            //节点主键
            string SuperId = Safe.SafeReplace(ht["SuperId"].ToString());
            if (SuperId == "")
            {
                return null;
            }

            //上级节点
            string SuperKey = Safe.SafeReplace(ht["SuperKey"].ToString());
            if (SuperKey == "")
            {
                return null;
            }
            //节点
            string NodeKey = Safe.SafeReplace(ht["NodeKey"].ToString());
            if (NodeKey == "")
            {
                return null;
            }

            //页码
            int PageNo = Convert.ToInt32(ht["PageNo"].ToString());

            //页码
            int PageSize = Convert.ToInt32(ht["PageSize"].ToString());

            int n1 = (PageNo - 1) * PageSize;
            int n2 = PageNo * PageSize;
  
            string sql = "SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY  " + NodeKey + " asc ) AS RowNo,*  FROM " + TableName + " where  (UserId =" + UserId + " and  " + SuperKey + "= " + SuperId + ") ) AS A  where  RowNo >" + n1 + " and RowNo <= " + n2 + "";
            DataTable dt = helper.Sel(sql);
            dt.Columns.Remove("RowNo");
            string sql1 = "select count(*) from " + TableName + " where  UserId =" + UserId + " and  " + SuperKey + "= " + SuperId;
            RecordCount = int.Parse(helper.One(sql1));
            PageCount = RecordCount % PageSize != 0 ? RecordCount / PageSize + 1 : RecordCount / PageSize;

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



        /// <summary>
        /// 添加节点
        /// </summary>
        /// <returns></returns>
        public string AddNode(Hashtable ht)
        {

            //获取表名
            string TableName = Safe.SafeReplace(ht["TableName"].ToString());
            if (TableName == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }


            //节点字段名称
            string Title = Safe.SafeReplace(ht["Title"].ToString());
            if (Title == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            //节点名称
            string Name = Safe.SafeReplace(ht["Name"].ToString());
            if (Name == "")
            {
                return "{\"Type\":-1,\"Message\":\"名称不能为空\"}";
            }
            if (Name.Length > 50) {
                return "{\"Type\":-1,\"Message\":\"名称过长\"}";
            }

            if (!Convert.ToBoolean(ht["AASame"]))
            {
                string sameid = helper.One("select top 1 Id from " + TableName + " where " + Title + " = '" + Name + "' and ParentId= " + ParentId);
                if (sameid != null && sameid != "")
                {
                    return "{\"Type\":-1,\"Message\":\"该节点名称已被使用,请重新输入!\"}";
                }
            }


            //上级节点字段名称
            string SuperKey = Safe.SafeReplace(ht["SuperKey"].ToString());
            if (SuperKey == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            //上级树的等级字段
            string LevelKey = Safe.SafeReplace(ht["LevelKey"].ToString());
            if (LevelKey == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            //节点关键字字段名称
            string NodeKey = Safe.SafeReplace(ht["NodeKey"].ToString());
            if (NodeKey == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            //上级节点键值
            string SuperId = "";
            if (ht.ContainsKey("SuperId")) {
                SuperId = Safe.SafeReplace(ht["SuperId"].ToString());
            }
            string SuperLevel = "0";

            if (SuperId != "")
            {
                SuperLevel = helper.One("select [" + LevelKey + "] from " + TableName + " where " + NodeKey + "=" + SuperId + "  and ParentId= " + ParentId);
                if (SuperLevel == null || SuperLevel == "") {
                    return "{\"Type\":-1,\"Message\":\"操作失败\"}";
                }
            }
            else {
                SuperId = "0";
            }


            //插入新的节点
            string NewId = helper.Ins("insert into " + TableName + "(" + Title + "," + SuperKey + ",[" + LevelKey + "],UserId,ParentId)  values('" + Name + "'," + SuperId + "," + (Convert.ToInt16(SuperLevel) + 1) + "," + UserId + "," + ParentId + ")");

            if (NewId != null && NewId != "") {
                if (NodeKey == "Id")
                {
                    return "{\"Type\":0,\"Message\":\"操作成功\",\"" + NodeKey + "\":" + NewId + "}";
                }
                else {
                    string NodeId = helper.One("select " + NodeKey + " from " + TableName + " where Id=" + NewId);
                    return "{\"Type\":0,\"Message\":\"操作成功\",\"" + NodeKey + "\":" + NodeId + "}";
                }
            }

            return "{\"Type\":-1,\"Message\":\"操作失败\"}";

        }



        /// <summary>
        /// 编辑节点
        /// </summary>
        /// <returns></returns>
        public string EditNode(Hashtable ht)
        {

            //获取表名
            string TableName = Safe.SafeReplace(ht["TableName"].ToString());
            if (TableName == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            //节点字段名称
            string Title = Safe.SafeReplace(ht["Title"].ToString());
            if (Title == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            //节点名称
            string Name = Safe.SafeReplace(ht["Name"].ToString());
            if (Name == "")
            {
                return "{\"Type\":-1,\"Message\":\"名称不能为空\"}";
            }
            if (Name.Length > 50)
            {
                return "{\"Type\":-1,\"Message\":\"名称过长\"}";
            }

            //节点关键字字段名称
            string NodeKey = Safe.SafeReplace(ht["NodeKey"].ToString());
            if (NodeKey == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            //节点关键字值
            string NodeId = Safe.SafeReplace(ht["NodeId"].ToString());
            if (NodeId == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            if (!Convert.ToBoolean(ht["AASame"]))
            {
                string sameid = helper.One("select top 1 Id from " + TableName + " where " + Title + " = '" + Name + "' and " + NodeKey + " <> " + NodeId + " and ParentId= " + ParentId);
                if (sameid != null && sameid != "")
                {
                    return "{\"Type\":-1,\"Message\":\"该节点名称已被使用,请重新输入!\"}";
                }
            }

            int result =  helper.Upd("update " + TableName + " set " + Title + "='" + Name + "' where " + NodeKey + " = " + NodeId + " and UserId=" + UserId);

            if (result>0)
            {
                return "{\"Type\":0,\"Message\":\"操作成功\"}";
            }

            return "{\"Type\":-1,\"Message\":\"操作失败\"}";

        }

        /// <summary>
        /// 删除前检测是否有关联数据
        /// </summary>
        /// <returns></returns>
        public bool CheckDelNode(Hashtable ht)
        {

            //获取表名
            string TableName = Safe.SafeReplace(ht["TableName"].ToString());
            if (TableName == "")
            {
                
            }

            //节点关键字值
            string NodeId = Safe.SafeReplace(ht["NodeId"].ToString());
            if (NodeId == "")
            {
                
            }


            return false;
        }



        /// <summary>
        /// 删除节点
        /// </summary>
        /// <returns></returns>
        public string DelNode(Hashtable ht)
        {

            //获取表名
            string TableName = Safe.SafeReplace(ht["TableName"].ToString());
            if (TableName == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            //节点关键字字段名称
            string NodeKey = Safe.SafeReplace(ht["NodeKey"].ToString());
            if (NodeKey == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            //节点关键字值
            string NodeId = Safe.SafeReplace(ht["NodeId"].ToString());
            if (NodeId == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            //上级节点字段名称
            string SuperKey = Safe.SafeReplace(ht["SuperKey"].ToString());
            if (SuperKey == "")
            {
                return "{\"Type\":-1,\"Message\":\"操作失败\"}";
            }

            int Count = 0;

            Count = ReDelNode(TableName, NodeKey, SuperKey, NodeId, Count, Convert.ToBoolean(ht["RealDel"]));

            if (Count > 0)
            {
                return "{\"Type\":0,\"Message\":\"操作成功\"}";
            }

            return "{\"Type\":-1,\"Message\":\"操作失败\"}";
        }

        /// <summary>
        /// 删除节点以及子节点
        /// </summary>
        /// <returns></returns>
        private int ReDelNode(string TableName, string NodeKey, string SuperKey, string NodeId, int Count, bool RealDel)
        {
            string Ids = helper.OneList("select " + NodeKey + " from " + TableName + " where " + SuperKey + " in (" + NodeId + ") and UserId=" + UserId);
            
            if (Ids != null && Ids != "") {
                Count = ReDelNode(TableName, NodeKey, SuperKey, Ids, Count, RealDel);
            }

            if (RealDel)
            {
                Count += helper.Del("delete from " + TableName + " where  " + NodeKey + " in (" + NodeId + ") and UserId=" + UserId);
            }
            else
            {
                Count += helper.Upd("update " + TableName + " set DelState=1 where  " + NodeKey + " in (" + NodeId + ") and UserId=" + UserId);
            }


            return Count;
        }


    }
}