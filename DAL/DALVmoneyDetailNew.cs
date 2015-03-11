using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Common.Base;

namespace DAL
{
    public class DALVmoneyDetailNew : BaseDAL
    {

		public DALVmoneyDetailNew(){
            TableName = "VmoneyDetailNew";
        }
        

        /// <summary>
        /// Linqƴ��Ȩ��Where
        /// </summary>
        /// <param name="Permissions">Ȩ�����0: UserId 1: ParentId 2: UserId��ParentId 3: UserId��ParentId 4: �޸�������</param>
        /// <param name="Where">Linq��Where����</param>
        /// <returns></returns>
        public System.Linq.Expressions.Expression<Func<Model.VmoneyDetailNew, bool>> SetPermissions(int Permissions, System.Linq.Expressions.Expression<Func<Model.VmoneyDetailNew, bool>> Where)
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
        /// Sqlƴ��Ȩ��Where
        /// </summary>
        /// <param name="Permissions">Ȩ�����0: UserId 1: ParentId 2: UserId��ParentId 3: UserId��ParentId 4: �޸�������</param>
        /// <param name="Where">Linq��Where����</param>
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
        /// ����Id��ȡһ��Model.VmoneyDetailNew����
        /// </summary>
        /// <param name="Id">��ѯ��Id</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
        /// <returns>Model.VmoneyDetailNew</returns>
        public Model.VmoneyDetailNew GetModel(int Id, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.VmoneyDetailNew, bool>> Where = PredicateExtensionses.True<Model.VmoneyDetailNew>();
				Where = Where.And(p => p.Id == Id);
				
				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.VmoneyDetailNew>().Where(Where);

				return Query.ToList<Model.VmoneyDetailNew>().DefaultIfEmpty().First<Model.VmoneyDetailNew>();
			}
        }

        /// <summary>
        /// ��Jsonת����ʵ����
        /// </summary>
        /// <param name="Json">��Ҫ��ת����Json</param>
        /// <remarks>Json���ֶ�Ϊ��ʱע�����θ�ֵ</remarks>
        /// <returns>ʵ����Model.VmoneyDetailNew</returns>
        public Model.VmoneyDetailNew GetModel(string Json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return GetModel(JsonSerializer.Deserialize<System.Collections.Hashtable>(Json));
        }


        /// <summary>
        /// ��Hashtableת����ʵ����
        /// </summary>
        /// <param name="Ht">��Ҫ��ת����Hashtable</param>
        /// <returns>ʵ����Model.VmoneyDetailNew</returns>
        public Model.VmoneyDetailNew GetModel(System.Collections.Hashtable Ht,int SafeType=0)
        {
            Model.VmoneyDetailNew M = new Model.VmoneyDetailNew();

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
                    if(Ht["DelState"]== null||"".Equals(Ht["DelState"])) M.DelState = 0; else  M.DelState = Convert.ToInt32(Ht["DelState"]);
                }
                else{
                      M.DelState = 0;
                }


                if (Ht.ContainsKey("Addtime"))
                {
                    if (Ht["Addtime"] == null||"".Equals(Ht["Addtime"])) M.Addtime = DateTime.Now; else if (Ht["Addtime"] != null&&!"".Equals(Ht["Addtime"])) M.Addtime = Convert.ToDateTime(Ht["Addtime"]);
                }
                else{
                      M.Addtime =  DateTime.Now ;
                }


                if (Ht.ContainsKey("FromUserId"))
                {
                    if (Ht["FromUserId"] != null&&!"".Equals(Ht["FromUserId"]))  M.FromUserId = Convert.ToInt32(Ht["FromUserId"]);
                }


                if (Ht.ContainsKey("ToUserId"))
                {
                    if(Ht["ToUserId"]== null) M.ToUserId = null; else M.ToUserId = SafeType == 0 ? Convert.ToString(Ht["ToUserId"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["ToUserId"]));
                }


                if (Ht.ContainsKey("Vmoney"))
                {
                    if (Ht["Vmoney"] != null&&!"".Equals(Ht["Vmoney"]))  M.Vmoney = Convert.ToDecimal(Ht["Vmoney"]);
                }


