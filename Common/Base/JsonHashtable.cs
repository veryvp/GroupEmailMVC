using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections;
namespace Common.Base
{
    public class JsonHashtable
    {
        /// <summary>
        /// 反序列化字符串
        /// <param name="obj">需要反序列化的字符串</param>
        /// <returns>MyHashTable</returns>
        ///  </summary>
        public MyHashTable DecodeSort(string JsonStr)
        {
            MyHashTable hashTable = new MyHashTable();
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            object Json = jsonSerializer.DeserializeObject(JsonStr);
            if (Json.GetType().FullName == "System.Object[]")
            {
                object[] JsonList = (object[])Json;
                int i = 0;
                foreach (object obj in JsonList)
                {
                    hashTable.Add(i, DecodeObjSort(obj));
                    i++;
                }

            }
            else
            {
                hashTable = (MyHashTable)DecodeObjSort(Json);
            }


            return hashTable;
        }


        /// <summary>
        /// 反序列化对象
        /// <param name="obj">需要反序列化的对象</param>
        /// <returns>MyHashTable</returns>
        ///  </summary>
        private object DecodeObjSort(object Obj)
        {
            MyHashTable ht = new MyHashTable();
            //int index = 0;
            //if (Obj.GetType().FullName == "System.Object[]")
            //{
            //    object[] JsonList = (object[])Obj;
            //    foreach (object O in JsonList)
            //    {
            //        //ht = DecodeObjSort(O);这样写只会读出最后一条数据
            //        ht.Add(index,DecodeObjSort(O));
            //        index++;
            //    }
            //}
            if (Obj.GetType().FullName == "System.Object[]")
            {
                object[] JsonList = (object[])Obj;
                int i = 0;
                foreach (object O in JsonList)
                {

                    if (O.GetType().Name == "Dictionary`2")
                    {
                        JsonList[i] = (MyHashTable)DecodeObj(O);
                    }
                    else
                    {
                        JsonList[i] = O.ToString();
                    }
                    i = i + 1;
                }
                return JsonList;
            }
            else
            {
                if (!Obj.Equals(null))
                {
                    System.Collections.Generic.Dictionary<string, object> OList = (System.Collections.Generic.Dictionary<string, object>)Obj;
                    foreach (KeyValuePair<string, object> obj in OList)
                    {
                        string Key = obj.Key.ToString();
                        string Value = obj.Value == null ? null : obj.Value.ToString();

                        if (Value == "System.Object[]")
                        {
                            object[] JsonList1 = (object[])obj.Value;
                            if (JsonList1.Length > 0)
                            {
                                ht.Add(Key, DecodeObjSort(obj.Value));
                            }
                            else {
                                ht.Add(Key, "");
                            }
                          
                        }
                        else if (Value == "System.Collections.Generic.Dictionary`2[System.String,System.Object]")
                        {
                            Dictionary<string, object> ObjValue = (Dictionary<string, object>)obj.Value;
                            if(ObjValue.Count==0){
                               ht.Add(Key, "");
                            }else{
                                ht.Add(Key, DecodeObjSort(obj.Value));
                            }
                            
                        }
                        else
                        {
                            ht.Add(Key, Value);
                        }
                    }

                }
                return (MyHashTable)ht;
            }
        }



        /// <summary>
        /// 反序列化字符串
        /// <param name="obj">需要反序列化的字符串</param>
        /// <returns>Hashtable</returns>
        ///  </summary>
        public Hashtable EasyDecode(string JsonStr)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            Hashtable hashTable = jsonSerializer.Deserialize<Hashtable>(JsonStr);
            hashTable = RestoreValue(hashTable);
            return hashTable;

        }


        /// <summary>
        /// 反序列化字符串
        /// <param name="obj">需要反序列化的字符串</param>
        /// <returns>Hashtable</returns>
        ///  </summary>
        public Hashtable Decode(string JsonStr)
        {
            Hashtable hashTable = new Hashtable();
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            object Json = jsonSerializer.DeserializeObject(JsonStr);
            if (Json.GetType().FullName == "System.Object[]")
            {
                object[] JsonList = (object[])Json;
                int i = 0;
                foreach (object obj in JsonList)
                {
                    hashTable.Add(i, DecodeObj(obj));
                    i++;
                }

            }
            else
            {
                hashTable = (Hashtable)DecodeObj(Json);
            }


            return hashTable;
        }


