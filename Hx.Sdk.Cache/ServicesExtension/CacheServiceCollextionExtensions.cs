using Hx.Sdk.Cache;
using Hx.Sdk.Cache.Options;
using Hx.Sdk.ConfigureOptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 缓存的扩展类
    /// </summary>
    public static class CacheServiceCollextionExtensions
    {
        /// <summary>
        /// 添加内置缓存
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddMemoryCacheSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddMemoryCache();
            services.AddScoped<IMemoryCache, MemoryCache>();
            //services.AddSingleton<IMemoryCache>(factory =>
            //{
            //    var cache = new MemoryCache(new MemoryCacheOptions());
            //    return cache;
            //});
            return services;
        }

        /// <summary>
        /// 添加redis缓存
        /// 在appsettings.json中配置连接字符串
        /// Redis:{ConnectionString:""}
        /// </summary>
        /// <param name="services"></param>
        public static void AddRedisCacheSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                //获取连接字符串
                string redisConfiguration = AppSettings.GetConfig("Redis", "ConnectionString");
                //var configuration = ConfigurationOptions.Parse(redisConfiguration, true);
                //configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(redisConfiguration, null);
            });
            services.AddTransient<IRedisHandler, RedisHandler>();
            services.AddConfigurableOptions<RedisSettingsOptions>();
            services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());
            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = 
            });
        }

        /// <summary>
        /// 添加redis缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">redis连接字符串</param>
        public static void AddRedisCacheSetup(this IServiceCollection services, string connectionString)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                return ConnectionMultiplexer.Connect(connectionString, null);
            });
            services.AddTransient<IRedisHandler, RedisHandler>();
        }
    }
}
