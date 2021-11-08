using Hx.Sdk.EventBus.Internal;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 事件总线中间件
    /// </summary>
    public static class EventBusApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加规RabbitMQ中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCapRabbitMQ(this IApplicationBuilder app)
        {
            Penetrates.ServiceProvider = app.ApplicationServices;
            return app;
        }
    }
}
