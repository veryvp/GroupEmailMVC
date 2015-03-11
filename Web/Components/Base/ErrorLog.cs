using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBUtility;

namespace Web.Components
{
    /// <summary>
    /// 错误日志记录
    /// </summary>
    public class ErrorLog : BasePage
    {
        DBUtility.SqlManage Helper = new DBUtility.SqlManage();
        
        /// <summary>
        /// 添加一条日志信息
        /// </summary>
        /// <param name="LogLevel">日志级别 Fatal（致命错误） Error (一般错误) warn(警告) Info(一般信息) Debug (调试信息)</param>
        /// <param name="Operate">操作</param>
        /// <param name="Message">消息描述</param>
        /// <param name="Url">请求地址(默认当前请求url)</param>
        /// <param name="Source">错误源(默认当前出错方法)</param>
        ///  <param name="Exception">异常信息(默认服务器最后一个错误信息)</param>
        /// <returns></returns>
        public string InsertLogs(string LogLevel, string Operate, string Message, string Url="", string Source="", string Exception="")
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
            
            LogLevel = Safe.SafeReplace(LogLevel);
            Operate = Safe.SafeReplace(Operate);
            string MachineName = Safe.SafeReplace(System.Net.Dns.GetHostName());
            string IP = Common.Base.IPHelper.GetIPAddress();
            Url = (Url == "") ? (HttpContext.Current== null ? "":Safe.SafeReplace(HttpContext.Current.Request.Url.ToString())) : Safe.SafeReplace(Url);
            Source = (Source == "") ? st.GetFrame(1).GetMethod().Name : Safe.SafeReplace(Source);
            Exception = Safe.SafeReplace(Exception);
            Message = Safe.SafeReplace(Message);

            string UserId = "";
            string ParentId = "";
            if (HttpContext.Current != null)
            {
                if (base.UserInfo != null)
                {
                    Model.SessionUser U = base.UserInfo;
                    UserId = U.UserId.ToString();
                    ParentId = U.ParentId.ToString();
                }
            }
            string Sql = "insert into UserLogs(LogLevel,Operate,MachineName,IP,Url,Source,Exception,Message,UserId,ParentId)  values('" + LogLevel + "','" + Operate + "','" + MachineName + "','" + IP + "','" + Url + "','" + Source + "','" + Exception + "','" + Message + "','" + UserId + "','" + ParentId+"')";
            
            return Helper.Ins(Sql);
            //System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);

            //model.LogLevel = LogLevel;
            //model.Operate = Operate;
            //model.MachineName = System.Net.Dns.GetHostName();
            //model.IP = GetIP();
            //model.Url = (Url == "") ? HttpContext.Current.Request.Url.ToString() : Url;
            //model.Source = (Source == "") ? st.GetFrame(1).GetMethod().Name.ToString() : Source;
            //model.Exception = Exception;
            //model.Message = Message;
            //model.UserId = U.UserId;
            //model.ParentId = U.ParentId;
            //DALUserLogs1.Add(model);
        }

        /// <summary>
        /// 添加一条日志信息
        /// </summary>
        /// <param name="Operate">操作</param>
        ///  <param name="Exception">异常信息</param>
        /// <returns></returns>
        public string InsertLogs(string Operate,Exception Ex)
        {
            return InsertLogs("Error", Operate, Ex.Message, "", Ex.Source, Ex.ToString());
        }





    }
}