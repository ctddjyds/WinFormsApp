using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BaiduNetworkSearch
{
    class UtilityClass
    {
        public static T GetObject<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            T obj = (T)serializer.ReadObject(ms);
            return obj;
        }
    }
}
