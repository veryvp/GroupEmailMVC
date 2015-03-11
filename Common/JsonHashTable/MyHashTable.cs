using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Common.Base
{
    public class MyHashTable: Hashtable
    {
            private JsonHashtable myjson = new JsonHashtable();
            private ArrayList list = new ArrayList();
            /// <summary>
            /// 添加数据
            ///  </summary>
            public override void Add(object key, object value)
            {
                base.Add(key, value);
                list.Add(key);
            }
            /// <summary>
            /// 清空数据
            ///  </summary>
            public override void Clear()
            {
                base.Clear();
                list.Clear();
            }
            /// <summary>
            /// 按key删除数据
            ///  </summary>
            public override void Remove(object key)
            {
                base.Remove(key);
                list.Remove(key);
            }
            /// <summary>
            /// 按装入顺序返回Keys
            /// <returns>ICollection</returns>
            ///  </summary>
            public override ICollection Keys
            {
                get
                {
                    return list;
                }
            }
            /// <summary>
            /// 反序列化字符串
            /// <param name="obj">需要反序列化的字符串</param>
            /// <returns>MyHashTable</returns>
            ///  </summary>
            public MyHashTable Decode(string json) {
                return myjson.DecodeSort(json);
            }
    }
}