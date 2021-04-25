using Hx.Sdk.Cache.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.Sdk.Cache.Internal
{
    internal class RedisCache : IRedisCache, IDisposable
    {

        // KEYS[1] = = key
        // ARGV[1] = absolute-expiration - ticks as long (-1 for none)
        // ARGV[2] = sliding-expiration - ticks as long (-1 for none)
        // ARGV[3] = relative-expiration (long, in seconds, -1 for none) - Min(absolute-expiration - Now, sliding-expiration)
        // ARGV[4] = data - byte[]
        // this order should not change LUA script depends on it
        private const string SetScript = (@"
                redis.call('HMSET', KEYS[1], 'absexp', ARGV[1], 'sldexp', ARGV[2], 'data', ARGV[4])
                if ARGV[3] ~= '-1' then
                  redis.call('EXPIRE', KEYS[1], ARGV[3])
                end
                return 1");
        private const string AbsoluteExpirationKey = "absexp";
        private const string SlidingExpirationKey = "sldexp";
        private const string DataKey = "data";
        private const long NotPresent = -1;

        private volatile ConnectionMultiplexer _connection;
        private IDatabase _database;
        /// <summary>
		/// 数据库编号
		/// </summary>
		private int _dbNum;

        private readonly RedisCacheOptions _options;
        private readonly string _instance;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        internal RedisCache(IOptions<RedisCacheOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            _options = optionsAccessor.Value;

            // 这允许对单个后端缓存进行分区，以与多个应用程序/服务一起使用。.
            _instance = _options.InstanceName ?? string.Empty;
            _dbNum = _options.ConfigurationOptions?.DefaultDatabase ?? 0;
        }
        #region IDistributedCache接口实现
        public byte[] Get(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return GetAndRefresh(key, getData: true);
        }

        public async Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken))
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            token.ThrowIfCancellationRequested();

            return await GetAndRefreshAsync(key, getData: true, token: token);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (options == null) throw new ArgumentNullException(nameof(options));
            Connect();
            var now = DateTimeOffset.UtcNow;
            var absoluteExpiration = GetAbsoluteExpiration(now, options);
            this.Do(db =>
            {
                var result = db.ScriptEvaluate(SetScript, new RedisKey[] { GetFullKey(key) },
                       new RedisValue[]
                       {
                                absoluteExpiration?.Ticks ?? NotPresent,
                                options.SlidingExpiration?.Ticks ?? NotPresent,
                                GetExpirationInSeconds(now, absoluteExpiration, options) ?? NotPresent,
                                value
                       });
                return result;
            });
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            if (value == null) throw new ArgumentNullException(nameof(value));

            if (options == null) throw new ArgumentNullException(nameof(options));

            token.ThrowIfCancellationRequested();

            await ConnectAsync(token);

            var now = DateTimeOffset.UtcNow;

            var absoluteExpiration = GetAbsoluteExpiration(now, options);

            await this.Do(db =>
            {
                var result = db.ScriptEvaluateAsync(SetScript, new RedisKey[] { GetFullKey(key) },
                       new RedisValue[]
                       {
                                absoluteExpiration?.Ticks ?? NotPresent,
                                options.SlidingExpiration?.Ticks ?? NotPresent,
                                GetExpirationInSeconds(now, absoluteExpiration, options) ?? NotPresent,
                                value
                       });
                return result;
            });
        }

        public void Refresh(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            GetAndRefresh(key, getData: false);
        }


        public async Task RefreshAsync(string key, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            token.ThrowIfCancellationRequested();

            await GetAndRefreshAsync(key, getData: false, token: token);
        }



        private byte[] GetAndRefresh(string key, bool getData)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            Connect();
            // 这也会根据需要重置LRU状态.
            // TODO: 可以通过服务器端的一项操作来完成此操作吗？ 可能的窍门就是DateTimeOffset数学.
            RedisValue[] results = this.Do(db =>
            {
                if (getData)
                {
                    return db.HashMemberGet(GetFullKey(key), AbsoluteExpirationKey, SlidingExpirationKey, DataKey);
                }
                return db.HashMemberGet(GetFullKey(key), AbsoluteExpirationKey, SlidingExpirationKey);
            });
            // TODO: Error handling
            if (results.Length >= 2)
            {
                MapMetadata(results, out DateTimeOffset? absExpr, out TimeSpan? sldExpr);
                Refresh(key, absExpr, sldExpr);
            }

            if (results.Length >= 3 && results[2].HasValue)
            {
                return results[2];
            }

            return null;
        }

        private async Task<byte[]> GetAndRefreshAsync(string key, bool getData, CancellationToken token = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            token.ThrowIfCancellationRequested();

            await ConnectAsync(token);

            // 这也会根据需要重置LRU状态.
            // TODO: 可以通过服务器端的一项操作来完成此操作吗？ 可能的窍门就是DateTimeOffset计算.
            RedisValue[] results = await this.Do(db =>
            {
                if (getData)
                {
                    return  db.HashMemberGetAsync(GetFullKey(key), AbsoluteExpirationKey, SlidingExpirationKey, DataKey);
                }
                return db.HashMemberGetAsync(GetFullKey(key), AbsoluteExpirationKey, SlidingExpirationKey);
            });
            // TODO: Error handling
            if (results.Length >= 2)
            {
                MapMetadata(results, out DateTimeOffset? absExpr, out TimeSpan? sldExpr);
                await RefreshAsync(key, absExpr, sldExpr, token);
            }

            if (results.Length >= 3 && results[2].HasValue)
            {
                return results[2];
            }

            return null;
        }

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            Connect();
            this.Do(db => db.KeyDelete(GetFullKey(key)));
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            await ConnectAsync(token);
            await this.Do(db => db.KeyDeleteAsync(GetFullKey(key)));
        }

        private void MapMetadata(RedisValue[] results, out DateTimeOffset? absoluteExpiration, out TimeSpan? slidingExpiration)
        {
            absoluteExpiration = null;
            slidingExpiration = null;
            var absoluteExpirationTicks = (long?)results[0];
            if (absoluteExpirationTicks.HasValue && absoluteExpirationTicks.Value != NotPresent)
            {
                absoluteExpiration = new DateTimeOffset(absoluteExpirationTicks.Value, TimeSpan.Zero);
            }
            var slidingExpirationTicks = (long?)results[1];
            if (slidingExpirationTicks.HasValue && slidingExpirationTicks.Value != NotPresent)
            {
                slidingExpiration = new TimeSpan(slidingExpirationTicks.Value);
            }
        }

        private void Refresh(string key, DateTimeOffset? absExpr, TimeSpan? sldExpr)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (sldExpr.HasValue)
            {
                // Note 如果只有绝对到期（或两者都没有），则刷新不起作用.
                TimeSpan? expr;
                if (absExpr.HasValue)
                {
                    var relExpr = absExpr.Value - DateTimeOffset.Now;
                    expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                }
                else
                {
                    expr = sldExpr;
                }
                this.Do(db => db.KeyExpire(GetFullKey(key), expr));
            }
        }

        private async Task RefreshAsync(string key, DateTimeOffset? absExpr, TimeSpan? sldExpr, CancellationToken token = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            token.ThrowIfCancellationRequested();
            if (sldExpr.HasValue)
            {
                // Note 如果只有绝对到期（或两者都没有），则刷新不起作用.
                TimeSpan? expr;
                if (absExpr.HasValue)
                {
                    var relExpr = absExpr.Value - DateTimeOffset.Now;
                    expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                }
                else
                {
                    expr = sldExpr;
                }
                await this.Do(db => db.KeyExpireAsync(GetFullKey(key), expr));
            }
        }

        /// <summary>
        /// 获取过期秒数
        /// </summary>
        /// <param name="creationTime">当前时间</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="options">配置</param>
        /// <returns></returns>
        private static long? GetExpirationInSeconds(DateTimeOffset now, DateTimeOffset? absoluteExpiration, DistributedCacheEntryOptions options)
        {
            if (absoluteExpiration.HasValue && options.SlidingExpiration.HasValue)
            {
                return (long)Math.Min((absoluteExpiration.Value - now).TotalSeconds, options.SlidingExpiration.Value.TotalSeconds);
            }
            else if (absoluteExpiration.HasValue)
            {
                return (long)(absoluteExpiration.Value - now).TotalSeconds;
            }
            else if (options.SlidingExpiration.HasValue)
            {
                return (long)options.SlidingExpiration.Value.TotalSeconds;
            }
            return null;
        }

        private static DateTimeOffset? GetAbsoluteExpiration(DateTimeOffset now, DistributedCacheEntryOptions options)
        {
            if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= now)
            {
                throw new ArgumentOutOfRangeException(nameof(DistributedCacheEntryOptions.AbsoluteExpiration),
                    options.AbsoluteExpiration.Value, "The absolute expiration value must be greater than now.");
            }
            var absoluteExpiration = options.AbsoluteExpiration;
            if (options.AbsoluteExpirationRelativeToNow.HasValue)
            {
                absoluteExpiration = now + options.AbsoluteExpirationRelativeToNow;
            }

            return absoluteExpiration;
        }

        /// <summary>
        /// 获取完整的key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetFullKey(string key)
        {
            return _instance + key;
        }
        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
        }
        #endregion

        #region IRedisCache接口实现
        public void SetRedisDbNum(int dbNum)
        {
            if (dbNum < 0 && dbNum > 15) throw new ArgumentOutOfRangeException(nameof(dbNum), "The redis database numbers range from 0 to 15.");
            this._dbNum = dbNum;
        }

        public string StringGet(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            Connect();
            return this.Do<string>(db => db.StringGet(GetFullKey(key), CommandFlags.None));
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var value = StringGet(key);
            var result = this.Do<T>(db =>
            {
                var value = db.StringGet(key, CommandFlags.None);
                if (!value.HasValue) return default;
                return JsonConvert.DeserializeObject<T>(value);
            });
            return result;
        }

        public bool Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            key = this.GetFullKey(key);
            string json = JsonConvert.SerializeObject(value);
            Connect();
            return this.Do<bool>((IDatabase db) => db.StringSet(key, json, expiry, When.Always, CommandFlags.None));
        }

        public bool KeyExists(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            key = this.GetFullKey(key);
            Connect();
            return this.Do<bool>(db => db.KeyExists(key, CommandFlags.None));
        }

        public void ClearDb(int dbNum, bool clearAll = false)
        {
            if (dbNum < 0 && dbNum > 15) throw new ArgumentOutOfRangeException(nameof(dbNum), "The redis database numbers range from 0 to 15.");
            Connect();
            var server = _connection.GetServer(_options.ConfigurationOptions.SslHost);
            if (clearAll)
            {
                server.FlushAllDatabases(CommandFlags.None);
            }
            else
            {
                server.FlushDatabase(dbNum, CommandFlags.None);
            }
        }

        public async Task<string> StringGetAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            Connect();
            return await this.Do(db => db.StringGetAsync(GetFullKey(key), CommandFlags.None));
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var value = await StringGetAsync(key);
			return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            key = this.GetFullKey(key);
            string json = JsonConvert.SerializeObject(value);
            Connect();
            return await this.Do((IDatabase db) => db.StringSetAsync(key, json, expiry, When.Always, CommandFlags.None));
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            key = this.GetFullKey(key);
            Connect();
            return await this.Do(db => db.KeyExistsAsync(key, CommandFlags.None));
        }

        public async Task ClearDbAsync(int dbNum, bool clearAll = false)
        {
            if (dbNum < 0 && dbNum > 15) throw new ArgumentOutOfRangeException(nameof(dbNum), "The redis database numbers range from 0 to 15.");
            Connect();
            var server = _connection.GetServer(_options.ConfigurationOptions.SslHost);
            if (clearAll)
            {
                await server.FlushAllDatabasesAsync(CommandFlags.None);
            }
            else
            {
                await server.FlushDatabaseAsync(dbNum, CommandFlags.None);
            }
        }
        #endregion

        #region 私有方法

        private void Connect()
        {
            if (_database != null) return;
            _connectionLock.Wait();
            try
            {
                if (_database == null)
                {
                    if (_options.ConfigurationOptions != null)
                    {
                        _connection = ConnectionMultiplexer.Connect(_options.ConfigurationOptions);
                    }
                    else
                    {
                        _connection = ConnectionMultiplexer.Connect(_options.Configuration);
                    }
                    _database = _connection.GetDatabase();
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        private async Task ConnectAsync(CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            await _connectionLock.WaitAsync(token);
            try
            {
                if (_options.ConfigurationOptions != null)
                {
                    _connection = await ConnectionMultiplexer.ConnectAsync(_options.ConfigurationOptions);
                }
                else
                {
                    _connection = await ConnectionMultiplexer.ConnectAsync(_options.Configuration);
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        /// <summary>
		/// 获取数据库
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		private T Do<T>(Func<IDatabase, T> func)
        {
            IDatabase database = this._connection.GetDatabase(_dbNum, null);
            return func(database);
        }
        #endregion
    }
}
