using FreeRedis;
using Hx.Sdk.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

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
        /// <param name="setupAction"></param>
        public static IServiceCollection AddCache(this IServiceCollection services, Action<CacheSettingsOptions> setupAction = null)
        {
            CacheSettingsOptions cacheSettingsOptions = new CacheSettingsOptions();
            setupAction?.Invoke(cacheSettingsOptions);
            services.AddNativeMemoryCache();
            if (cacheSettingsOptions.CacheType == CacheTypeEnum.Redis)
            {
                if (string.IsNullOrEmpty(cacheSettingsOptions.ConnectionString))
                    throw new ArgumentNullException("ConnectionString不能为空");
                ConnectionStringBuilder[] slaveConnectionStrings = null;
                if (cacheSettingsOptions.SlaveConnectionStrings != null && cacheSettingsOptions.SlaveConnectionStrings.Any())
                {
                    slaveConnectionStrings = cacheSettingsOptions.SlaveConnectionStrings.Select(r => ConnectionStringBuilder.Parse(r)).ToArray();
                }
                services.AddRedisCache(ConnectionStringBuilder.Parse(cacheSettingsOptions.ConnectionString), slaveConnectionStrings);
            }
            return services;
        }

        /// <summary>
        /// 添加redis缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">ConnectionString构造器</param>
        /// <param name="slaveConnectionStrings">ConnectionString构造器</param>
        public static IServiceCollection AddRedisCache(this IServiceCollection services, ConnectionStringBuilder connectionString, ConnectionStringBuilder[] slaveConnectionStrings = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSingleton<IRedisClient>(provider =>
            {
                if (slaveConnectionStrings == null)
                {
                    return new RedisClient(connectionString);
                }
                else
                {
                    return new RedisClient(connectionString, slaveConnectionStrings);
                }
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
