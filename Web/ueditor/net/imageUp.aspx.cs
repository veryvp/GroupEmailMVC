using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Collections;
namespace Web.ueditor.net
{
    public partial class imageUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string t = Request.QueryString["t"];
           Response.ContentType = "text/plain";
            //上传配置
            int size = 2;           //文件大小限制,单位MB                             //文件大小限制，单位MB
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };         //文件允许格式

            HttpContext context = HttpContext.Current;

            //上传图片
            Hashtable info = new Hashtable();


            string pathbase = null;
            int path = 1;
            if (path == 1)
            {
                pathbase = "/upload/";

            }
            else
            {
                pathbase = "/upload1/";
            }

           Response.End();
        }


        private string GetRootURI()
        {

            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            HttpRequest Req;
            if (HttpCurrent != null)
            {
                Req = HttpCurrent.Request;
                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")

                    //直接安装在 Web 站点 

                    AppPath = UrlAuthority;

                else

                    //安装在虚拟子目录下 

                    AppPath = UrlAuthority + Req.ApplicationPath;

            }

            return AppPath;

        } 
    }
}