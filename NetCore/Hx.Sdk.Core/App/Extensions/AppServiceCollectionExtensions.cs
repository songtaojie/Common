using Microsoft.Extensions.Configuration;

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
        /// <param name="config">配置文件</param>
        /// <param name="configure">服务配置</param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddApp(this IServiceCollection services,IConfiguration config, Action<IServiceCollection> configure = null)
        {
            services.AddHostApp(s =>
            {
                // 添加 HttContext 访问器
                services.AddUserContext();

                // 注册swagger
                // 判断是否启用规范化文档
                if (App.Settings.EnabledSwagger == true) services.AddSwaggerDocuments(config);

                // 判断是否启用规范化文档
                if (App.Settings.EnabledUnifyResult == true) services.AddUnifyResult();

                //判断是否启用全局异常处理
                if (App.Settings.EnabledExceptionFilter == true) services.AddFriendlyException();

                // 判断是否启用事件总线
                if (App.Settings.EnabledCap == true) services.AddCapRabbitMQForMySql(config);
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
            services.AddConfigureOptions<AppSettingsOptions>();

            // 注册内存和分布式内存
            services.AddMemoryCache(); 
            services.AddDistributedMemoryCache();

            // 注册全局依赖注入
            services.AddNativeDependencyInjection(App.EffectiveTypes);

            // 自定义服务
            configure?.Invoke(services);

            return services;
        }

    }
}