        /// <summary>
        /// 反序列化对象
        /// <param name="obj">需要反序列化的对象</param>
        /// <returns>MyHashTable</returns>
        ///  </summary>
        private object DecodeObj(object Obj)
        {
            Hashtable ht = new Hashtable();
            //if (Obj.GetType().FullName == "System.Object[]")
            //{
            //    object[] JsonList = (object[])Obj;
            //    foreach (object O in JsonList)
            //    {
            //        ht = DecodeObj(O);
            //    }
            //}
            if (Obj.GetType().FullName == "System.Object[]")
            {
                object[] JsonList = (object[])Obj;
                int i = 0;
                foreach (object O in JsonList)
                {

                    if (O.GetType().Name == "Dictionary`2")
                    {
                        JsonList[i] = (Hashtable)DecodeObj(O);
                    }
                    else
                    {
                        JsonList[i] = O.ToString();
                    }
                    i = i + 1;
                }
                return JsonList;
            }
            else
            {
                if (!Obj.Equals(null))
                {
                    System.Collections.Generic.Dictionary<string, object> OList = (System.Collections.Generic.Dictionary<string, object>)Obj;
                    foreach (KeyValuePair<string, object> obj in OList)
                    {
                        string Key = obj.Key.ToString();
                        string Value = obj.Value == null ? null : obj.Value.ToString();

                        if (Value == "System.Object[]")
                        {
                            object[] JsonList1 = (object[])obj.Value;
                            if (JsonList1.Length > 0)
                            {
                                ht.Add(Key, DecodeObj(obj.Value));
                                //ht.Add(Key, JsonList1);
                            }
                            else
                            {
                                ht.Add(Key, "");
                            }

                        }
                        else if (Value == "System.Collections.Generic.Dictionary`2[System.String,System.Object]")
                        {
                            Dictionary<string, object> ObjValue = (Dictionary<string, object>)obj.Value;
                            if (ObjValue.Count == 0)
                            {
                                ht.Add(Key, "");
                            }
                            else
                            {
                                ht.Add(Key, DecodeObj(obj.Value));
                            }

                        }
                        else
                        {
                            ht.Add(Key, Value);
                        }
                    }

                }
                return (Hashtable)ht;
            }
        }


         /// <summary>
        /// 将json转化成实体类
        /// </summary>
        /// <returns>实体类</returns>
        public T JsonToModel<T>(string JsonString)
        {
            if (string.IsNullOrEmpty(JsonString))
            {
                return default(T);
            }
            System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return jsonSerializer.Deserialize<T>(JsonString);
        }


