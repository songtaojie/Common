using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hx.Common.Extension;
using Hx.Common.Config;
using Hx.Common.Helper;
using Newtonsoft.Json;

namespace Hx.Common.Redis
{
	/// <summary>
	/// redis的处理类
	/// 需要在config的AppSetting中配置键为RedisConnection的连接字符串
	/// </summary>
	public class RedisHandler: IRedisHandler
	{
		private readonly ConnectionMultiplexer _conn;

		/// <summary>
		/// key的前缀
		/// </summary>
		public string KeyPrefix;

		/// <summary>
		/// 数据库编号
		/// </summary>
		private int DbNum { get; set; }

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="dbNum"></param>
		public RedisHandler(int dbNum = 0) : this(dbNum, null)
		{
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="dbNum"></param>
		/// <param name="readWriteHosts">redis连接字符串</param>
		public RedisHandler(int dbNum, string readWriteHosts)
		{
			this.DbNum = dbNum;
			this._conn = (string.IsNullOrWhiteSpace(readWriteHosts) ? RedisConnectionHelp.Instance : RedisConnectionHelp.GetConnectionMultiplexer(readWriteHosts));
		}

		/// <summary>
		/// 设置数据库编号
		/// </summary>
		/// <param name="dbNum"></param>
		public void SetRedisDbNum(int dbNum = 0)
		{
			this.DbNum = dbNum;
		}

		/// <summary>
		/// 清除某个数据库中的数据
		/// </summary>
		/// <param name="dbNum"></param>
		public void ClearRedisDb(int dbNum)
		{
			ErrorHelper.ThrowIfTrue(dbNum < 0 && dbNum > 15, "redis数据库编号范围在0-15之间");
			ErrorHelper.ThrowIfNullOrEmpty(this._conn.Configuration, "数据库连接不能为空");
			this._conn.GetServer(this._conn.Configuration.Split(new char[]
			{
				','
			})[0], null).FlushDatabase(dbNum, CommandFlags.None);
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
			return this.Do<T>((IDatabase db) => JsonConvert.DeserializeObject<T>(db.StringGet(key, CommandFlags.None)));
		}

		/// <summary>
		/// 获取指定的值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="listKey"></param>
		/// <returns></returns>
		public List<T> Get<T>(List<string> listKey)
		{
			List<string> newKeys = listKey.Select(new Func<string, string>(this.AddSysCustomKey)).ToList<string>();
			RedisValue[] array = this.Do<RedisValue[]>((IDatabase db) => db.StringGet(this.ConvertRedisKeys(newKeys), CommandFlags.None));
			List<T> list = new List<T>();
			if (array.Any<RedisValue>())
			{
				for (int i = 0; i < array.Length; i++)
				{
					list.Add(JsonConvert.DeserializeObject<T>(array[i]));
				}
			}
			return list;
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
		/// 移除数据库中所有值
		/// </summary>
		/// <param name="DbNum"></param>
		public void RemoveDb(int DbNum)
		{
			this._conn.GetServer("", 111, null).FlushDatabase(DbNum, CommandFlags.None);
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
			return JsonConvert.DeserializeObject<T>(await this.Do<Task<RedisValue>>((IDatabase db) => db.StringGetAsync(key, CommandFlags.None)));
		}

		/// <summary>
		/// 异步获取集合
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="listKey"></param>
		/// <returns></returns>
		public async Task<List<T>> GetAsync<T>(List<string> listKey)
		{
			List<string> newKeys = listKey.Select(new Func<string, string>(this.AddSysCustomKey)).ToList<string>();
			RedisValue[] array = await this.Do<Task<RedisValue[]>>((IDatabase db) => db.StringGetAsync(this.ConvertRedisKeys(newKeys), CommandFlags.None));
			List<T> list = new List<T>();
			if (array.Any<RedisValue>())
			{
				for (int i = 0; i < array.Length; i++)
				{
					list.Add(JsonConvert.DeserializeObject<T>(array[i]));
				}
			}
			return list;
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

		
		private RedisKey[] ConvertRedisKeys(List<string> redisKeys)
		{
			var redisKeyList = new List<RedisKey>();
			foreach (var key in redisKeys)
			{
				redisKeyList.Add(key);
			}
			return redisKeyList.ToArray<RedisKey>();
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
