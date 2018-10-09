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
        public static string RedisConnection
        {
            get { return ConfigurationManager.AppSettings["RedisConnection"]; }
        }
    }
}
