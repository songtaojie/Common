using Hx.Sdk.Core;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
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
        /// 泛型主机注入，内置注入Web主机相关配置
        /// </summary>
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <param name="injectAutofac">是否使用Autofac依赖注入接管原生的依赖注入，内部已经注入Autofac相关的配置，设置为true不需要做Autofac相关的配置</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder InjectHxWebApp(this IHostBuilder hostBuilder, bool injectAutofac = true)
        {
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.InjectHxWebApp(injectAutofac);
            });
            if (AppExtend.InjectAutofac)
            {
                hostBuilder.InjectContainerBuilder();
            }
            return hostBuilder;
        }

        /// <summary>
        /// 泛型主机注入Hx.Sdk.Core
        /// </summary>
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <param name="injectAutofac">是否使用Autofac依赖注入接管原生的依赖注入</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder InjectHxApp(this IHostBuilder hostBuilder, bool injectAutofac = true)
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