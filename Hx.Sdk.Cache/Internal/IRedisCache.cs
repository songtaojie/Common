﻿using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.Cache
{
    /// <summary>
	/// redis处理类接口
	/// 需要在config的AppSetting中配置键为RedisConnection的连接字符串
	/// </summary>
    public interface IRedisCache: IDistributedCache
	{
		/// <summary>
		/// 设置redis数据库编号
		/// </summary>
		/// <param name="dbNum"></param>
		void SetRedisDbNum(int dbNum);

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <param name="key">缓存的键</param>
		/// <returns></returns>
		string StringGet(string key);

		/// <summary>
		/// 获取Hash数据
		/// </summary>
		/// <param name="key">缓存的键</param>
		/// <returns></returns>
		string HashGet(string key);

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">缓存的键</param>
		/// <returns></returns>
		T Get<T>(string key);

		/// <summary>
		/// 设置数据
		/// </summary>
		/// <param name="key">缓存的键</param>
		/// <param name="value">缓存的值</param>
		/// <param name="expiry">过期时间</param>
		/// <returns></returns>
		bool StringSet(string key, string value, TimeSpan? expiry = null);

		/// <summary>
		/// 设置Hash数据
		/// </summary>
		/// <param name="key">缓存的键</param>
		/// <param name="value">缓存的值</param>
		/// <param name="options">过期时间设置</param>
		/// <returns></returns>
		bool HashSet(string key, string value, DistributedCacheEntryOptions options = null);

		/// <summary>
		/// 设置数据
		/// </summary>
		/// <typeparam name="T">数据的类型参数</typeparam>
		/// <param name="key">缓存的键</param>
		/// <param name="value">缓存的值</param>
		/// <param name="expiry">过期时间</param>
		/// <returns></returns>
		bool Set<T>(string key, T value, TimeSpan? expiry = null);

		/// <summary>
		/// 是否存在key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool KeyExists(string key);

		/// <summary>
		/// 清除某个数据库中的数据
		/// </summary>
		/// <param name="dbNum">如果是清除摸个数据库，填写编号</param>
		/// <param name="clearAll">清除所有，为true时，此时dbNum参数没用</param>
		void ClearDb(int dbNum, bool clearAll = false);

		/// <summary>
		/// 获取数据异步
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<string> StringGetAsync(string key);

		/// <summary>
		/// 获取Hash数据异步
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<string> HashGetAsync(string key);

		/// <summary>
		/// 获取数据异步
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<T> GetAsync<T>(string key);

		/// <summary>
		/// 设置数据异步
		/// </summary>
		/// <param name="key">缓存的键</param>
		/// <param name="value">缓存的值</param>
		/// <param name="expiry">过期时间</param>
		/// <returns></returns>
		Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null);

		/// <summary>
		/// 设置Hash数据异步
		/// </summary>
		/// <param name="key">缓存的键</param>
		/// <param name="value">缓存的值</param>
		/// <param name="expiry">过期时间设置</param>
		/// <returns></returns>
		Task<bool> HashSetAsync(string key, string value, DistributedCacheEntryOptions options = null);

		/// <summary>
		/// 设置数据异步
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">缓存的键</param>
		/// <param name="value">缓存的值</param>
		/// <param name="expiry">过期时间</param>
		/// <returns></returns>
		Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null);

		/// <summary>
		/// 是否存在key
		/// </summary>
		/// <param name="key">缓存的键</param>
		/// <returns></returns>
		Task<bool> KeyExistsAsync(string key);

		/// <summary>
		/// 清除某个数据库中的数据
		/// </summary>
		/// <param name="dbNum">如果是清除某个数据库，填写编号</param>
		/// <param name="clearAll">清除所有，为true时，此时dbNum参数没用</param>
		Task ClearDbAsync(int dbNum, bool clearAll = false);
	}
}
