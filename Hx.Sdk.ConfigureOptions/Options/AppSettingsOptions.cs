using Microsoft.Extensions.Configuration;
using System;

namespace Hx.Sdk.ConfigureOptions
{
    /// <summary>
    /// 应用全局配置
    /// </summary>
    public sealed class AppSettingsOptions : IConfigurableOptions<AppSettingsOptions>
    {
        /// <summary>
        /// 集成 MiniProfiler 组件
        /// </summary>
        public bool? EnabledMiniProfiler { get; set; }

        /// <summary>
        /// 是否启用规范化文档Swagger
        /// </summary>
        public bool? EnabledSwagger { get; set; }

        /// <summary>
        /// 是否启用引用程序集扫描
        /// </summary>
        public bool? EnabledReferenceAssemblyScan { get; set; }

        /// <summary>
        /// 是否打印数据库连接信息到 MiniProfiler 中
        /// </summary>
        public bool? PrintDbConnectionInfo { get; set; }

        /// <summary>
        /// 是否使用IdentityServer4授权认证，false：使用jwt授权认证
        /// </summary>
        public bool? UseIdentityServer4 { get; set; }

        /// <summary>
        ///Aop切面type全名
        /// </summary>
        public string[] AopTypeFullName { get; set; }

        /// <summary>
        /// 是否启用规范化结果
        /// </summary>
        public bool? EnabledUnifyResult { get; set; }

        /// <summary>
        /// 后期配置
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public void PostConfigure(AppSettingsOptions options, IConfiguration configuration)
        {
            options.EnabledMiniProfiler ??= true;
            options.EnabledSwagger ??= true;
            options.EnabledReferenceAssemblyScan ??= false;
            options.PrintDbConnectionInfo ??= true;
            options.UseIdentityServer4 ??= false;
            options.AopTypeFullName ??= Array.Empty<string>();
            options.EnabledUnifyResult ??= true;
        }
    }
}
