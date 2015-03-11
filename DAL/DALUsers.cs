using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Common.Base;

namespace DAL
{
    public class DALUsers : BaseDAL
    {

		public DALUsers(){
            TableName = "Users";
        }
        

        /// <summary>
        /// Linq拼接权限Where
        /// </summary>
        /// <param name="Permissions">权限类别0: UserId 1: ParentId 2: UserId和ParentId 3: UserId或ParentId 4: 无附加条件</param>
        /// <param name="Where">Linq的Where条件</param>
        /// <returns></returns>
        public System.Linq.Expressions.Expression<Func<Model.Users, bool>> SetPermissions(int Permissions, System.Linq.Expressions.Expression<Func<Model.Users, bool>> Where)
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
        /// 根据Id获取一条Model.Users数据
        /// </summary>
        /// <param name="Id">查询的Id</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
        /// <returns>Model.Users</returns>
        public Model.Users GetModel(int Id, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.Users, bool>> Where = PredicateExtensionses.True<Model.Users>();
				Where = Where.And(p => p.Id == Id);
				
				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.Users>().Where(Where);

				return Query.ToList<Model.Users>().DefaultIfEmpty().First<Model.Users>();
			}
        }

        /// <summary>
        /// 将Json转换成实体类
        /// </summary>
        /// <param name="Json">需要被转换的Json</param>
        /// <remarks>Json中字段为空时注意整形赋值</remarks>
        /// <returns>实体类Model.Users</returns>
        public Model.Users GetModel(string Json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return GetModel(JsonSerializer.Deserialize<System.Collections.Hashtable>(Json));
        }


