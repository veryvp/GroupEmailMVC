using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public string sizeauto = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string Url = this.Request.Url.ToString().ToLower(); ;
            if (Url.IndexOf("/box/") > 0)
            {
                sizeauto = "<script>$(function () {sizeauto();}); </script>";
            }
        }
    }
}