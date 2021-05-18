using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.Core;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 应用服务集合拓展类（由框架内部调用）
    /// </summary>
    [SkipScan]
    public static class AppServiceCollectionExtensions
    {
        /// <summary>
        /// 添加应用配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configure">服务配置</param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddApp(this IServiceCollection services, Action<IServiceCollection> configure = null)
        {
            services.AddHostApp(s =>
            {
                // 添加 HttContext 访问器
                ConsoleHelper.WriteInfoLine("Add the HttpContextAccessor and UserContext service");
                services.AddUserContext();

                // 注册MiniProfiler 组件
                if (App.Settings.EnabledMiniProfiler == true)
                {
                    services.AddMiniProfilerService();
                }
                // 注册swagger
                // 判断是否启用规范化文档
                if (App.Settings.EnabledSwagger == true)
                {
                    services.AddSwaggerDocuments();
                }
                // 判断是否启用规范化文档
                if (App.Settings.EnabledUnifyResult == true)
                {
                    services.AddUnifyResult();
                }
                //判断是否启用全局异常处理
                if (App.Settings.EnabledExceptionFilter == true)
                {
                    services.AddFriendlyException();
                }
            });
           
            // 自定义服务
            configure?.Invoke(services);
            return services;
        }

        /// <summary>
        /// 添加主机应用配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configure">服务配置</param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddHostApp(this IServiceCollection services, Action<IServiceCollection> configure = null)
        {
            // 注册全局配置选项
            ConsoleHelper.WriteInfoLine("Add the AppSetting configuration service");
            services.AddConfigurableOptions<AppSettingsOptions>();

            // 注册内存和分布式内存
            ConsoleHelper.WriteInfoLine("Add the MemoryCache service");
            services.AddMemoryCache();  // .NET 5.0.3+ 需要手动注册了
            services.AddDistributedMemoryCache();

            // 注册全局依赖注入
            if (!AppExtend.InjectAutofac)
            {
                services.AddNativeDependencyInjection();
            }

            // 自定义服务
            configure?.Invoke(services);

            return services;
        }

    }
}