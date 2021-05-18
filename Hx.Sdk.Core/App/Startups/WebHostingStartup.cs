using Hx.Sdk.Core.Internal;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
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
            builder.ConfigureHxAppConfiguration();
            // 自动注入 AddApp() 服务
            builder.ConfigureServices(services =>
            {
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.Core ConfigureServices");
                // 注册 Startup 过滤器
                services.AddTransient<IStartupFilter, StartupFilter>();
                // 初始化应用服务
                services.AddApp();
                ConsoleHelper.WriteSuccessLine("Begin Hx.Sdk.Core ConfigureServices", true);
            });
        }
    }
}