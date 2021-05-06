using Hx.Sdk.Core;
using Hx.Sdk.Core.Internal;
using Hx.Sdk.Core.Options;
using Hx.Sdk.DependencyInjection;
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
            // 注册内存和分布式内存
            services.AddMemoryCache();  // .NET 5.0.3+ 需要手动注册了
            services.AddDistributedMemoryCache();

            // 注册全局配置选项
            ConsoleHelper.WriteInfoLine("Add the AppSetting configuration service");
            services.AddConfigurableOptions<AppSettingsOptions>();

            // 添加 HttContext 访问器
            ConsoleHelper.WriteInfoLine("Add the HttpContextAccessor and UserContext service");
            services.AddUserContext();

            // 注册全局依赖注入
            //if (App.Settings.InjectAutofac != true)
            //{
            //    services.AddNativeDependencyInjection(App.EffectiveTypes);
            //}
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
            // 注册内存和分布式内存
            services.AddMemoryCache();  // .NET 5.0.3+ 需要手动注册了
            services.AddDistributedMemoryCache();

            // 注册全局配置选项
            services.AddConfigurableOptions<AppSettingsOptions>();

            // 注册全局依赖注入
            //services.AddNativeDependencyInjection(App.EffectiveTypes);

            // 自定义服务
            configure?.Invoke(services);

            return services;
        }

    }
}