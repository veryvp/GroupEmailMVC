using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Common.Base;

namespace DAL
{
    public class DALCustDataReport : BaseDAL
    {

		public DALCustDataReport(){
            TableName = "CustDataReport";
        }
        

        /// <summary>
        /// Linq拼接权限Where
        /// </summary>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <param name="Where">Linq的Where条件</param>
        /// <returns></returns>
        public System.Linq.Expressions.Expression<Func<Model.CustDataReport, bool>> SetPermissions(int Permissions, System.Linq.Expressions.Expression<Func<Model.CustDataReport, bool>> Where)
        {

            if (Permissions == 0)
            {
                Where = Where.And(p => p.UserId == User.UserId);
            }
            if (Permissions == 1)
            {
                Where = Where.And(p => p.ParentId == User.ParentId);
            }
            if (Permissions == 2)
            {
                Where = Where.And(p => p.UserId == User.UserId && p.ParentId == User.ParentId);
            }
            if (Permissions == 3)
            {
                Where = Where.And(p => p.UserId == User.UserId || p.ParentId == User.ParentId);
            }
            if (Permissions == 4)
            {

            }


            return Where;
        }




		
        /// <summary>
        /// Sql拼接权限Where
        /// </summary>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <param name="Where">Linq的Where条件</param>
        /// <returns></returns>
        public override string SetPermissions(int Permissions)
        {

            if (Permissions == 0)
            {
                return " and UserId = '" + User.UserId + "'";
            }
            if (Permissions == 1)
            {
                return " and ParentId = '" + User.ParentId + "'";
            }
            if (Permissions == 2)
            {
                return " and (UserId = '" + User.UserId + "' and ParentId = '" + User.ParentId + "')";
            }
            if (Permissions == 3)
            {
                return " and (UserId = '" + User.UserId + "' or ParentId = '" + User.ParentId + "')";
            }
            if (Permissions == 4)
            {

            }

        
            return "";
        }



        /////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 根据Id获取一条Model.CustDataReport数据
        /// </summary>
        /// <param name="Id">查询的Id</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
        /// <returns>Model.CustDataReport</returns>
        public Model.CustDataReport GetModel(int Id, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.CustDataReport, bool>> Where = PredicateExtensionses.True<Model.CustDataReport>();
				Where = Where.And(p => p.Id == Id);
				
				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.CustDataReport>().Where(Where);

				return Query.ToList<Model.CustDataReport>().DefaultIfEmpty().First<Model.CustDataReport>();
			}
        }

        /// <summary>
        /// 将Json转换成实体类
        /// </summary>
        /// <param name="Json">需要被转换的Json</param>
        /// <remarks>Json中字段为空时注意整形赋值</remarks>
        /// <returns>实体类Model.CustDataReport</returns>
        public Model.CustDataReport GetModel(string Json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return GetModel(JsonSerializer.Deserialize<System.Collections.Hashtable>(Json));
        }