                if (Ht.ContainsKey("Type"))
                {
                    if(Ht["Type"]== null) M.Type = null; else M.Type = SafeType == 0 ? Convert.ToString(Ht["Type"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Type"]));
                }


                if (Ht.ContainsKey("TypeId"))
                {
                    if (Ht["TypeId"] != null&&!"".Equals(Ht["TypeId"]))  M.TypeId = Convert.ToInt16(Ht["TypeId"]);
                }


                if (Ht.ContainsKey("Description"))
                {
                    if(Ht["Description"]== null) M.Description = null; else M.Description = SafeType == 0 ? Convert.ToString(Ht["Description"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Description"]));
                }


                if (Ht.ContainsKey("taskid"))
                {
                    if (Ht["taskid"] != null&&!"".Equals(Ht["taskid"]))  M.taskid = Convert.ToInt32(Ht["taskid"]);
                }


                if (Ht.ContainsKey("tasktype"))
                {
                    if (Ht["tasktype"] != null&&!"".Equals(Ht["tasktype"]))  M.tasktype = Convert.ToInt16(Ht["tasktype"]);
                }




            return M;
        }


        /// <summary>
        /// ��ʵ����Model.VmoneyDetailNewת����Json
        /// </summary>
        /// <returns>Json�ַ���</returns>
        public string ModelToJson(Model.VmoneyDetailNew M)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return JsonSerializer.Serialize(M);
        }




        ///////////////////////////////////////////���////////////////////////////////////////////////////



        /// <summary>
        /// ��Model.VmoneyDetailNew���һ������
        /// </summary>
        /// <param name="M">Model.VmoneyDetailNewʵ��</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>�������</returns>
        public string Add(Model.VmoneyDetailNew M,bool Check=true)
        {
            return Add(ModelToJson(M),Check);
        }


        /// <summary>
        /// ����Json�ַ������һ������
        /// </summary>
        /// <param name="string">Json�ַ���</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>�������</returns>
        public string Add(string Json,bool Check=true)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			JsonHashtable JsonHashtable = new JsonHashtable();

            return Add(JsonHashtable.RestoreValue(JsonSerializer.Deserialize<System.Collections.Hashtable>(Json)),Check); ;
        }


        /// <summary>
        /// ��Model.VmoneyDetailNew���һ������
        /// </summary>
        /// <param name="Ht">��Ҫ��ӵ�Hashtable</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>�������</returns>
        public string Add(System.Collections.Hashtable Ht,bool Check=true)
        {
            //�޳�����ʱ��
            if (Ht.ContainsKey("AddTime"))
                Ht.Remove("AddTime");
			if(Check){
				//��֤�ֶ��Ƿ���Ϲ淶
				string MessageResult = Verify(Ht, 0);//0:��� 1:����

				if (MessageResult != null)
				{
					return MessageResult;
				}
			}
            return AddHt(Ht);
        }


        /// <summary>
        /// ��Model.VmoneyDetailNew���һ������
        /// </summary>
        /// <param name="Ht">��Ҫ��ӵ�MyHashTable</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>�������ݵ�Id</returns>
        public string Add(Common.Base.MyHashTable Ht,bool Check=true)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return Add((System.Collections.Hashtable)Ht,Check);
        }


        /// <summary>
        /// ��Model.VmoneyDetailNew���һ������
        /// </summary>
        /// <param name="Model">Model.VmoneyDetailNewʵ��</param>
        /// <returns>�������</returns>
        private string AddHt(System.Collections.Hashtable Ht)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.VmoneyDetailNew Model = GetModel(Ht,1);
	            Model.UserId = User.UserId;
            Model.ParentId = User.ParentId;

				Context.CreateObjectSet<Model.VmoneyDetailNew>().AddObject(Model);
				Context.SaveChanges();

