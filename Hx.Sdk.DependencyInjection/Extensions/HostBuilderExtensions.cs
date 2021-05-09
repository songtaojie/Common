using Autofac.Extensions.DependencyInjection;
using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.Extensions;
using Microsoft.Extensions.DependencyInjection;
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
            ConsoleExtensions.WriteSuccessLine("Use Autofac takes over Native Dependency Injection Service",true);
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((hostBuilderContext, containerBuilder) =>
            {
                ConsoleExtensions.WriteSuccessLine("Begin Autofac ContainerBuilder ");
                var effectiveTypes = App.EffectiveTypes;
                var aopTypeNames = App.Settings.AopTypeFullName;
                IEnumerable<Type> aopTypes = null;
                if (aopTypeNames != null && aopTypeNames.Length > 0)
                {
                    aopTypeNames = aopTypeNames.Select(t => t.ToLower()).ToArray();
                    aopTypes = effectiveTypes.Where(t => aopTypeNames.Contains(t.FullName.ToLower()));
                }
                ConsoleExtensions.WriteInfoLine("Add the Autofac Dependency Injection service");
                if (aopTypes.Count() > 0)
                {
                    var apoTypeNames = aopTypes.Select(type => string.Format("[{0}]", type.FullName));
                    ConsoleExtensions.WriteInfoLine($"Add the Aop Types ${string.Join(",", apoTypeNames)}");
                }
                containerBuilder.AddAutofacDependencyInjection(effectiveTypes, aopTypes);
                ConsoleExtensions.WriteSuccessLine("End Autofac ContainerBuilder", true);
            });
            return hostBuilder;
        }
    }
}
