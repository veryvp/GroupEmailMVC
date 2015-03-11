using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DBUtility;
using System.Collections;
using System.IO;
using Common.FileStreamEncode;


namespace Web.Components.Base
{
    public class FileUpload : BasePage
    {
        DAL.DALFileUpload DALFileUpload1 = new DAL.DALFileUpload();
     //   DAL.DALSAttachment DALSAttachment = new DAL.DALSAttachment();
        Model.FileUpload FileUpload1 = new Model.FileUpload();
     //   Model.SAttachment SAttachment1 = new Model.SAttachment();
        SqlManage SqlManage1 = new SqlManage();

        /// <summary>
        /// 保存上传的文件的详细信息
        /// </summary>
        /// <param name="Type">上传的类型 1.图片；2.制表格；3.常用的图片和常用的办公文件</param>
        /// <param name="FilePath">保存在服务器的文件名</param>
        /// <param name="FileName">实际文件名</param>
        /// <param name="FileSize">文件大小（byte）</param>
        ///  <param name="Remarks">文件备注</param>
        /// <returns>是否保存成功</returns>
        public string Add(string Type, string FilePath, string FileName, int FileSize, string Remarks, string FileType, int TypeId, int TypeCode)
        {
            FileUpload1.Type = Type;
            FileUpload1.FilePath = FilePath;
            FileUpload1.FileName = FileName;
            FileUpload1.FileSize = FileSize;
            FileUpload1.Remarks = Remarks;
            FileUpload1.FileType = FileType;
            FileUpload1.UpStatue = "0";
         
            FileUpload1.TypeId = TypeId;
            FileUpload1.TypeCode = (byte)TypeCode;
            return DALFileUpload1.Add(FileUpload1);
        }

        ///// <summary>
        ///// 保存上传附件详细信息
        ///// </summary>
        ///// <param name="FilePath">保存在服务器的文件名</param>
        ///// <param name="FileName">实际文件名</param>
        ///// <param name="FileSize">文件大小（byte）</param>
        ///// <returns>是否保存成功</returns>
        //public string Add(string FilePath, string FileName, int FileSize)
        //{
        //    SAttachment1.FilePath = FilePath;
        //    SAttachment1.FileName = FileName;
        //    SAttachment1.Size = FileSize;
        //    return DALSAttachment.Add(SAttachment1);
        //}

        /// <summary>
        /// 删除文件的信息
        /// </summary>
        /// <param name="Id">文件的id</param>
        /// <returns>是否删除成功</returns>
        public string Del(int Id)
        {
            return DALFileUpload1.Del(Id);
        }

        /// <summary>
        /// 删除文件的信息(真删除)
        /// </summary>
        /// <param name="Id">文件的id</param>
        /// <returns>是否删除成功</returns>
        public string DelTrue(int Id)
        {
            FileUpNew FileUpNew1 = new FileUpNew();
            string ServerFileName = SqlManage1.One("select top 1 FilePath from FileUpload where userid=" + this.UserId + " and delstate=1 and Id=" + Id);
            if (ServerFileName != null)
            {
                Hashtable HtDel = JsonHashtable1.Decode(FileUpNew1.DelFile(ServerFileName));
                if (HtDel["Type"] + "" == "0")
                {
                    return DALFileUpload1.DelTrue(Id);
                }
            }
            return "{ \"Message\": \"操作失败\",\"Type\":\"-1\"}";
        }
        /// <summary>
        /// 显示图片
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageNo"></param>
        /// <param name="Key"></param>
        /// <param name="SortOrder"></param>
        /// <returns></returns>
        public DataTable ShowPhoto(int PageSize, int PageNo, string Key, string SortOrder, string ThisType)
        {
            
            Helper1.Where.Add("Type", ThisType);
            int TypeId = 0;
            if (Key != "")
            {
                Hashtable Ht = new Hashtable();
                Ht = JsonHashtable1.EasyDecode(Key);
                if (Ht["TypeId"].ToString() != "")
                {
                    TypeId = int.Parse(Ht["TypeId"].ToString());
                }
            }
            Helper1.Where.Add("TypeId", TypeId + "");
            Helper1.Order.Add("Master", "desc");
            Helper1.Order.Add("Id", "desc");

            return Helper1.Paging("FileUpload", "Id,FileName,FilePath,Remarks,Master", Helper1.Where, Helper1.Order, PageSize, PageNo);
        }

