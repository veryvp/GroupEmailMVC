using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Web.Components;
using Common.FileStreamEncode;


namespace Web.Controls
{
    public partial class FilePostCtr : System.Web.UI.UserControl
    {

        //class="asdasd" remark="test"  autoupload="false" a="UPBefore111"
        public string value;
        public int num = 0;
        public string remark = "";
      
        public int type = 0;
        public int isshow = 0;
        public string Class = "", Autoupload = "0", Before = "0", Behind = "0", UPLocal = "0", FSize = "", FType = "", typeid="4444";
        public Common.Base.JsonHashtable JsonHashtable1 = new Common.Base.JsonHashtable();
        DAL.DALFileUpload DALFileUpload1 = new DAL.DALFileUpload();
        Model.FileUpload FileUpload1 = new Model.FileUpload();
        Web.Components.Base.FileUpload FileOpe = new Web.Components.Base.FileUpload();
           private DBUtility.SqlManage SqlHelper= new DBUtility.SqlManage();
         public SqlManageHelper Helper1 = new SqlManageHelper();

        protected void Page_Load(object sender, EventArgs e)
        {

            AjaxPro.Utility.RegisterTypeForAjax(typeof(FilePostCtr));








        }
        [AjaxPro.AjaxMethod]
        public string GetTK(string a,string s)
        {
          
            return "";

        }





        [AjaxPro.AjaxMethod]

        public string JsonGet(string s,string filetype)
        {
            string str = "";
            if (filetype == "6") {
                //str = DALSAttachment.GetJson(int.Parse(s), "UpStatue,ErrorMS");
                //string type = JsonHashtable1.GetNodeByKey(str, "UpStatue");

                //string ms = JsonHashtable1.GetNodeByKey(str, "ErrorMS");
                //if (type == "0")
                //    return JsonHashtable1.AddNode(JsonHashtable1.AddNode(JsonHashtable1.AddNode(DALSAttachment.GetJson(int.Parse(s), "Id,Name,FileName,Size,FilePath,ContentId"), "Type", "-1"), "Message", "上传失败"), "url", q.FileUrl(JsonHashtable1.GetNodeByKey(DALSAttachment.GetJson(int.Parse(s), "FilePath"), "FilePath"), 1, 1, JsonHashtable1.GetNodeByKey(DALSAttachment.GetJson(int.Parse(s), "FileName"), "FileName")));
                //else
                //    return JsonHashtable1.AddNode(JsonHashtable1.AddNode(JsonHashtable1.AddNode(DALSAttachment.GetJson(int.Parse(s), "Id,Name,FileName,Size,FilePath,ContentId"), "Type", (type == "1" ? "0" : type)), "Message", ms), "url", q.FileUrl(JsonHashtable1.GetNodeByKey(DALSAttachment.GetJson(int.Parse(s), "FilePath"), "FilePath"), 1, 1, JsonHashtable1.GetNodeByKey(DALSAttachment.GetJson(int.Parse(s), "FileName"), "FileName")));

            return str;
            }
            else
            {
                 str = DALFileUpload1.GetJson(int.Parse(s), "UpStatue,ErrorMS");
                 string type = JsonHashtable1.GetNodeByKey(str, "UpStatue");

                 string ms = JsonHashtable1.GetNodeByKey(str, "ErrorMS");
                 if (type == "0")
                     return JsonHashtable1.AddNode(JsonHashtable1.AddNode(JsonHashtable1.AddNode(DALFileUpload1.GetJson(int.Parse(s), "Id,FilePath,Remarks,FileType"), "Type", "-1"), "Message", ms), "url", "");
                 else
                     return JsonHashtable1.AddNode(JsonHashtable1.AddNode(JsonHashtable1.AddNode(DALFileUpload1.GetJson(int.Parse(s), "Id,FilePath,Remarks,FileType"), "Type", (type == "1" ? "0" : type)), "Message", ms), "url", "");

            }

          
            return "{ \"Message\": \"操作失败\",\"Type\":-1}";
        }

