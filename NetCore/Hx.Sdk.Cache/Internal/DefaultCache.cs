using FreeRedis;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.Cache
{
    /// <summary>
    /// 内存缓存缓存
    /// </summary>
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
            set => Set(key, value,-1);
        }

        public int Count => (_memoryCache as MemoryCache).Count;


        public bool ContainsKey(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            return _memoryCache.TryGetValue(key,out object _);
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            return _memoryCache.Get<T>(key);
        }


        public void Remove(params string[] keys)
        {
            foreach (var key in keys)
            {
                _memoryCache.Remove(key);
            }
        }

        public bool Set<T>(string key, T value, int? expiry = -1)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (expiry.HasValue && expiry > 0)
            {
                var result = _memoryCache.Set(key, value, TimeSpan.FromSeconds(expiry.Value));
                return result != null;
                
            }
            else
            {
                var result = _memoryCache.Set(key, value);
                return result != null;
            }
        }

        public bool Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (expiry.HasValue)
            {
                var result = _memoryCache.Set(key, value, expiry.Value);
                return result != null;
            }
            else
            {
                var result = _memoryCache.Set(key, value);
                return result != null;
            }
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (ContainsKey(key))
            {
                var bytes = _memoryCache.Get<byte[]>(key);
                return Encoding.UTF8.GetString(bytes);
            }
            return null;
        }

        public bool Set(string key, string value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(value))
            {
                return Set(key, Array.Empty<byte>(), expiry);
            }
            else
            {
                return Set(key, Encoding.UTF8.GetBytes(value), expiry);
            }
        }
       
    }
}
