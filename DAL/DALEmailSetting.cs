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
        /// Linqƴ��Ȩ��Where
        /// </summary>
        /// <param name="Permissions">Ȩ�����0: UserId 1: ParentId 2: UserId��ParentId 3: UserId��ParentId 4: �޸�������</param>
        /// <param name="Where">Linq��Where����</param>
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
        /// ����Id��ȡһ��Model.EmailSetting����
        /// </summary>
        /// <param name="Id">��ѯ��Id</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
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
        /// ��Jsonת����ʵ����
        /// </summary>
        /// <param name="Json">��Ҫ��ת����Json</param>
        /// <remarks>Json���ֶ�Ϊ��ʱע�����θ�ֵ</remarks>
        /// <returns>ʵ����Model.EmailSetting</returns>
        public Model.EmailSetting GetModel(string Json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return GetModel(JsonSerializer.Deserialize<System.Collections.Hashtable>(Json));
        }


        /// <summary>
        /// ��Hashtableת����ʵ����
        /// </summary>
        /// <param name="Ht">��Ҫ��ת����Hashtable</param>
        /// <returns>ʵ����Model.EmailSetting</returns>
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
        /// ��ʵ����Model.EmailSettingת����Json
        /// </summary>
        /// <returns>Json�ַ���</returns>
        public string ModelToJson(Model.EmailSetting M)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return JsonSerializer.Serialize(M);
        }




        ///////////////////////////////////////////���////////////////////////////////////////////////////



        /// <summary>
        /// ��Model.EmailSetting���һ������
        /// </summary>
        /// <param name="M">Model.EmailSettingʵ��</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>�������</returns>
        public string Add(Model.EmailSetting M,bool Check=true)
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
        /// ��Model.EmailSetting���һ������
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
        /// ��Model.EmailSetting���һ������
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
        /// ��Model.EmailSetting���һ������
        /// </summary>
        /// <param name="Model">Model.EmailSettingʵ��</param>
        /// <returns>�������</returns>
        private string AddHt(System.Collections.Hashtable Ht)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.EmailSetting Model = GetModel(Ht,1);
	            Model.UserId = User.UserId;
            Model.ParentId = User.ParentId;

				Context.CreateObjectSet<Model.EmailSetting>().AddObject(Model);
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
        /// <returns>�µ�ʵ����Model.EmailSetting��Ȩ�޲����򷵻�null</returns>
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
        /// ����Hashtable����Model.EmailSetting����
        /// </summary>
        /// <param name="Ht">��Ҫ���µ�Hashtable</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
        /// <returns>true or false</returns>
        private string UpdateHt(System.Collections.Hashtable Ht, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.EmailSetting M = HashtableToUpdateModel(Ht, Permissions,1);
				if (M == null) return "{\"Type\":-1,\"Message\":\"���ݲ����ڻ�Ȩ�޲���\"}";
				Context.EmailSetting.Attach(M);
				Context.ObjectStateManager.ChangeObjectState(M, System.Data.EntityState.Modified);
				int Result = Context.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
				return "{\"Type\":0,\"Message\":\"�����ɹ�\",\"Id\":\"" + M.Id + "\"}";
			}
        }


        /// <summary>
        /// ����Model����Model.EmailSetting����
        /// </summary>
        /// <param name="Model">��Ҫ���µ�Model</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>true or false</returns>
        public string Update(Model.EmailSetting M, int Permissions = 0,bool Check=true)
        {
            return Update(ModelToJson(M), Permissions,Check);
        }


        /// <summary>
        /// ����Hashtable����Model.EmailSetting����
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
        /// ����MyHashTable����Model.EmailSetting����
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
        /// ����Json����Model.EmailSetting����
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
        /// ��Model.EmailSetting��ӻ�ɾ��һ������
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
        /// <returns>EmailSetting��List����</returns>
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
