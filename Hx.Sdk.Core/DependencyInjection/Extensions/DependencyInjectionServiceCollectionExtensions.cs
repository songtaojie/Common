using Autofac;
using Hx.Sdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public static class DependencyInjectionServiceCollectionExtensions
    {
        /// <summary>
        /// 使用.Net Core自带的DI添加依赖注入接口
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddNativeDependencyInjection(this IServiceCollection services)
        {
            // 判断是否安装了 DependencyInjection 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.DependencyInjection));
            if (diAssembly != null)
            {
                // 加载 ObjectMapper 拓展类型和拓展方法
                var diServiceCollectionExtensionsType = diAssembly.GetType($"Microsoft.Extensions.DependencyInjection.NativeDependencyInjectionServiceCollectionExtensions");
                var addObjectMapperMethod = diServiceCollectionExtensionsType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(u => u.Name == "AddNativeDependencyInjection");
                ConsoleHelper.WriteInfoLine("Add the Native Dependency Injection service");
                return addObjectMapperMethod.Invoke(null, new object[] { services, App.EffectiveTypes }) as IServiceCollection;
            }

            return services;
        }

        /// <summary>
        /// 使用autofac进行依赖注入
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <returns>服务集合</returns>
        public static ContainerBuilder AddAutofacDependencyInjection(this ContainerBuilder builder)
        {
            // 判断是否安装了 DependencyInjection 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.DependencyInjection));
            if (diAssembly != null)
            {
                // 加载 ObjectMapper 拓展类型和拓展方法
                var diServiceCollectionExtensionsType = diAssembly.GetType($"Microsoft.Extensions.DependencyInjection.AutofacDependencyInjectionServiceCollectionExtensions");
                var addObjectMapperMethod = diServiceCollectionExtensionsType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(u => u.Name == "AddAutofacDependencyInjection");

                var effectiveTypes = Hx.Sdk.Core.App.EffectiveTypes;
                var aopTypeNames = App.Settings.AopTypeFullName;
                IEnumerable<Type> aopTypes = null;
                if (aopTypeNames != null && aopTypeNames.Length > 0)
                {
                    aopTypeNames = aopTypeNames.Select(t => t.ToLower()).ToArray();
                    aopTypes = effectiveTypes.Where(t => aopTypeNames.Contains(t.FullName.ToLower()));
                }
                ConsoleHelper.WriteInfoLine("Add the Autofac Dependency Injection service");
                if (aopTypes.Count() > 0)
                {
                    var apoTypeNames = aopTypes.Select(type => string.Format("[{0}]", type.FullName));
                    ConsoleHelper.WriteInfoLine($"Add the Aop Types ${string.Join(",", apoTypeNames)}");
                }
                return addObjectMapperMethod.Invoke(null, new object[] { builder, effectiveTypes, aopTypes}) as ContainerBuilder;
            }

            return builder;
        }

    }
}
