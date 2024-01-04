using FreeRedis;
using Hx.Cache;
using Hx.Cache.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 缓存的扩展类
    /// </summary>
    public static class CacheServiceCollextionExtensions
    {
        private const string CacheSettingsKey = "CacheSettings";
        private const string CacheSettings_MemoryKey = "CacheSettings:Memory";
        private const string CacheSettings_RedisKey = "CacheSettings:Redis";
        private const string CacheTypeKey = "CacheSettings:CacheType";
        /// <summary>
        /// 添加内置缓存MemoryCache
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        public static IServiceCollection AddNativeMemoryCache(this IServiceCollection services, Action<CacheSettingsOptions> setupAction = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
            else
            {
                services.AddOptions<CacheSettingsOptions>()
                   .BindConfiguration(CacheSettingsKey);
            }
            
            services.Configure(setupAction);
            services.AddOptions<MemoryCacheOptions>()
                .Configure<IOptions<CacheSettingsOptions>>((options, cacheSettingsOptions) =>
                {
                    var cacheSettings = cacheSettingsOptions.Value;
                    if (cacheSettings.Memory != null)
                    {
                        if (cacheSettings.Memory.CompactionPercentage > 0 && cacheSettings.Memory.CompactionPercentage < 1)
                        {
                            options.CompactionPercentage = cacheSettings.Memory.CompactionPercentage;
                        }
                        if (cacheSettings.Memory.SizeLimit.HasValue)
                        {
                            options.SizeLimit = cacheSettings.Memory.SizeLimit;
                        }
                    }
                });
            services.AddOptions<MemoryDistributedCacheOptions>()
               .Configure<IOptions<CacheSettingsOptions>>((options, cacheSettingsOptions) =>
               {
                   var cacheSettings = cacheSettingsOptions.Value;
                   if (cacheSettings.Memory != null)
                   {
                       if (cacheSettings.Memory.CompactionPercentage > 0 && cacheSettings.Memory.CompactionPercentage < 1)
                       {
                           options.CompactionPercentage = cacheSettings.Memory.CompactionPercentage;
                       }
                       if (cacheSettings.Memory.SizeLimit.HasValue)
                       {
                           options.SizeLimit = cacheSettings.Memory.SizeLimit;
                       }
                   }
               });
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
            var cacheTypeStr = configuration[CacheTypeKey];
            CacheTypeEnum cacheType = CacheTypeEnum.Memory;
            if (!string.IsNullOrEmpty(cacheTypeStr))
            {
                Enum.TryParse(cacheTypeStr, true, out cacheType);
            }
            return services.AddCache(cacheType);
        }

        /// <summary>
        /// 缓存注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cacheType"></param>
        public static IServiceCollection AddCache(this IServiceCollection services, CacheTypeEnum? cacheType = CacheTypeEnum.Memory)
        {
            if (cacheType == CacheTypeEnum.Redis)
            {
                services.AddRedisCache();
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
        /// <param name="setupAction">ConnectionString构造器</param>
        public static IServiceCollection AddRedisCache(this IServiceCollection services, Action<RedisCacheSettingsOptions> setupAction = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (setupAction != null) services.Configure(setupAction);
            services.AddOptions<CacheSettingsOptions>()
                .BindConfiguration(CacheSettingsKey)
                .Configure(options =>
                {
                    setupAction?.Invoke(options.Redis ?? new RedisCacheSettingsOptions());
                });
            AddRedisCacheCore(services);
            return services;
        }

        private static IServiceCollection AddRedisCacheCore(IServiceCollection services)
        {
            services.AddSingleton<IRedisClient>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<CacheSettingsOptions>>().Value;
                var redisOptions = options.Redis;
                if (string.IsNullOrEmpty(redisOptions.ConnectionString))
                    throw new ArgumentNullException(nameof(RedisCacheSettingsOptions.ConnectionString));

                if (redisOptions.SlaveConnectionStrings == null || !redisOptions.SlaveConnectionStrings.Any())
                {
                    return new RedisClient(redisOptions.ConnectionString);
                }
                else
                {
                    var slaveConnectionStrings = redisOptions.SlaveConnectionStrings.Select(r => ConnectionStringBuilder.Parse(r)).ToArray();
                    return new RedisClient(redisOptions.ConnectionString, slaveConnectionStrings);
                }
            });

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
