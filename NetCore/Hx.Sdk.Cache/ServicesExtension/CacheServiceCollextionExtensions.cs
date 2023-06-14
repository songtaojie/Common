using FreeRedis;
using Hx.Sdk.Cache;
using Hx.Sdk.Cache.Internal;
using Hx.Sdk.Cache.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 缓存的扩展类
    /// </summary>
    public static class CacheServiceCollextionExtensions
    {
        /// <summary>
        /// 添加内置缓存MemoryCache
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddNativeMemoryCache(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            // 注册内存和分布式内存
            services.AddMemoryCache();  // .NET 5.0.3+ 需要手动注册了
            services.AddDistributedMemoryCache();
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
            ConfigureRedisSettings(services);
            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            //services.AddSingleton<ConnectionMultiplexer>(resolver =>
            //{
            //    var options = resolver.GetRequiredService<IOptions<RedisCacheOptions>>().Value;
            //    if (options.ConfigurationOptions == null)
            //    {
            //        return ConnectionMultiplexer.Connect(options.Configuration);
            //    }
            //    return ConnectionMultiplexer.Connect(options.ConfigurationOptions);
            //});
            services.AddSingleton<IDistributedCache, RedisCache>();
            services.AddTransient<IRedisCache, RedisCache>();
    }

        /// <summary>
        /// 添加redis缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction">redis配置</param>
        public static void AddRedisCache(this IServiceCollection services, Action<RedisSettingsOptions> setupAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddOptions<RedisSettingsOptions>().Configure(setupAction);
            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            services.AddSingleton<IDistributedCache, RedisCache>();
            services.AddTransient<IRedisCache, RedisCache>();
            var r = new RedisClient("192.168.164.10:6379,database=1"); //redis 6.0
        }


        /// <summary>
        /// 添加 Redis 配置
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureRedisSettings(IServiceCollection services)
        {
            // 获取配置节点
            // 配置验证
            services.AddOptions<RedisSettingsOptions>()
                    .BindConfiguration("RedisSettings")
                    .ValidateDataAnnotations();

            // 选项后期配置
            services.PostConfigure<RedisSettingsOptions>(options =>
            {
                _ = options.SetDefaultRedisSettings(options);
            });
        }
    }
}
