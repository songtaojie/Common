using Hx.Sdk;
using Hx.Sdk.Core;
using Hx.Sdk.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 应用服务集合拓展类（由框架内部调用）
    /// </summary>
    [SkipScan]
    public static class AppServiceCollectionExtensions
    {
        /// <summary>
        /// MiniProfiler 插件路径
        /// </summary>
        private const string MiniProfilerRouteBasePath = "/index-mini-profiler";

        /// <summary>
        /// 添加应用配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configure">服务配置</param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddApp(this IServiceCollection services, Action<IServiceCollection> configure = null)
        {
            InternalApp.HostEnvironment = services.BuildServiceProvider
            // 注册内存和分布式内存
            services.AddMemoryCache();  // .NET 5.0.3+ 需要手动注册了
            services.AddDistributedMemoryCache();
            // 注册全局配置选项
            services.AddConfigurableOptions<AppSettingsOptions>();

            // 添加 HttContext 访问器
            services.AddHttpContextAccessor();
            ////// 注册MiniProfiler 组件
            //if (App.Settings.InjectMiniProfiler == true)
            //{
            //    services.AddMiniProfiler(options =>
            //    {
            //        options.RouteBasePath = MiniProfilerRouteBasePath;
            //    }).AddRelationalDiagnosticListener();
            //}

            // 注册全局依赖注入
            services.AddDependencyInjection();

            // 自定义服务
            configure?.Invoke(services);
            return services;
        }
    }
}