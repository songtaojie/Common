using DotNetCore.CAP;
using Hx.Sdk.EventBus;
using Hx.Sdk.EventBus.Internal;
using Hx.Sdk.EventBus.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQServiceCollectionExtensions
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
        /// 添加rabbitmq，配置文件中需要配置RabbitMqSettings节点
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, int retryCount = 5)
        {
            //注册IRabbitMQPersistentConnection服务用于设置RabbitMQ连接
            services.AddSingleton<IRabbitMQPersistentConnection>(resolver =>
            {
                var logger = resolver.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var configuration = resolver.GetRequiredService<IConfiguration>();
                var hostName = configuration["RabbitMqSettings:HostName"];
                if (string.IsNullOrEmpty(hostName)) throw new MissingMemberException("RabbitMqSettings:HostName is missing from the Appsettings.js file");
                _ = int.TryParse(configuration["RabbitMqSettings:Port"], out int port);
                var factory = new ConnectionFactory()
                {
                    HostName = hostName,
                    DispatchConsumersAsync = true,
                    VirtualHost = configuration["RabbitMqSettings:VirtualHost"],
                    UserName = configuration["RabbitMqSettings:UserName"],
                    Password = configuration["RabbitMqSettings:Password"],
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
        public static IServiceCollection AddCapRabbitMQ(this IServiceCollection services, Action<CapOptions> capOptions = null,
           Action<RabbitMQOptions> mqOption = null)
        {
            services.BuildServiceProvider();
            services.AddOptions<CapOptions>()
               //.BindConfiguration("CapOptions")
               .Configure<IConfiguration>((options, config) =>
               {
                   _ = int.TryParse(config["CapRabbitMQSettings:Cap:FailedRetryCount"], out int failedRetryCount);
                   options.DefaultGroupName = config["CapRabbitMQSettings:Cap:DefaultGroupName"];
                   options.FailedRetryCount = failedRetryCount;
                   options.UseMySql(config["CapRabbitMQSettings:ConnectionString"]);
                   if (mqOption == null)
                   {
                       options.UseRabbitMQ(mqOption =>
                       {
                           _ = int.TryParse(config["CapRabbitMQSettings:RabbitMQ:HostName"], out int port);
                           mqOption.HostName = config["CapRabbitMQSettings:RabbitMQ:HostName"];
                           mqOption.VirtualHost = config["CapRabbitMQSettings:RabbitMQ:VirtualHost"];
                           mqOption.UserName = config["CapRabbitMQSettings:RabbitMQ:UserName"];
                           mqOption.Password = config["CapRabbitMQSettings:RabbitMQ:Password"];
                           mqOption.Port = port;
                       });
                   }
               });
            services.AddCap(options =>
            {
                capOptions?.Invoke(options);
                if (mqOption != null) options.UseRabbitMQ(mqOption);
            });

            services.AddTransient<IEventBus, EventBusCapRabbitMQ>();
            return services;
        }
    }
}
