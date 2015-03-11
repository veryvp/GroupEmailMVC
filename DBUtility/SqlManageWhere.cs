using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using System.Web;


namespace DBUtility
{
    public class SqlManageWhere : ArrayList
    {

        /// <summary>
        /// 添加and条件(列名等于参数)
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="Parameters">参数</param>
        public void Add(string ColumnName, string Parameters)
        {
            ArrayList a1 = new ArrayList();
            a1.Add("and");
            a1.Add(ColumnName);
            a1.Add(Parameters);
            this.Add(a1);
        }
        /// <summary>
        /// 添加时间类型and条件(列名等于参数)
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="Parameters">参数</param>
        public void Add(string ColumnName, DateTime Parameters)
        {
            ArrayList a1 = new ArrayList();
            a1.Add("and");
            a1.Add(ColumnName);
            a1.Add(Parameters);
            this.Add(a1);
        }


        /// <summary>
        /// 添加自定义and条件
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="Parameters">条件</param>
        /// <param name="Nexus">参数</param>
        public void Add(string ColumnName, string Parameters, string Nexus)
        {
            ArrayList a1 = new ArrayList();
            a1.Add("and");
            a1.Add(ColumnName);
            a1.Add(Parameters);
            a1.Add(Nexus);
            this.Add(a1);
        }

        /// <summary>
        /// 添加时间类型自定义and条件
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="Parameters">条件</param>
        /// <param name="Nexus">时间类型参数</param>
        public void Add(string ColumnName, string Parameters, DateTime Nexus)
        {
            ArrayList a1 = new ArrayList();
            a1.Add("and");
            a1.Add(ColumnName);
            a1.Add(Parameters);
            a1.Add(Nexus);
            this.Add(a1);
        }


        /// <summary>
        /// 添加自定义and条件
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="Parameters">条件</param>
        /// <param name="Nexus">参数</param>
        public void Add(string ColumnName, string Parameters, ArrayList Nexus)
        {
            ArrayList a1 = new ArrayList();
            a1.Add("and");
            a1.Add(ColumnName);
            a1.Add(Parameters);
            a1.Add(Nexus);
            this.Add(a1);
        }


        /// <summary>
        /// 添加自定义and条件
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="Nexus">参数</param>
        public void AddOrLike(string ColumnName, string Nexus) 
        {
            ArrayList a1 = new ArrayList();
            a1.Add("and");
            a1.Add(ColumnName);
            a1.Add("like");
            a1.Add(Nexus);
            this.Add(a1);
        }



        /// <summary>
        /// 添加or条件(列名等于参数)
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="Parameters">参数</param>
        public void Or(string ColumnName, string Parameters)
        {
            ArrayList a1 = new ArrayList();
            a1.Add("or");
            a1.Add(ColumnName);
            a1.Add(Parameters);
            this.Add(a1);
        }
        /// <summary>
        /// 添加时间类型or条件(列名等于参数)
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="Parameters">时间类型参数</param>
        public void Or(string ColumnName, DateTime Parameters)
        {
            ArrayList a1 = new ArrayList();
            a1.Add("or");
            a1.Add(ColumnName);
            a1.Add(Parameters);
            this.Add(a1);
        }

        /// <summary>
        /// 添加自定义or条件
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="Parameters">条件</param>
        /// <param name="Nexus">参数</param>
        public void Or(string ColumnName, string Parameters, string Nexus)
        {
            ArrayList a1 = new ArrayList();
            a1.Add("or");
            a1.Add(ColumnName);
            a1.Add(Parameters);
            a1.Add(Nexus);
            this.Add(a1);
        }

        /// <summary>
        /// 添加时间类型自定义or条件
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="Parameters">条件</param>
        /// <param name="Nexus">时间类型参数</param>
        public void Or(string ColumnName, string Parameters, DateTime Nexus)
        {
            ArrayList a1 = new ArrayList();
            a1.Add("or");
            a1.Add(ColumnName);
            a1.Add(Parameters);
            a1.Add(Nexus);
            this.Add(a1);
        }

        /// <summary>
        /// 添加时间类型自定义or条件
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="Parameters">条件</param>
        /// <param name="Nexus">时间类型参数</param>
        public void Or(string ColumnName, string Parameters, ArrayList Nexus)
        {
            ArrayList a1 = new ArrayList();
            a1.Add("or");
            a1.Add(ColumnName);
            a1.Add(Parameters);
            a1.Add(Nexus);
            this.Add(a1);
        }

        public string Contains(string ColumnName)
        {
            ArrayList A = this;
            for (int i = 0; i < A.Count; i++)
            {
                ArrayList str = (ArrayList)A[i];
                if (str[1].ToString() == ColumnName)
                {
                    return str[2].ToString();
                }
                
            }
            return null;
        }

        public void Remove(string ColumnName)
        {
            ArrayList A = this;
            for (int i = 0; i < A.Count; i++)
            {
                ArrayList str = (ArrayList)A[i];
                if (str[1].ToString() == ColumnName)
                {
                    A.RemoveAt(i);
                }

            }
        }


