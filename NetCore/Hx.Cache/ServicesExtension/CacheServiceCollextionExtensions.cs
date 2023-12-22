using FreeRedis;
using Hx.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.AccessControl;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 缓存的扩展类
    /// </summary>
    public static class CacheServiceCollextionExtensions
    {
        private const string CacheTypeKey = "CacheSettings:CacheType";
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
        /// <param name="configuration"></param>
        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<CacheSettingsOptions>()
               .Configure<IConfiguration>((options, configuration) =>
               {
                   options.Initialize(configuration);
               });

            var cacheTypeStr = configuration[CacheTypeKey];
            if (!string.IsNullOrEmpty(cacheTypeStr) 
                && Enum.TryParse(cacheTypeStr, true, out CacheTypeEnum cacheType) 
                && cacheType == CacheTypeEnum.Redis)
            {
                services.AddSingleton<IRedisClient>(provider =>
                {
                    var options = provider.GetRequiredService<IOptions<CacheSettingsOptions>>().Value;
                    if (string.IsNullOrEmpty(options.ConnectionString))
                        throw new ArgumentNullException(nameof(CacheSettingsOptions.ConnectionString));

                    if (options.SlaveConnectionStrings == null || !options.SlaveConnectionStrings.Any())
                    {
                        return new RedisClient(options.ConnectionString);
                    }
                    else
                    {
                        return new RedisClient(options.ConnectionString, options.SlaveConnectionStrings.ToArray());
                    }
                });
                AddRedisCacheCore(services);
            }
            else
            {
                services.AddNativeMemoryCache();
            }
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
            if (cacheSettingsOptions.CacheType == CacheTypeEnum.Redis)
            {
                services.AddRedisCache(cacheSettingsOptions.ConnectionString, cacheSettingsOptions.SlaveConnectionStrings?.ToArray());
            }
            else
            {
                services.AddNativeMemoryCache();
            }
            return services;
        }

        /// <summary>
        /// 添加redis缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">ConnectionString构造器</param>
        /// <param name="slaveConnectionStrings">ConnectionString构造器</param>
        public static IServiceCollection AddRedisCache(this IServiceCollection services, ConnectionStringBuilder connectionString, params ConnectionStringBuilder[] slaveConnectionStrings)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            services.AddSingleton<IRedisClient>(provider =>
            {
                if (slaveConnectionStrings == null || !slaveConnectionStrings.Any())
                {
                    return new RedisClient(connectionString);
                }
                else
                {
                    return new RedisClient(connectionString, slaveConnectionStrings);
                }
            });
            AddRedisCacheCore(services);
            return services;
        }

        private static IServiceCollection AddRedisCacheCore(IServiceCollection services)
        {
            services.AddSingleton<IDistributedCache, DistributedCache>(provider =>
            {
                var redisClient = provider.GetService<IRedisClient>();
                return new DistributedCache(redisClient as RedisClient);
            });
            services.AddSingleton<IRedisCache, RedisCache>();
            services.AddSingleton<ICache, RedisCache>();
            services.AddMemoryCache();
            return services;
        }
    }
}