        /// <summary>
        /// 将实体类转化成json
        /// </summary>
        /// <returns>Json字符串</returns>
        public string ModelToJson<T>(T Model)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            return jsonSerializer.Serialize(Model);
        }

        
        /// <summary>
        /// 还原哈希表中被编码的字符串
        /// </summary>
        /// <param name="Ht">需要被还原的哈希表</param>
        /// <returns>还原后的哈希表</returns>
        public Hashtable RestoreValue(Hashtable Ht) {
            Hashtable ht = new Hashtable();
            foreach (string key in Ht.Keys) {
                string value = Ht[key] == null ? null : HttpContext.Current.Server.UrlDecode(Ht[key].ToString().Trim());
                ht.Add(key, value);
            }

            return ht;
        }

        /// <summary>
        /// 还原哈希表中被编码的字符串
        /// </summary>
        /// <param name="Ht">需要被还原的哈希表</param>
        /// <returns>还原后的哈希表</returns>
        public MyHashTable RestoreValue(MyHashTable Ht)
        {
            foreach (string key in Ht.Keys)
            {
                Ht[key] = Ht[key] == null ? null : HttpContext.Current.Server.UrlDecode(Ht[key].ToString().Trim());
            }

            return Ht;
        }



        /// <summary>
        /// json添加节点 只适用单层json
        /// </summary>
        /// <param name="Json">需要操作的Json</param>
        /// <param name="InsertKey">需要插入的Key</param>
        /// <param name="InsertValue">需要插入的值</param>
        /// <returns>Json字符串</returns>
        public string AddNode(string Json, string InsertKey, object InsertValue)
        {
            Json = Json.Insert(Json.LastIndexOf("}"), ",\"" + InsertKey + "\":\"" + InsertValue.ToString() + "\"");
            return Json;
        }

        ///// <summary>
        ///// json删除节点
        ///// </summary>
        ///// <param name="Json">需要操作的Json</param>
        ///// <param name="RemoveKey">需要删除的Key</param>
        ///// <returns>Json字符串</returns>
        //public string RemoveNode(string Json, string RemoveKey)
        //{
        //    Json = Json.Trim();
        //    Json = Json.Trim('{');
        //    Json = Json.Trim('}');

        //    string NewJson = "{";
        //    string[] str = Json.Split(',');
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        string a = str[i].ToString();
        //        string[] a1 = a.Split(':');
        //        string key = a1[0].Trim();
        //        if (key != "\"" + RemoveKey + "\"")
        //        {
        //            NewJson = NewJson + key + ":" + a1[1] + ",";
        //        }
        //    }
        //    NewJson = NewJson.Trim(',');
        //    NewJson = NewJson + "}";

        //    return NewJson;
        //}


        ///// <summary>
        ///// json修改节点
        ///// </summary>
        ///// <param name="Json">需要操作的Json</param>
        ///// <param name="ModifiedKey">需要修改的Key</param>
        ///// <param name="NewValue">新的值</param>
        ///// <returns>Json字符串</returns>
        //public string ModifyNode(string Json, string ModifiedKey, string NewValue)
        //{

        //    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        //    object sdsd = jsonSerializer.DeserializeObject(Json);
        //    Json = Json.Trim();
        //    Json = Json.Trim('{');
        //    Json = Json.Trim('}');
        //    string NewJson = "{";
        //    string[] str = Json.Split(',');
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        string a = str[i].ToString();
        //        string[] a1 = a.Split(':');
        //        string key = a1[0].Trim();
        //        if (key == "\"" + ModifiedKey + "\"")
        //        {
        //            NewJson = NewJson + key + ":\"" + NewValue.Trim() + "\",";
        //        }
        //        else {
        //            NewJson = NewJson + key + ":" + a1[1].Trim() + ",";
        //        }
        //    }
        //    NewJson = NewJson.Trim(',');
        //    NewJson = NewJson + "}";

        //    return NewJson;
        //}



        /// <summary>
        /// json根据key查询节点
        /// </summary>
        /// <param name="Json">需要操作的Json</param>
        /// <param name="Key">需要删除的Key</param>
        /// <returns>第一个满足条件的值</returns>
        public string GetNodeByKey(string Json, string Key)
        {
            Json = Json.Trim();
            Json = Json.Trim('{');
            Json = Json.Trim('}');


            string[] str = Json.Split(',');
            for (int i = 0; i < str.Length; i++)
            {
                string a = str[i].ToString();
                string[] a1 = a.Split(':');
                string key = a1[0].Trim();
                if (key.Contains(Key))
                {
                    return a1[1].Trim().Trim('\"');
                }
            }

            return null;
        }

        /// <summary>
        /// 将DataTable转换为Hashtable
        /// </summary>
        /// <param name="DT">需要被转换的DataTable</param>
        /// <returns>转换之后的Hashtable</returns>
        public Hashtable DataTableToHashtable(System.Data.DataTable DT)
        {
            Hashtable Ht = new Hashtable();
            if (DT.Rows.Count == 1)
            {
                foreach (System.Data.DataColumn DC in DT.Columns)
                {
                    Ht.Add(DC.ColumnName, DT.Rows[0][DC.ColumnName]);
                }
            }
            if (DT.Rows.Count > 1)
            {
                Hashtable[] ht = new Hashtable[DT.Rows.Count];
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    foreach (System.Data.DataColumn DC in DT.Columns)
                    {
                        ht[i].Add(DC.ColumnName, DT.Rows[i][DC.ColumnName]);
                    }
                }
                Ht.Add("DataList", ht);
            }


            return Ht;
        }


        /// <summary>
        /// 将DataTable转为Json
        /// </summary>
        /// <param name="DT">需要被转换的DataTable</param>
        /// <returns>转换之后的Json</returns>
        public string DataTableToJson(System.Data.DataTable DT)
        {
            System.Text.StringBuilder JsonSb = new System.Text.StringBuilder("{");
            if (DT.Rows.Count == 1)
            {
                foreach (System.Data.DataColumn DC in DT.Columns)
                {
                    JsonSb.Append("\"" + DC.ColumnName + "\":\"" + DT.Rows[0][DC.ColumnName] + "\",");
                }
                JsonSb.Remove(JsonSb.Length - 1, 1);
            }

            if (DT.Rows.Count > 1)
            {
                JsonSb.Append("\"DataList\":[ ");

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    JsonSb.Append("{");
                    foreach (System.Data.DataColumn Col in DT.Columns)
                    {
                        JsonSb.Append("\"" + Col.ColumnName + "\":\"" + DT.Rows[i][Col.ColumnName] + "\",");
                    }
                    JsonSb.Remove(JsonSb.Length - 1, 1);
                    JsonSb.Append("},");
                }
                JsonSb.Remove(JsonSb.Length - 1, 1);
                JsonSb.Append("]");
            }


            JsonSb.Append("}");

            return JsonSb.ToString();
        }

        //哈希表转字符串
        public string HashtableToString(Hashtable ht) {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(ht);
        }

        /// <summary>
        /// 验证HashTable是否全部为""
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public bool IsNullHashtable(Hashtable ht)
        {
            bool a = true;
            foreach (DictionaryEntry de in ht)
            {
                string Key = de.Key.ToString();
                string Value = de.Value.ToString();
                if (Value != "")
                {
                    a = false;
                    break;
                }
            }
            return a;
        }
    }
}