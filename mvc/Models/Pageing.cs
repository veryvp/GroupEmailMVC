using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;

namespace mvc.Models
{
    public class Pageing
    {
        public int Page { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public DataTable Data { get; set; }
        public string Type { get; set; }



        public string ToJson()
        {
            return "{\"Page\":" + this.Page + ",\"PageCount\":" + this.PageCount + ",\"RecordCount\":" + this.RecordCount + ",\"Data\":" + Dtb2Json(this.Data) + ",\"Type\":0}";
        }


        /// <summary>
        /// 将datatable转换为json  
        /// </summary>
        /// <param name="dtb">Dt</param>
        /// <returns>JSON字符串</returns>
        public string Dtb2Json(DataTable dtb)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            System.Collections.ArrayList dic = new System.Collections.ArrayList();
            foreach (DataRow dr in dtb.Rows)
            {
                System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                foreach (DataColumn dc in dtb.Columns)
                {
                    drow.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                dic.Add(drow);

            }
            //序列化  
            return jss.Serialize(dic);
        }
    }

}