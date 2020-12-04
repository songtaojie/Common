using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Hx.Sdk.NetCore.AutoMapper
{
    /// <summary>
    /// autoMapper启动类
    /// </summary>
    public static class AutoMapperSetup
    {
        /// <summary>
        /// 添加数据库上下文
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="assemblyList">要加载的数据的程序集</param>
        public static void AddAutoMapperSetup(this IServiceCollection services,List<Assembly> assemblyList)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (assemblyList == null) throw new ArgumentNullException(nameof(assemblyList));
            services.AddAutoMapper(c=> 
            {
                c.AddProfile(new MyMapperProfile(assemblyList));
            });
            //AutoMapperConfig.RegisterMappings();
        }

        /// <summary>
        /// 添加数据库上下文
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configAction">要加载的数据的程序集</param>
        public static void AddAutoMapperSetup(this IServiceCollection services, Action<IMapperConfigurationExpression> configAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddAutoMapper(c =>
            {
                configAction?.Invoke(c);
            });
        }
    }
}
