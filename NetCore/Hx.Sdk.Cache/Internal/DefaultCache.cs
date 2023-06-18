using FreeRedis;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.Cache
{
    internal class DefaultCache : ICache
    {
        private IMemoryCache _memoryCache;

        public DefaultCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public object this[string key] 
        {
            get => Get<object>(key);
            set => Set(key, value);
        }

        public int Count => (_memoryCache as MemoryCache).Count;


        public bool ContainsKey(string key)
        {
            return _memoryCache.TryGetValue(key,out object _);
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Remove(params string[] keys)
        {
            foreach (var key in keys)
            {
                _memoryCache.Remove(key);
            }
        }

        public bool Set<T>(string key, T value, int expire = -1)
        {
            if (expire <= 0)
            {
                var result = _memoryCache.Set(key, value);
                return result != null;
            }
            else
            {
                var result = _memoryCache.Set(key, value, TimeSpan.FromSeconds(expire));
                return result != null;
            }
        }

        public bool Set<T>(string key, T value, TimeSpan expire)
        {
            var result = _memoryCache.Set(key, value, expire);
            return result != null;
        }
    }
}
