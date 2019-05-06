using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Common.Cache
{
    /// <summary>
    /// System.Web.Caching.Cache缓存类的封装操作
    /// </summary>
    public sealed class CacheHelper
    {
        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
            return cache[key];
        }

        /// <summary>
        /// 设置缓存的值
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存的值</param>
        public static void Set(string key, object value)
        {
            System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
            cache[key] = value;
        }
        /// <summary>
        /// 设置缓存的值
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存的值</param>
        /// <param name="time">缓存时间</param>
        public static void Set(string key, object value, DateTime time)
        {
            System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
            cache.Insert(key, value, null, time, TimeSpan.Zero);
        }
        /// <summary>
        /// 根据键从缓存中移除数据
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
            cache.Remove(key);
        }
    }
}
