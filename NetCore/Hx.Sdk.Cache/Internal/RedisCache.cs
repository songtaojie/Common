﻿using FreeRedis;
using System;

namespace Hx.Sdk.Cache
{
    internal class RedisCache : IRedisCache
    {
        /// <summary>
		/// 数据库编号
		/// </summary>
		private static int? _dbNum;
        private readonly IRedisClient _redisClient;

        public int Count => throw new NotImplementedException();

        public object this[string key] 
        { 
            get => Get<object>(key); 
            set => Set(key,value,-1); 
        }

        public RedisCache(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        #region ICache接口实现
        public void SetRedisDbNum(int dbNum)
        {
            _dbNum = dbNum;
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            return this.Do<string>(db =>
            {
                return db.Get(key);
            });
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            return this.Do<T>(db =>
            {
                return db.Get<T>(key);
            });
        }

        public bool Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            return this.Do<bool>((db) =>
            {
                if (expiry.HasValue)
                {
                    db.Set(key, value, expiry.Value);
                }
                else
                {
                    db.Set(key, value);
                }
                return true;
            });
        }


        public bool Set(string key, string value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            return this.Do<bool>(db =>
            {
                if (expiry.HasValue)
                {
                    db.Set(key, value, expiry.Value);
                }
                else
                {
                    db.Set(key, value);
                }
                return true;
            });
        }


        public bool ContainsKey(string key)
        {
            return this.Do(db =>
            {
                return db.Exists(key);
            });
        }

        public bool Set<T>(string key, T value, int? expire = -1)
        {
            return this.Do(db =>
            {
                if (expire.HasValue && expire > 0)
                {
                    db.Set(key, value, TimeSpan.FromSeconds(expire.Value));
                }
                else
                {
                    db.Set(key, value);
                }
                return true;
            });
            
        }

        public void Remove(params string[] keys)
        {
            this.Do(db =>
            {
                return db.Del(keys);
            });
        }
        #endregion
        #region 私有方法
        /// <summary>
		/// 获取数据库
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		private T Do<T>(Func<IRedisClient, T> func)
        {
            if (_dbNum.HasValue && _dbNum > 0)
            {
                using var database = this._redisClient.GetDatabase(_dbNum);
                return func(database);
            }
            else
            {
                return func(_redisClient);
            }
        }

        #endregion
    }
}
