using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Common.Json
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 序列化(排除某些字段)
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="fields">要排除的字段</param>
        /// <returns></returns>
        public static string ToJsonExclude(object obj, params string[] fields)
        {
            return ToJsonExclude(obj, DateTimeSerializeMode.ISO, fields);
        }
        /// <summary>
        /// 序列化(排除某些字段)
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="dateFormat">日期序列化的格式</param>
        /// <param name="fields">要排除的字段</param>
        /// <returns></returns>
        public static string ToJsonExclude(object obj, DateTimeSerializeMode dateFormat, params string[] fields)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            if (dateFormat != DateTimeSerializeMode.MS)
            {
                List<JsonConverter> converters = new List<JsonConverter>();
                if (dateFormat == DateTimeSerializeMode.ISO)
                {
                    IsoDateTimeConverter c = new IsoDateTimeConverter();
                    c.DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss";
                    converters.Add(c);
                }
                else if (dateFormat == DateTimeSerializeMode.JS)
                {
                    converters.Add(new JavaScriptDateTimeConverter());
                }
                setting.Converters = converters;
            }
            if (fields.Length > 0)
                setting.ContractResolver = new ExcludePropertiesContractResolver(fields);
            return JsonConvert.SerializeObject(obj, Formatting.None, setting);
        }
        /// <summary>
        /// 序列化(包含某些字段)
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="fields">要序列化的字段</param>
        /// <returns></returns>
        public static string ToJsonInclude(object obj, params string[] fields)
        {
            return ToJsonInclude(obj, DateTimeSerializeMode.ISO, fields);
        }
        /// <summary>
        /// 序列化(包含某些字段)
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="dateFormat">日期序列化的格式</param>
        /// <param name="fields">要序列化的字段</param>
        /// <returns></returns>
        public static string ToJsonInclude(object obj, DateTimeSerializeMode dateFormat, params string[] fields)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            if (dateFormat != DateTimeSerializeMode.MS)
            {
                List<JsonConverter> converters = new List<JsonConverter>();
                if (dateFormat == DateTimeSerializeMode.ISO)
                {
                    IsoDateTimeConverter c = new IsoDateTimeConverter()
                    {
                        DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss"
                    };
                    converters.Add(c);
                }
                else if (dateFormat == DateTimeSerializeMode.JS)
                {
                    converters.Add(new JavaScriptDateTimeConverter());
                }
                setting.Converters = converters;
            }
            if (fields.Length > 0)
                setting.ContractResolver = new IncludePropertiesContractResolver(fields);
            return JsonConvert.SerializeObject(obj, Formatting.None, setting);
        }
        /// <summary>
        /// 反序列化json字符串为指定类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public T DeserializeObject<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
        
        /// <summary>
        /// JToken 转为 具体类型的 对象
        /// </summary>
        /// <param name="token"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object JTokenTo(JToken token, Type type)
        {
            bool isNull = IsJTokenNull(token);
            if (isNull && type.IsValueType && (Nullable.GetUnderlyingType(type) == null))
            {
                throw new Exception("Null 无法转换为非空类型 " + type);
            }

            if (isNull) return null;

            MethodInfo mi = typeof(JToken).GetMethod("ToObject", new Type[] { });
            MethodInfo mi2 = mi.MakeGenericMethod(new Type[] { type });
            return mi2.Invoke(token, null);
        }
        /// <summary>
        /// JToken 转为 具体类型的 对象(泛型方法)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T JTokenTo<T>(JToken token)
        {
            Type type = typeof(T);
            return (T)JTokenTo(token, type);
        }
        /// <summary>
        /// 检查 JToken 是否为null
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsJTokenNull(JToken token)
        {
            return token == null || token.Type == JTokenType.Null;
        }

        /// <summary>
        /// 检查 JToken 是否为null或empty(比如空字符串)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsJTokenNullOrEmpty(JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }

        /// <summary>
        /// 将DataTable转换成json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            int count = dt.Rows.Count;
            if (count != 0)
            {
                jsonBuilder.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonBuilder.Append("");
                        jsonBuilder.Append(dt.Columns[j].ColumnName);
                        jsonBuilder.Append(":\'");
                        jsonBuilder.Append(dt.Rows[i][j].ToString());
                        jsonBuilder.Append("\',");
                    }
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("},");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("]");
                return jsonBuilder.ToString();
            }
            else
            {
                return null;
            }
        }
    }
    /// <summary>
    /// 日期序列化格式
    /// </summary>
    public enum DateTimeSerializeMode
    {
        /// <summary>
        /// 一般用这个, "2013-10-08T09:42:52", 这也是Ext.Net默认序列化的日期格式，前台对应dateFormat="c"
        /// </summary>
        ISO,
        /// <summary>
        /// "\/Date(1381196572853+0800)\/", 这是Newtonsoft默认序列化的日期格式，前台对应dateFormat="MS"
        /// </summary>
        MS,
        /// <summary>
        /// new Date(1381196572853)
        /// </summary>
        JS
    }
    internal class ExcludePropertiesContractResolver : DefaultContractResolver
    {
        List<string> lstExclude;
        public ExcludePropertiesContractResolver(IEnumerable<string> excludedProperties)
        {
            lstExclude = new List<string>(excludedProperties);
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return new List<JsonProperty>(base.CreateProperties(type, memberSerialization)).FindAll(delegate (JsonProperty p)
            {
                return !lstExclude.Contains(p.PropertyName);
            });
        }
    }

    internal class IncludePropertiesContractResolver : DefaultContractResolver
    {
        List<string> lstInclude;
        public IncludePropertiesContractResolver(IEnumerable<string> includedProperties)
        {
            lstInclude = new List<string>(includedProperties);
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return new List<JsonProperty>(base.CreateProperties(type, memberSerialization)).FindAll(delegate (JsonProperty p)
            {
                return lstInclude.Contains(p.PropertyName);
            });
        }
    }
}
