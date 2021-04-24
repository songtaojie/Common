using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Cache
{
    /// <summary>
    /// 内置的缓存对象
    /// </summary>
    internal class MemoryCache : IMemoryCache
    {
        private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;
        //还是通过构造函数的方法，获取
        public MemoryCache(Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Set<T>(string key, T value, TimeSpan time)
        {
            _cache.Set<T>(key, value, time);
        }
    }
}
