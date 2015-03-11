using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Common.Base;

namespace DAL
{
    public class DALRestSharpLog : BaseDAL
    {

		public DALRestSharpLog(){
            TableName = "RestSharpLog";
        }
        

        /// <summary>
        /// Linqƴ��Ȩ��Where
        /// </summary>
        /// <param name="Permissions">Ȩ�����0: UserId 1: ParentId 2: UserId��ParentId 3: UserId��ParentId 4: �޸�������</param>
        /// <param name="Where">Linq��Where����</param>
        /// <returns></returns>
        public System.Linq.Expressions.Expression<Func<Model.RestSharpLog, bool>> SetPermissions(int Permissions, System.Linq.Expressions.Expression<Func<Model.RestSharpLog, bool>> Where)
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
        /// ����Id��ȡһ��Model.RestSharpLog����
        /// </summary>
        /// <param name="Id">��ѯ��Id</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
        /// <returns>Model.RestSharpLog</returns>
        public Model.RestSharpLog GetModel(long Id, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.RestSharpLog, bool>> Where = PredicateExtensionses.True<Model.RestSharpLog>();
				Where = Where.And(p => p.Id == Id);
				
				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.RestSharpLog>().Where(Where);

				return Query.ToList<Model.RestSharpLog>().DefaultIfEmpty().First<Model.RestSharpLog>();
			}
        }

        /// <summary>
        /// ��Jsonת����ʵ����
        /// </summary>
        /// <param name="Json">��Ҫ��ת����Json</param>
        /// <remarks>Json���ֶ�Ϊ��ʱע�����θ�ֵ</remarks>
        /// <returns>ʵ����Model.RestSharpLog</returns>
        public Model.RestSharpLog GetModel(string Json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return GetModel(JsonSerializer.Deserialize<System.Collections.Hashtable>(Json));
        }


        /// <summary>
        /// ��Hashtableת����ʵ����
        /// </summary>
        /// <param name="Ht">��Ҫ��ת����Hashtable</param>
        /// <returns>ʵ����Model.RestSharpLog</returns>
        public Model.RestSharpLog GetModel(System.Collections.Hashtable Ht,int SafeType=0)
        {
            Model.RestSharpLog M = new Model.RestSharpLog();

                if (Ht.ContainsKey("Id"))
                {
                    if (Ht["Id"] != null&&!"".Equals(Ht["Id"]))  M.Id = Convert.ToInt64(Ht["Id"]);
                }


                if (Ht.ContainsKey("UserId"))
                {
                    if (Ht["UserId"] != null&&!"".Equals(Ht["UserId"]))  M.UserId = Convert.ToInt32(Ht["UserId"]);
                }


                if (Ht.ContainsKey("ParentId"))
                {
                    if (Ht["ParentId"] != null&&!"".Equals(Ht["ParentId"]))  M.ParentId = Convert.ToInt32(Ht["ParentId"]);
                }


                if (Ht.ContainsKey("OwnerId"))
                {
                    if (Ht["OwnerId"] != null&&!"".Equals(Ht["OwnerId"]))  M.OwnerId = Convert.ToInt32(Ht["OwnerId"]);
                }


                if (Ht.ContainsKey("AddTime"))
                {
                    if(Ht["AddTime"]== null) M.AddTime = null; else if (Ht["AddTime"] != null&&!"".Equals(Ht["AddTime"])) M.AddTime = Convert.ToDateTime(Ht["AddTime"]);
                }


                if (Ht.ContainsKey("DelState"))
                {
                    if (Ht["DelState"] != null&&!"".Equals(Ht["DelState"]))  M.DelState = Convert.ToByte(Ht["DelState"]);
                }


                if (Ht.ContainsKey("Operate"))
                {
                    if (Ht["Operate"] != null&&!"".Equals(Ht["Operate"]))  M.Operate = Convert.ToByte(Ht["Operate"]);
                }


                if (Ht.ContainsKey("PhoneNo"))
                {
                    if(Ht["PhoneNo"]== null) M.PhoneNo = null; else M.PhoneNo = SafeType == 0 ? Convert.ToString(Ht["PhoneNo"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["PhoneNo"]));
                }


                if (Ht.ContainsKey("VerifyCode"))
                {
                    if(Ht["VerifyCode"]== null) M.VerifyCode = null; else M.VerifyCode = SafeType == 0 ? Convert.ToString(Ht["VerifyCode"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["VerifyCode"]));
                }


                if (Ht.ContainsKey("IP"))
                {
                    if(Ht["IP"]== null) M.IP = null; else M.IP = SafeType == 0 ? Convert.ToString(Ht["IP"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["IP"]));
                }


                if (Ht.ContainsKey("IsSuccess"))
                {
                    if (Ht["IsSuccess"] != null&&!"".Equals(Ht["IsSuccess"]))  M.IsSuccess = Convert.ToByte(Ht["IsSuccess"]);
                }


                if (Ht.ContainsKey("Result"))
                {
                    if(Ht["Result"]== null) M.Result = null; else M.Result = SafeType == 0 ? Convert.ToString(Ht["Result"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Result"]));
                }


                if (Ht.ContainsKey("VerifyType"))
                {
                    if (Ht["VerifyType"] != null&&!"".Equals(Ht["VerifyType"]))  M.VerifyType = Convert.ToByte(Ht["VerifyType"]);
                }




            return M;
        }


        /// <summary>
        /// ��ʵ����Model.RestSharpLogת����Json
        /// </summary>
        /// <returns>Json�ַ���</returns>
        public string ModelToJson(Model.RestSharpLog M)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return JsonSerializer.Serialize(M);
        }




        ///////////////////////////////////////////���////////////////////////////////////////////////////



        /// <summary>
        /// ��Model.RestSharpLog���һ������
        /// </summary>
        /// <param name="M">Model.RestSharpLogʵ��</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>�������</returns>
        public string Add(Model.RestSharpLog M,bool Check=true)
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
        /// ��Model.RestSharpLog���һ������
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
        /// ��Model.RestSharpLog���һ������
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
        /// ��Model.RestSharpLog���һ������
        /// </summary>
        /// <param name="Model">Model.RestSharpLogʵ��</param>
        /// <returns>�������</returns>
        private string AddHt(System.Collections.Hashtable Ht)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.RestSharpLog Model = GetModel(Ht,1);
	            Model.UserId = User.UserId;
            Model.ParentId = User.ParentId;

				Context.CreateObjectSet<Model.RestSharpLog>().AddObject(Model);
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
        /// <returns>�µ�ʵ����Model.RestSharpLog��Ȩ�޲����򷵻�null</returns>
        public Model.RestSharpLog HashtableToUpdateModel(System.Collections.Hashtable Ht, int Permissions = 0, int SafeType = 0)
        {

            Model.RestSharpLog M = new Model.RestSharpLog();

            M = GetModel(Convert.ToInt64(Ht["Id"]), Permissions);

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

                if (Ht.ContainsKey("OwnerId"))
                {
                    if(Ht["OwnerId"]== null) M.OwnerId = null; else  M.OwnerId = Convert.ToInt32(Ht["OwnerId"]);
                }

                if (Ht.ContainsKey("AddTime"))
                {
                    if(Ht["AddTime"]== null) M.AddTime = null; else if (Ht["AddTime"] != null&&!"".Equals(Ht["AddTime"])) M.AddTime = Convert.ToDateTime(Ht["AddTime"]);
                }

                if (Ht.ContainsKey("DelState"))
                {
                    if(Ht["DelState"]== null) M.DelState = null; else  M.DelState = Convert.ToByte(Ht["DelState"]);
                }

                if (Ht.ContainsKey("Operate"))
                {
                    if(Ht["Operate"]== null) M.Operate = null; else  M.Operate = Convert.ToByte(Ht["Operate"]);
                }

                if (Ht.ContainsKey("PhoneNo"))
                {
                    if(Ht["PhoneNo"]== null) M.PhoneNo = null; else M.PhoneNo = SafeType == 0 ? Convert.ToString(Ht["PhoneNo"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["PhoneNo"]));
                }

                if (Ht.ContainsKey("VerifyCode"))
                {
                    if(Ht["VerifyCode"]== null) M.VerifyCode = null; else M.VerifyCode = SafeType == 0 ? Convert.ToString(Ht["VerifyCode"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["VerifyCode"]));
                }

                if (Ht.ContainsKey("IP"))
                {
                    if(Ht["IP"]== null) M.IP = null; else M.IP = SafeType == 0 ? Convert.ToString(Ht["IP"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["IP"]));
                }

                if (Ht.ContainsKey("IsSuccess"))
                {
                    if(Ht["IsSuccess"]== null) M.IsSuccess = null; else  M.IsSuccess = Convert.ToByte(Ht["IsSuccess"]);
                }

                if (Ht.ContainsKey("Result"))
                {
                    if(Ht["Result"]== null) M.Result = null; else M.Result = SafeType == 0 ? Convert.ToString(Ht["Result"]) : DBUtility.Safe.SafeReplace(Convert.ToString(Ht["Result"]));
                }

                if (Ht.ContainsKey("VerifyType"))
                {
                    if(Ht["VerifyType"]== null) M.VerifyType = null; else  M.VerifyType = Convert.ToByte(Ht["VerifyType"]);
                }


            }


            return M;
        }

