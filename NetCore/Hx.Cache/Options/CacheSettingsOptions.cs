using FreeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Cache
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheSettingsOptions : IPostConfigureOptions<CacheSettingsOptions>
    {
        /// <summary>
        /// 缓存类型
        /// Redis、Memory
        /// </summary>
        public CacheTypeEnum? CacheType { get; set; }

        /// <summary>
        /// 用于连接到Redis的配置。
        /// </summary>
        public ConnectionStringBuilder ConnectionString { get; set; }

        /// <summary>
        /// Slave连接字符串
        /// </summary>
        public IEnumerable<ConnectionStringBuilder> SlaveConnectionStrings { get; set; }

        /// <summary>
        /// 后置配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void PostConfigure(string name, CacheSettingsOptions options)
        {
            CacheType ??=  CacheTypeEnum.Memory;
        }

        internal void Initialize(IConfiguration configuration)
        {
            var cacheTypeStr = configuration["CacheSettings:CacheType"];
            if (!string.IsNullOrEmpty(cacheTypeStr)
                && Enum.TryParse<CacheTypeEnum>(cacheTypeStr,true, out CacheTypeEnum cacheType))
            {
                CacheType = cacheType;
            }

            var connectionString = configuration["CacheSettings:ConnectionString"];
            if (!string.IsNullOrEmpty(connectionString))
            {
                ConnectionString = connectionString;
            }
            var slaveSection = configuration.GetSection("CacheSettings:SlaveConnectionStrings");
            var slaveChildren = slaveSection.GetChildren();
            if (slaveChildren.Any())
            {
                var slaveConnectionStrings = new List<ConnectionStringBuilder>();
                foreach (var item in slaveChildren)
                {
                    slaveConnectionStrings.Add(item.Value);
                }
                SlaveConnectionStrings = slaveConnectionStrings;
            }
        }
    }

    /// <summary>
    /// 缓存类型
    /// </summary>
    public enum CacheTypeEnum
    {
        /// <summary>
        /// 内存缓存
        /// </summary>
        Memory = 1,
        /// <summary>
        /// Redis缓存
        /// </summary>
        Redis = 2
    }
}
