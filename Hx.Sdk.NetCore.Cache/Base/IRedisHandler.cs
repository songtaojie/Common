using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetCore.Cache
{
	/// <summary>
	/// redis处理类接口
	/// </summary>
	public interface IRedisHandler
	{
		/// <summary>
		/// 设置redis数据库编号
		/// </summary>
		/// <param name="dbNum"></param>
		void SetRedisDbNum(int dbNum);

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		string Get(string key);

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		T Get<T>(string key);

		/// <summary>
		/// 设置数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="expiry"></param>
		/// <returns></returns>
		bool Set<T>(string key, T value, TimeSpan? expiry = null);

		/// <summary>
		/// 是否存在key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool KeyExists(string key);

		/// <summary>
		/// 移除数据
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool Remove(string key);

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
		Task<string> GetAsync(string key);

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
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="expiry"></param>
		/// <returns></returns>
		Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null);

		/// <summary>
		/// 是否存在key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<bool> KeyExistsAsync(string key);

		/// <summary>
		/// 移除数据，异步
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<bool> RemoveAsync(string key);

		/// <summary>
		/// 清除某个数据库中的数据
		/// </summary>
		/// <param name="dbNum">如果是清除摸个数据库，填写编号</param>
		/// <param name="clearAll">清除所有，为true时，此时dbNum参数没用</param>
		Task ClearDbAsync(int dbNum,bool clearAll = false);
	}
}
