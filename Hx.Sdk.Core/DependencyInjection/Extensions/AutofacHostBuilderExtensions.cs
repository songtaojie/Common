﻿using Autofac.Extensions.DependencyInjection;
using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.Core;
using Hx.Sdk.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutofacHostBuilderExtensions
    {
        /// <summary>
        /// 使用autofac接管原生的依赖注入
        /// 并注入ContainerBuilder
        /// </summary>
        /// <param name="hostBuilder">泛型主机</param>
        /// <returns></returns>
        public static IHostBuilder InjectContainerBuilder(this IHostBuilder hostBuilder)
        {
            ConsoleHelper.WriteWarningLine("Use Autofac takes over Native Dependency Injection Service", true);
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
                if (aopTypes != null && aopTypes.Any())
                {
                    var apoTypeNames = aopTypes.Select(type => string.Format("[{0}]", type.FullName));
                    ConsoleHelper.WriteInfoLine($"Add the Aop Types {string.Join(",", apoTypeNames)}");
                }
                containerBuilder.AddAutofacDependencyInjection(effectiveTypes, aopTypes);
                ConsoleHelper.WriteSuccessLine("End Autofac ContainerBuilder", true);
            });
            return hostBuilder;
        }
    }
}
