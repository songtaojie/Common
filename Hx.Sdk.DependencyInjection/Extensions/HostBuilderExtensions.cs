using Autofac.Extensions.DependencyInjection;
using Hx.Sdk.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// 使用autofac接管原生的依赖注入
        /// 并注入ContainerBuilder
        /// </summary>
        /// <param name="hostBuilder">泛型主机</param>
        /// <returns></returns>
        public static IHostBuilder InjectContainerBuilder(this IHostBuilder hostBuilder)
        {
            ConsoleHelper.WriteWarningLine("Use Autofac takes over Native Dependency Injection Service",true);
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((hostBuilderContext, containerBuilder) =>
            {
                ConsoleHelper.WriteSuccessLine("Begin Autofac ContainerBuilder ");
                var effectiveTypes = App.EffectiveTypes;
                var aopTypeNames = App.Settings.AopTypeFullName;
                IEnumerable<Type> aopTypes = null;
                if (aopTypeNames != null && aopTypeNames.Length > 0)
                {
                    aopTypeNames = aopTypeNames.Select(t => t.ToLower()).ToArray();
                    aopTypes = effectiveTypes.Where(t => aopTypeNames.Contains(t.FullName.ToLower()));
                }
                ConsoleHelper.WriteInfoLine("Add the Autofac Dependency Injection service");
                if (aopTypes!=null && aopTypes.Count() > 0)
                {
                    var apoTypeNames = aopTypes.Select(type => string.Format("[{0}]", type.FullName));
                    ConsoleHelper.WriteInfoLine($"Add the Aop Types {string.Join(",", apoTypeNames)}");
                }
                containerBuilder.AddAutofacDependencyInjection(effectiveTypes, aopTypes);
                ConsoleHelper.WriteSuccessLine("End Autofac ContainerBuilder", true);
            });
            return hostBuilder;
        }

        ///// <summary>
        ///// 获取应用有效程序集
        ///// </summary>
        ///// <returns>IEnumerable</returns>
        //private static IEnumerable<Assembly> GetAssemblies()
        //{
        //    // 需排除的程序集后缀
        //    var excludeAssemblyNames = new string[] {
        //        "Database.Migrations"
        //    };

        //    // 读取应用配置
        //    var dependencyContext = DependencyContext.Default;

        //    // 读取项目程序集或 Hx.Sdk 发布的包，或手动添加引用的dll，或配置特定的包前缀
        //    var scanAssemblies = dependencyContext.CompileLibraries
        //        .Where(u =>
        //               (u.Type == "project" && !excludeAssemblyNames.Any(j => u.Name.EndsWith(j)))
        //               || (u.Type == "package" && u.Name.StartsWith("Hx.Sdk")))    // 判断是否启用引用程序集扫描
        //        .Select(u => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(u.Name)))
        //        .ToList();

        //    return scanAssemblies;
        //}
    }
}
