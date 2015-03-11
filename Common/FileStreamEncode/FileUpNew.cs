using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace Common.FileStreamEncode
{
    public class FileUpNew
    {

        private string FileRoot = System.Configuration.ConfigurationManager.AppSettings["FileRoot"];
        /// <summary>
        /// 上传文件保存的位置，默认保存在根目录下的upload文件夹
        /// </summary>
        public string FilePath = "/upload/";

        /// <summary>
        /// 上传文件保存的类型限制，默认是所有的图片文件类型 
        /// </summary>
        public string FileType = "Pic";

        /// <summary>
        /// 上传文件大小限制，默认是2M，单位是KB
        /// </summary>
        public int FileMaxSize = 2048;


        public string Save(HttpPostedFile file, string filename="", int type = 0)
        {
            int size = file.ContentLength;

            int size1 = size / 1024;
            if (size1 > FileMaxSize)
            {
                return ("{ \"Message\": \"文件大小超过限制，最大不能超过" + FileMaxSize + "KB\",\"Type\":-1}");
            }



            string FileExtensionName = getFileExtension(file.FileName);

            if (FileType == "Excel")
            {
                if ((FileExtensionName != ".xls") && (FileExtensionName != ".xlsx") && (FileExtensionName != ".csv"))
                {
                    return ("{ \"Message\": \"文件类型错误，只能上传指定类型的文件\",\"Type\":-1}");
                }

            }
            else if (FileType == "Pic")
            {
                if ((FileExtensionName != ".jpeg") && (FileExtensionName != ".jpg") && (FileExtensionName != ".png") && (FileExtensionName != ".gif"))
                {
                    return ("{ \"Message\": \"文件类型错误，只能上传指定类型的文件\",\"Type\":-1}");
                }
            }
            else if (FileType == "File")
            {
                if ((FileExtensionName != ".jpeg") && (FileExtensionName != ".jpg") && (FileExtensionName != ".png") && (FileExtensionName != ".gif") && (FileExtensionName != ".xls") && (FileExtensionName != ".xlsx") && (FileExtensionName != ".csv") && (FileExtensionName != ".doc") && (FileExtensionName != ".docx") && (FileExtensionName != ".ppt") && (FileExtensionName != ".pptx") && (FileExtensionName != ".txt") && (FileExtensionName != ".pdf") && (FileExtensionName != ".zip") && (FileExtensionName != ".rar"))
                {
                    return ("{ \"Message\": \"文件类型错误，只能上传指定类型的文件\",\"Type\":-1}");
                }

            }

            if (FilePath!="/upload/" && !System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(FilePath)))
                //System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(FilePath));
                System.IO.Directory.CreateDirectory( FilePath);


            ////type == 1客户导入
            if (type == 1)
            {

                //当天上传的文件放到已当天日期命名的文件夹中
                //string filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + FileExtensionName;
                if(filename=="")
                    filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + FileExtensionName;
                string dateFolder = HttpContext.Current.Server.MapPath(FilePath) + "//" + filename;
                //string dateFolder =  FilePath + "//" + filename;


                file.SaveAs(dateFolder);


                //q.Upload("20131004-项目需求表.xlsx", "D:\\20131004-项目需求表.xlsx");

                return ("{ \"FileName\": \"" + (FilePath == "/upload/" ? "" : FilePath) + filename + "\" ,\"url\": \"" + FilePath + filename + "\",\"storename\": \"" + filename + "\" ,\"Message\": \"操作成功\",\"Type\":0}");

            }
            else
            {
                //当天上传的文件放到已当天日期命名的文件夹中
                 filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + FileExtensionName;
                //string dateFolder = HttpContext.Current.Server.MapPath(FilePath) + "//" + filename;
                string dateFolder = FileRoot + FilePath + "//" + filename;
                file.SaveAs(dateFolder);

            }

            return ("{ \"FileName\": \"" + (FilePath == "/upload/" ? "" : FilePath) + filename + "\",\"Message\": \"操作成功\",\"Type\":0}");
 
        }


        /// <returns>原文件的扩展名(localFileExtension);若返回为null,表明文件无后缀名;若返回为"",则表明扩展名为非法.</returns> 
        private string getFileExtension(string myFileName)
        {
            string FileName = myFileName.ToLower();
            int n1 = FileName.LastIndexOf(".");

            FileName = FileName.Substring(n1);
            return FileName;
        }

        /// <summary>
        /// 删除服务器的文件
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        public string DelFile(string FileName)
        {
            //string dateFolder = HttpContext.Current.Server.MapPath(FilePath) + "//" + FileName;
            string dateFolder = FileRoot + FilePath + "//" + FileName;
            if (File.Exists(dateFolder))
            {
                File.Delete(dateFolder);
                return "{ \"Message\": \"操作成功\",\"Type\":\"0\"}";
            }
            return "{ \"Message\": \"操作失败\",\"Type\":\"-1\"}";
        }


    }
}

