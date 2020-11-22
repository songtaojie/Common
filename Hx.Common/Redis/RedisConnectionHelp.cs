using Hx.Common.Config;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Common.Redis
{
	/// <summary>
	/// redis连接的帮助类
	/// </summary>
	internal class RedisConnectionHelp
	{
		/// <summary>
		/// redis连接的字符串
		/// </summary>
		private static string RedisConnectionString;

		/// <summary>
		/// 锁对象
		/// </summary>
		private static readonly object Locker = new object();

		/// <summary>
		/// 实例对象
		/// </summary>
		private static ConnectionMultiplexer _instance;

		/// <summary>
		/// 连接缓存集合
		/// </summary>
		private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache = new ConcurrentDictionary<string, ConnectionMultiplexer>();
		/// <summary>
		/// redis操作的实例对象
		/// </summary>
		public static ConnectionMultiplexer Instance
		{
			get
			{
				if (_instance == null)
				{
					object locker = Locker;
					lock (locker)
					{
						if (_instance == null || !_instance.IsConnected)
						{
                            _instance = GetManager(null);
						}
					}
				}
				return _instance;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004ED4 File Offset: 0x000030D4
		public static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
		{
			if (!ConnectionCache.ContainsKey(connectionString))
			{
                ConnectionCache[connectionString] = GetManager(connectionString);
			}
			return ConnectionCache[connectionString];
		}

		/// <summary>
		/// 获取redis操作的实例
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		private static ConnectionMultiplexer GetManager(string connectionString = null)
		{
            RedisConnectionString = ConfigManager.GetAppSettingValue("RedisConnection");
			connectionString = (connectionString ?? RedisConnectionString);
			if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
			ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString, null);
			connectionMultiplexer.ConnectionFailed += MuxerConnectionFailed;
			connectionMultiplexer.ConnectionRestored += MuxerConnectionRestored;
			connectionMultiplexer.ErrorMessage += MuxerErrorMessage;
			connectionMultiplexer.ConfigurationChanged += MuxerConfigurationChanged;
			connectionMultiplexer.HashSlotMoved += MuxerHashSlotMoved;
			connectionMultiplexer.InternalError += MuxerInternalError;
			return connectionMultiplexer;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
		{
			Console.WriteLine("Configuration changed: " + e.EndPoint);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
		{
			Console.WriteLine("ErrorMessage: " + e.Message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
		{
			Console.WriteLine("ConnectionRestored: " + e.EndPoint);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
		{
			Console.WriteLine(string.Concat(new object[]
			{
				"重新连接：Endpoint failed: ",
				e.EndPoint,
				", ",
				e.FailureType,
				(e.Exception == null) ? "" : (", " + e.Exception.Message)
			}));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
		{
			Console.WriteLine(string.Concat(new object[]
			{
				"HashSlotMoved:NewEndPoint",
				e.NewEndPoint,
				", OldEndPoint",
				e.OldEndPoint
			}));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
		{
			Console.WriteLine("InternalError:Message" + e.Exception.Message);
		}
	}
}
