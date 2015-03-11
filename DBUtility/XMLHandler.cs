using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections;

namespace DBUtility
{
    public class XMLHandler
    {
        
        string MessageResult = null;
        System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        /// <summary>
        /// Xml验证Hashtable
        /// </summary>
        /// <param name="ht">需要验证的Hashtable</param>
        /// <param name="XmlName">Xml文件名</param>
        /// <param name="MethodType">0:添加  1:修改 2:模板</param>
        /// <returns>验证结果VerifyMessage</returns>
        public string VerifyHt(Hashtable ht, string XmlName, int MethodType)
        {
            FileInfo file = new FileInfo(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/Check/"+XmlName + ".config");
            if (file.Exists)
            {
                StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/Check/" + XmlName + ".config", Encoding.GetEncoding("utf-8"));
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(sr.ReadToEnd());

                    //验证数据格式是否满足条件
                    XmlNodeList TableList = xmlDoc.SelectSingleNode("/Verify/Table").ChildNodes;

                    foreach (XmlElement XeNode in TableList)
                    {
                        if (ht.ContainsKey(XeNode.Name))
                        {
                            foreach (XmlElement Xe in XeNode.ChildNodes)
                            {
                                //若为模板则不验证是否为空
                                if (MethodType == 2 && Xe.Name == "NotEmpty") continue;
                                object Limit = Xe.GetAttribute("Limit");
                                string Message = Xe.GetAttribute("Message");
                                string Decimal = Xe.GetAttribute("Decimal");

                                if (VerifyField(Xe.Name, ht[XeNode.Name], Limit, Decimal, XeNode.GetAttribute("Type")))
                                {
                                    continue;
                                }
                                else
                                {
                                    MessageResult = XeNode.GetAttribute("FieldName") + (string.IsNullOrEmpty(Message) ? Xe.GetAttribute("Default") : Message);
                                    return MessageResult + "\",\"Name\":\"" + XeNode.Name;
                                }
                            }
                        }
                    }


                    //验证扩展条件
                    XmlNodeList ExtraList = xmlDoc.SelectSingleNode("/Verify/Extra").ChildNodes;

                    foreach (XmlElement XeNode in ExtraList)
                    {

                        if (XeNode.Name == "GreaterOrEqual")
                        {
                            if (!GreaterOrEqual(XeNode.ChildNodes, ht))
                            {
                                MessageResult = string.IsNullOrEmpty(XeNode.GetAttribute("Message")) ? (MessageResult.Replace("@Default", XeNode.GetAttribute("Default"))) : XeNode.GetAttribute("Message");
                                return MessageResult;
                            }
                        }
                        if (XeNode.Name == "Greater")
                        {
                            if (!Greater(XeNode.ChildNodes, ht))
                            {
                                MessageResult = string.IsNullOrEmpty(XeNode.GetAttribute("Message")) ? (MessageResult.Replace("@Default", XeNode.GetAttribute("Default"))) : XeNode.GetAttribute("Message");

                                return MessageResult;
                            }
                        }
                        if (XeNode.Name == "LessOrEqual")
                        {
                            if (!LessOrEqual(XeNode.ChildNodes, ht))
                            {
                                MessageResult = string.IsNullOrEmpty(XeNode.GetAttribute("Message")) ? (MessageResult.Replace("@Default", XeNode.GetAttribute("Default"))) : XeNode.GetAttribute("Message");
                                return MessageResult;
                            }
                        }
                        if (XeNode.Name == "Less")
                        {
                            if (!Less(XeNode.ChildNodes, ht))
                            {
                                MessageResult = string.IsNullOrEmpty(XeNode.GetAttribute("Message")) ? (MessageResult.Replace("@Default", XeNode.GetAttribute("Default"))) : XeNode.GetAttribute("Message");

                                return MessageResult;
                            }
                        }
                        if (XeNode.Name == "Equal")
                        {
                            if (!Equal(XeNode.ChildNodes, ht))
                            {
                                MessageResult = string.IsNullOrEmpty(XeNode.GetAttribute("Message")) ? (MessageResult.Replace("@Default", XeNode.GetAttribute("Default"))) : XeNode.GetAttribute("Message");
                                return MessageResult;
                            }
                        }
                        if (XeNode.Name == "NotEqual")
                        {
                            if (!NotEqual(XeNode.ChildNodes, ht))
                            {
                                MessageResult = string.IsNullOrEmpty(XeNode.GetAttribute("Message")) ? (MessageResult.Replace("@Default", XeNode.GetAttribute("Default"))) : XeNode.GetAttribute("Message");
                                return MessageResult;
                            }
                        }
                        if (MethodType != 2 && XeNode.Name == "OneNotEmpty")
                        {
                            if (!OneNotEmpty(XeNode.ChildNodes, ht))
                            {
                                MessageResult = string.IsNullOrEmpty(XeNode.GetAttribute("Message")) ? (MessageResult.Replace("@Default", XeNode.GetAttribute("Default"))) : XeNode.GetAttribute("Message");

                                return MessageResult;
                            }
                        }
                        if (MethodType != 2 && XeNode.Name == "AllEmpty")
                        {
                            if (!AllEmpty(XeNode.ChildNodes, ht))
                            {
                                MessageResult = string.IsNullOrEmpty(XeNode.GetAttribute("Message")) ? (MessageResult.Replace("@Default", XeNode.GetAttribute("Default"))) : XeNode.GetAttribute("Message");

                                return MessageResult;
                            }
                        } if (MethodType != 2 && XeNode.Name == "NotEmpty")
                        {
                            if (!NotEmpty(XeNode.ChildNodes, ht))
                            {
                                MessageResult = string.IsNullOrEmpty(XeNode.GetAttribute("Message")) ? (MessageResult.Replace("@Default", XeNode.GetAttribute("Default"))) : XeNode.GetAttribute("Message");

                                return MessageResult;
                            }
                        }
                    }
                    //若验证类型不为模板
                    if (MethodType != 2)
                    {
                        //验证字段唯一
                        XmlNodeList DatabaseList = xmlDoc.SelectSingleNode("/Verify/Database").ChildNodes;
                        DBUtility.SqlManage helper = new DBUtility.SqlManage();
                        foreach (XmlElement XeNode in DatabaseList)
                        {

                            foreach (XmlElement Xe in XeNode.ChildNodes)
                            {
                                if (ht.ContainsKey(Xe.Name) && !"".Equals(ht[Xe.Name]))
                                {
                                    bool flag = true;
                                    string Where = " and " + Safe.SafeReplace(Xe.Name) + "='" + Safe.SafeReplace(ht[Xe.Name].ToString()) + "' ";
                                    foreach (XmlElement xe in Xe.ChildNodes)
                                    {
                                        if (ht.ContainsKey(xe.Name))
                                        {
                                            Where += " and " + Safe.SafeReplace(xe.Name) + "='" + Safe.SafeReplace(ht[xe.Name].ToString()) + "' ";
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (flag)
                                    {
                                        if (MethodType == 1)
                                        {
                                            Where += " and Id <>'" + Convert.ToInt64(ht["Id"]) + "' ";
                                        }
                                        string sql = "select Id from " + Safe.SafeReplace(XmlName) + " where DelState = 0 " + Where;
                                        if (helper.One(sql) != null)
                                        {
                                            //MessageResult = string.IsNullOrEmpty(Xe.GetAttribute("Message")) ? (Xe.GetAttribute("FieldName") + Xe.GetAttribute("Default")) : Xe.GetAttribute("Message");
                                            MessageResult = (string.IsNullOrEmpty(Xe.GetAttribute("Message")) ? (Xe.GetAttribute("FieldName") + Xe.GetAttribute("Default")) : Xe.GetAttribute("Message")) + "\",\"Name\":\"" + Xe.Name;
                                            return MessageResult;
                                        }
                                    }


                                }
                            }

                        }
                    }

                }
                catch (Exception e) { }
                finally
                {
                    sr.Dispose();
                    sr.Close();
                }
            }
            return null;
        }


        /// <summary>
        /// Xml验证泛型
        /// </summary>
        /// <typeparam name="T">泛型如model类</typeparam>
        /// <param name="t">泛型变量</param>
        /// <param name="XmlName">Xml文件名</param>
        /// <param name="MethodType">0:添加  1:修改</param>
        /// <returns>验证结果VerifyMessage</returns>
        public string VerifyJson<T>(T t, string XmlName, int MethodType)
        {
            return VerifyJson(jsonSerializer.Deserialize<Hashtable>(jsonSerializer.Serialize(t)), XmlName, MethodType);

        }

        /// <summary>
        /// 验证字段
        /// </summary>
        /// <param name="Method">验证方法名</param>
        /// <param name="Value">需要验证的值</param>
        /// <param name="Limit">验证条件</param>
        /// <param name="Decimal">小数位数</param>
        /// <returns>bool</returns>
        public bool VerifyField(string Method, object Value, object Limit, string Decimal,string Type)
        {
            //if (Method == "Null")
            //{
            //    if (Value.Equals(null))
            //    {
            //        return false;
            //    }
            //}
            if (Method == "NotEmpty")
            {
                if ("".Equals(Value)) {
                    return false;
                }
            }
            if (Method == "MinLength" && !"".Equals(Value))
            {
                if (Value.ToString().Length < Convert.ToDouble(Limit)) {
                    return false;
                }
            }
            if (Method == "MaxLength" && !"".Equals(Value))
            {
                if (Value.ToString().Length > Convert.ToDouble(Limit))
                {
                    return false;
                }
            }
            if (Method == "Rex" && !"".Equals(Value))
            {
                return VerifyRex(Value.ToString(), Limit.ToString(), Decimal);
            }

            if (Method == "MaxValue" && !"".Equals(Value))
            {
                if ("DateTime".Equals(Type))
                {
                    if (Convert.ToDateTime(Value) > Convert.ToDateTime(Limit))
                    {
                        return false;
                    }
                }
                else
                {
                    if (Convert.ToDouble(Value) > Convert.ToDouble(Limit))
                    {
                        return false;
                    }
                }
            }

            if (Method == "MinValue" && !"".Equals(Value))
            {
                if ("DateTime".Equals(Type))
                {
                    if (Convert.ToDateTime(Value) < Convert.ToDateTime(Limit))
                    {
                        return false;
                    }
                }
                else
                {
                    if (Convert.ToDouble(Value) < Convert.ToDouble(Limit))
                    {
                        return false;
                    }
                }

            }
            
            return true;
        }



        /// <summary>
        /// 正则验证字段
        /// </summary>
        /// <param name="Value">需要验证的值</param>
        /// <param name="Limit">验证条件</param>
        /// <param name="Decimal">小数位数</param>
        /// <returns>bool</returns>
        public bool VerifyRex(string Value, string Limit, string Decimal)
        {
            Common.Base.Check check1 = new Common.Base.Check();
            //Check check1 = new Check();
            
            if (Limit == "IsTel")
            {
                if (!check1.IsTel(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsTelorMobile")
            {
                if (!check1.IsTelorMobile(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsEmail")
            {
                if (!check1.IsEmail(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsMobile")
            {
                if (!check1.IsMobile(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsDate")
            {
                if (!check1.IsDate(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsTime")
            {
                if (!check1.IsTime(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsDateTime")
            {
                if (!check1.IsDateTime(Value))
                {
                    return false;
                }
            }
            if (Limit == "IsSFZ")
            {
                if (!check1.IsSFZ(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsInt")
            {
                if (!check1.IsInt(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsIntZheng")
            {
                if (!check1.IsIntZheng(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsIntFeiFu")
            {
                if (!check1.IsIntFeiFu(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsIntFeiZheng")
            {
                if (!check1.IsIntFeiZheng(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsIntFu")
            {
                if (!check1.IsIntFu(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsNum")
            {
                if (!check1.IsNum(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsNumZheng")
            {
                if (!check1.IsNumZheng(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsNumFeiFu")
            {
                if (!check1.IsNumFeiFu(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsNumFu")
            {
                if (!check1.IsNumFu(Value))
                {
                    return false;
                }
            }

            if (Limit == "IsNumFeiZheng")
            {
                if (!check1.IsNumFeiZheng(Value))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 判断是否大于或等于
        /// </summary>
        /// <param name="NodeList">需要判断的节点</param>
        /// <param name="ht">需要判断的数据</param>
        /// <returns>bool</returns>
        public bool GreaterOrEqual(XmlNodeList NodeList,Hashtable ht) 
        { 
            string PreviousField = null;
            string PreviousFieldName = string.Empty;
            bool IsCorrect = true;//是否全部满足验证条件
            foreach (XmlElement Xe in NodeList)
            {
                if (ht.ContainsKey(Xe.Name) && !"".Equals(ht[Xe.Name]))
                {
                    if (IsCorrect)
                    {
                        if (PreviousField != null)
                        {
                            if ("DateTime".Equals(Xe.GetAttribute("Type")))
                            {
                                if ((Convert.ToDateTime(ht[PreviousField]) < Convert.ToDateTime(ht[Xe.Name])))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField;
                                    IsCorrect = false;
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(ht[PreviousField]) < Convert.ToDouble(ht[Xe.Name]))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField;
                                    IsCorrect = false;
                                }
                            }

                        }
                        PreviousField = Xe.Name;
                        PreviousFieldName = Xe.GetAttribute("FieldName");
                    }
                    else {
                        return IsCorrect;
                    }
                }
                else {
                    MessageResult = null;
                    return true;
                }
            }

            return IsCorrect;
        }


        /// <summary>
        /// 判断是否大于
        /// </summary>
        /// <param name="NodeList">需要判断的节点</param>
        /// <param name="ht">需要判断的数据</param>
        /// <returns>bool</returns>
        public bool Greater(XmlNodeList NodeList, Hashtable ht)
        {
            string PreviousField = null;
            string PreviousFieldName = string.Empty;
            bool IsCorrect = true;//是否全部满足验证条件
            foreach (XmlElement Xe in NodeList)
            {
                if (ht.ContainsKey(Xe.Name) && !"".Equals(ht[Xe.Name]))
                {
                    if (IsCorrect)
                    {
                        if (PreviousField != null)
                        {
                            if ("DateTime".Equals(Xe.GetAttribute("Type")))
                            {
                                if (Convert.ToDateTime(ht[PreviousField]) < Convert.ToDateTime(ht[Xe.Name]) || Convert.ToDateTime(ht[PreviousField]) == Convert.ToDateTime(ht[Xe.Name]))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField;
                                    IsCorrect = false;
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(ht[PreviousField]) < Convert.ToDouble(ht[Xe.Name]) || Convert.ToDouble(ht[PreviousField]) == Convert.ToDouble(ht[Xe.Name]))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField;
                                    IsCorrect = false;
                                }
                            }
                        }
                        PreviousField = Xe.Name;
                        PreviousFieldName = Xe.GetAttribute("FieldName");
                    }
                    else
                    {
                        return IsCorrect;
                    }
                }
                else
                {
                    MessageResult = null;
                    return true;
                }
            }
            return IsCorrect;
        }

        /// <summary>
        /// 判断是否小于或等于
        /// </summary>
        /// <param name="NodeList">需要判断的节点</param>
        /// <param name="ht">需要判断的数据</param>
        /// <returns>bool</returns>
        public bool LessOrEqual(XmlNodeList NodeList, Hashtable ht)
        {
            string PreviousField = null;
            string PreviousFieldName = string.Empty;
            bool IsCorrect = true;//是否全部满足验证条件
            foreach (XmlElement Xe in NodeList)
            {
                if (ht.ContainsKey(Xe.Name) && !"".Equals(ht[Xe.Name]))
                {
                    if (IsCorrect)
                    {
                        if (PreviousField != null)
                        {
                            if ("DateTime".Equals(Xe.GetAttribute("Type")))
                            {
                                if (Convert.ToDateTime(ht[PreviousField]) > Convert.ToDateTime(ht[Xe.Name]))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField; ;
                                    IsCorrect = false;
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(ht[PreviousField]) > Convert.ToDouble(ht[Xe.Name]))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField; ;
                                    IsCorrect = false;
                                }
                            }
                        }
                        PreviousField = Xe.Name;
                        PreviousFieldName = Xe.GetAttribute("FieldName");
                    }
                    else
                    {
                        return IsCorrect;
                    }
                }
                else {
                    MessageResult = null;
                    return true;
                }
            }
            return IsCorrect;
        }


        /// <summary>
        /// 判断是否小于
        /// </summary>
        /// <param name="NodeList">需要判断的节点</param>
        /// <param name="ht">需要判断的数据</param>
        /// <returns>bool</returns>
        public bool Less(XmlNodeList NodeList, Hashtable ht)
        {
            string PreviousField = null;
            string PreviousFieldName = string.Empty;
            bool IsCorrect = true;//是否全部满足验证条件
            foreach (XmlElement Xe in NodeList)
            {
                if (ht.ContainsKey(Xe.Name) && !"".Equals(ht[Xe.Name]))
                {
                    if (IsCorrect)
                    {
                        if (PreviousField != null)
                        {
                            if ("DateTime".Equals(Xe.GetAttribute("Type")))
                            {
                                if (Convert.ToDateTime(ht[PreviousField]) > Convert.ToDateTime(ht[Xe.Name]) || Convert.ToDateTime(ht[PreviousField]) == Convert.ToDateTime(ht[Xe.Name]))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField;
                                    IsCorrect = false;
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(ht[PreviousField]) > Convert.ToDouble(ht[Xe.Name]) || Convert.ToDouble(ht[PreviousField]) == Convert.ToDouble(ht[Xe.Name]))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField;
                                    IsCorrect = false;
                                }
                            }
                        }
                        PreviousField = Xe.Name;
                        PreviousFieldName = Xe.GetAttribute("FieldName");
                    }
                    else
                    {
                        return IsCorrect;
                    }
                }
                else
                {
                    MessageResult = null;
                    return true;
                }
            }
            return IsCorrect;
        }


        /// <summary>
        /// 判断是否等于
        /// </summary>
        /// <param name="NodeList">需要判断的节点</param>
        /// <param name="ht">需要判断的数据</param>
        /// <returns>bool</returns>
        public bool Equal(XmlNodeList NodeList, Hashtable ht)
        {
            string PreviousField = null;
            string PreviousFieldName = string.Empty;
            bool IsCorrect = true;//是否全部满足验证条件
            foreach (XmlElement Xe in NodeList)
            {
                if (ht.ContainsKey(Xe.Name) && !"".Equals(ht[Xe.Name]))
                {
                    if (IsCorrect)
                    {
                        if (PreviousField != null)
                        {
                            if ("DateTime".Equals(Xe.GetAttribute("Type")))
                            {
                                if (Convert.ToDateTime(ht[PreviousField]) != Convert.ToDateTime(ht[Xe.Name]))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField;
                                    IsCorrect = false;
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(ht[PreviousField]) != Convert.ToDouble(ht[Xe.Name]))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField;
                                    IsCorrect = false;
                                }
                            }
                        }
                        PreviousField = Xe.Name;
                        PreviousFieldName = Xe.GetAttribute("FieldName");
                    }
                    else
                    {
                        return IsCorrect;
                    }
                }
                else {
                    MessageResult = null;
                    return true;
                }
            }
            return IsCorrect;
        }


        /// <summary>
        /// 判断是否不等于
        /// </summary>
        /// <param name="NodeList">需要判断的节点</param>
        /// <param name="ht">需要判断的数据</param>
        /// <returns>bool</returns>
        public bool NotEqual(XmlNodeList NodeList, Hashtable ht)
        {
            string PreviousField = null;
            string PreviousFieldName = string.Empty;
            bool IsCorrect = true;//是否全部满足验证条件
            foreach (XmlElement Xe in NodeList)
            {
                if (ht.ContainsKey(Xe.Name) && !"".Equals(ht[Xe.Name]))
                {
                    if (IsCorrect)
                    {
                        if (PreviousField != null)
                        {
                            if ("DateTime".Equals(Xe.GetAttribute("Type")))
                            {
                                if (Convert.ToDateTime(ht[PreviousField]) == Convert.ToDateTime(ht[Xe.Name]))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField;
                                    IsCorrect = false;
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(ht[PreviousField]) == Convert.ToDouble(ht[Xe.Name]))
                                {
                                    MessageResult = PreviousFieldName + "@Default" + Xe.GetAttribute("FieldName") + "\",\"Name\":\"" + PreviousField;
                                    IsCorrect = false;
                                }
                            }
                        }
                        PreviousField = Xe.Name;
                        PreviousFieldName = Xe.GetAttribute("FieldName");
                    }
                    else
                    {
                        return IsCorrect;
                    }
                }
                else {
                    MessageResult = null;
                    return true;
                }
            }
            return IsCorrect;
        }


        /// <summary>
        /// 判断是否至少有一个不为空
        /// </summary>
        /// <param name="NodeList">需要判断的节点</param>
        /// <param name="ht">需要判断的数据</param>
        /// <returns>bool</returns>
        public bool OneNotEmpty(XmlNodeList NodeList, Hashtable ht)
        {
            if (NodeList.Count < 1)
            {
                MessageResult = null;
                return true;
            }
            bool IsCorrect = false;//是否有一个满足条件
            foreach (XmlElement Xe in NodeList)
            {
                if (ht.ContainsKey(Xe.Name))
                {
                    if (!IsCorrect)
                    {
                        if (ht[Xe.Name] != null && !"".Equals(ht[Xe.Name]))
                        {
                            MessageResult = null;
                            IsCorrect = true;
                        }
                    }
                }
                else {
                    MessageResult = null;
                    return true;
                }
                MessageResult += "\"" + Xe.GetAttribute("FieldName") + "\"";
            }
            MessageResult += "@Default" + "\",\"Name\":\"" + ((XmlElement)NodeList[0]).Name;
            if (IsCorrect) {
                MessageResult = null;
            }
         

            return IsCorrect;
        }

        /// <summary>
        /// 判断是否全部为空
        /// </summary>
        /// <param name="NodeList">需要判断的节点</param>
        /// <param name="ht">需要判断的数据</param>
        /// <returns>bool</returns>
        public bool AllEmpty(XmlNodeList NodeList, Hashtable ht)
        {
            bool IsCorrect = true;//是否全部满足条件
            foreach (XmlElement Xe in NodeList)
            {
                if (ht.ContainsKey(Xe.Name))
                {
                    if (IsCorrect)
                    {
                        if (ht[Xe.Name] != null && !"".Equals(ht[Xe.Name]))
                        {
                            MessageResult = Xe.GetAttribute("FieldName") + "@Default" + "\",\"Name\":\"" + Xe.Name;
                            IsCorrect = false;
                        }
                    }
                    else
                    {
                        return IsCorrect;
                    }
                }
                else {
                    MessageResult = null;
                    return true;
                }
            }
            return IsCorrect;
        }


        /// <summary>
        /// 判断是否不为空
        /// </summary>
        /// <param name="NodeList">需要判断的节点</param>
        /// <param name="ht">需要判断的数据</param>
        /// <returns>bool</returns>
        public bool NotEmpty(XmlNodeList NodeList, Hashtable ht)
        {
            bool IsCorrect = true;//是否全部满足条件
            foreach (XmlElement Xe in NodeList)
            {
                if (ht.ContainsKey(Xe.Name))
                {
                    if (IsCorrect)
                    {
                        if (ht[Xe.Name] != null && "".Equals(ht[Xe.Name]))
                        {
                            MessageResult = Xe.GetAttribute("FieldName") + "@Default" + "\",\"Name\":\"" + Xe.Name;
                            IsCorrect = false;
                        }
                    }
                    else
                    {
                        return IsCorrect;
                    }
                }
                else {
                    MessageResult = null;
                    return true;
                }
            }
            return IsCorrect;
        }

        
       
    }


}


  
    