using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Common.Base;

namespace DAL
{
    public class DALEmailSetting : BaseDAL
    {

		public DALEmailSetting(){
            TableName = "EmailSetting";
        }
        

        /// <summary>
        /// Linq拼接权限Where
        /// </summary>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <param name="Where">Linq的Where条件</param>
        /// <returns></returns>
        public System.Linq.Expressions.Expression<Func<Model.EmailSetting, bool>> SetPermissions(int Permissions, System.Linq.Expressions.Expression<Func<Model.EmailSetting, bool>> Where)
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
        /// 根据Id获取一条Model.EmailSetting数据
        /// </summary>
        /// <param name="Id">查询的Id</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
        /// <returns>Model.EmailSetting</returns>
        public Model.EmailSetting GetModel(int Id, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.EmailSetting, bool>> Where = PredicateExtensionses.True<Model.EmailSetting>();
				Where = Where.And(p => p.Id == Id);
				
				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.EmailSetting>().Where(Where);

				return Query.ToList<Model.EmailSetting>().DefaultIfEmpty().First<Model.EmailSetting>();
			}
        }

        /// <summary>
        /// 将Json转换成实体类
        /// </summary>
        /// <param name="Json">需要被转换的Json</param>
        /// <remarks>Json中字段为空时注意整形赋值</remarks>
        /// <returns>实体类Model.EmailSetting</returns>
        public Model.EmailSetting GetModel(string Json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return GetModel(JsonSerializer.Deserialize<System.Collections.Hashtable>(Json));
        }


