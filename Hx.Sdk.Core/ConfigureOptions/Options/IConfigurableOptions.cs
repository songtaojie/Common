﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Hx.Sdk.ConfigurableOptions
{
    /// <summary>
    /// 应用选项依赖接口
    /// </summary>
    public partial interface IConfigurableOptions { }

    /// <summary>
    /// 带验证的应用选项依赖接口
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="TOptionsValidation"></typeparam>
    public partial interface IPostConfigureOptions<TOptions, TOptionsValidation> : IPostConfigureOptions<TOptions>
        where TOptions : class, IConfigurableOptions
        where TOptionsValidation : class, IValidateOptions<TOptions>
    {
    }

    /// <summary>
    /// 带监听的应用选项依赖接口
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public interface IPostConfigureOptionsListener<TOptions> : IPostConfigureOptions<TOptions>
         where TOptions : class, IConfigurableOptions
    {
        /// <summary>
        /// 监听
        /// </summary>
        /// <param name="options">要配置的选项实例</param>
        /// <param name="configuration"></param>
        void OnListener(TOptions options, IConfiguration configuration);
    }
}