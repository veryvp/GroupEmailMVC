using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Common.Base;

namespace DAL
{
    public class DALFileUpload : BaseDAL
    {

		public DALFileUpload(){
            TableName = "FileUpload";
        }
        

        /// <summary>
        /// Linqƴ��Ȩ��Where
        /// </summary>
        /// <param name="Permissions">Ȩ�����0: UserId 1: ParentId 2: UserId��ParentId 3: UserId��ParentId 4: �޸�������</param>
        /// <param name="Where">Linq��Where����</param>
        /// <returns></returns>
        public System.Linq.Expressions.Expression<Func<Model.FileUpload, bool>> SetPermissions(int Permissions, System.Linq.Expressions.Expression<Func<Model.FileUpload, bool>> Where)
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
        /// ����Id��ȡһ��Model.FileUpload����
        /// </summary>
        /// <param name="Id">��ѯ��Id</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
        /// <returns>Model.FileUpload</returns>
        public Model.FileUpload GetModel(int Id, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.FileUpload, bool>> Where = PredicateExtensionses.True<Model.FileUpload>();
				Where = Where.And(p => p.Id == Id);
				
				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.FileUpload>().Where(Where);

				return Query.ToList<Model.FileUpload>().DefaultIfEmpty().First<Model.FileUpload>();
			}
        }

        /// <summary>
        /// ��Jsonת����ʵ����
        /// </summary>
        /// <param name="Json">��Ҫ��ת����Json</param>
        /// <remarks>Json���ֶ�Ϊ��ʱע�����θ�ֵ</remarks>
        /// <returns>ʵ����Model.FileUpload</returns>
        public Model.FileUpload GetModel(string Json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return GetModel(JsonSerializer.Deserialize<System.Collections.Hashtable>(Json));
        }


