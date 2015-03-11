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
        /// Linqƴ��Ȩ��Where
        /// </summary>
        /// <param name="Permissions">Ȩ�����0: UserId 1: ParentId 2: UserId��ParentId 3: UserId��ParentId 4: �޸�������</param>
        /// <param name="Where">Linq��Where����</param>
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
        /// ����Id��ȡһ��Model.CustDataReport����
        /// </summary>
        /// <param name="Id">��ѯ��Id</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
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
        /// ��Jsonת����ʵ����
        /// </summary>
        /// <param name="Json">��Ҫ��ת����Json</param>
        /// <remarks>Json���ֶ�Ϊ��ʱע�����θ�ֵ</remarks>
        /// <returns>ʵ����Model.CustDataReport</returns>
        public Model.CustDataReport GetModel(string Json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return GetModel(JsonSerializer.Deserialize<System.Collections.Hashtable>(Json));
        }


        /// <summary>
        /// ��Hashtableת����ʵ����
        /// </summary>
        /// <param name="Ht">��Ҫ��ת����Hashtable</param>
        /// <returns>ʵ����Model.CustDataReport</returns>
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
        /// ��ʵ����Model.CustDataReportת����Json
        /// </summary>
        /// <returns>Json�ַ���</returns>
        public string ModelToJson(Model.CustDataReport M)
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return JsonSerializer.Serialize(M);
        }




        ///////////////////////////////////////////���////////////////////////////////////////////////////



        /// <summary>
        /// ��Model.CustDataReport���һ������
        /// </summary>
        /// <param name="M">Model.CustDataReportʵ��</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>�������</returns>
        public string Add(Model.CustDataReport M,bool Check=true)
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
        /// ��Model.CustDataReport���һ������
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
        /// ��Model.CustDataReport���һ������
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
        /// ��Model.CustDataReport���һ������
        /// </summary>
        /// <param name="Model">Model.CustDataReportʵ��</param>
        /// <returns>�������</returns>
        private string AddHt(System.Collections.Hashtable Ht)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.CustDataReport Model = GetModel(Ht,1);
	            Model.UserId = User.UserId;
            Model.ParentId = User.ParentId;

				Context.CreateObjectSet<Model.CustDataReport>().AddObject(Model);
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
        /// <returns>�µ�ʵ����Model.CustDataReport��Ȩ�޲����򷵻�null</returns>
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
        /// ����Hashtable����Model.CustDataReport����
        /// </summary>
        /// <param name="Ht">��Ҫ���µ�Hashtable</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
        /// <returns>true or false</returns>
        private string UpdateHt(System.Collections.Hashtable Ht, int Permissions = 0)
        {
			using (Model.Entities Context = new Model.Entities()){
				Model.CustDataReport M = HashtableToUpdateModel(Ht, Permissions,1);
				if (M == null) return "{\"Type\":-1,\"Message\":\"���ݲ����ڻ�Ȩ�޲���\"}";
				Context.CustDataReport.Attach(M);
				Context.ObjectStateManager.ChangeObjectState(M, System.Data.EntityState.Modified);
				int Result = Context.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
				return "{\"Type\":0,\"Message\":\"�����ɹ�\",\"Id\":\"" + M.Id + "\"}";
			}
        }


        /// <summary>
        /// ����Model����Model.CustDataReport����
        /// </summary>
        /// <param name="Model">��Ҫ���µ�Model</param>
        /// <param name="Permissions">Ȩ�����0:UserId 1:ParentId 2:UserId��ParentId 3:UserId��ParentId 4:�޸�������</param>
		/// <param name="Check">�Ƿ���֤</param>
        /// <returns>true or false</returns>
        public string Update(Model.CustDataReport M, int Permissions = 0,bool Check=true)
        {
            return Update(ModelToJson(M), Permissions,Check);
        }


        /// <summary>
        /// ����Hashtable����Model.CustDataReport����
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
        /// ����MyHashTable����Model.CustDataReport����
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
        /// ����Json����Model.CustDataReport����
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
        /// ��Model.CustDataReport��ӻ�ɾ��һ������
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
        /// <returns>CustDataReport��List����</returns>
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
