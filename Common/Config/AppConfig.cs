using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Config
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 获取Redis的链接配置
        /// </summary>
        public static string RedisConnection
        {
            get { return ConfigurationManager.AppSettings["RedisConnection"]; }
        }
        /// <summary>
        /// 获取Memcached的服务器端口的配置,多个服务器使用,分割
        /// </summary>
        public static string[] MemcachedServices
        {
            get
            {
                string service = ConfigurationManager.AppSettings["MemcachedServices"];
                if (string.IsNullOrEmpty(service))
                {
                    return new string[] { "127.0.0.1:11211" };
                }
                else
                {
                    return service.Split(',');
                }
            }
        }
    }
}
