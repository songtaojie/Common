using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.Core;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 应用服务集合拓展类（由框架内部调用）
    /// </summary>
    [SkipScan]
    public static class AppServiceCollectionExtensions
    {
        /// <summary>
        /// 添加应用配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configure">服务配置</param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddApp(this IServiceCollection services, Action<IServiceCollection> configure = null)
        {
            services.AddHostApp(s =>
            {
                // 添加 HttContext 访问器
                ConsoleExtensions.WriteInfoLine("Add the HttpContextAccessor and UserContext service");
                services.AddUserContext();

                // 注册MiniProfiler 组件
                services.AddMiniProfilerService();
                // 注册swagger
                services.AddSwaggerDocuments();
            });
           
            // 自定义服务
            configure?.Invoke(services);
            return services;
        }

        /// <summary>
        /// 添加主机应用配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configure">服务配置</param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddHostApp(this IServiceCollection services, Action<IServiceCollection> configure = null)
        {
            var provier = services.BuildServiceProvider();
            var webEnv = provier.GetService<IWebHostEnvironment>();
            var env = provier.GetService<IHostEnvironment>();
            var builder = provier.GetService<IConfigurationBuilder>();
            var configuration = provier.GetService<IConfiguration>();
            // 注册全局配置选项
            ConsoleExtensions.WriteInfoLine("Add the AppSetting configuration service");
            services.AddConfigurableOptions<AppSettingsOptions>();

            // 注册内存和分布式内存
            ConsoleExtensions.WriteInfoLine("Add the MemoryCache service");
            services.AddMemoryCache();  // .NET 5.0.3+ 需要手动注册了
            services.AddDistributedMemoryCache();

            // 注册全局依赖注入
            if (!AppExtend.InjectAutofac)
            {
                services.AddNativeDependencyInjection();
            }

            // 自定义服务
            configure?.Invoke(services);

            return services;
        }

    }
}