using Hx.Sdk.Common.Helper;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetCore.Cache
{
	/// <summary>
	/// redis的处理类
	/// 需要在config的AppSetting中配置键为RedisConnection的连接字符串
	/// </summary>
	internal class RedisHandler : IRedisHandler
	{

		private readonly ConnectionMultiplexer _conn;
		private readonly IDatabase _database;

		public RedisHandler(ConnectionMultiplexer conn)
		{
			_conn = conn;
			_database = conn.GetDatabase();
		}

        /// <summary>
        /// key的前缀
        /// </summary>
        public string KeyPrefix { get; set; }

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
			this.DbNum = dbNum;
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
			ErrorHelper.ThrowIfTrue(dbNum < 0 && dbNum > 15, "redis数据库编号范围在0-15之间");
			ErrorHelper.ThrowIfNullOrEmpty(this._conn.Configuration, "数据库连接不能为空");
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
			ErrorHelper.ThrowIfTrue(dbNum < 0 && dbNum > 15, "redis数据库编号范围在0-15之间");
			ErrorHelper.ThrowIfNullOrEmpty(this._conn.Configuration, "数据库连接不能为空");
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

		/// <summary>
		/// 释放资源
		/// </summary>
		public void Dispose()
		{
			this._conn.Dispose();
		}
      
    }
}
