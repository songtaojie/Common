using Autofac.Extensions.DependencyInjection;
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
        /// <param name="useAutofac">是否使用Autofac,设置为true时，需要添加Autofac相关的配置或者直接泛型主机
        /// 调用InjectAutofacBuilder来进行Autofac的配置</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder InjectHxWebApp(this IWebHostBuilder hostBuilder, bool useAutofac = false)
        {
            hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.Core ConfigureAppConfiguration");
                // 存储环境对象
                InternalApp.HostEnvironment = InternalApp.WebHostEnvironment = hostingContext.HostingEnvironment;

                // 加载配置
                InternalApp.AddConfigureFiles(config, InternalApp.HostEnvironment);
                ConsoleHelper.WriteSuccessLine("End Hx.Sdk.Core ConfigureAppConfiguration", true);
            });

            // 自动注入 AddApp() 服务
            hostBuilder.ConfigureServices(services =>
            {
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.Core ConfigureServices");
                // 添加全局配置和存储服务提供器
                InternalApp.InternalServices = services;

                // 初始化应用服务
                services.AddApp(s =>
                {
                    App.Settings.InjectAutofac = useAutofac;
                });
                ConsoleHelper.WriteSuccessLine("End Hx.Sdk.Core ConfigureServices", true);
            });
            return hostBuilder;
        }

        /// <summary>
        ///  Web 主机注入Hx.Sdk.Core
        /// </summary>
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <param name="useAutofac">是否使用Autofac依赖注入接管原生的依赖注入，内部已经注入Autofac相关的配置，设置为true不需要做Autofac相关的配置</param>
        /// <returns>IWebHostBuilder</returns>
        public static IHostBuilder InjectHxWebApp(this IHostBuilder hostBuilder,bool useAutofac = true)
        {
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.InjectHxWebApp(useAutofac);
            });

            if (useAutofac)
            {
                hostBuilder.InjectAutofacBuilder();
            }
            return hostBuilder;
        }

        /// <summary>
        /// 泛型主机注入Hx.Sdk.Core
        /// </summary>
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <param name="useAutofac">是否使用Autofac依赖注入接管原生的依赖注入</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder InjectHxApp(this IHostBuilder hostBuilder, bool useAutofac = true)
        {
            hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.Core ConfigureAppConfiguration");
                // 存储环境对象
                InternalApp.HostEnvironment = hostingContext.HostingEnvironment;

                // 加载配置
                InternalApp.AddConfigureFiles(config, InternalApp.HostEnvironment);
                ConsoleHelper.WriteSuccessLine("End Hx.Sdk.Core ConfigureAppConfiguration", true);
            });
            // 自动注入 AddApp() 服务
            hostBuilder.ConfigureServices(services =>
            {
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.Core ConfigureServices");
                // 添加全局配置和存储服务提供器
                InternalApp.InternalServices = services;

                // 初始化应用服务
                services.AddHostApp(s => 
                {
                    App.Settings.InjectAutofac = useAutofac;
                });
                ConsoleHelper.WriteSuccessLine("End Hx.Sdk.Core ConfigureServices",true);
            });
            if (useAutofac)
            {
                hostBuilder.InjectAutofacBuilder();
            }
            return hostBuilder;
        }

        /// <summary>
        /// 使用autofac接管原生的依赖注入
        /// 并注入ContainerBuilder
        /// </summary>
        /// <param name="hostBuilder">泛型主机</param>
        /// <returns></returns>
        public static IHostBuilder InjectAutofacBuilder(this IHostBuilder hostBuilder)
        {
            ConsoleHelper.WriteSuccessLine("Use Autofac takes over Native Dependency Injection Service",true);
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((hostBuilderContext, containerBuilder) =>
            {
                System.Console.WriteLine();
                ConsoleHelper.WriteSuccessLine("Begin Autofac ContainerBuilder ");
                containerBuilder.AddAutofacDependencyInjection();
                ConsoleHelper.WriteSuccessLine("End Autofac ContainerBuilder", true);
            });
            return hostBuilder;
        }
    }
}