        public string getstring(SqlManageWhere WhereParameters)
        {
            string where1 = "(__A__";
            if (WhereParameters != null)
            {

                //for (int j = 0; j < WhereParameters.Length; j++)
                //{
                    ArrayList a = WhereParameters;
                    if (a != null)
                    {
                        for (int i = 0; i < a.Count; i++)
                        {
                            if (a[i].GetType().Name.ToString() == "ArrayList")
                            {

                                string aa = getstr(i, a[i]);
                                where1 = where1 + aa;

                                if (i == a.Count - 1)
                                {
                                    where1 = where1 + ")";
                                }
                            }
                        }
                    }

                //}
            }
            where1 = where1.Replace("(__A__and", "");
            where1 = where1.Replace("(__A__or", "");
            return where1;
        }

        private string getstr1(int num, ArrayList b)
        {
            string where1 = "";
            if (num == 0)
            {
                b[0] = "";
            }
            if (b.Count == 2)
            {
                if (b[1].ToString().ToLower() == "null")
                {
                    where1 = where1 + " " + b[0] + " null ";
                }
                else
                {
                    where1 = where1 + " " + b[0] + " " + b[1] + " ";
                }

            }
            if (b.Count == 3)
            {

                if (b[2].ToString().ToLower() == "null")
                {
                    where1 = where1 + " " + b[0] + " " + b[1] + " null ";
                }
                else
                {
                    if (b[1].ToString() != "UserId" && b[1].ToString() != "ParentId" && b[1].ToString() != "(UserId")
                    {
                        where1 = where1 + " " + b[0] + " (" + b[1] + "='" + b[2] + "') ";
                    }
                    else
                    {
                        where1 = where1 + " " + b[0] + " (" + b[1] + "=" + b[2] + ") ";
                    }
                }
            }
            if (b.Count == 4)
            {

                if ((b[2].ToString().ToLower() == "like") || (b[2].ToString().ToLower() == "not like"))
                {
                    b[3] = " '%" + b[3] + "%' ";
                }
                if ((b[2].ToString().ToLower() == "%like") || (b[2].ToString().ToLower() == "not %like"))
                {
                    b[3] = " '%" + b[3] + "' ";
                    b[2] = "like";
                }
                if ((b[2].ToString().ToLower() == "like%") || (b[2].ToString().ToLower() == "not like%"))
                {
                    b[3] = " '" + b[3] + "%' ";
                    b[2] = "like";
                }
                if ((b[2].ToString().ToLower() == "in") || (b[2].ToString().ToLower() == "not in"))
                {
                    b[3] = " ('" + b[3] + "') ";
                }

                if ((b[2].ToString().Contains(">")) || (b[2].ToString().Contains("<"))|| (b[2].ToString().Contains("=")))
                {
                    b[3] = " '" + b[3] + "' ";
                }


                if (b[3].ToString().ToLower() == "null")
                {
                    where1 = where1 + " " + b[0] + " " + b[1] + " " + b[2] + " null ";
                }
                else
                {
                    //若字段名称包含","
                    if (b[1].ToString().Contains(",")) {
                        string[] strs = b[1].ToString().Split(',');
                        string tempstr =" " + b[0]+" (";
                        foreach (string str in strs) {
                            tempstr += str + " " + b[2] + b[3] + " or ";
                        }
                        tempstr = tempstr.Substring(0, tempstr.LastIndexOf("or")) + ")";
                        where1 = (where1 + tempstr);
                        return where1;
                    }
                    where1 = where1 + " " + b[0] + " (" + b[1] + " " + b[2] + b[3] + ") ";
                }
            }
            return where1;
        }


        private string getstr(int num, object a)
        {
            ArrayList b = (ArrayList)a;
            for (int i = 0; i < b.Count; i++)
            {
                string type = b[i].GetType().Name.ToString();
                if (type != "DateTime" && type!="ArrayList") {
                    b[i] = DBUtility.Safe.SafeReplace(b[i].ToString());
                }

                if (type == "ArrayList"){
                    ArrayList array = (ArrayList)b[i];
                    StringBuilder newstr = new StringBuilder();
                    for (int j = 0; j < array.Count; j++)
                    {
                        if (array[j].GetType().Name.ToString() != "DateTime")
                            array[j] = DBUtility.Safe.SafeReplace(array[j].ToString());
                        newstr.Append("" + array[j] + "','");
                    }
                    if (array.Count > 0) {
                        newstr.Remove(newstr.Length - 3, 3);
                    }
                    
                    b[i] = newstr.ToString();
                }
              
                if (b.Count == 4 && i == 2)
                {
                    b[i] = b[i].ToString().Replace("not&nbsp;", "not ");
                    b[i] = b[i].ToString().Replace("&nbsp;not", " not");
                    b[i] = b[i].ToString().Replace("&nbsp;not&nbsp;", " not ");
                    b[i] = b[i].ToString().Replace("&lt;", "<");
                    b[i] = b[i].ToString().Replace("&gt;", ">");
                }
            }

            string where1 = "";

            if (num == 0)
            {
                where1 = where1 + b[0] + " (";
                if (b.Count > 1 && b[b.Count - 1].ToString() == "") return (where1 + " 1=1 ");
                where1 = where1 + getstr1(num, b);
            }
            else
            {
                if (b.Count > 1 && b[b.Count - 1].ToString() == "") return where1;
                where1 = where1 + getstr1(num, b);
            }



            return where1;

        }
    }
}