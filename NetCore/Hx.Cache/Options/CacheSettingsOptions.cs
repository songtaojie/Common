﻿using FreeRedis;
using Hx.Cache.Options;
using Microsoft.Extensions.Caching.Memory;
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
        /// redis配置
        /// </summary>
        public RedisCacheSettingsOptions Redis {  get; set; }

        /// <summary>
        /// 缓存配置
        /// </summary>
        public MemoryDistributedCacheOptions Memory {  get; set; }

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