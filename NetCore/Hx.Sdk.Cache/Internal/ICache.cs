using FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.Cache
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 获取和设置缓存，永不过期
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { get; set; }

        /// <summary>
        ///  缓存个数
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 是否包含缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsKey(string key);

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间，秒</param>
        /// <returns></returns>
        bool Set<T>(string key, T value, int? expire = -1);

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <typeparam name="T">数据的类型参数</typeparam>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool Set<T>(string key, T value, TimeSpan? expiry = null);

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
		/// 获取数据
		/// </summary>
		/// <param name="key">缓存的键</param>
		/// <returns></returns>
		string Get(string key);

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool Set(string key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// 批量移除缓存项
        /// </summary>
        /// <param name="keys">键集合</param>
        void Remove(params string[] keys);
      
    }
}
