using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.ConfigureOptions.Internal;
using Hx.Sdk.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Hosting
{
    /// <summary>
    /// Web主机扩展类
    /// </summary>
    public static class WebHostBuilderExtensions
    {
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
                ConsoleExtensions.WriteSuccessLine("Begin Hx.Sdk.ConfigureOptions ConfigureAppConfiguration");
                // 存储环境对象
                InternalApp.HostEnvironment = InternalApp.WebHostEnvironment = hostingContext.HostingEnvironment;

                // 加载配置
                InternalApp.AddConfigureFiles(config, InternalApp.HostEnvironment);
                configureDelegate?.Invoke(hostingContext, config);
                ConsoleExtensions.WriteSuccessLine("End Hx.Sdk.ConfigureOptions ConfigureAppConfiguration", true);
            });
            // 自动注入 AddConfigurableOptions() 服务
            hostBuilder.ConfigureServices(services =>
            {
                // 添加全局配置和存储服务提供器
                InternalApp.InternalServices = services;
                ConsoleExtensions.WriteInfoLine("Add the AppSettingsOptions configuration object");
                services.AddConfigurableOptions<AppSettingsOptions>();
            });

            return hostBuilder;
        }


    }
}
