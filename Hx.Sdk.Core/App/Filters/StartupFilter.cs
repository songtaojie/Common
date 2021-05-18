using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// 应用启动时自动注册中间件
    /// </summary>
    /// <remarks>
    /// </remarks>
    [SkipScan]
    public class StartupFilter : IStartupFilter
    {
        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.Core Startup Configure");
                // 设置响应报文头信息，标记框架类型
                app.Use(async (context, next) =>
                {
                    context.Request.EnableBuffering();  // 启动 Request Body 重复读，解决微信问题

                    await next.Invoke();
                });
                // 调用默认中间件
                app.UseHxApp();
                ConsoleHelper.WriteSuccessLine("End Hx.Sdk.Core Startup Configure",true);
                next(app);
            };
        }
    }
}