				return "{\"Type\":0,\"Message\":\"�����ɹ�\",\"Id\":\"" + Model.Id + "\"}";
			}
        }


        ////////////////////////////////////�޸�///////////////////////////////////////


        /// <summary>
        /// ��Hashtableת���ɸ����õ�ʵ����
        /// </summary>
        /// <param name="Ht">��Ҫ��ת����Hashtable</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
		/// <param name="SafeType">�Ƿ�����ȫ����</param>
        /// <remarks>����ʱע���޳�����ʱ��</remarks>
        /// <returns>�µ�ʵ����Model.VmoneyDetailNew��Ȩ�޲����򷵻�null</returns>
        public Model.VmoneyDetailNew HashtableToUpdateModel(System.Collections.Hashtable Ht, int Permissions = 0, int SafeType = 0)
        {

            Model.VmoneyDetailNew M = new Model.VmoneyDetailNew();

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
                    if(Ht["DelState"]== null) M.DelState = null; else  M.DelState = Convert.ToInt32(Ht["DelState"]);
                }

                if (Ht.ContainsKey("Addtime"))
                {
                    if(Ht["Addtime"]== null) M.Addtime = null; else if (Ht["Addtime"] != null&&!"".Equals(Ht["Addtime"])) M.Addtime = Convert.ToDateTime(Ht["Addtime"]);
                }

                if (Ht.ContainsKey("FromUserId"))
                {
                    if(Ht["FromUserId"]== null) M.FromUserId = null; else  M.FromUserId = Convert.ToInt32(Ht["FromUserId"]);
                }

                if (Ht.ContainsKey("ToUserId"))
                {
                    if(Ht["ToUserId"]== null) M.ToUserId = null; else M.ToUserId = SafeType == 0 ? Convert.ToString(Ht["ToUserId"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["ToUserId"]));
                }

                if (Ht.ContainsKey("Vmoney"))
                {
                    if(Ht["Vmoney"]== null) M.Vmoney = null; else  M.Vmoney = Convert.ToDecimal(Ht["Vmoney"]);
                }

                if (Ht.ContainsKey("Type"))
                {
                    if(Ht["Type"]== null) M.Type = null; else M.Type = SafeType == 0 ? Convert.ToString(Ht["Type"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Type"]));
                }

                if (Ht.ContainsKey("TypeId"))
                {
                    if(Ht["TypeId"]== null) M.TypeId = null; else  M.TypeId = Convert.ToInt16(Ht["TypeId"]);
                }

                if (Ht.ContainsKey("Description"))
                {
                    if(Ht["Description"]== null) M.Description = null; else M.Description = SafeType == 0 ? Convert.ToString(Ht["Description"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Description"]));
                }

                if (Ht.ContainsKey("taskid"))
                {
                    if(Ht["taskid"]== null) M.taskid = null; else  M.taskid = Convert.ToInt32(Ht["taskid"]);
                }

                if (Ht.ContainsKey("tasktype"))
                {
                    if(Ht["tasktype"]== null) M.tasktype = null; else  M.tasktype = Convert.ToInt16(Ht["tasktype"]);
                }


            }


            return M;
        }

        /// <summary>
        /// ����Hashtable����Model.VmoneyDetailNew����
        /// </summary>
        /// <param name="Ht">��Ҫ���µ�Hashtable</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
        /// <returns>true or false</returns>
        private string UpdateHt(System.Collections.Hashtable Ht, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.VmoneyDetailNew M = HashtableToUpdateModel(Ht, Permissions,1);
				if (M == null) return "{\"Type\":-1,\"Message\":\"���ݲ����ڻ�Ȩ�޲���\"}";
				Context.VmoneyDetailNew.Attach(M);
				Context.ObjectStateManager.ChangeObjectState(M, System.Data.EntityState.Modified);
				int Result = Context.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
				return "{\"Type\":0,\"Message\":\"�����ɹ�\",\"Id\":\"" + M.Id + "\"}";
			}
        }


        /// <summary>
        /// ����Model����Model.VmoneyDetailNew����
        /// </summary>
        /// <param name="Model">��Ҫ���µ�Model</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>true or false</returns>
        public string Update(Model.VmoneyDetailNew M, int Permissions = 0,bool Check=true)
        {
            return Update(ModelToJson(M), Permissions,Check);
        }


        /// <summary>
        /// ����Hashtable����Model.VmoneyDetailNew����
        /// </summary>
        /// <param name="Ht">��Ҫ�����Hashtable</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>true or false</returns>
        public string Update(System.Collections.Hashtable Ht, int Permissions = 0,bool Check=true)
        {
            //�޳�����ʱ��
            if (Ht.ContainsKey("AddTime"))
                Ht.Remove("AddTime");
			if(Check){
				//��֤�ֶ��Ƿ���Ϲ淶
				string MessageResult = Verify(Ht, 1); //0:��� 1:����

				if (MessageResult != null)
				{
					return MessageResult;
				}
			}
            return UpdateHt(Ht, Permissions);
        }


        /// <summary>
        /// ����MyHashTable����Model.VmoneyDetailNew����
        /// </summary>
        /// <param name="Ht">��Ҫ�����MyHashTable</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>true or false</returns>
        public string Update(Common.Base.MyHashTable Ht, int Permissions = 0,bool Check=true)
        {
            return Update((System.Collections.Hashtable)Ht, Permissions,Check);
        }



        /// <summary>
        /// ����Json����Model.VmoneyDetailNew����
        /// </summary>
        /// <param name="Json">��Ҫ�����Json</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>true or false</returns>
        public string Update(string Json, int Permissions = 0,bool Check=true)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			JsonHashtable JsonHashtable = new JsonHashtable();

            return Update(JsonHashtable.RestoreValue(JsonSerializer.Deserialize<System.Collections.Hashtable>(DBUtility.Safe.SafeReplace(Json))), Permissions,Check);
        }



		/// <summary>
        /// ��Model.VmoneyDetailNew��ӻ�ɾ��һ������
        /// </summary>
        /// <param name="Ht">��Ҫ��ӵ�Hashtable</param>
		/// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>�������</returns>
        public string AddOrUpdate(System.Collections.Hashtable Ht, int Permissions = 0 , bool Check=true)
        {
            if (Ht.Contains("Id"))
            {
                string Id = Ht["Id"].ToString().Trim();
                if (Id == "") { return "{\"Type\":-1,\"Message\":\"���ܴ���δָ����Ϣ\"}"; }
                return Update(Ht, Permissions, Check);
            }
            else
            {
                return Add(Ht, Check);
            }


        }




        /////////////////////////////////////////////////��ѯ//////////////////////////////////////////////////////////////

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="Permissions">Ȩ�����0: UserId 1: ParentId 2: UserId��ParentId 3: UserId��ParentId 4: �޸�������</param>
        /// <returns>VmoneyDetailNew��List����</returns>
        public List<Model.VmoneyDetailNew> GetAllList(int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.VmoneyDetailNew, bool>> Where = PredicateExtensionses.True<Model.VmoneyDetailNew>();

				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.VmoneyDetailNew>().Where(Where);

				return Query.ToList<Model.VmoneyDetailNew>();
			}
        }


	




    }
}
