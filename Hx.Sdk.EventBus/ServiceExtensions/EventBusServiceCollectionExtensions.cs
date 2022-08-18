using DotNetCore.CAP;
using Hx.Sdk.EventBus;
using Hx.Sdk.EventBus.Internal;
using Hx.Sdk.EventBus.Options;
using Hx.Sdk.EventBus.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventBusServiceCollectionExtensions
    {
        /// <summary>
        /// 添加RabbitMq
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mqOption">配置</param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, Func<RabbitMQOptions> mqOption, int retryCount = 5)
        {
            if (mqOption == null) throw new ArgumentNullException(nameof(mqOption), "no configuration information passed in");
            Penetrates.InternalServices = services;
            var option = mqOption.Invoke();
            if (string.IsNullOrEmpty(option.HostName)) throw new ArgumentNullException("HostName is missing");
            //注册IRabbitMQPersistentConnection服务用于设置RabbitMQ连接
            services.AddSingleton<IRabbitMQPersistentConnection>(resolver =>
            {
                var logger = resolver.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = option.HostName,
                    DispatchConsumersAsync = true,
                    VirtualHost = option.VirtualHost,
                    UserName = option.UserName,
                    Password = option.Password,
                    Port = option.Port,
                };
                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            //注册单例模式的EventBusRabbitMQ
            services.AddSingleton<IEventBus, EventBusRabbitMQ>();
            return services;
        }

        /// <summary>
        /// 添加rabbitmq，配置文件中需要配置RabbitMQSettings节点
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, int retryCount = 5)
        {
            Penetrates.InternalServices = services;
            //注册IRabbitMQPersistentConnection服务用于设置RabbitMQ连接
            services.AddSingleton<IRabbitMQPersistentConnection>(resolver =>
            {
                var logger = resolver.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var configuration = resolver.GetRequiredService<IConfiguration>();
                var hostName = configuration["RabbitMQSettings:HostName"];
                if (string.IsNullOrEmpty(hostName)) throw new MissingMemberException("RabbitMQSettings:HostName is missing from the Appsettings.js file");
                _ = int.TryParse(configuration["RabbitMQSettings:Port"], out int port);
                var factory = new ConnectionFactory()
                {
                    HostName = hostName,
                    DispatchConsumersAsync = true,
                    VirtualHost = configuration["RabbitMQSettings:VirtualHost"],
                    UserName = configuration["RabbitMQSettings:UserName"],
                    Password = configuration["RabbitMQSettings:Password"],
                    Port = port,
                };
                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            //注册单例模式的EventBusRabbitMQ
            services.AddSingleton<IEventBus, EventBusRabbitMQ>();
            return services;
        }

        /// <summary>
        /// 添加cap RabbitMq，默认使用mysql，配置连接字符串CapRabbitMQSettings:ConnectionString
        /// </summary>
        /// <param name="services"></param>
        /// <param name="capOptions">cap配置，如果没设置，需要配置CapRabbitMQSettings:Cap</param>
        /// <param name="mqOption">RabbitMQ配置，如果没设置，需要配置CapRabbitMQSettings:RabbitMQ</param>
        /// <returns></returns>
        public static IServiceCollection AddCapRabbitMQForMySql(this IServiceCollection services, Action<CapOptions, IConfiguration> capOptions = null)
        {
            services.AddCapRabbitMQ((capOptions,config) => 
            {
                capOptions.UseMySql(config["CapSettings:ConnectionString"]);
            });
            return services;
        }

        /// <summary>
        /// 添加cap RabbitMq
        /// </summary>
        /// <param name="services"></param>
        /// <param name="capOptions">cap配置，如果没设置，需要配置CapSettings:Cap</param>
        /// <returns></returns>
        public static IServiceCollection AddCapRabbitMQ(this IServiceCollection services, Action<CapOptions, IConfiguration> capOptions = null)
        {
            Penetrates.InternalServices = services;
            ConfigureCapSettings(services);
            services.AddCap(options =>
            {
                var config = Penetrates.ServiceProvider.GetRequiredService<IConfiguration>();
                var capSettingsOptions = Penetrates.ServiceProvider.GetRequiredService<IOptions<CapSettingsOptions>>();
                var capSettings = capSettingsOptions.Value;
                options.DefaultGroupName = capSettings.Cap.DefaultGroupName;
                options.FailedRetryCount = capSettings.Cap.FailedRetryCount;
                options.FailedRetryInterval = capSettings.Cap.FailedRetryInterval;
                //options.UseMySql(config["CapSettings:ConnectionString"]);
                options.UseRabbitMQ(options =>
                {
                    options.HostName = capSettings.RabbitMQ.HostName;
                    options.VirtualHost = capSettings.RabbitMQ.VirtualHost;
                    options.UserName = capSettings.RabbitMQ.UserName;
                    options.Password = capSettings.RabbitMQ.Password;
                    options.Port = capSettings.RabbitMQ.Port;
                });
                capOptions?.Invoke(options, config);
            });

            services.AddTransient<IEventBus, EventBusCapRabbitMQ>();
            return services;
        }

        /// <summary>
        /// 添加 capsetting 配置
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureCapSettings(IServiceCollection services)
        {
            // 获取配置节点
            // 配置验证
            services.AddOptions<CapSettingsOptions>()
                    .BindConfiguration("CapSettings")
                    .ValidateDataAnnotations();

            // 选项后期配置
            services.PostConfigure<CapSettingsOptions>(options =>
            {
                _ = options.SetDefaultRedisSettings(options);
            });
        }
    }
}
