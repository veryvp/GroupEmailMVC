using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;




namespace DBUtility
{
    public class SqlManageOrder : ArrayList
    {
        public void Add(string ColumnName, string Direction)
        {
            Direction = Direction.ToLower();
            if (Direction == "")
            {
                Direction = "desc";
            }
            if (Direction != "asc" && Direction != "desc")
            {
                return;
            }
            string[] a1 = new string[2];
            a1[0] = ColumnName;
            a1[1] = Direction;
            this.Add(a1);
        }


        public void Add(string Json)
        {
            Hashtable OrderHt = new Hashtable();
            OrderHt = Decode(Json);

            foreach (DictionaryEntry o in OrderHt)
            {
                string[] a1 = new string[2];
                a1[0] = DBUtility.Safe.SafeReplace(o.Key.ToString());
                a1[1] = DBUtility.Safe.SafeReplace(o.Value.ToString());
                this.Add(a1);
            }

        }



        public string getstring(SqlManageOrder OrderParameters)
        {
            if (OrderParameters == null)
            {
                return "id asc";
            }
            string where1 = "";
            if (OrderParameters != null && OrderParameters.Count>0)
            {
                for (int j = 0; j < OrderParameters.Count; j++)
                {
                    string[] a = (string[])OrderParameters[j];
                    if (a.Length == 1)
                    {
                        where1 = where1 + DBUtility.Safe.SafeReplace(a[0]) + " desc,";
                    }
                    else
                    {
                        where1 = where1 + DBUtility.Safe.SafeReplace(a[0]) + " " + DBUtility.Safe.SafeReplace(a[1]) + ",";
                    }
                }
                where1 = where1.Remove(where1.Length - 1);
                where1 = where1 + " ";
            }
            return where1;
        }

        private Hashtable Decode(string Json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Hashtable HashTable = jsonSerializer.Deserialize<Hashtable>(Json);
            return HashTable;
        }

    }

}