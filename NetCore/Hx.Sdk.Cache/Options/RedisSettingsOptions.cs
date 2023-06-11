using StackExchange.Redis;

namespace Hx.Sdk.Cache.Options
{
    /// <summary>
    /// RedisSettings的配置选项
    /// </summary>
    public class RedisSettingsOptions 
    {
        /// <summary>
        /// RedisSettings的配置选项
        /// </summary>
        public RedisSettingsOptions()
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
        /// 设置默认 Redis 配置
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        internal RedisSettingsOptions SetDefaultRedisSettings(RedisSettingsOptions options)
        {
            options.InstanceName ??= "Hx";
            if (!string.IsNullOrEmpty(options.Configuration) && options.ConfigurationOptions == null)
            {
                options.ConfigurationOptions = ConfigurationOptions.Parse(options.Configuration, true);
                options.ConfigurationOptions.DefaultDatabase ??= 0;
            }

            return options;
        }
    }
}
