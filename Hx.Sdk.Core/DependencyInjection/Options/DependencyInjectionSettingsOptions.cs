using Hx.Sdk.ConfigurableOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Hx.Sdk.DependencyInjection
{
    /// <summary>
    /// 依赖注入配置选项
    /// </summary>
    public sealed class DependencyInjectionSettingsOptions : IPostConfigureOptions<DependencyInjectionSettingsOptions>
    {
        /// <summary>
        /// 外部注册定义
        /// </summary>
        public ExternalService[] Definitions { get; set; }

        /// <summary>
        /// 后期配置
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public void PostConfigure(string name,DependencyInjectionSettingsOptions options)
        {
            options.Definitions ??= Array.Empty<ExternalService>();
        }
    }
}