        /// <summary>
        /// ����Hashtable����Model.RestSharpLog����
        /// </summary>
        /// <param name="Ht">��Ҫ���µ�Hashtable</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
        /// <returns>true or false</returns>
        private string UpdateHt(System.Collections.Hashtable Ht, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.RestSharpLog M = HashtableToUpdateModel(Ht, Permissions,1);
				if (M == null) return "{\"Type\":-1,\"Message\":\"���ݲ����ڻ�Ȩ�޲���\"}";
				Context.RestSharpLog.Attach(M);
				Context.ObjectStateManager.ChangeObjectState(M, System.Data.EntityState.Modified);
				int Result = Context.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
				return "{\"Type\":0,\"Message\":\"�����ɹ�\",\"Id\":\"" + M.Id + "\"}";
			}
        }


        /// <summary>
        /// ����Model����Model.RestSharpLog����
        /// </summary>
        /// <param name="Model">��Ҫ���µ�Model</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>true or false</returns>
        public string Update(Model.RestSharpLog M, int Permissions = 0,bool Check=true)
        {
            return Update(ModelToJson(M), Permissions,Check);
        }


        /// <summary>
        /// ����Hashtable����Model.RestSharpLog����
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
        /// ����MyHashTable����Model.RestSharpLog����
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
        /// ����Json����Model.RestSharpLog����
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
        /// ��Model.RestSharpLog��ӻ�ɾ��һ������
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
        /// <returns>RestSharpLog��List����</returns>
        public List<Model.RestSharpLog> GetAllList(int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				System.Linq.Expressions.Expression<Func<Model.RestSharpLog, bool>> Where = PredicateExtensionses.True<Model.RestSharpLog>();

				if (DelState != -1)
                {
                    Where = Where.And(p => p.DelState == DelState);
                }

				Where = SetPermissions(Permissions, Where);

				var Query = Context.CreateObjectSet<Model.RestSharpLog>().Where(Where);

				return Query.ToList<Model.RestSharpLog>();
			}
        }


	




    }
}
