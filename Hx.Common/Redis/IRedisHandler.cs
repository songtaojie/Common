using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Common.Redis
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
		/// 清除redis数据库
		/// </summary>
		/// <param name="dbNum"></param>
		void ClearRedisDb(int dbNum);

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
		/// 获取数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		T Get<T>(string key);

		/// <summary>
		/// 获取集合数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="listKey"></param>
		/// <returns></returns>
		List<T> Get<T>(List<string> listKey);

		/// <summary>
		/// 移除数据
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool Remove(string key);

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
		/// 获取数据异步
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<T> GetAsync<T>(string key);

		/// <summary>
		/// 获取集合数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="listKey"></param>
		/// <returns></returns>
		Task<List<T>> GetAsync<T>(List<string> listKey);

		/// <summary>
		/// 移除数据，异步
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<bool> RemoveAsync(string key);
	}
}
