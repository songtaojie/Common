using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.Sdk.Cache
{
    /// <summary>
    /// redis的处理类
    /// 需要在config的AppSetting中配置键为RedisConnection的连接字符串
    /// </summary>
    internal class RedisHandler : RedisCache
	{

		private readonly ConnectionMultiplexer _conn;
		private readonly IDatabase _database;
		private readonly RedisCacheOptions _options;
		private readonly string _instance;

		public RedisHandler(ConnectionMultiplexer conn, IOptions<RedisCacheOptions> optionsAccessor):base(optionsAccessor)
		{
			_conn = conn;
			_database = conn.GetDatabase();
			if (optionsAccessor == null) throw new ArgumentNullException("optionsAccessor");
			_options = optionsAccessor.Value;
			this._instance = _options.InstanceName;
		}

		/// <summary>
		/// 数据库编号
		/// </summary>
		private int DbNum { get; set; }

		/// <summary>
		/// 设置数据库编号
		/// </summary>
		/// <param name="dbNum"></param>
		public void SetRedisDbNum(int dbNum = 0)
		{
			_options.ConfigurationOptions.DefaultDatabase = dbNum;
		}

		public string Get(string key)
		{
			key = this.AddSysCustomKey(key);
			return this.Do<string>(db => db.StringGet(key, CommandFlags.None));
		}

		/// <summary>
		/// 获取指定的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T Get<T>(string key)
		{
			key = this.AddSysCustomKey(key);
			var result =  this.Do<T>(db =>
			{
				var value = db.StringGet(key, CommandFlags.None);
				if (!value.HasValue) return default;
				return JsonConvert.DeserializeObject<T>(value);
			});
			return result;
		}

		/// <summary>
		/// 设置值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="expiry"></param>
		/// <returns></returns>
		public bool Set<T>(string key, T value, TimeSpan? expiry = null)
		{
			key = this.AddSysCustomKey(key);
			string json = JsonConvert.SerializeObject(value);
			return this.Do<bool>((IDatabase db) => db.StringSet(key, json, expiry, When.Always, CommandFlags.None));
		}

		public bool KeyExists(string key)
		{
			key = this.AddSysCustomKey(key);
			return this.Do<bool>(db => db.KeyExists(key, CommandFlags.None));
		}

		/// <summary>
		/// 移除值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Remove(string key)
		{
			key = this.AddSysCustomKey(key);
			return this.Do<bool>((IDatabase db) => db.KeyDelete(key, CommandFlags.None));
		}

		/// <summary>
		/// 清除某个数据库中的数据
		/// </summary>
		/// <param name="dbNum">如果是清除摸个数据库，填写编号</param>
		/// <param name="clearAll">清除所有，为true时，此时dbNum参数没用</param>
		public void ClearDb(int dbNum, bool clearAll = false)
		{
			if (dbNum < 0 && dbNum > 15) throw new Exception("redis数据库编号范围在0-15之间");
			if (string.IsNullOrEmpty(this._conn.Configuration)) throw new Exception("数据库连接不能为空");
			//var endpoint = _conn.GetEndPoints();
			//var server = _conn.GetServer(endpoint.First());
			var server = this._conn.GetServer(this._conn.Configuration.Split(new char[]
			{
				','
			})[0], null);
			if (clearAll)
			{
				server.FlushAllDatabases(CommandFlags.None);
			}
			else
			{
				server.FlushDatabase(dbNum, CommandFlags.None);
			}
		}
		public async Task<string> GetAsync(string key)
		{
			key = this.AddSysCustomKey(key);
			return await this.Do((IDatabase db) => db.StringGetAsync(key, CommandFlags.None));
		}

		/// <summary>
		/// 异步获取值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<T> GetAsync<T>(string key)
		{
			key = this.AddSysCustomKey(key);
			var value = await this.Do(db => db.StringGetAsync(key, CommandFlags.None));
			if (!value.HasValue) return default;
			return JsonConvert.DeserializeObject<T>(value);
		}

		/// <summary>
		/// 设置值，异步
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="expiry"></param>
		/// <returns></returns>
		public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
		{
			key = this.AddSysCustomKey(key);
			string json = JsonConvert.SerializeObject(value);
			return await this.Do<Task<bool>>((IDatabase db) => db.StringSetAsync(key, json, expiry, When.Always, CommandFlags.None));
		}

		public async Task<bool> KeyExistsAsync(string key)
		{
			key = this.AddSysCustomKey(key);
			return await this.Do<Task<bool>>(db => db.KeyExistsAsync(key, CommandFlags.None));
		}

		/// <summary>
		/// 移除值，异步
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<bool> RemoveAsync(string key)
		{
			key = this.AddSysCustomKey(key);
			return await this.Do<Task<bool>>((IDatabase db) => db.KeyDeleteAsync(key, CommandFlags.None));
		}

		/// <summary>
		/// 清除某个数据库中的数据
		/// </summary>
		/// <param name="dbNum">如果是清除摸个数据库，填写编号</param>
		/// <param name="clearAll">清除所有，为true时，此时dbNum参数没用</param>
		public async Task ClearDbAsync(int dbNum,bool clearAll = false)
		{
			if (dbNum < 0 && dbNum > 15) throw new Exception("redis数据库编号范围在0-15之间");
			if (string.IsNullOrEmpty(this._conn.Configuration)) throw new Exception("数据库连接不能为空");
			var server = this._conn.GetServer(this._conn.Configuration.Split(new char[]
			{
				','
			})[0], null);
			if (clearAll)
			{
				await	server.FlushAllDatabasesAsync(CommandFlags.None);
			}
			else
			{
				await	server.FlushDatabaseAsync(dbNum, CommandFlags.None);
			}
		}

		/// <summary>
		/// 获取key
		/// </summary>
		/// <param name="oldKey"></param>
		/// <returns></returns>
		private string AddSysCustomKey(string oldKey)
		{
			return this.KeyPrefix + oldKey;
		}

		/// <summary>
		/// 获取数据库
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		private T Do<T>(Func<IDatabase, T> func)
		{
			IDatabase database = this._conn.GetDatabase(this.DbNum, null);
			return func(database);
		}
    }




    public class RedisCache : IDistributedCache, IDisposable
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
        private IDatabase _cache;

        private readonly RedisCacheOptions _options;
        private readonly string _instance;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        public RedisCache(IOptions<RedisCacheOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            _options = optionsAccessor.Value;

            // This allows partitioning a single backend cache for use with multiple apps/services.
            _instance = _options.InstanceName ?? string.Empty;
        }

        public byte[] Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return GetAndRefresh(key, getData: true);
        }

        public async Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();

            return await GetAndRefreshAsync(key, getData: true, token: token);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Connect();

            var creationTime = DateTimeOffset.UtcNow;

            var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

            var result = _cache.ScriptEvaluate(SetScript, new RedisKey[] { _instance + key },
                new RedisValue[]
                {
                        absoluteExpiration?.Ticks ?? NotPresent,
                        options.SlidingExpiration?.Ticks ?? NotPresent,
                        GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
                        value
                });
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            token.ThrowIfCancellationRequested();

            await ConnectAsync(token);

            var creationTime = DateTimeOffset.UtcNow;

            var absoluteExpiration = GetAbsoluteExpiration(creationTime, options);

            await _cache.ScriptEvaluateAsync(SetScript, new RedisKey[] { _instance + key },
                new RedisValue[]
                {
                        absoluteExpiration?.Ticks ?? NotPresent,
                        options.SlidingExpiration?.Ticks ?? NotPresent,
                        GetExpirationInSeconds(creationTime, absoluteExpiration, options) ?? NotPresent,
                        value
                });
        }

        public void Refresh(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            GetAndRefresh(key, getData: false);
        }

        public async Task RefreshAsync(string key, CancellationToken token = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();

            await GetAndRefreshAsync(key, getData: false, token: token);
        }

        private void Connect()
        {
            if (_cache != null)
            {
                return;
            }

            _connectionLock.Wait();
            try
            {
                if (_cache == null)
                {
                    if (_options.ConfigurationOptions != null)
                    {
                        _connection = ConnectionMultiplexer.Connect(_options.ConfigurationOptions);
                    }
                    else
                    {
                        _connection = ConnectionMultiplexer.Connect(_options.Configuration);
                    }
                    _cache = _connection.GetDatabase();
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        private async Task ConnectAsync(CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (_cache != null)
            {
                return;
            }

            await _connectionLock.WaitAsync(token);
            try
            {
                if (_cache == null)
                {
                    if (_options.ConfigurationOptions != null)
                    {
                        _connection = await ConnectionMultiplexer.ConnectAsync(_options.ConfigurationOptions);
                    }
                    else
                    {
                        _connection = await ConnectionMultiplexer.ConnectAsync(_options.Configuration);
                    }

                    _cache = _connection.GetDatabase();
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        private byte[] GetAndRefresh(string key, bool getData)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Connect();

            // This also resets the LRU status as desired.
            // TODO: Can this be done in one operation on the server side? Probably, the trick would just be the DateTimeOffset math.
            RedisValue[] results;
            if (getData)
            {
                results = _cache.HashMemberGet(_instance + key, AbsoluteExpirationKey, SlidingExpirationKey, DataKey);
            }
            else
            {
                results = _cache.HashMemberGet(_instance + key, AbsoluteExpirationKey, SlidingExpirationKey);
            }

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
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();

            await ConnectAsync(token);

            // This also resets the LRU status as desired.
            // TODO: Can this be done in one operation on the server side? Probably, the trick would just be the DateTimeOffset math.
            RedisValue[] results;
            if (getData)
            {
                results = await _cache.HashMemberGetAsync(_instance + key, AbsoluteExpirationKey, SlidingExpirationKey, DataKey);
            }
            else
            {
                results = await _cache.HashMemberGetAsync(_instance + key, AbsoluteExpirationKey, SlidingExpirationKey);
            }

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
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Connect();

            _cache.KeyDelete(_instance + key);
            // TODO: Error handling
        }

        public async Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            await ConnectAsync(token);

            await _cache.KeyDeleteAsync(_instance + key);
            // TODO: Error handling
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
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            // Note Refresh has no effect if there is just an absolute expiration (or neither).
            TimeSpan? expr = null;
            if (sldExpr.HasValue)
            {
                if (absExpr.HasValue)
                {
                    var relExpr = absExpr.Value - DateTimeOffset.Now;
                    expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                }
                else
                {
                    expr = sldExpr;
                }
                _cache.KeyExpire(_instance + key, expr);
                // TODO: Error handling
            }
        }

        private async Task RefreshAsync(string key, DateTimeOffset? absExpr, TimeSpan? sldExpr, CancellationToken token = default(CancellationToken))
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            token.ThrowIfCancellationRequested();

            // Note Refresh has no effect if there is just an absolute expiration (or neither).
            TimeSpan? expr = null;
            if (sldExpr.HasValue)
            {
                if (absExpr.HasValue)
                {
                    var relExpr = absExpr.Value - DateTimeOffset.Now;
                    expr = relExpr <= sldExpr.Value ? relExpr : sldExpr;
                }
                else
                {
                    expr = sldExpr;
                }
                await _cache.KeyExpireAsync(_instance + key, expr);
                // TODO: Error handling
            }
        }

        private static long? GetExpirationInSeconds(DateTimeOffset creationTime, DateTimeOffset? absoluteExpiration, DistributedCacheEntryOptions options)
        {
            if (absoluteExpiration.HasValue && options.SlidingExpiration.HasValue)
            {
                return (long)Math.Min(
                    (absoluteExpiration.Value - creationTime).TotalSeconds,
                    options.SlidingExpiration.Value.TotalSeconds);
            }
            else if (absoluteExpiration.HasValue)
            {
                return (long)(absoluteExpiration.Value - creationTime).TotalSeconds;
            }
            else if (options.SlidingExpiration.HasValue)
            {
                return (long)options.SlidingExpiration.Value.TotalSeconds;
            }
            return null;
        }

        private static DateTimeOffset? GetAbsoluteExpiration(DateTimeOffset creationTime, DistributedCacheEntryOptions options)
        {
            if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= creationTime)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(DistributedCacheEntryOptions.AbsoluteExpiration),
                    options.AbsoluteExpiration.Value,
                    "The absolute expiration value must be in the future.");
            }
            var absoluteExpiration = options.AbsoluteExpiration;
            if (options.AbsoluteExpirationRelativeToNow.HasValue)
            {
                absoluteExpiration = creationTime + options.AbsoluteExpirationRelativeToNow;
            }

            return absoluteExpiration;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
        }
    }
}
