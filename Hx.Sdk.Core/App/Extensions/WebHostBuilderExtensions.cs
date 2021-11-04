using Hx.Sdk.Core;
using Hx.Sdk.Core.Internal;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Microsoft.AspNetCore.Hosting
{
    /// <summary>
    /// 主机构建器拓展类
    /// </summary>
    [SkipScan]
    public static class WebHostBuilderExtensions
    {

        /// <summary>
        /// Web 主机配置
        /// </summary>
        /// <param name="hostBuilder">Web主机构建器</param>
        /// <param name="injectAutofac">是否使用Autofac依赖注入接管原生的依赖注入，
        /// 设置为true时，只需引用Hx.Sdk.DependencyInjection程序集包中泛型主机IHostBuilder的
        /// 扩展方法InjectContainerBuilder即可完成配置</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder ConfigureHxWebApp(this IWebHostBuilder hostBuilder, bool injectAutofac = false)
        {
            hostBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "Hx.Sdk.Core");
            InternalApp.InjectAutofac = injectAutofac;
            return hostBuilder;
        }

        /// <summary>
        /// Web主机配置Configuration
        /// </summary>
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <param name="configureDelegate">配置对象</param>
        /// <returns>IHostBuilder</returns>
        public static IWebHostBuilder ConfigureHxAppConfiguration(this IWebHostBuilder hostBuilder, Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate = null)
        {
            // 自动装载配置
            hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                // 存储环境对象
                InternalApp.HostEnvironment = InternalApp.WebHostEnvironment = hostingContext.HostingEnvironment;

                // 加载配置
                InternalApp.AddConfigureFiles(config, InternalApp.HostEnvironment);
                configureDelegate?.Invoke(hostingContext, config);
            });

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                // 存储服务提供器
                InternalApp.InternalServices = services;
                // 存储配置对象
                InternalApp.Configuration = hostContext.Configuration;
                // 注册 Startup 过滤器
                services.AddTransient<IStartupFilter, StartupFilter>();
                // 初始化应用服务
                services.AddApp();
                ConsoleHelper.WriteSuccessLine("complete Hx.Sdk.Core ConfigureServices", true);
            });
            return hostBuilder;
        }
    }
}
