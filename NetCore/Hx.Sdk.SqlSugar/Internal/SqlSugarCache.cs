using Microsoft.Extensions.Caching.Memory;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Hx.Sdk.SqlSugar
{
    /// <summary>
    /// SqlSugar二级缓存
    /// </summary>
    internal class SqlSugarCache : ICacheService
    {
        private readonly IMemoryCache _cache;
        public SqlSugarCache(IMemoryCache cache) 
        {
            _cache = cache;
        }

        public void Add<V>(string key, V value)
        {
            _cache.Set(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            _cache.Set(key, value, DateTimeOffset.Now.AddSeconds(cacheDurationInSeconds));
        }

        public bool ContainsKey<V>(string key)
        {
            return _cache.TryGetValue(key, out _);
        }

        public V Get<V>(string key)
        {
            return _cache.Get<V>(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);

            //.NET 7
            //var coherentState = _cache.GetType().GetField("_coherentState", flags).GetValue(_cache);
            //var entries = coherentState.GetType().GetField("_entries", flags).GetValue(coherentState);

            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (!_cache.TryGetValue<V>(cacheKey, out V value))
            {
                value = create();
                _cache.Set(cacheKey, value, DateTime.Now.AddSeconds(cacheDurationInSeconds));
            }
            return value;
        }

        public void Remove<V>(string key)
        {
            _cache.Remove(key);
        }
    }
}
