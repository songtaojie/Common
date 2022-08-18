using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.EventBus.Options
{
    /// <summary>
    /// cap设置
    /// </summary>
    public class CapSettingsOptions
    {
        public string ConnectionString { get; set; }
        /// <summary>
        /// cap相关配置
        /// </summary>
        public CapOptions Cap { get; set; }

        /// <summary>
        /// RabbitMQ配置
        /// </summary>
        public RabbitMQOptions RabbitMQ { get; set; }


        /// <summary>
        /// 设置默认 Redis 配置
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        internal CapSettingsOptions SetDefaultRedisSettings(CapSettingsOptions options)
        {
            options.Cap ??= new CapOptions()
            { 
                FailedRetryCount=3,
                FailedRetryInterval=30,
            };
            options.RabbitMQ ??= new RabbitMQOptions()
            {
                Port = 5672
            };
            return options;
        }
    }
}
