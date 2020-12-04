using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.NetCore.Cache
{
    /// <summary>
    /// 内置缓存接口
    /// </summary>
    public interface ICaching
    {
        /// <summary>
        /// 根据缓存的键获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 设置缓存的值
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存的值</param>
        void Set<T>(string key, T value, TimeSpan time);
    }
}
