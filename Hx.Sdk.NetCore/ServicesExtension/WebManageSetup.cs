using Hx.Sdk.NetCore.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.NetCore.ServicesExtension
{
    public static class WebManageSetup
    {
        /// <summary>
        /// 添加web管理类用于一些路径的处理
        /// 使用时只需要在构造函数注入IWebManager即可
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddWebManagerSetup(this IServiceCollection services, IHostEnvironment env)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSingleton<IWebManager>(new WebManager(env));
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        /// <summary>
        /// 添加UserContent,可以获取用户信息
        /// 操作cookie
        /// 使用时只需要在构造函数注入IUserContext即可
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddUserContextSetup(this IServiceCollection services, IHostEnvironment env)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            return services;
        }
    }
}
