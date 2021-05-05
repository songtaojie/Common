using Hx.Sdk.Core;
using Hx.Sdk.Core.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Web相关的拓展类
    /// </summary>
    public static class WebServiceCollectionExtensions
    {
        /// <summary>
        /// 添加UserContent,可以获取用户信息
        /// 操作cookie
        /// 使用时只需要在构造函数注入IUserContext即可
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddUserContext(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            return services;
        }

        /// <summary>
        /// 添加web管理类用于一些路径的处理
        /// 使用时只需要在构造函数注入IWebManager即可
        /// </summary>
        /// <param name="services">服务</param>
        public static IServiceCollection AddWebManager(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSingleton<IWebManager, WebManager>();
            return services;
        }
    }
}
