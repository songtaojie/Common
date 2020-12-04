﻿using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.Cache
{
    /// <summary>
    /// Memcached的帮助类，可以再web.config或者app.config的AppSettings节点配置
    /// 键为MemcachedServices的值如127.0.0.1:11212，多个值使用,分割开来
    /// </summary>
    public sealed class MemcachedHelper
    {
        private static readonly MemcachedClient mc = null;
        static MemcachedHelper()
        {
            //服务器配置
            string[] serverlist = Config.ConfigManager.MemcachedServices;
            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(serverlist);

            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;

            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;

            pool.MaintenanceSleep = 30;
            pool.Failover = true;

            pool.Nagle = false;
            pool.Initialize();
            // 获得客户端实例
            mc = new MemcachedClient
            {
                EnableCompression = false
            };
        }
        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Set(string key, object value)
        {
            return mc.Set(key, value);
        }
        /// <summary>
        /// 存储数据，并设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool Set(string key, object value, DateTime time)
        {
            return mc.Set(key, value, time);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return mc.Get(key);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Delete(string key)
        {
            if (mc.KeyExists(key))
            {
                return mc.Delete(key);

            }
            return false;

        }
    }
}
