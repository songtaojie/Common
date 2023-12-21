using FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Cache
{
    /// <summary>
	/// redis处理类接口
	/// </summary>
    public interface IRedisCache:ICache
	{
		/// <summary>
		/// 设置redis数据库编号
		/// </summary>
		/// <param name="dbNum"></param>
		void SetRedisDbNum(int dbNum);
	}
}
