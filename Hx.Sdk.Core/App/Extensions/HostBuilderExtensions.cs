using Hx.Sdk.Core;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 主机构建器拓展类
    /// </summary>
    [SkipScan]
    public static class HostBuilderExtensions
    {

        /// <summary>
        /// 用预先配置的默认值初始化类的新实例<see cref="IWebHostBuilder"/>
        /// 注意，使用此扩展方法后代替ConfigureWebHostDefaults会自动配置IWebHostBuilder的ConfigureHxWebApp
        /// </summary>
        /// <remarks>
        ///    以下默认值应用于<see cref="IWebHostBuilder"/>:
        ///    使用Kestrel作为web服务器并使用应用程序的配置提供商来配置它，
        ///    添加HostFiltering中间件，
        ///    如果ASPNETCORE_FORWARDEDHEADERS_ENABLED=true则添加ForwardedHeaders中间件，
        ///    并启用IIS集成。
        /// </remarks> 
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <param name="configure">配置回调</param>
        /// <param name="injectAutofac">是否使用Autofac依赖注入接管原生的依赖注入，设置为true内部自动注入Autofac相关的配置</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureHxWebHostDefaults(this IHostBuilder hostBuilder, Action<IWebHostBuilder> configure,bool injectAutofac = true)
        {
            AppExtend.InjectAutofac = injectAutofac;
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureHxWebApp(injectAutofac);
                configure(webBuilder);
            });
            if (AppExtend.InjectAutofac)
            {
                hostBuilder.InjectContainerBuilder();
            }
            return hostBuilder;
        }

        /// <summary>
        /// 泛型主机配置Hx.Sdk.Core
        /// </summary>
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <param name="injectAutofac">是否使用Autofac依赖注入接管原生的依赖注入</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureHxApp(this IHostBuilder hostBuilder, bool injectAutofac = true)
        {
            AppExtend.InjectAutofac = injectAutofac;
            hostBuilder.ConfigureHxAppConfiguration();
            // 自动注入 AddApp() 服务
            hostBuilder.ConfigureServices(services =>
            {
                ConsoleExtensions.WriteSuccessLine("Begin Hx.Sdk.Core ConfigureServices");
                // 初始化应用服务
                services.AddHostApp();
                ConsoleExtensions.WriteSuccessLine("End Hx.Sdk.Core ConfigureServices", true);
            });
            if (AppExtend.InjectAutofac)
            {
                hostBuilder.InjectContainerBuilder();
            }
            return hostBuilder;
        }
    }
}