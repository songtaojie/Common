using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.NetCore.Config
{
    /// <summary>
    /// webmanager帮助类注入
    /// </summary>
    public static class WebManageSetup
    {
        /// <summary>
        /// 添加web管理类用于一些路径的处理
        /// 使用时只需要在构造函数注入IWebManager即可
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="env">环境变量</param>
        public static IServiceCollection AddWebManagerSetup(this IServiceCollection services, IHostEnvironment env)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSingleton<IWebManager>(new WebManager(env));
            return services;
        }
    }
}
