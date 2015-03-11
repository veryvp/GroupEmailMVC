using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Web.Components.Base
{
    public class FileDownLoad : BasePage
    {
        //private Qiniu.QiniuUpload QiniuUpload1 = new Qiniu.QiniuUpload();
        private string FileRoot = System.Configuration.ConfigurationManager.AppSettings["FileRoot"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DataTable GetFile(string Id)
        {
            Helper1.Where.Add("Id", Id );
            return Helper1.Sel("[iTradeCRM].[dbo].[FileUpload]", "FilePath,FileName", Helper1.Where, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DataTable GetAttach(string Id, string EType)
        {

            Helper1.Where.Add("Id", Id);
            if (EType == "1")
            {
                return Helper1.Sel("[iTradeEM].[dbo].[SAttachment]", "Name,FilePath,FileName", Helper1.Where, null);
            }
            else {
                return Helper1.Sel("[iTradeEM].[dbo].[Attachment]", "Name,FilePath,FileName", Helper1.Where, null);
            }
        }

        /// <summary>
        /// 同步服务器文件(先判断当前服务器是否存在文件,若存在则不进行操作,若不存在则从七牛下载文件到本地)
        /// </summary>
        /// <param name="Name">七牛文件名</param>
        /// <param name="FilePath">本地文件保存路径</param>
        public void FileSyn(string Name,string FilePath) {
            FilePath = FileRoot + FilePath;
            if (!System.IO.File.Exists(FilePath))
            {
                string FolderPath = FilePath.Substring(0, FilePath.LastIndexOf("/"));//文件存放文件夹路径
                if (!System.IO.Directory.Exists(FolderPath))
                {
                    System.IO.Directory.CreateDirectory(FolderPath);
                }
                //System.IO.File.WriteAllBytes(FilePath, QiniuUpload1.FileUrl(Name));
            }
        }

    }
}