        /// <summary>
        /// 保存修改的备注信息
        /// </summary>
        /// <param name="Id">修改的Id</param>
        /// <param name="Remarks">要修改的备注的内容</param>
        /// <returns></returns>
        public string SaveEdit(int Id, string Remarks)
        {
            FileUpload1 = DALFileUpload1.GetModel(Id);
            FileUpload1.Remarks = Remarks;
            return DALFileUpload1.Update(FileUpload1);
        }


        /// <summary>
        /// 设置或者取消为主图
        /// </summary>
        /// <param name="ThisId">要设置的Id</param>
        /// <returns></returns>
        public string SendMaster(int Id)
        {
            FileUpload1 = DALFileUpload1.GetModel(Id);
            if (FileUpload1.Master == 0)
            { FileUpload1.Master = 1; }
            else
            {
                FileUpload1.Master = 0; 
            }
            return DALFileUpload1.Update(FileUpload1);
        }


        /// 获取自己的所有上传的文件的详细信息   默认是搜索自己的，按照ID倒叙排序 （文档库）
        /// </summary>
        /// <returns>文件的详细的信息表</returns>
        public DataTable Sel(int TypeId)
        {
            ;
            Helper1.Where.Add("Type", "客户文件上传");
            Helper1.Where.Add("TypeId", TypeId + "");
            Helper1.Order.Add("Id", "desc");
            return Helper1.Sel("ViewFileUpload", "Id,Remarks,Addtime,FileName,UserName", Helper1.Where, null);
        }

        /// 获取自己的所有上传的文件的详细信息(回收站)  默认是搜索自己的，按照ID倒叙排序（文档库）
        /// </summary>
        /// <returns>文件的详细信息表</returns>
        public DataTable SelDelete(int TypeId)
        {
            return SqlManage1.Sel("select Id,Remarks,Addtime,FileName,UserName from ViewFileUpload where userid=" + this.UserId + " and userid=(select userid from cust where Id='" + TypeId + "') and TypeId='" + TypeId + "' and Type='客户文件上传' and delstate=1 order by id desc");
        }

        /// 获取自己的所有上传的图片的详细信息(回收站)  默认是搜索自己的，按照ID倒叙排序
        /// </summary>
        /// <returns>图片的详细信息表</returns>
        public DataTable SelPhotoDelete(int TypeId, string ThisType,string TableName)
        {
            return SqlManage1.Sel("select Id,Remarks,Addtime,FileName,UserName,FilePath from ViewFileUpload where userid='" + this.UserId + "' and userid=(select userid from " + TableName + " where Id='" + TypeId + "') and TypeId='" + TypeId + "' and DelState='1' and Type='" + ThisType + "' order by id desc");
        }

        /// <summary>
        /// 还原回收站的文件
        /// </summary>
        /// <param name="ThisId">要还原的文件的Id</param>
        /// <returns></returns>
        public string RenewFile(int ThisId, int TypeId, string TableName = "cust")
        {
            int i = SqlManage1.Upd("update FileUpload set delstate=0 where userid=" + this.UserId + " and userid=(select userid from " + TableName + " where Id='" + TypeId + "') and TypeId='" + TypeId + "' and delstate=1 and id=" + ThisId);
            if (i == 0) { return "{ \"Message\": \"操作失败\",\"Type\":\"-1\"}"; }
            return "{ \"Message\": \"操作成功\",\"Type\":\"0\"}";
        }

        //public DataTable LastOne()
        //{
        //    DBUtility.SqlManage SqlManage1 = new DBUtility.SqlManage();
        //    //默认是搜索自己的，按照ID倒叙排序
        //    DataTable ta = SqlManage1.Sel("select top 1 * from FileUpload where userid="+this.UserId+" and delstate=0 order by id desc");
        //    return ta;
        //}

        /// <summary>
        /// 判断是否存在该Id
        /// </summary>
        /// <returns></returns>
        internal string BoolExists(string ThisId,string TableName)
        {
           return SqlManage1.One("select top 1  Id from " + TableName + " where userid='"+this.UserId+"' and id='"+ThisId+"'");
        }
    }
}