using FreeRedis;
using Hx.Sdk.Cache;
using Hx.Sdk.Cache.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSingleton<ICache, DefaultCache>();
            return services;
        }

        /// <summary>
        /// 缓存注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置</param>
        public static IServiceCollection AddCache(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddNativeMemoryCache();
            var cacheType = configuration["CacheSettings:CacheType"];
            if ("Redis".Equals(cacheType, StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddRedisCache();
            }
            return services;
        }

        /// <summary>
        /// 添加redis缓存
        /// 在appsettings.json中配置CacheSettings配置选项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builderAction">ConnectionString构造器</param>
        public static IServiceCollection AddRedisCache(this IServiceCollection services,Action<ConnectionStringBuilder> builderAction = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSingleton<IRedisClient>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration["CacheSettings:ConnectionString"];
                ConnectionStringBuilder connectionStringBuilder = ConnectionStringBuilder.Parse(connectionString);
                builderAction?.Invoke(connectionStringBuilder);
                return new RedisClient(connectionStringBuilder);
            });
            services.AddSingleton<IDistributedCache, DistributedCache>(provider => 
            {
                var redisClient = provider.GetService<IRedisClient>();
                return new DistributedCache(redisClient as RedisClient);
            });
            services.AddSingleton<IRedisCache, RedisCache>();
            services.AddSingleton<ICache, RedisCache>();
            return services;
        }
    }
}