        /// <summary>
        /// 将Hashtable转换成实体类
        /// </summary>
        /// <param name="Ht">需要被转换的Hashtable</param>
        /// <returns>实体类Model.Users</returns>
        public Model.Users GetModel(System.Collections.Hashtable Ht,int SafeType=0)
        {
            Model.Users M = new Model.Users();

                if (Ht.ContainsKey("Id"))
                {
                    if (Ht["Id"] != null&&!"".Equals(Ht["Id"]))  M.Id = Convert.ToInt32(Ht["Id"]);
                }


                if (Ht.ContainsKey("UserNo"))
                {
                    if(Ht["UserNo"]== null) M.UserNo = null; else M.UserNo = SafeType == 0 ? Convert.ToString(Ht["UserNo"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["UserNo"]));
                }


                if (Ht.ContainsKey("UserName"))
                {
                    if(Ht["UserName"]== null) M.UserName = null; else M.UserName = SafeType == 0 ? Convert.ToString(Ht["UserName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["UserName"]));
                }


                if (Ht.ContainsKey("Password"))
                {
                    M.Password = SafeType == 0 ? Convert.ToString(Ht["Password"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Password"]));
                }


                if (Ht.ContainsKey("TypeId"))
                {
                    if (Ht["TypeId"] != null&&!"".Equals(Ht["TypeId"]))  M.TypeId = Convert.ToInt32(Ht["TypeId"]);
                }


                if (Ht.ContainsKey("SubClassId"))
                {
                    if (Ht["SubClassId"] != null&&!"".Equals(Ht["SubClassId"]))  M.SubClassId = Convert.ToInt32(Ht["SubClassId"]);
                }


                if (Ht.ContainsKey("RealName"))
                {
                    if(Ht["RealName"]== null) M.RealName = null; else M.RealName = SafeType == 0 ? Convert.ToString(Ht["RealName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["RealName"]));
                }


                if (Ht.ContainsKey("Sex"))
                {
                    if (Ht["Sex"] != null&&!"".Equals(Ht["Sex"]))  M.Sex = Convert.ToInt32(Ht["Sex"]);
                }


                if (Ht.ContainsKey("GroupId"))
                {
                    if (Ht["GroupId"] != null&&!"".Equals(Ht["GroupId"]))  M.GroupId = Convert.ToInt32(Ht["GroupId"]);
                }


                if (Ht.ContainsKey("Address"))
                {
                    if(Ht["Address"]== null) M.Address = null; else M.Address = SafeType == 0 ? Convert.ToString(Ht["Address"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Address"]));
                }


                if (Ht.ContainsKey("Tel"))
                {
                    if(Ht["Tel"]== null) M.Tel = null; else M.Tel = SafeType == 0 ? Convert.ToString(Ht["Tel"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Tel"]));
                }


                if (Ht.ContainsKey("Email"))
                {
                    if(Ht["Email"]== null) M.Email = null; else M.Email = SafeType == 0 ? Convert.ToString(Ht["Email"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Email"]));
                }


                if (Ht.ContainsKey("Birthday"))
                {
                    if(Ht["Birthday"]== null) M.Birthday = null; else if (Ht["Birthday"] != null&&!"".Equals(Ht["Birthday"])) M.Birthday = Convert.ToDateTime(Ht["Birthday"]);
                }


                if (Ht.ContainsKey("QQ"))
                {
                    if(Ht["QQ"]== null) M.QQ = null; else M.QQ = SafeType == 0 ? Convert.ToString(Ht["QQ"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["QQ"]));
                }


                if (Ht.ContainsKey("WangWang"))
                {
                    if(Ht["WangWang"]== null) M.WangWang = null; else M.WangWang = SafeType == 0 ? Convert.ToString(Ht["WangWang"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["WangWang"]));
                }


                if (Ht.ContainsKey("LatelyLogin"))
                {
                    if(Ht["LatelyLogin"]== null) M.LatelyLogin = null; else M.LatelyLogin = SafeType == 0 ? Convert.ToString(Ht["LatelyLogin"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["LatelyLogin"]));
                }


                if (Ht.ContainsKey("Description"))
                {
                    if(Ht["Description"]== null) M.Description = null; else M.Description = SafeType == 0 ? Convert.ToString(Ht["Description"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Description"]));
                }


                if (Ht.ContainsKey("Status"))
                {
                    if (Ht["Status"] != null&&!"".Equals(Ht["Status"]))  M.Status = Convert.ToInt32(Ht["Status"]);
                }


                if (Ht.ContainsKey("ParentId"))
                {
                    if (Ht["ParentId"] != null&&!"".Equals(Ht["ParentId"]))  M.ParentId = Convert.ToInt32(Ht["ParentId"]);
                }


                if (Ht.ContainsKey("LatestLoginTime"))
                {
                    if(Ht["LatestLoginTime"]== null) M.LatestLoginTime = null; else if (Ht["LatestLoginTime"] != null&&!"".Equals(Ht["LatestLoginTime"])) M.LatestLoginTime = Convert.ToDateTime(Ht["LatestLoginTime"]);
                }


                if (Ht.ContainsKey("LatestLoginIP"))
                {
                    if(Ht["LatestLoginIP"]== null) M.LatestLoginIP = null; else M.LatestLoginIP = SafeType == 0 ? Convert.ToString(Ht["LatestLoginIP"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["LatestLoginIP"]));
                }


                if (Ht.ContainsKey("yqCode"))
                {
                    if(Ht["yqCode"]== null) M.yqCode = null; else M.yqCode = SafeType == 0 ? Convert.ToString(Ht["yqCode"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["yqCode"]));
                }


                if (Ht.ContainsKey("PhoneNo"))
                {
                    if(Ht["PhoneNo"]== null) M.PhoneNo = null; else M.PhoneNo = SafeType == 0 ? Convert.ToString(Ht["PhoneNo"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["PhoneNo"]));
                }


                if (Ht.ContainsKey("FaxNo"))
                {
                    if(Ht["FaxNo"]== null) M.FaxNo = null; else M.FaxNo = SafeType == 0 ? Convert.ToString(Ht["FaxNo"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["FaxNo"]));
                }


                if (Ht.ContainsKey("Vmoney"))
                {
                    if (Ht["Vmoney"] != null&&!"".Equals(Ht["Vmoney"]))  M.Vmoney = Convert.ToDecimal(Ht["Vmoney"]);
                }


                if (Ht.ContainsKey("Homesite"))
                {
                    if(Ht["Homesite"]== null) M.Homesite = null; else M.Homesite = SafeType == 0 ? Convert.ToString(Ht["Homesite"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Homesite"]));
                }


                if (Ht.ContainsKey("ManagerName"))
                {
                    if(Ht["ManagerName"]== null) M.ManagerName = null; else M.ManagerName = SafeType == 0 ? Convert.ToString(Ht["ManagerName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["ManagerName"]));
                }


                if (Ht.ContainsKey("RefUserNo"))
                {
                    if(Ht["RefUserNo"]== null) M.RefUserNo = null; else M.RefUserNo = SafeType == 0 ? Convert.ToString(Ht["RefUserNo"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["RefUserNo"]));
                }


                if (Ht.ContainsKey("NickName"))
                {
                    if(Ht["NickName"]== null) M.NickName = null; else M.NickName = SafeType == 0 ? Convert.ToString(Ht["NickName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["NickName"]));
                }


                if (Ht.ContainsKey("EnglishName"))
                {
                    if(Ht["EnglishName"]== null) M.EnglishName = null; else M.EnglishName = SafeType == 0 ? Convert.ToString(Ht["EnglishName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["EnglishName"]));
                }


                if (Ht.ContainsKey("IdNumber"))
                {
                    if(Ht["IdNumber"]== null) M.IdNumber = null; else M.IdNumber = SafeType == 0 ? Convert.ToString(Ht["IdNumber"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["IdNumber"]));
                }


                if (Ht.ContainsKey("LoginWay"))
                {
                    if (Ht["LoginWay"] != null&&!"".Equals(Ht["LoginWay"]))  M.LoginWay = Convert.ToInt32(Ht["LoginWay"]);
                }


                if (Ht.ContainsKey("UserId"))
                {
                    if (Ht["UserId"] != null&&!"".Equals(Ht["UserId"]))  M.UserId = Convert.ToInt32(Ht["UserId"]);
                }


                if (Ht.ContainsKey("Department"))
                {
                    if(Ht["Department"]== null) M.Department = null; else M.Department = SafeType == 0 ? Convert.ToString(Ht["Department"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Department"]));
                }


                if (Ht.ContainsKey("Position"))
                {
                    if(Ht["Position"]== null) M.Position = null; else M.Position = SafeType == 0 ? Convert.ToString(Ht["Position"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Position"]));
                }


                if (Ht.ContainsKey("Hobby"))
                {
                    if(Ht["Hobby"]== null) M.Hobby = null; else M.Hobby = SafeType == 0 ? Convert.ToString(Ht["Hobby"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Hobby"]));
                }


                if (Ht.ContainsKey("HeadPortrait"))
                {
                    if(Ht["HeadPortrait"]== null) M.HeadPortrait = null; else M.HeadPortrait = SafeType == 0 ? Convert.ToString(Ht["HeadPortrait"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["HeadPortrait"]));
                }


                if (Ht.ContainsKey("StateBirthday"))
                {
                    if (Ht["StateBirthday"] != null&&!"".Equals(Ht["StateBirthday"]))  M.StateBirthday = Convert.ToInt32(Ht["StateBirthday"]);
                }


                if (Ht.ContainsKey("DelState"))
                {
                    if (Ht["DelState"] != null&&!"".Equals(Ht["DelState"]))  M.DelState = Convert.ToInt32(Ht["DelState"]);
                }


                if (Ht.ContainsKey("Addtime"))
                {
                    if(Ht["Addtime"]== null) M.Addtime = null; else if (Ht["Addtime"] != null&&!"".Equals(Ht["Addtime"])) M.Addtime = Convert.ToDateTime(Ht["Addtime"]);
                }


                if (Ht.ContainsKey("UserCode"))
                {
                    if(Ht["UserCode"]== null) M.UserCode = null; else M.UserCode = SafeType == 0 ? Convert.ToString(Ht["UserCode"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["UserCode"]));
                }


                if (Ht.ContainsKey("CheckCode"))
                {
                    if(Ht["CheckCode"]== null) M.CheckCode = null; else M.CheckCode = SafeType == 0 ? Convert.ToString(Ht["CheckCode"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["CheckCode"]));
                }


                if (Ht.ContainsKey("CodeTime"))
                {
                    if(Ht["CodeTime"]== null) M.CodeTime = null; else if (Ht["CodeTime"] != null&&!"".Equals(Ht["CodeTime"])) M.CodeTime = Convert.ToDateTime(Ht["CodeTime"]);
                }


                if (Ht.ContainsKey("CodeState"))
                {
                    if (Ht["CodeState"] != null&&!"".Equals(Ht["CodeState"]))  M.CodeState = Convert.ToInt32(Ht["CodeState"]);
                }


                if (Ht.ContainsKey("TakeState"))
                {
                    if (Ht["TakeState"] != null&&!"".Equals(Ht["TakeState"]))  M.TakeState = Convert.ToInt32(Ht["TakeState"]);
                }


                if (Ht.ContainsKey("Actid"))
                {
                    if (Ht["Actid"] != null&&!"".Equals(Ht["Actid"]))  M.Actid = Convert.ToInt16(Ht["Actid"]);
                }


                if (Ht.ContainsKey("LoginCount"))
                {
                    if (Ht["LoginCount"] != null&&!"".Equals(Ht["LoginCount"]))  M.LoginCount = Convert.ToInt32(Ht["LoginCount"]);
                }


                if (Ht.ContainsKey("modify"))
                {
                    if(Ht["modify"]== null) M.modify = null; else if (Ht["modify"] != null&&!"".Equals(Ht["modify"])) M.modify = Convert.ToDateTime(Ht["modify"]);
                }


                if (Ht.ContainsKey("referrer"))
                {
                    if (Ht["referrer"] != null&&!"".Equals(Ht["referrer"]))  M.referrer = Convert.ToInt32(Ht["referrer"]);
                }


                if (Ht.ContainsKey("TaobaoID"))
                {
                    if(Ht["TaobaoID"]== null) M.TaobaoID = null; else M.TaobaoID = SafeType == 0 ? Convert.ToString(Ht["TaobaoID"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["TaobaoID"]));
                }


                if (Ht.ContainsKey("taobao_user_nick"))
                {
                    if(Ht["taobao_user_nick"]== null) M.taobao_user_nick = null; else M.taobao_user_nick = SafeType == 0 ? Convert.ToString(Ht["taobao_user_nick"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["taobao_user_nick"]));
                }


                if (Ht.ContainsKey("social_uid"))
                {
                    if(Ht["social_uid"]== null) M.social_uid = null; else M.social_uid = SafeType == 0 ? Convert.ToString(Ht["social_uid"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["social_uid"]));
                }


                if (Ht.ContainsKey("social_user_nick"))
                {
                    if(Ht["social_user_nick"]== null) M.social_user_nick = null; else M.social_user_nick = SafeType == 0 ? Convert.ToString(Ht["social_user_nick"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["social_user_nick"]));
                }


                if (Ht.ContainsKey("openid"))
                {
                    if(Ht["openid"]== null) M.openid = null; else M.openid = SafeType == 0 ? Convert.ToString(Ht["openid"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["openid"]));
                }


                if (Ht.ContainsKey("open_user_nick"))
                {
                    if(Ht["open_user_nick"]== null) M.open_user_nick = null; else M.open_user_nick = SafeType == 0 ? Convert.ToString(Ht["open_user_nick"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["open_user_nick"]));
                }




            return M;
        }


        /// <summary>
        /// 将实体类Model.Users转换成Json
        /// </summary>
        /// <returns>Json字符串</returns>
        public string ModelToJson(Model.Users M)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return JsonSerializer.Serialize(M);
        }




        ///////////////////////////////////////////添加////////////////////////////////////////////////////



        /// <summary>
        /// 表Model.Users添加一条数据
        /// </summary>
        /// <param name="M">Model.Users实体</param>
		/// <param name="Check">是否验证</param>
        /// <returns>新增结果</returns>
        public string Add(Model.Users M,bool Check=true)
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
        /// 表Model.Users添加一条数据
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
        /// 表Model.Users添加一条数据
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
        /// 表Model.Users添加一条数据
        /// </summary>
        /// <param name="Model">Model.Users实体</param>
        /// <returns>新增结果</returns>
        private string AddHt(System.Collections.Hashtable Ht)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.Users Model = GetModel(Ht,1);
	            Model.UserId = User.UserId;
            Model.ParentId = User.ParentId;

				Context.CreateObjectSet<Model.Users>().AddObject(Model);
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
        /// <returns>新的实体类Model.Users若权限不足则返回null</returns>
        public Model.Users HashtableToUpdateModel(System.Collections.Hashtable Ht, int Permissions = 0, int SafeType = 0)
        {

            Model.Users M = new Model.Users();

            M = GetModel(Convert.ToInt32(Ht["Id"]), Permissions);

            if (M != null)
            {
                if (Ht.ContainsKey("UserNo"))
                {
                    if(Ht["UserNo"]== null) M.UserNo = null; else M.UserNo = SafeType == 0 ? Convert.ToString(Ht["UserNo"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["UserNo"]));
                }

                if (Ht.ContainsKey("UserName"))
                {
                    if(Ht["UserName"]== null) M.UserName = null; else M.UserName = SafeType == 0 ? Convert.ToString(Ht["UserName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["UserName"]));
                }

                if (Ht.ContainsKey("Password"))
                {
                    M.Password = SafeType == 0 ? Convert.ToString(Ht["Password"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Password"]));
                }

                if (Ht.ContainsKey("TypeId"))
                {
                    if(Ht["TypeId"]== null) M.TypeId = null; else  M.TypeId = Convert.ToInt32(Ht["TypeId"]);
                }

                if (Ht.ContainsKey("SubClassId"))
                {
                    if(Ht["SubClassId"]== null) M.SubClassId = null; else  M.SubClassId = Convert.ToInt32(Ht["SubClassId"]);
                }

                if (Ht.ContainsKey("RealName"))
                {
                    if(Ht["RealName"]== null) M.RealName = null; else M.RealName = SafeType == 0 ? Convert.ToString(Ht["RealName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["RealName"]));
                }

                if (Ht.ContainsKey("Sex"))
                {
                    if(Ht["Sex"]== null) M.Sex = null; else  M.Sex = Convert.ToInt32(Ht["Sex"]);
                }

                if (Ht.ContainsKey("GroupId"))
                {
                    if(Ht["GroupId"]== null) M.GroupId = null; else  M.GroupId = Convert.ToInt32(Ht["GroupId"]);
                }

                if (Ht.ContainsKey("Address"))
                {
                    if(Ht["Address"]== null) M.Address = null; else M.Address = SafeType == 0 ? Convert.ToString(Ht["Address"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Address"]));
                }

                if (Ht.ContainsKey("Tel"))
                {
                    if(Ht["Tel"]== null) M.Tel = null; else M.Tel = SafeType == 0 ? Convert.ToString(Ht["Tel"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Tel"]));
                }

                if (Ht.ContainsKey("Email"))
                {
                    if(Ht["Email"]== null) M.Email = null; else M.Email = SafeType == 0 ? Convert.ToString(Ht["Email"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Email"]));
                }

                if (Ht.ContainsKey("Birthday"))
                {
                    if(Ht["Birthday"]== null) M.Birthday = null; else if (Ht["Birthday"] != null&&!"".Equals(Ht["Birthday"])) M.Birthday = Convert.ToDateTime(Ht["Birthday"]);
                }

                if (Ht.ContainsKey("QQ"))
                {
                    if(Ht["QQ"]== null) M.QQ = null; else M.QQ = SafeType == 0 ? Convert.ToString(Ht["QQ"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["QQ"]));
                }

                if (Ht.ContainsKey("WangWang"))
                {
                    if(Ht["WangWang"]== null) M.WangWang = null; else M.WangWang = SafeType == 0 ? Convert.ToString(Ht["WangWang"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["WangWang"]));
                }

                if (Ht.ContainsKey("LatelyLogin"))
                {
                    if(Ht["LatelyLogin"]== null) M.LatelyLogin = null; else M.LatelyLogin = SafeType == 0 ? Convert.ToString(Ht["LatelyLogin"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["LatelyLogin"]));
                }

                if (Ht.ContainsKey("Description"))
                {
                    if(Ht["Description"]== null) M.Description = null; else M.Description = SafeType == 0 ? Convert.ToString(Ht["Description"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Description"]));
                }

                if (Ht.ContainsKey("Status"))
                {
                    if(Ht["Status"]== null) M.Status = null; else  M.Status = Convert.ToInt32(Ht["Status"]);
                }

                if (Ht.ContainsKey("ParentId"))
                {
                    if(Ht["ParentId"]== null) M.ParentId = null; else  M.ParentId = Convert.ToInt32(Ht["ParentId"]);
                }

                if (Ht.ContainsKey("LatestLoginTime"))
                {
                    if(Ht["LatestLoginTime"]== null) M.LatestLoginTime = null; else if (Ht["LatestLoginTime"] != null&&!"".Equals(Ht["LatestLoginTime"])) M.LatestLoginTime = Convert.ToDateTime(Ht["LatestLoginTime"]);
                }

                if (Ht.ContainsKey("LatestLoginIP"))
                {
                    if(Ht["LatestLoginIP"]== null) M.LatestLoginIP = null; else M.LatestLoginIP = SafeType == 0 ? Convert.ToString(Ht["LatestLoginIP"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["LatestLoginIP"]));
                }

                if (Ht.ContainsKey("yqCode"))
                {
                    if(Ht["yqCode"]== null) M.yqCode = null; else M.yqCode = SafeType == 0 ? Convert.ToString(Ht["yqCode"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["yqCode"]));
                }

                if (Ht.ContainsKey("PhoneNo"))
                {
                    if(Ht["PhoneNo"]== null) M.PhoneNo = null; else M.PhoneNo = SafeType == 0 ? Convert.ToString(Ht["PhoneNo"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["PhoneNo"]));
                }

                if (Ht.ContainsKey("FaxNo"))
                {
                    if(Ht["FaxNo"]== null) M.FaxNo = null; else M.FaxNo = SafeType == 0 ? Convert.ToString(Ht["FaxNo"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["FaxNo"]));
                }

                if (Ht.ContainsKey("Vmoney"))
                {
                    if(Ht["Vmoney"]== null) M.Vmoney = null; else  M.Vmoney = Convert.ToDecimal(Ht["Vmoney"]);
                }

                if (Ht.ContainsKey("Homesite"))
                {
                    if(Ht["Homesite"]== null) M.Homesite = null; else M.Homesite = SafeType == 0 ? Convert.ToString(Ht["Homesite"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Homesite"]));
                }

                if (Ht.ContainsKey("ManagerName"))
                {
                    if(Ht["ManagerName"]== null) M.ManagerName = null; else M.ManagerName = SafeType == 0 ? Convert.ToString(Ht["ManagerName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["ManagerName"]));
                }

                if (Ht.ContainsKey("RefUserNo"))
                {
                    if(Ht["RefUserNo"]== null) M.RefUserNo = null; else M.RefUserNo = SafeType == 0 ? Convert.ToString(Ht["RefUserNo"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["RefUserNo"]));
                }

                if (Ht.ContainsKey("NickName"))
                {
                    if(Ht["NickName"]== null) M.NickName = null; else M.NickName = SafeType == 0 ? Convert.ToString(Ht["NickName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["NickName"]));
                }

                if (Ht.ContainsKey("EnglishName"))
                {
                    if(Ht["EnglishName"]== null) M.EnglishName = null; else M.EnglishName = SafeType == 0 ? Convert.ToString(Ht["EnglishName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["EnglishName"]));
                }

                if (Ht.ContainsKey("IdNumber"))
                {
                    if(Ht["IdNumber"]== null) M.IdNumber = null; else M.IdNumber = SafeType == 0 ? Convert.ToString(Ht["IdNumber"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["IdNumber"]));
                }

                if (Ht.ContainsKey("LoginWay"))
                {
                    if(Ht["LoginWay"]== null) M.LoginWay = null; else  M.LoginWay = Convert.ToInt32(Ht["LoginWay"]);
                }

                if (Ht.ContainsKey("UserId"))
                {
                    if(Ht["UserId"]== null) M.UserId = null; else  M.UserId = Convert.ToInt32(Ht["UserId"]);
                }

                if (Ht.ContainsKey("Department"))
                {
                    if(Ht["Department"]== null) M.Department = null; else M.Department = SafeType == 0 ? Convert.ToString(Ht["Department"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Department"]));
                }

                if (Ht.ContainsKey("Position"))
                {
                    if(Ht["Position"]== null) M.Position = null; else M.Position = SafeType == 0 ? Convert.ToString(Ht["Position"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Position"]));
                }

                if (Ht.ContainsKey("Hobby"))
                {
                    if(Ht["Hobby"]== null) M.Hobby = null; else M.Hobby = SafeType == 0 ? Convert.ToString(Ht["Hobby"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Hobby"]));
                }

                if (Ht.ContainsKey("HeadPortrait"))
                {
                    if(Ht["HeadPortrait"]== null) M.HeadPortrait = null; else M.HeadPortrait = SafeType == 0 ? Convert.ToString(Ht["HeadPortrait"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["HeadPortrait"]));
                }

                if (Ht.ContainsKey("StateBirthday"))
                {
                    if(Ht["StateBirthday"]== null) M.StateBirthday = null; else  M.StateBirthday = Convert.ToInt32(Ht["StateBirthday"]);
                }

                if (Ht.ContainsKey("DelState"))
                {
                    if(Ht["DelState"]== null) M.DelState = null; else  M.DelState = Convert.ToInt32(Ht["DelState"]);
                }

                if (Ht.ContainsKey("Addtime"))
                {
                    if(Ht["Addtime"]== null) M.Addtime = null; else if (Ht["Addtime"] != null&&!"".Equals(Ht["Addtime"])) M.Addtime = Convert.ToDateTime(Ht["Addtime"]);
                }

                if (Ht.ContainsKey("UserCode"))
                {
                    if(Ht["UserCode"]== null) M.UserCode = null; else M.UserCode = SafeType == 0 ? Convert.ToString(Ht["UserCode"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["UserCode"]));
                }

                if (Ht.ContainsKey("CheckCode"))
                {
                    if(Ht["CheckCode"]== null) M.CheckCode = null; else M.CheckCode = SafeType == 0 ? Convert.ToString(Ht["CheckCode"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["CheckCode"]));
                }

                if (Ht.ContainsKey("CodeTime"))
                {
                    if(Ht["CodeTime"]== null) M.CodeTime = null; else if (Ht["CodeTime"] != null&&!"".Equals(Ht["CodeTime"])) M.CodeTime = Convert.ToDateTime(Ht["CodeTime"]);
                }

                if (Ht.ContainsKey("CodeState"))
                {
                    if(Ht["CodeState"]== null) M.CodeState = null; else  M.CodeState = Convert.ToInt32(Ht["CodeState"]);
                }

                if (Ht.ContainsKey("TakeState"))
                {
                    if(Ht["TakeState"]== null) M.TakeState = null; else  M.TakeState = Convert.ToInt32(Ht["TakeState"]);
                }

                if (Ht.ContainsKey("Actid"))
                {
                    if(Ht["Actid"]== null) M.Actid = null; else  M.Actid = Convert.ToInt16(Ht["Actid"]);
                }

                if (Ht.ContainsKey("LoginCount"))
                {
                    if(Ht["LoginCount"]== null) M.LoginCount = null; else  M.LoginCount = Convert.ToInt32(Ht["LoginCount"]);
                }

                if (Ht.ContainsKey("modify"))
                {
                    if(Ht["modify"]== null) M.modify = null; else if (Ht["modify"] != null&&!"".Equals(Ht["modify"])) M.modify = Convert.ToDateTime(Ht["modify"]);
                }

                if (Ht.ContainsKey("referrer"))
                {
                    if(Ht["referrer"]== null) M.referrer = null; else  M.referrer = Convert.ToInt32(Ht["referrer"]);
                }

                if (Ht.ContainsKey("TaobaoID"))
                {
                    if(Ht["TaobaoID"]== null) M.TaobaoID = null; else M.TaobaoID = SafeType == 0 ? Convert.ToString(Ht["TaobaoID"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["TaobaoID"]));
                }

                if (Ht.ContainsKey("taobao_user_nick"))
                {
                    if(Ht["taobao_user_nick"]== null) M.taobao_user_nick = null; else M.taobao_user_nick = SafeType == 0 ? Convert.ToString(Ht["taobao_user_nick"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["taobao_user_nick"]));
                }

                if (Ht.ContainsKey("social_uid"))
                {
                    if(Ht["social_uid"]== null) M.social_uid = null; else M.social_uid = SafeType == 0 ? Convert.ToString(Ht["social_uid"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["social_uid"]));
                }

                if (Ht.ContainsKey("social_user_nick"))
                {
                    if(Ht["social_user_nick"]== null) M.social_user_nick = null; else M.social_user_nick = SafeType == 0 ? Convert.ToString(Ht["social_user_nick"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["social_user_nick"]));
                }

                if (Ht.ContainsKey("openid"))
                {
                    if(Ht["openid"]== null) M.openid = null; else M.openid = SafeType == 0 ? Convert.ToString(Ht["openid"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["openid"]));
                }

                if (Ht.ContainsKey("open_user_nick"))
                {
                    if(Ht["open_user_nick"]== null) M.open_user_nick = null; else M.open_user_nick = SafeType == 0 ? Convert.ToString(Ht["open_user_nick"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["open_user_nick"]));
                }


            }


            return M;
        }

        /// <summary>
        /// 根据Hashtable更新Model.Users数据
        /// </summary>
        /// <param name="Ht">需要更新的Hashtable</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
        /// <returns>true or false</returns>
        private string UpdateHt(System.Collections.Hashtable Ht, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.Users M = HashtableToUpdateModel(Ht, Permissions,1);
				if (M == null) return "{\"Type\":-1,\"Message\":\"数据不存在或权限不足\"}";
				Context.Users.Attach(M);
				Context.ObjectStateManager.ChangeObjectState(M, System.Data.EntityState.Modified);
				int Result = Context.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
				return "{\"Type\":0,\"Message\":\"操作成功\",\"Id\":\"" + M.Id + "\"}";
			}
        }


        /// <summary>
        /// 根据Model更新Model.Users数据
        /// </summary>
        /// <param name="Model">需要更新的Model</param>
        /// <param name="Permissions">权限类别0:UserId 1:ParentId 2:UserId和ParentId 3:UserId或ParentId 4:无附加条件</param>
		/// <param name="Check">是否验证</param>
        /// <returns>true or false</returns>
        public string Update(Model.Users M, int Permissions = 0,bool Check=true)
        {
            return Update(ModelToJson(M), Permissions,Check);
        }


        /// <summary>
        /// 根据Hashtable更新Model.Users数据
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
        /// 根据MyHashTable更新Model.Users数据
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
        /// 根据Json更新Model.Users数据
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
        /// 表Model.Users添加或删除一条数据
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
        /// <returns>Users的List集合</returns>
        public List<Model.Users> GetAllList(int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.Users, bool>> Where = PredicateExtensionses.True<Model.Users>();

				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.Users>().Where(Where);

				return Query.ToList<Model.Users>();
			}
        }


	




    }
}
