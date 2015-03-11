using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Common.FileStreamEncode;

namespace Web.Ajax
{
    public partial class CheckCode : BasePage
    {
        ///<summary>图形验证码</summary>
    
     
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";

            string Code = HttpContext.Current.Request.Params["Code"];

            object validateNum = Session["ValidateNum"];
            string ValidateNum = Code.Trim().ToUpper();
            if (!ValidateNum.Equals(validateNum))
            {

                Response.Write("0");
            }
            else
            {
                Response.Write("1");
            }

         


           

        }

       
    }
}