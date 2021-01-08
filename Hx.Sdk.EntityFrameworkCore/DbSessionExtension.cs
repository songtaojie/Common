using Hx.Sdk.EntityFrameworkCore.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.EntityFrameworkCore
{
    /// <summary>
    /// DBsession扩展类
    /// </summary>
    public static class DbSessionExtension
    {
        /// <summary>
        /// 添加数据库上下文
        /// </summary>
        /// <typeparam name="T">数据库上下文实现类</typeparam>
        /// <param name="services">服务</param>
        public static void AddDbSession<T>(this IServiceCollection services)where T:DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddDbContext<DbContext, T>();
            services.AddScoped<IDbSession, DbSession>();
        }

        /// <summary>
        /// 添加DbSession，使用前需要添加DBContext
        /// </summary>
        /// <param name="services">服务</param>
        public static void AddDbSession(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddScoped<IDbSession, DbSession>();
        }
    }
}
