using Hx.Sdk.Core.Internal;
using Hx.Sdk.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Hx.Sdk.Core.WebHostingStartup))]

namespace Hx.Sdk.Core
{
    /// <summary>
    /// 配置程序启动时自动注入
    /// </summary>
    [SkipScan]
    public sealed class WebHostingStartup : IHostingStartup
    {
        /// <summary>
        /// 配置应用启动
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(IWebHostBuilder builder)
        {
            // 自动装载配置
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.Core ConfigureAppConfiguration");
                // 存储环境对象
                InternalApp.HostEnvironment = InternalApp.WebHostEnvironment = hostingContext.HostingEnvironment;

                // 加载配置
                InternalApp.AddConfigureFiles(config, InternalApp.HostEnvironment);
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.Core ConfigureAppConfiguration");
            });
            // 自动注入 AddApp() 服务
            builder.ConfigureServices(services =>
            {
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.Core ConfigureServices");
                // 添加全局配置和存储服务提供器
                InternalApp.InternalServices = services;
                // 注册 Startup 过滤器
                services.AddTransient<IStartupFilter, StartupFilter>();
                // 初始化应用服务
                services.AddApp();
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.Core ConfigureAppConfiguration",true);
            });
        }
    }
}