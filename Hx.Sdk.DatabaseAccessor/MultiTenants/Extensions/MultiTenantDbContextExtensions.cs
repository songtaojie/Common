﻿using Hx.Sdk.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Hx.Sdk.DatabaseAccessor.Extensions
{
    /// <summary>
    /// 多租户数据库上下文拓展
    /// </summary>
    [SkipScan]
    public static class MultiTenantDbContextExtensions
    {
        /// <summary>
        /// 刷新多租户缓存
        /// </summary>
        /// <param name="dbContext"></param>
        public static void RefreshTenantCache(this DbContext dbContext)
        {
            // 判断 HttpContext 是否存在
            var httpContext = App.HttpContext;
            if (httpContext == null) return;

            // 获取主机地址
            var host = httpContext.Request.Host.Value;

            // 获取服务提供器
            var scoped = httpContext.RequestServices;

            // 缓存的 Key
            var tenantCachedKey = $"MULTI_TENANT:{host}";

            // 从内存缓存中移除多租户信息
            var distributedCache = scoped.GetService<IDistributedCache>();
            distributedCache.Remove(tenantCachedKey);
        }
    }
}