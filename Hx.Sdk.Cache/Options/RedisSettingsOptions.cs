using Hx.Sdk.ConfigureOptions;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Cache.Options
{
    /// <summary>
    /// 应用全局配置
    /// </summary>
    public sealed class RedisSettingsOptions :RedisCacheOptions, IConfigurableOptions<RedisSettingsOptions>
    {
        /// <summary>
        /// 是否启用规范化文档
        /// </summary>
        public bool? InjectSpecificationDocument { get; set; }

        /// <summary>
        /// 是否启用引用程序集扫描
        /// </summary>
        public bool? EnabledReferenceAssemblyScan { get; set; }

        /// <summary>
        /// 是否打印数据库连接信息到 MiniProfiler 中
        /// </summary>
        public bool? PrintDbConnectionInfo { get; set; }

        /// <summary>
        /// 配置支持的包前缀名
        /// </summary>
        public string[] SupportPackageNamePrefixs { get; set; }

        /// <summary>
        /// 后期配置
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public void PostConfigure(RedisSettingsOptions options, IConfiguration configuration)
        {
            options.InstanceName ??= "Hx";
            options.InjectSpecificationDocument ??= true;
            options.EnabledReferenceAssemblyScan ??= false;
            options.PrintDbConnectionInfo ??= true;
            options.SupportPackageNamePrefixs ??= Array.Empty<string>();
        }
    }
}
