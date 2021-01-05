using Hx.Sdk.EntityFrameworkCore.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.EntityFrameworkCore
{
    public static class DbSessionExtension
    {
        /// <summary>
        /// 添加数据库上下文
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddDbSession<T>(this IServiceCollection services)where T:DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddDbContext<T>();
            services.AddScoped<IDbSession, DbSession>();
        }
    }
}
