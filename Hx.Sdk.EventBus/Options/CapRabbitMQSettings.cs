using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.EventBus.Options
{
    public class CapRabbitMQSettings
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
    }
}
