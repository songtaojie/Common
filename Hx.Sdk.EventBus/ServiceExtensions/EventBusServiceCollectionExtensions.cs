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
            //注册IRabbitMQPersistentConnection服务用于设置RabbitMQ连接
            services.AddSingleton<IRabbitMQPersistentConnection>(resolver =>
            {
                var logger = resolver.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var config = resolver.GetRequiredService<IConfiguration>();
                var hostName = config["RabbitMQSettings:HostName"];
                if (string.IsNullOrEmpty(hostName)) throw new MissingMemberException("RabbitMQSettings:HostName is missing from the Appsettings.js file");
                _ = int.TryParse(config["RabbitMQSettings:Port"], out int port);
                var factory = new ConnectionFactory()
                {
                    HostName = hostName,
                    DispatchConsumersAsync = true,
                    VirtualHost = config["RabbitMQSettings:VirtualHost"],
                    UserName = config["RabbitMQSettings:UserName"],
                    Password = config["RabbitMQSettings:Password"],
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
        /// <returns></returns>
        public static IServiceCollection AddCapRabbitMQForMySql(this IServiceCollection services, IConfiguration config, Action<CapOptions, IConfiguration> capOptions = null)
        {
            services.AddCapRabbitMQ(config , capOptions => 
            {
                capOptions.UseMySql(config["CapSettings:ConnectionString"]);
            });
            return services;
        }

        /// <summary>
        /// 添加cap RabbitMq
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config">配置</param>
        /// <param name="capOptions">cap配置，如果没设置，需要配置CapSettings:Cap</param>
        /// <returns></returns>
        public static IServiceCollection AddCapRabbitMQ(this IServiceCollection services, IConfiguration config,Action<CapOptions> capOptions = null)
        {
            services.AddCap(options =>
            {
                options.DefaultGroupName = config["CapSettings:Cap:DefaultGroupName"];
                int failedRetryCount = 5;
                var failedRetryCountStr = config["CapSettings:Cap:FailedRetryCount"];
                if (!string.IsNullOrEmpty(failedRetryCountStr)) 
                    _ = int.TryParse(failedRetryCountStr, out failedRetryCount);
                options.FailedRetryCount = failedRetryCount;
                var failedRetryIntervalStr = config["CapSettings:Cap:FailedRetryCount"];
                if (!string.IsNullOrEmpty(failedRetryIntervalStr))
                {
                    int failedRetryInterval = 60;
                    _ = int.TryParse(failedRetryIntervalStr, out failedRetryInterval);
                    options.FailedRetryInterval = failedRetryInterval;
                }
                options.UseRabbitMQ(options =>
                {
                    var portStr = config["CapSettings:RabbitMQ:Port"];
                    int port = 5672;
                    if (!string.IsNullOrEmpty(portStr)) _ = int.TryParse(portStr, out port);
                    options.HostName = config["CapSettings:RabbitMQ:HostName"];
                    options.VirtualHost = config["CapSettings:RabbitMQ:VirtualHost"];
                    options.UserName = config["CapSettings:RabbitMQ:UserName"];
                    options.Password = config["CapSettings:RabbitMQ:Password"];
                    options.Port = port;
                });
                capOptions?.Invoke(options);
            });

            services.AddTransient<IEventBus, EventBusCapRabbitMQ>();
            return services;
        }

        /// <summary>
        /// 添加默认实现
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEmptyEventBus(this IServiceCollection services)
        {
            services.AddTransient<IEventBus, EmptyEventBus>();
            return services;
        }
    }
}
