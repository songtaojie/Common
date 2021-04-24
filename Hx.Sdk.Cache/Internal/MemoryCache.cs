using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
        }

        public bool Remove(string key)
        {
            try
            {
                _cache.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> RemoveAsync(string key)
        {
            var result = Remove(key);
            return Task.FromResult(result);
        }

        public void Set<T>(string key, T value, TimeSpan time)
        {
            _cache.Set<T>(key, value, time);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan time)
        {
            Set<T>(key, value, time);
            return Task.CompletedTask;
        }
    }
}
