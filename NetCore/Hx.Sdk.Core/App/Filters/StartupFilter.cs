using Hx.Sdk.Core.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

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
                InternalApp.RootServices = app.ApplicationServices;
                // 设置响应报文头信息，标记框架类型
                // 环境名
                var envName = InternalApp.HostEnvironment?.EnvironmentName ?? "Unknown";

                // 设置响应报文头信息
                app.Use(async (context, next) =>
                {
                    // 处理 WebSocket 请求
                    if (context.IsWebSocketRequest()) await next.Invoke();
                    else
                    {
                        // 输出当前环境标识
                        context.Response.Headers["environment"] = envName;

                        // 执行下一个中间件
                        await next.Invoke();

                        // 解决刷新 Token 时间和 Token 时间相近问题
                        if (!context.Response.HasStarted
                            && context.Response.StatusCode == StatusCodes.Status401Unauthorized
                            && context.Response.Headers.ContainsKey("access-token")
                            && context.Response.Headers.ContainsKey("x-access-token"))
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        }
                    }
                });

                app.Use(async (context, next) =>
                {
                    context.Request.EnableBuffering();  // 启动 Request Body 重复读，解决微信问题

                    await next.Invoke();
                });
                // 调用默认中间件
                app.UseHxApp();
                next(app);
            };
        }
    }
}
