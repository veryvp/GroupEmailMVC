using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBUtility;

namespace Web.Components
{
    /// <summary>
    /// 记录群发API调用明细
    /// </summary>
    public class GroupEmailAPICallsRecord : BasePage
    {

        private SqlManage SqlHelper = new SqlManage();

        /// <summary>
        /// 记录API调用
        /// </summary>
        /// <param name="APICount">调用次数</param>
        /// <param name="PlatformCode">调用平台</param>
        /// <param name="Operation">操作</param>
        public void RecordAPI(int APICount, string PlatformCode, string Operation)
        {
            string UserId = "";
            string ParentId = "";
            if (HttpContext.Current != null)
            {
                Model.SessionUser U = base.UserInfo;
                UserId = U.UserId.ToString();
                ParentId = U.ParentId.ToString();
            } 

            string IP = Common.Base.IPHelper.GetIPAddress();
            SqlHelper.Ins("insert into APICallsDetail(UserId,ParentId,APICount,PlatformCode,Operation,IP) values('" + UserId + "','" + ParentId + "','" + APICount + "','" + DBUtility.Safe.SafeReplace(PlatformCode) + "','" + DBUtility.Safe.SafeReplace(Operation) + "','" + DBUtility.Safe.SafeReplace(IP) + "')");
            
        }

     

    }
}