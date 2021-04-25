﻿using Hx.Sdk.Cache;
using Hx.Sdk.Cache.Internal;
using Hx.Sdk.Cache.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;

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
            return services;
        }

        /// <summary>
        /// 添加redis缓存
        /// 在appsettings.json中配置RedisCache配置选项
        /// </summary>
        /// <param name="services"></param>
        public static void AddRedisCache(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddConfigurableOptions<RedisCacheOptions>();
            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            services.AddSingleton<ConnectionMultiplexer>(resolver =>
            {
                var options = resolver.GetRequiredService<IOptions<RedisCacheOptions>>().Value;
                if (options.ConfigurationOptions == null)
                {
                    return ConnectionMultiplexer.Connect(options.ConfigurationOptions);
                }
                return ConnectionMultiplexer.Connect(options.Configuration);
            });
            services.AddSingleton<IDistributedCache, RedisCache>();
            services.AddScoped<IRedisCache, RedisCache>();
        }

        /// <summary>
        /// 添加redis缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">redis连接字符串</param>
        public static void AddRedisCache(this IServiceCollection services, Action<RedisCacheOptions> setupAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //services.AddOptions();
            services.Configure(setupAction);
            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            services.AddSingleton<ConnectionMultiplexer>(resolver =>
            {
                var options = resolver.GetRequiredService<IOptions<RedisCacheOptions>>().Value;
                if (options.ConfigurationOptions == null)
                {
                    return ConnectionMultiplexer.Connect(options.ConfigurationOptions);
                }
                return ConnectionMultiplexer.Connect(options.Configuration);
            });
            services.AddSingleton<IDistributedCache, RedisCache>();
            services.AddScoped<IRedisCache, RedisCache>();
        }
    }
}