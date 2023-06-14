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
        /// </summary>
        public CacheTypeEnum? CacheType { get; set; }




        /// <summary>
        /// 后置配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void PostConfigure(string name, CacheSettingsOptions options)
        {
            throw new NotImplementedException();
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
