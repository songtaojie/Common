﻿using Autofac.Extensions.DependencyInjection;
using Hx.Sdk.Core;
using Hx.Sdk.Core.Internal;
using Hx.Sdk.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 主机构建器拓展类
    /// </summary>
    [SkipScan]
    public static class HostBuilderExtensions
    {

        /// <summary>
        /// Web 主机注入Hx.Sdk.Core
        /// </summary>
        /// <param name="hostBuilder">Web主机构建器</param>
        /// <param name="assemblyName">外部程序集名称</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder InjectHxWebApp(this IWebHostBuilder hostBuilder, string assemblyName = "Hx.Sdk.Core")
        {
            hostBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, assemblyName);
            return hostBuilder;
        }


        /// <summary>
        /// Web 主机注入Hx.Sdk.Core
        /// </summary>
        /// <param name="hostBuilder">Web主机构建器</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder InjectHxWebApp(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                // 存储环境对象
                InternalApp.HostEnvironment = InternalApp.WebHostEnvironment = hostingContext.HostingEnvironment;

                // 加载配置
                InternalApp.AddConfigureFiles(config, InternalApp.HostEnvironment);
            });

            // 自动注入 AddApp() 服务
            hostBuilder.ConfigureServices(services =>
            {
                // 添加全局配置和存储服务提供器
                InternalApp.InternalServices = services;

                // 初始化应用服务
                services.AddApp();
            });
            return hostBuilder;
        }

        /// <summary>
        /// 泛型主机注入Hx.Sdk.Core
        /// </summary>
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <returns>IWebHostBuilder</returns>
        public static IHostBuilder InjectHxWebApp(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureWebHostDefaults(webBuilder => 
            {
                webBuilder.InjectHxWebApp();
            });

            if (App.Settings.InjectAutofac == true)
            {
                hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
                hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((hostBuilderContext, containerBuilder) =>
                {
                    containerBuilder.AddAutofacDependencyInjection(Hx.Sdk.Core.App.EffectiveTypes);
                });
            }
            return hostBuilder;
        }

        /// <summary>
        /// 泛型主机注入Hx.Sdk.Core
        /// </summary>
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <returns>IWebHostBuilder</returns>
        public static IHostBuilder InjectHxApp(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                // 存储环境对象
                InternalApp.HostEnvironment = hostingContext.HostingEnvironment;

                // 加载配置
                InternalApp.AddConfigureFiles(config, InternalApp.HostEnvironment);
            });
            // 自动注入 AddApp() 服务
            hostBuilder.ConfigureServices(services =>
            {

                // 添加全局配置和存储服务提供器
                InternalApp.InternalServices = services;

                // 初始化应用服务
                services.AddHostApp();
            });

            return hostBuilder;
        }
    }
}