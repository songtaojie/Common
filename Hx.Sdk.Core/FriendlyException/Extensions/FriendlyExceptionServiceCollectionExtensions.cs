using Hx.Sdk.DependencyInjection;
using Hx.Sdk.FriendlyException;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 友好异常服务拓展类
    /// </summary>
    [SkipScan]
    public static class FriendlyExceptionServiceCollectionExtensions
    {
        /// <summary>
        /// 添加友好异常服务拓展服务
        /// </summary>
        /// <typeparam name="TGlobalExceptionHandler">异常错误码处理器</typeparam>
        /// <param name="mvcBuilder">Mvc构建器</param>
        /// <param name="enabledGlobalExceptionFilter">是否启用全局异常过滤器</param>
        /// <returns></returns>
        public static IMvcBuilder AddFriendlyException<TGlobalExceptionHandler>(this IMvcBuilder mvcBuilder, bool enabledGlobalExceptionFilter = true)
            where TGlobalExceptionHandler : class, IGlobalExceptionHandler
        {
            var services = mvcBuilder.Services;

            // 添加全局异常过滤器
            mvcBuilder.AddFriendlyException(enabledGlobalExceptionFilter);

            // 单例注册异常状态码提供器
            services.AddSingleton<IGlobalExceptionHandler, TGlobalExceptionHandler>();

            return mvcBuilder;
        }

      
        /// <summary>
        /// 添加友好异常服务拓展服务
        /// </summary>
        /// <param name="mvcBuilder">Mvc构建器</param>
        /// <param name="enabledGlobalExceptionFilter">是否启用全局异常过滤器</param>
        /// <returns></returns>
        public static IMvcBuilder AddFriendlyException(this IMvcBuilder mvcBuilder, bool enabledGlobalExceptionFilter = true)
        {
            // 添加友好异常配置文件支持
            mvcBuilder.Services.AddConfigurableOptions<ExceptionSettingsOptions>();

            // 添加全局异常过滤器
            if (enabledGlobalExceptionFilter)
                mvcBuilder.AddMvcFilter<FriendlyExceptionFilter>();

            return mvcBuilder;
        }

        /// <summary>
        /// 添加友好异常服务拓展服务
        /// </summary>
        /// <typeparam name="TGlobalExceptionHandler">异常错误码处理器</typeparam>
        /// <param name="services"></param>
        /// <param name="enabledGlobalExceptionFilter">是否启用全局异常过滤器</param>
        /// <returns></returns>
        public static IServiceCollection AddFriendlyException<TGlobalExceptionHandler>(this IServiceCollection services, bool enabledGlobalExceptionFilter = true)
            where TGlobalExceptionHandler : class, IGlobalExceptionHandler
        {
            // 添加全局异常过滤器
            services.AddFriendlyException(enabledGlobalExceptionFilter);

            // 单例注册异常状态码提供器
            services.AddSingleton<IGlobalExceptionHandler, TGlobalExceptionHandler>();

            return services;
        }


        /// <summary>
        /// 添加友好异常服务拓展服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="enabledGlobalExceptionFilter">是否启用全局异常过滤器</param>
        /// <returns></returns>
        public static IServiceCollection AddFriendlyException(this IServiceCollection services, bool enabledGlobalExceptionFilter = true)
        {
            // 添加友好异常配置文件支持
            services.AddConfigurableOptions<ExceptionSettingsOptions>();

            // 添加全局异常过滤器
            if (enabledGlobalExceptionFilter)
                services.AddMvcFilter<FriendlyExceptionFilter>();

            return services;
        }
    }
}