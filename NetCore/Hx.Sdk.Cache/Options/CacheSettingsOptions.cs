using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.Cache
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
        public string CacheType { get; set; }

        /// <summary>
        /// 用于连接到Redis的配置。
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 后置配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void PostConfigure(string name, CacheSettingsOptions options)
        {
            CacheType ??= "Memory";
        }
    }
}