        /// <summary>
        /// ��Hashtableת����ʵ����
        /// </summary>
        /// <param name="Ht">��Ҫ��ת����Hashtable</param>
        /// <returns>ʵ����Model.FileUpload</returns>
        public Model.FileUpload GetModel(System.Collections.Hashtable Ht,int SafeType=0)
        {
            Model.FileUpload M = new Model.FileUpload();

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
                    if(Ht["DelState"]== null||"".Equals(Ht["DelState"])) M.DelState = 0; else  M.DelState = Convert.ToByte(Ht["DelState"]);
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


                if (Ht.ContainsKey("Type"))
                {
                    if(Ht["Type"]== null) M.Type = null; else M.Type = SafeType == 0 ? Convert.ToString(Ht["Type"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Type"]));
                }


                if (Ht.ContainsKey("FilePath"))
                {
                    if(Ht["FilePath"]== null) M.FilePath = null; else M.FilePath = SafeType == 0 ? Convert.ToString(Ht["FilePath"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["FilePath"]));
                }


                if (Ht.ContainsKey("FileSize"))
                {
                    if (Ht["FileSize"] != null&&!"".Equals(Ht["FileSize"]))  M.FileSize = Convert.ToInt32(Ht["FileSize"]);
                }


                if (Ht.ContainsKey("FileName"))
                {
                    if(Ht["FileName"]== null) M.FileName = null; else M.FileName = SafeType == 0 ? Convert.ToString(Ht["FileName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["FileName"]));
                }


                if (Ht.ContainsKey("Remarks"))
                {
                    if(Ht["Remarks"]== null) M.Remarks = null; else M.Remarks = SafeType == 0 ? Convert.ToString(Ht["Remarks"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Remarks"]));
                }


                if (Ht.ContainsKey("FileType"))
                {
                    if(Ht["FileType"]== null) M.FileType = null; else M.FileType = SafeType == 0 ? Convert.ToString(Ht["FileType"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["FileType"]));
                }


                if (Ht.ContainsKey("TypeId"))
                {
                    if (Ht["TypeId"] != null&&!"".Equals(Ht["TypeId"]))  M.TypeId = Convert.ToInt32(Ht["TypeId"]);
                }


                if (Ht.ContainsKey("Master"))
                {
                    if(Ht["Master"]== null||"".Equals(Ht["Master"])) M.Master = 0; else  M.Master = Convert.ToByte(Ht["Master"]);
                }
                else{
                      M.Master = 0;
                }


                if (Ht.ContainsKey("TypeCode"))
                {
                    if (Ht["TypeCode"] != null&&!"".Equals(Ht["TypeCode"]))  M.TypeCode = Convert.ToByte(Ht["TypeCode"]);
                }


                if (Ht.ContainsKey("UpStatue"))
                {
                    if(Ht["UpStatue"]== null) M.UpStatue = null; else M.UpStatue = SafeType == 0 ? Convert.ToString(Ht["UpStatue"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["UpStatue"]));
                }


                if (Ht.ContainsKey("ErrorMS"))
                {
                    if(Ht["ErrorMS"]== null) M.ErrorMS = null; else M.ErrorMS = SafeType == 0 ? Convert.ToString(Ht["ErrorMS"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["ErrorMS"]));
                }




            return M;
        }


        /// <summary>
        /// ��ʵ����Model.FileUploadת����Json
        /// </summary>
        /// <returns>Json�ַ���</returns>
        public string ModelToJson(Model.FileUpload M)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return JsonSerializer.Serialize(M);
        }




        ///////////////////////////////////////////���////////////////////////////////////////////////////



        /// <summary>
        /// ��Model.FileUpload���һ������
        /// </summary>
        /// <param name="M">Model.FileUploadʵ��</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>�������</returns>
        public string Add(Model.FileUpload M,bool Check=true)
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
        /// ��Model.FileUpload���һ������
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
        /// ��Model.FileUpload���һ������
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
        /// ��Model.FileUpload���һ������
        /// </summary>
        /// <param name="Model">Model.FileUploadʵ��</param>
        /// <returns>�������</returns>
        private string AddHt(System.Collections.Hashtable Ht)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.FileUpload Model = GetModel(Ht,1);
	            Model.UserId = User.UserId;
            Model.ParentId = User.ParentId;

				Context.CreateObjectSet<Model.FileUpload>().AddObject(Model);
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
        /// <returns>�µ�ʵ����Model.FileUpload��Ȩ�޲����򷵻�null</returns>
        public Model.FileUpload HashtableToUpdateModel(System.Collections.Hashtable Ht, int Permissions = 0, int SafeType = 0)
        {

            Model.FileUpload M = new Model.FileUpload();

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

                if (Ht.ContainsKey("Type"))
                {
                    if(Ht["Type"]== null) M.Type = null; else M.Type = SafeType == 0 ? Convert.ToString(Ht["Type"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Type"]));
                }

                if (Ht.ContainsKey("FilePath"))
                {
                    if(Ht["FilePath"]== null) M.FilePath = null; else M.FilePath = SafeType == 0 ? Convert.ToString(Ht["FilePath"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["FilePath"]));
                }

                if (Ht.ContainsKey("FileSize"))
                {
                    if(Ht["FileSize"]== null) M.FileSize = null; else  M.FileSize = Convert.ToInt32(Ht["FileSize"]);
                }

                if (Ht.ContainsKey("FileName"))
                {
                    if(Ht["FileName"]== null) M.FileName = null; else M.FileName = SafeType == 0 ? Convert.ToString(Ht["FileName"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["FileName"]));
                }

                if (Ht.ContainsKey("Remarks"))
                {
                    if(Ht["Remarks"]== null) M.Remarks = null; else M.Remarks = SafeType == 0 ? Convert.ToString(Ht["Remarks"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Remarks"]));
                }

                if (Ht.ContainsKey("FileType"))
                {
                    if(Ht["FileType"]== null) M.FileType = null; else M.FileType = SafeType == 0 ? Convert.ToString(Ht["FileType"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["FileType"]));
                }

                if (Ht.ContainsKey("TypeId"))
                {
                    if(Ht["TypeId"]== null) M.TypeId = null; else  M.TypeId = Convert.ToInt32(Ht["TypeId"]);
                }

                if (Ht.ContainsKey("Master"))
                {
                    if(Ht["Master"]== null) M.Master = null; else  M.Master = Convert.ToByte(Ht["Master"]);
                }

                if (Ht.ContainsKey("TypeCode"))
                {
                    if(Ht["TypeCode"]== null) M.TypeCode = null; else  M.TypeCode = Convert.ToByte(Ht["TypeCode"]);
                }

                if (Ht.ContainsKey("UpStatue"))
                {
                    if(Ht["UpStatue"]== null) M.UpStatue = null; else M.UpStatue = SafeType == 0 ? Convert.ToString(Ht["UpStatue"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["UpStatue"]));
                }

                if (Ht.ContainsKey("ErrorMS"))
                {
                    if(Ht["ErrorMS"]== null) M.ErrorMS = null; else M.ErrorMS = SafeType == 0 ? Convert.ToString(Ht["ErrorMS"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["ErrorMS"]));
                }


            }


            return M;
        }

        /// <summary>
        /// ����Hashtable����Model.FileUpload����
        /// </summary>
        /// <param name="Ht">��Ҫ���µ�Hashtable</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
        /// <returns>true or false</returns>
        private string UpdateHt(System.Collections.Hashtable Ht, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.FileUpload M = HashtableToUpdateModel(Ht, Permissions,1);
				if (M == null) return "{\"Type\":-1,\"Message\":\"���ݲ����ڻ�Ȩ�޲���\"}";
				Context.FileUpload.Attach(M);
				Context.ObjectStateManager.ChangeObjectState(M, System.Data.EntityState.Modified);
				int Result = Context.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
				return "{\"Type\":0,\"Message\":\"�����ɹ�\",\"Id\":\"" + M.Id + "\"}";
			}
        }


        /// <summary>
        /// ����Model����Model.FileUpload����
        /// </summary>
        /// <param name="Model">��Ҫ���µ�Model</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>true or false</returns>
        public string Update(Model.FileUpload M, int Permissions = 0,bool Check=true)
        {
            return Update(ModelToJson(M), Permissions,Check);
        }


        /// <summary>
        /// ����Hashtable����Model.FileUpload����
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
        /// ����MyHashTable����Model.FileUpload����
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
        /// ����Json����Model.FileUpload����
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
        /// ��Model.FileUpload��ӻ�ɾ��һ������
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
        /// <returns>FileUpload��List����</returns>
        public List<Model.FileUpload> GetAllList(int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.FileUpload, bool>> Where = PredicateExtensionses.True<Model.FileUpload>();

				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.FileUpload>().Where(Where);

				return Query.ToList<Model.FileUpload>();
			}
        }


	




    }
}