        /// <summary>
        /// 将Hashtable转换成实体类
        /// </summary>
        /// <param name="Ht">需要被转换的Hashtable</param>
        /// <returns>实体类Model.CustDataReport</returns>
        public Model.CustDataReport GetModel(System.Collections.Hashtable Ht,int SafeType=0)
        {
            Model.CustDataReport M = new Model.CustDataReport();

                if (Ht.ContainsKey("Id"))
                {
                    if (Ht["Id"] != null&&!"".Equals(Ht["Id"]))  M.Id = Convert.ToInt32(Ht["Id"]);
                }


                if (Ht.ContainsKey("UserId"))
                {
                    if (Ht["UserId"] != null&&!"".Equals(Ht["UserId"]))  M.UserId = Convert.ToInt32(Ht["UserId"]);
                }


                if (Ht.ContainsKey("ParentId"))
                {
                    if (Ht["ParentId"] != null&&!"".Equals(Ht["ParentId"]))  M.ParentId = Convert.ToInt32(Ht["ParentId"]);
                }


                if (Ht.ContainsKey("DelState"))
                {
                    if (Ht["DelState"] != null&&!"".Equals(Ht["DelState"]))  M.DelState = Convert.ToByte(Ht["DelState"]);
                }


                if (Ht.ContainsKey("Addtime"))
                {
                    if(Ht["Addtime"]== null) M.Addtime = null; else if (Ht["Addtime"] != null&&!"".Equals(Ht["Addtime"])) M.Addtime = Convert.ToDateTime(Ht["Addtime"]);
                }


                if (Ht.ContainsKey("CustDataTaskId"))
                {
                    if (Ht["CustDataTaskId"] != null&&!"".Equals(Ht["CustDataTaskId"]))  M.CustDataTaskId = Convert.ToInt32(Ht["CustDataTaskId"]);
                }


                if (Ht.ContainsKey("Row"))
                {
                    if (Ht["Row"] != null&&!"".Equals(Ht["Row"]))  M.Row = Convert.ToInt32(Ht["Row"]);
                }


                if (Ht.ContainsKey("ColumnNmae"))
                {
                    if(Ht["ColumnNmae"]== null) M.ColumnNmae = null; else M.ColumnNmae = SafeType == 0 ? Convert.ToString(Ht["ColumnNmae"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["ColumnNmae"]));
                }


                if (Ht.ContainsKey("Message"))
                {
                    if(Ht["Message"]== null) M.Message = null; else M.Message = SafeType == 0 ? Convert.ToString(Ht["Message"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Message"]));
                }


                if (Ht.ContainsKey("UpdateOrAdd"))
                {
                    if(Ht["UpdateOrAdd"]== null) M.UpdateOrAdd = null; else M.UpdateOrAdd = SafeType == 0 ? Convert.ToString(Ht["UpdateOrAdd"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["UpdateOrAdd"]));
                }


                if (Ht.ContainsKey("SendTime"))
                {
                    if(Ht["SendTime"]== null) M.SendTime = null; else if (Ht["SendTime"] != null&&!"".Equals(Ht["SendTime"])) M.SendTime = Convert.ToDateTime(Ht["SendTime"]);
                }


                if (Ht.ContainsKey("DeliveryTime"))
                {
                    if(Ht["DeliveryTime"]== null) M.DeliveryTime = null; else if (Ht["DeliveryTime"] != null&&!"".Equals(Ht["DeliveryTime"])) M.DeliveryTime = Convert.ToDateTime(Ht["DeliveryTime"]);
                }


                if (Ht.ContainsKey("OpenTime"))
                {
                    if(Ht["OpenTime"]== null) M.OpenTime = null; else if (Ht["OpenTime"] != null&&!"".Equals(Ht["OpenTime"])) M.OpenTime = Convert.ToDateTime(Ht["OpenTime"]);
                }


                if (Ht.ContainsKey("ClickTime"))
                {
                    if(Ht["ClickTime"]== null) M.ClickTime = null; else if (Ht["ClickTime"] != null&&!"".Equals(Ht["ClickTime"])) M.ClickTime = Convert.ToDateTime(Ht["ClickTime"]);
                }


                if (Ht.ContainsKey("UnsubscribedTime"))
                {
                    if(Ht["UnsubscribedTime"]== null) M.UnsubscribedTime = null; else if (Ht["UnsubscribedTime"] != null&&!"".Equals(Ht["UnsubscribedTime"])) M.UnsubscribedTime = Convert.ToDateTime(Ht["UnsubscribedTime"]);
                }


                if (Ht.ContainsKey("FailureTime"))
                {
                    if(Ht["FailureTime"]== null) M.FailureTime = null; else if (Ht["FailureTime"] != null&&!"".Equals(Ht["FailureTime"])) M.FailureTime = Convert.ToDateTime(Ht["FailureTime"]);
                }


                if (Ht.ContainsKey("BouncedTime"))
                {
                    if(Ht["BouncedTime"]== null) M.BouncedTime = null; else if (Ht["BouncedTime"] != null&&!"".Equals(Ht["BouncedTime"])) M.BouncedTime = Convert.ToDateTime(Ht["BouncedTime"]);
                }


                if (Ht.ContainsKey("BouncedType"))
                {
                    if (Ht["BouncedType"] != null&&!"".Equals(Ht["BouncedType"]))  M.BouncedType = Convert.ToByte(Ht["BouncedType"]);
                }


                if (Ht.ContainsKey("Platform"))
                {
                    if(Ht["Platform"]== null) M.Platform = null; else M.Platform = SafeType == 0 ? Convert.ToString(Ht["Platform"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Platform"]));
                }


                if (Ht.ContainsKey("Name"))
                {
                    if(Ht["Name"]== null) M.Name = null; else M.Name = SafeType == 0 ? Convert.ToString(Ht["Name"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Name"]));
                }


                if (Ht.ContainsKey("Email"))
                {
                    if(Ht["Email"]== null) M.Email = null; else M.Email = SafeType == 0 ? Convert.ToString(Ht["Email"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Email"]));
                }




            return M;
        }


        /// <summary>
        /// 将实体类Model.CustDataReport转换成Json
        /// </summary>
        /// <returns>Json字符串</returns>
        public string ModelToJson(Model.CustDataReport M)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return JsonSerializer.Serialize(M);
        }




        ///////////////////////////////////////////添加////////////////////////////////////////////////////



        /// <summary>
        /// 表Model.CustDataReport添加一条数据
        /// </summary>
        /// <param name="M">Model.CustDataReport实体</param>
		/// <param name="Check">是否验证</param>
        /// <returns>新增结果</returns>
        public string Add(Model.CustDataReport M,bool Check=true)
        {
            return Add(ModelToJson(M),Check);
        }


        /// <summary>
        /// 根据Json字符串添加一条数据
        /// </summary>
        /// <param name="string">Json字符串</param>
		/// <param name="Check">是否验证</param>
        /// <returns>新增结果</returns>
        public string Add(string Json,bool Check=true)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			JsonHashtable JsonHashtable = new JsonHashtable();

            return Add(JsonHashtable.RestoreValue(JsonSerializer.Deserialize<System.Collections.Hashtable>(Json)),Check); ;
        }


        /// <summary>
        /// 表Model.CustDataReport添加一条数据
        /// </summary>
        /// <param name="Ht">需要添加的Hashtable</param>
		/// <param name="Check">是否验证</param>
        /// <returns>新增结果</returns>
        public string Add(System.Collections.Hashtable Ht,bool Check=true)
        {
            //剔除创建时间
            if (Ht.ContainsKey("AddTime"))
                Ht.Remove("AddTime");
			if(Check){
				//验证字段是否符合规范
				string MessageResult = Verify(Ht, 0);//0:添加 1:更新

				if (MessageResult != null)
				{
					return MessageResult;
				}
			}
            return AddHt(Ht);
        }


        /// <summary>
        /// 表Model.CustDataReport添加一条数据
        /// </summary>
        /// <param name="Ht">需要添加的MyHashTable</param>
		/// <param name="Check">是否验证</param>
        /// <returns>新增数据的Id</returns>
        public string Add(Common.Base.MyHashTable Ht,bool Check=true)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return Add((System.Collections.Hashtable)Ht,Check);
        }


        /// <summary>
        /// 表Model.CustDataReport添加一条数据
        /// </summary>
        /// <param name="Model">Model.CustDataReport实体</param>
        /// <returns>新增结果</returns>
        private string AddHt(System.Collections.Hashtable Ht)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.CustDataReport Model = GetModel(Ht,1);
	            Model.UserId = User.UserId;
            Model.ParentId = User.ParentId;

				Context.CreateObjectSet<Model.CustDataReport>().AddObject(Model);
				Context.SaveChanges();

				return "{\"Type\":0,\"Message\":\"操作成功\",\"Id\":\"" + Model.Id + "\"}";
			}
        }


        ////////////////////////////////////修改///////////////////////////////////////


        /// <summary>
        /// 将Hashtable转换成更新用的实体类
        /// </summary>
        /// <param name="Ht">需要被转换的Hashtable</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
		/// <param name="SafeType">是否做安全处理</param>
        /// <remarks>调用时注意剔除创建时间</remarks>
        /// <returns>新的实体类Model.CustDataReport若权限不足则返回null</returns>
        public Model.CustDataReport HashtableToUpdateModel(System.Collections.Hashtable Ht, int Permissions = 0, int SafeType = 0)
        {

            Model.CustDataReport M = new Model.CustDataReport();

            M = GetModel(Convert.ToInt32(Ht["Id"]), Permissions);

            if (M != null)
            {
                if (Ht.ContainsKey("UserId"))
                {
                    if(Ht["UserId"]== null) M.UserId = null; else  M.UserId = Convert.ToInt32(Ht["UserId"]);
                }

                if (Ht.ContainsKey("ParentId"))
                {
                    if(Ht["ParentId"]== null) M.ParentId = null; else  M.ParentId = Convert.ToInt32(Ht["ParentId"]);
                }

                if (Ht.ContainsKey("DelState"))
                {
                    if(Ht["DelState"]== null) M.DelState = null; else  M.DelState = Convert.ToByte(Ht["DelState"]);
                }

                if (Ht.ContainsKey("Addtime"))
                {
                    if(Ht["Addtime"]== null) M.Addtime = null; else if (Ht["Addtime"] != null&&!"".Equals(Ht["Addtime"])) M.Addtime = Convert.ToDateTime(Ht["Addtime"]);
                }

                if (Ht.ContainsKey("CustDataTaskId"))
                {
                    if(Ht["CustDataTaskId"]== null) M.CustDataTaskId = null; else  M.CustDataTaskId = Convert.ToInt32(Ht["CustDataTaskId"]);
                }

                if (Ht.ContainsKey("Row"))
                {
                    if(Ht["Row"]== null) M.Row = null; else  M.Row = Convert.ToInt32(Ht["Row"]);
                }

                if (Ht.ContainsKey("ColumnNmae"))
                {
                    if(Ht["ColumnNmae"]== null) M.ColumnNmae = null; else M.ColumnNmae = SafeType == 0 ? Convert.ToString(Ht["ColumnNmae"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["ColumnNmae"]));
                }

                if (Ht.ContainsKey("Message"))
                {
                    if(Ht["Message"]== null) M.Message = null; else M.Message = SafeType == 0 ? Convert.ToString(Ht["Message"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Message"]));
                }

                if (Ht.ContainsKey("UpdateOrAdd"))
                {
                    if(Ht["UpdateOrAdd"]== null) M.UpdateOrAdd = null; else M.UpdateOrAdd = SafeType == 0 ? Convert.ToString(Ht["UpdateOrAdd"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["UpdateOrAdd"]));
                }

                if (Ht.ContainsKey("SendTime"))
                {
                    if(Ht["SendTime"]== null) M.SendTime = null; else if (Ht["SendTime"] != null&&!"".Equals(Ht["SendTime"])) M.SendTime = Convert.ToDateTime(Ht["SendTime"]);
                }

                if (Ht.ContainsKey("DeliveryTime"))
                {
                    if(Ht["DeliveryTime"]== null) M.DeliveryTime = null; else if (Ht["DeliveryTime"] != null&&!"".Equals(Ht["DeliveryTime"])) M.DeliveryTime = Convert.ToDateTime(Ht["DeliveryTime"]);
                }

                if (Ht.ContainsKey("OpenTime"))
                {
                    if(Ht["OpenTime"]== null) M.OpenTime = null; else if (Ht["OpenTime"] != null&&!"".Equals(Ht["OpenTime"])) M.OpenTime = Convert.ToDateTime(Ht["OpenTime"]);
                }

                if (Ht.ContainsKey("ClickTime"))
                {
                    if(Ht["ClickTime"]== null) M.ClickTime = null; else if (Ht["ClickTime"] != null&&!"".Equals(Ht["ClickTime"])) M.ClickTime = Convert.ToDateTime(Ht["ClickTime"]);
                }

                if (Ht.ContainsKey("UnsubscribedTime"))
                {
                    if(Ht["UnsubscribedTime"]== null) M.UnsubscribedTime = null; else if (Ht["UnsubscribedTime"] != null&&!"".Equals(Ht["UnsubscribedTime"])) M.UnsubscribedTime = Convert.ToDateTime(Ht["UnsubscribedTime"]);
                }

                if (Ht.ContainsKey("FailureTime"))
                {
                    if(Ht["FailureTime"]== null) M.FailureTime = null; else if (Ht["FailureTime"] != null&&!"".Equals(Ht["FailureTime"])) M.FailureTime = Convert.ToDateTime(Ht["FailureTime"]);
                }

                if (Ht.ContainsKey("BouncedTime"))
                {
                    if(Ht["BouncedTime"]== null) M.BouncedTime = null; else if (Ht["BouncedTime"] != null&&!"".Equals(Ht["BouncedTime"])) M.BouncedTime = Convert.ToDateTime(Ht["BouncedTime"]);
                }

                if (Ht.ContainsKey("BouncedType"))
                {
                    if(Ht["BouncedType"]== null) M.BouncedType = null; else  M.BouncedType = Convert.ToByte(Ht["BouncedType"]);
                }

                if (Ht.ContainsKey("Platform"))
                {
                    if(Ht["Platform"]== null) M.Platform = null; else M.Platform = SafeType == 0 ? Convert.ToString(Ht["Platform"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Platform"]));
                }

                if (Ht.ContainsKey("Name"))
                {
                    if(Ht["Name"]== null) M.Name = null; else M.Name = SafeType == 0 ? Convert.ToString(Ht["Name"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Name"]));
                }

                if (Ht.ContainsKey("Email"))
                {
                    if(Ht["Email"]== null) M.Email = null; else M.Email = SafeType == 0 ? Convert.ToString(Ht["Email"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Email"]));
                }


            }


            return M;
        }

        /// <summary>
        /// 根据Hashtable更新Model.CustDataReport数据
        /// </summary>
        /// <param name="Ht">需要更新的Hashtable</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
        /// <returns>true or false</returns>
        private string UpdateHt(System.Collections.Hashtable Ht, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.CustDataReport M = HashtableToUpdateModel(Ht, Permissions,1);
				if (M == null) return "{\"Type\":-1,\"Message\":\"数据不存在或权限不足\"}";
				Context.CustDataReport.Attach(M);
				Context.ObjectStateManager.ChangeObjectState(M, System.Data.EntityState.Modified);
				int Result = Context.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
				return "{\"Type\":0,\"Message\":\"操作成功\",\"Id\":\"" + M.Id + "\"}";
			}
        }


        /// <summary>
        /// 根据Model更新Model.CustDataReport数据
        /// </summary>
        /// <param name="Model">需要更新的Model</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
		/// <param name="Check">是否验证</param>
        /// <returns>true or false</returns>
        public string Update(Model.CustDataReport M, int Permissions = 0,bool Check=true)
        {
            return Update(ModelToJson(M), Permissions,Check);
        }


        /// <summary>
        /// 根据Hashtable更新Model.CustDataReport数据
        /// </summary>
        /// <param name="Ht">需要传入的Hashtable</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
		/// <param name="Check">是否验证</param>
        /// <returns>true or false</returns>
        public string Update(System.Collections.Hashtable Ht, int Permissions = 0,bool Check=true)
        {
            //剔除创建时间
            if (Ht.ContainsKey("AddTime"))
                Ht.Remove("AddTime");
			if(Check){
				//验证字段是否符合规范
				string MessageResult = Verify(Ht, 1); //0:添加 1:更新

				if (MessageResult != null)
				{
					return MessageResult;
				}
			}
            return UpdateHt(Ht, Permissions);
        }


        /// <summary>
        /// 根据MyHashTable更新Model.CustDataReport数据
        /// </summary>
        /// <param name="Ht">需要传入的MyHashTable</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
		/// <param name="Check">是否验证</param>
        /// <returns>true or false</returns>
        public string Update(Common.Base.MyHashTable Ht, int Permissions = 0,bool Check=true)
        {
            return Update((System.Collections.Hashtable)Ht, Permissions,Check);
        }



        /// <summary>
        /// 根据Json更新Model.CustDataReport数据
        /// </summary>
        /// <param name="Json">需要传入的Json</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
		/// <param name="Check">是否验证</param>
        /// <returns>true or false</returns>
        public string Update(string Json, int Permissions = 0,bool Check=true)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			JsonHashtable JsonHashtable = new JsonHashtable();

            return Update(JsonHashtable.RestoreValue(JsonSerializer.Deserialize<System.Collections.Hashtable>(DBUtility.Safe.SafeReplace(Json))), Permissions,Check);
        }



		/// <summary>
        /// 表Model.CustDataReport添加或删除一条数据
        /// </summary>
        /// <param name="Ht">需要添加的Hashtable</param>
		/// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
		/// <param name="Check">是否验证</param>
        /// <returns>新增结果</returns>
        public string AddOrUpdate(System.Collections.Hashtable Ht, int Permissions = 0 , bool Check=true)
        {
            if (Ht.Contains("Id"))
            {
                string Id = Ht["Id"].ToString().Trim();
                if (Id == "") { return "{\"Type\":-1,\"Message\":\"不能处理未指定信息\"}"; }
                return Update(Ht, Permissions, Check);
            }
            else
            {
                return Add(Ht, Check);
            }


        }




        /////////////////////////////////////////////////查询//////////////////////////////////////////////////////////////

        /// <summary>
        /// 获取表所有数据
        /// </summary>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <returns>CustDataReport的List集合</returns>
        public List<Model.CustDataReport> GetAllList(int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.CustDataReport, bool>> Where = PredicateExtensionses.True<Model.CustDataReport>();

				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.CustDataReport>().Where(Where);

				return Query.ToList<Model.CustDataReport>();
			}
        }


	




    }
}