        /// <summary>
        /// 将Hashtable转换成实体类
        /// </summary>
        /// <param name="Ht">需要被转换的Hashtable</param>
        /// <returns>实体类Model.EmailSetting</returns>
        public Model.EmailSetting GetModel(System.Collections.Hashtable Ht,int SafeType=0)
        {
            Model.EmailSetting M = new Model.EmailSetting();

                if (Ht.ContainsKey("Id"))
                {
                    if (Ht["Id"] != null&&!"".Equals(Ht["Id"]))  M.Id = Convert.ToInt32(Ht["Id"]);
                }


                if (Ht.ContainsKey("Account"))
                {
                    M.Account = SafeType == 0 ? Convert.ToString(Ht["Account"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Account"]));
                }


                if (Ht.ContainsKey("Enabled"))
                {
                    if (Ht["Enabled"] != null&&!"".Equals(Ht["Enabled"]))  M.Enabled = Convert.ToInt32(Ht["Enabled"]);
                }


                if (Ht.ContainsKey("Owner"))
                {
                    if (Ht["Owner"] != null&&!"".Equals(Ht["Owner"]))  M.Owner = Convert.ToInt32(Ht["Owner"]);
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
                    if (Ht["DelState"] != null&&!"".Equals(Ht["DelState"]))  M.DelState = Convert.ToInt32(Ht["DelState"]);
                }


                if (Ht.ContainsKey("Addtime"))
                {
                    if(Ht["Addtime"]== null) M.Addtime = null; else if (Ht["Addtime"] != null&&!"".Equals(Ht["Addtime"])) M.Addtime = Convert.ToDateTime(Ht["Addtime"]);
                }


                if (Ht.ContainsKey("ValidateTime"))
                {
                    if(Ht["ValidateTime"]== null) M.ValidateTime = null; else if (Ht["ValidateTime"] != null&&!"".Equals(Ht["ValidateTime"])) M.ValidateTime = Convert.ToDateTime(Ht["ValidateTime"]);
                }




            return M;
        }


        /// <summary>
        /// 将实体类Model.EmailSetting转换成Json
        /// </summary>
        /// <returns>Json字符串</returns>
        public string ModelToJson(Model.EmailSetting M)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return JsonSerializer.Serialize(M);
        }




        ///////////////////////////////////////////添加////////////////////////////////////////////////////



        /// <summary>
        /// 表Model.EmailSetting添加一条数据
        /// </summary>
        /// <param name="M">Model.EmailSetting实体</param>
		/// <param name="Check">是否验证</param>
        /// <returns>新增结果</returns>
        public string Add(Model.EmailSetting M,bool Check=true)
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
        /// 表Model.EmailSetting添加一条数据
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
        /// 表Model.EmailSetting添加一条数据
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
        /// 表Model.EmailSetting添加一条数据
        /// </summary>
        /// <param name="Model">Model.EmailSetting实体</param>
        /// <returns>新增结果</returns>
        private string AddHt(System.Collections.Hashtable Ht)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.EmailSetting Model = GetModel(Ht,1);
	            Model.UserId = User.UserId;
            Model.ParentId = User.ParentId;

				Context.CreateObjectSet<Model.EmailSetting>().AddObject(Model);
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
        /// <returns>新的实体类Model.EmailSetting若权限不足则返回null</returns>
        public Model.EmailSetting HashtableToUpdateModel(System.Collections.Hashtable Ht, int Permissions = 0, int SafeType = 0)
        {

            Model.EmailSetting M = new Model.EmailSetting();

            M = GetModel(Convert.ToInt32(Ht["Id"]), Permissions);

            if (M != null)
            {
                if (Ht.ContainsKey("Account"))
                {
                    M.Account = SafeType == 0 ? Convert.ToString(Ht["Account"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Account"]));
                }

                if (Ht.ContainsKey("Enabled"))
                {
                     M.Enabled = Convert.ToInt32(Ht["Enabled"]);
                }

                if (Ht.ContainsKey("Owner"))
                {
                    if(Ht["Owner"]== null) M.Owner = null; else  M.Owner = Convert.ToInt32(Ht["Owner"]);
                }

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
                    if(Ht["DelState"]== null) M.DelState = null; else  M.DelState = Convert.ToInt32(Ht["DelState"]);
                }

                if (Ht.ContainsKey("Addtime"))
                {
                    if(Ht["Addtime"]== null) M.Addtime = null; else if (Ht["Addtime"] != null&&!"".Equals(Ht["Addtime"])) M.Addtime = Convert.ToDateTime(Ht["Addtime"]);
                }

                if (Ht.ContainsKey("ValidateTime"))
                {
                    if(Ht["ValidateTime"]== null) M.ValidateTime = null; else if (Ht["ValidateTime"] != null&&!"".Equals(Ht["ValidateTime"])) M.ValidateTime = Convert.ToDateTime(Ht["ValidateTime"]);
                }


            }


            return M;
        }

        /// <summary>
        /// 根据Hashtable更新Model.EmailSetting数据
        /// </summary>
        /// <param name="Ht">需要更新的Hashtable</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
        /// <returns>true or false</returns>
        private string UpdateHt(System.Collections.Hashtable Ht, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.EmailSetting M = HashtableToUpdateModel(Ht, Permissions,1);
				if (M == null) return "{\"Type\":-1,\"Message\":\"数据不存在或权限不足\"}";
				Context.EmailSetting.Attach(M);
				Context.ObjectStateManager.ChangeObjectState(M, System.Data.EntityState.Modified);
				int Result = Context.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
				return "{\"Type\":0,\"Message\":\"操作成功\",\"Id\":\"" + M.Id + "\"}";
			}
        }


        /// <summary>
        /// 根据Model更新Model.EmailSetting数据
        /// </summary>
        /// <param name="Model">需要更新的Model</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
		/// <param name="Check">是否验证</param>
        /// <returns>true or false</returns>
        public string Update(Model.EmailSetting M, int Permissions = 0,bool Check=true)
        {
            return Update(ModelToJson(M), Permissions,Check);
        }


        /// <summary>
        /// 根据Hashtable更新Model.EmailSetting数据
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
        /// 根据MyHashTable更新Model.EmailSetting数据
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
        /// 根据Json更新Model.EmailSetting数据
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
        /// 表Model.EmailSetting添加或删除一条数据
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
        /// <returns>EmailSetting的List集合</returns>
        public List<Model.EmailSetting> GetAllList(int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.EmailSetting, bool>> Where = PredicateExtensionses.True<Model.EmailSetting>();

				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.EmailSetting>().Where(Where);

				return Query.ToList<Model.EmailSetting>();
			}
        }


	




    }
}
