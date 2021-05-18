using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.ConfigureOptions.Internal;
using Hx.Sdk.Core;
using Hx.Sdk.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 主机构建器拓展类
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// 泛型主机配置Configuration
        /// </summary>
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <param name="configureDelegate">配置对象</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureHxAppConfiguration(this IHostBuilder hostBuilder, Action<HostBuilderContext, IConfigurationBuilder> configureDelegate = null)
        {
            hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.ConfigureOptions ConfigureAppConfiguration");
                // 存储环境对象
                InternalApp.HostEnvironment = hostingContext.HostingEnvironment;

                // 加载配置
                InternalApp.AddConfigureFiles(config, InternalApp.HostEnvironment);
                configureDelegate?.Invoke(hostingContext, config);
                ConsoleHelper.WriteSuccessLine("End Hx.Sdk.ConfigureOptions ConfigureAppConfiguration", true);
            });
            hostBuilder.ConfigureServices(services =>
            {
                // 添加全局配置和存储服务提供器
                InternalApp.InternalServices = services;
                ConsoleHelper.WriteInfoLine("Add the AppSettingsOptions configuration object");
                services.AddConfigurableOptions<AppSettingsOptions>();
            });
            return hostBuilder;
        }
    }
}
