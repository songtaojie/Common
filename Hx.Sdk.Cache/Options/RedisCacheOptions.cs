using Hx.Sdk.ConfigureOptions;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Cache.Options
{
    /// <summary>
    /// RedisCache的配置选项
    /// </summary>
    public class RedisCacheOptions : IConfigurableOptions<RedisSettingsOptions>
    {
        public RedisCacheOptions()
        { 
        }

        /// <summary>
        /// 用于连接到Redis的配置。
        /// </summary>
        public string Configuration { get; set; }
        /// <summary>
        /// 用于连接到Redis的配置。这比Configuration更可取
        /// </summary>
        public ConfigurationOptions ConfigurationOptions { get; set; }
        /// <summary>
        /// Redis实例名称
        /// </summary>
        public string InstanceName { get; set; }

        public void PostConfigure(RedisSettingsOptions options, IConfiguration configuration)
        {
            ConfigurationOptions ??= new ConfigurationOptions()
            {
                DefaultDatabase = 0
            };
        }
    }
}
