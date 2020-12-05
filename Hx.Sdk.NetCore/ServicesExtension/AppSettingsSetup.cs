using Hx.Sdk.NetCore.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.NetCore.ServicesExtension
{
    public static class AppSettingsSetup
    {
        /// <summary>
        /// 添加AppSettings,注入为单例模式，
        /// 可以获取AppSettings.Json中的配置信息
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env">环境</param>
        public static IServiceCollection AddAppSettingsSetup(this IServiceCollection services, IHostEnvironment env)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSingleton(new AppSettings(env));
            return services;
        }
    }
}
