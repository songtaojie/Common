using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.Cache
{
    /// <summary>
    /// 内置缓存接口
    /// </summary>
    public interface IMemoryCache
    {
        /// <summary>
        /// 根据缓存的键获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 根据缓存的键获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 设置缓存的值
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存的值</param>
        void Set<T>(string key, T value, TimeSpan time);

        /// <summary>
        /// 设置缓存的值
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存的值</param>
        Task SetAsync<T>(string key, T value, TimeSpan time);

        /// <summary>
		/// 移除数据
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool Remove(string key);

        /// <summary>
		/// 移除数据
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<bool> RemoveAsync(string key);
    }
}
