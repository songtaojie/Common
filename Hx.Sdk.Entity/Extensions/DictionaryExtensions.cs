using System.Collections.Generic;
using Hx.Sdk.Entity;
namespace Hx.Sdk.Extensions
{
    /// <summary>
    /// 字典类型扩展
    /// </summary>
    [SkipScan]
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 合并两个字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic">字典</param>
        /// <param name="newDic">新字典</param>
        /// <returns></returns>
        public static Dictionary<string, T> AddOrUpdate<T>(this Dictionary<string, T> dic, Dictionary<string, T> newDic)
        {
            foreach (var key in newDic.Keys)
            {
                if (dic.ContainsKey(key))
                    dic[key] = newDic[key];
                else
                    dic.Add(key, newDic[key]);
            }

            return dic;
        }
    }
}