        [AjaxPro.AjaxMethod]
        public string ReturnName(string json)
        {

            Hashtable Ht2 = JsonHashtable1.EasyDecode(json);

            string Type = Ht2["FileUp_Type"].ToString();
       
            if ((Type == "") || (Type == null))
            {
                return "{ \"Message\": \"操作失败\",\"Type\":-1}";

            }
            string Remark = Ht2["FileUp_Remark"].ToString();

            int TypeCode = Convert.ToInt32(Type);
            string TypeId = Ht2["FileUp_TypeId"].ToString();
            int TypeId1 = 0;
            if (TypeId != null && TypeId != "") { TypeId1 = int.Parse(TypeId); }

            FileUpNew FileUpNew1 = new FileUpNew();
            if (Type == "1")
            {
                FileUpNew1.FileMaxSize = 20480;
                FileUpNew1.FileType = "Excel";
                Type = "客户数据表导入";
            }
            else if (Type == "2")
            {
                FileUpNew1.FileMaxSize = 2048;
                FileUpNew1.FileType = "Pic";
                Type = "客户头像上传";
            }
            else if (Type == "3")
            {
                FileUpNew1.FileMaxSize = 20480;
                FileUpNew1.FileType = "File";
                Type = "客户文件上传";
            }
            else if (Type == "4")
            {
                FileUpNew1.FileMaxSize = 2048;
                FileUpNew1.FileType = "Pic";
                Type = "客户照片上传";
            }
            else if (Type == "5")
            {
                FileUpNew1.FileMaxSize = 2048;
                FileUpNew1.FileType = "Pic";
                Type = "产品照片上传";
            }
            else if (Type == "6")
            {
                FileUpNew1.FileMaxSize = 20480;
                FileUpNew1.FileType = "File";
                //FileUpNew1.FilePath = "/attach/" + UserId + "/" + DateTime.Now.ToString("yyyyMM") + "/";
                Type = "附件上传";
            }
            else if (Type == "7")
            {
                FileUpNew1.FileMaxSize = 20480;
                FileUpNew1.FileType = "Pic";
                Type = "个人照片上传";
            }

            string nf = Ht2["FileUp_Namefile"].ToString();


            string filename2 = Common.StringHtmlJscript.CEntrypt.Encrypt(DateTime.Now.ToString("yyyyMMddHHmmssffff")) + nf.Substring(nf.LastIndexOf("."));
            //string str = FileUpNew1.Save(file, filename, int.Parse(bktype));
          
            string str = ("{ \"FileName\": \"" + filename2 + "\"  ,\"Message\": \"操作成功\",\"Type\":0}");
            Hashtable Ht = JsonHashtable1.EasyDecode(str);

            //上传成功
            if (Ht["Type"].ToString() == "0")
            {
                HttpContext.Current.Session["PhotoName"] = Ht["FileName"] + "";
                if (Type == "附件上传")
                {
                   // return JsonHashtable1.AddNode(FileOpe.Add(Ht["FileName"] + "", Ht2["FileUp_Namefile"].ToString(), int.Parse(Ht2["FileUp_Filesize"].ToString())), "fname", filename2);

                    return JsonHashtable1.AddNode("{\"Type\":0,\"Message\":\"操作成功\",\"Id\":\"0\"}", "fname", filename2);
                }
                else
                {
                    return JsonHashtable1.AddNode(FileOpe.Add(Type, Ht["FileName"] + "", Ht2["FileUp_Namefile"].ToString(), int.Parse(Ht2["FileUp_Filesize"].ToString()), Remark, FileUpNew1.FileType, TypeId1, TypeCode), "fname", filename2);
                }

            }
            else
            {
                return str;
            }
        }


        public string ReutrnFileName(string name)
        {
            string nf = name;


            string filename2 = Common.StringHtmlJscript.CEntrypt.Encrypt(DateTime.Now.ToString("yyyyMMddHHmmssffff")) + nf.Substring(nf.LastIndexOf("."));

            return filename2;
        }

    }


}

