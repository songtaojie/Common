using Hx.Sdk;
using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.Core;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 应用中间件拓展类
    /// </summary>
    [SkipScan]
    public static class AppApplicationBuilderExtensions
    {

        /// <summary>
        /// 添加应用中间件
        /// </summary>
        /// <param name="app">应用构建器</param>
        /// <param name="configure">应用配置</param>
        /// <returns>应用构建器</returns>
        internal static IApplicationBuilder UseHxApp(this IApplicationBuilder app, Action<IApplicationBuilder> configure = null)
        {
            
            // 启用 MiniProfiler组件
            if (App.Settings.InjectMiniProfiler == true)
            {
                ConsoleExtensions.WriteInfoLine("Use the MiniProfiler ApplicationBuilder");
                app.UseMiniProfiler();
            }
            app.UseSwaggerDocuments();

            // 调用自定义服务
            configure?.Invoke(app);
            return app;
        }
    }
}