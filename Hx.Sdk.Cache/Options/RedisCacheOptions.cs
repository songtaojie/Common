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
    public class RedisCacheOptions : IConfigurableOptions<RedisCacheOptions>
    {
        /// <summary>
        /// RedisCache的配置选项
        /// </summary>
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

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="options">选项</param>
        /// <param name="configuration">配置对象</param>
        public void PostConfigure(RedisCacheOptions options, IConfiguration configuration)
        {
            options.InstanceName ??= "Hx";
            if (!string.IsNullOrEmpty(Configuration) && ConfigurationOptions ==null)
            {
                ConfigurationOptions = ConfigurationOptions.Parse(Configuration, true);
                ConfigurationOptions.DefaultDatabase ??= 0;
            }
        }
    }
}
