﻿using Hx.Sdk.Core.Internal;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        }
    }
}