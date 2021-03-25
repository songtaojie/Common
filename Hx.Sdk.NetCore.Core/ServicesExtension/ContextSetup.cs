using Hx.Sdk.NetCore.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hx.Sdk.NetCore.Core
{
    /// <summary>
    /// UserContext的启动扩展类
    /// </summary>
    public static class ContextSetup
    {
        /// <summary>
        /// 添加UserContent,可以获取用户信息
        /// 操作cookie
        /// 使用时只需要在构造函数注入IUserContext即可
        /// </summary>
        /// <param name="services"></param>
        /// <param name="isUseIds4">是否是使用IdentityServer4认证</param>
        public static IServiceCollection AddUserContextSetup(this IServiceCollection services,bool isUseIds4 = false)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            UserContext.IsUseIds4 = isUseIds4;
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            return services;
        }
    }
}
