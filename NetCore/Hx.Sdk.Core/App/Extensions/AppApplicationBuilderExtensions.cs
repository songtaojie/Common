using Microsoft.AspNetCore.Http;

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
            
            // 判断是否启用规范化文档
            if (App.Settings.EnabledSwagger == true) app.UseSwaggerDocuments();

            if (App.Settings.EnabledUnifyResult == true) app.UseUnifyResultStatusCodes();
            // 调用自定义服务
            configure?.Invoke(app);
            return app;
        }

        /// <summary>
        /// 启用 Body 重复读功能
        /// </summary>
        /// <remarks>须在 app.UseRouting() 之前注册</remarks>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder EnableBuffering(this IApplicationBuilder app)
        {
            return app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });
        }
    }
}