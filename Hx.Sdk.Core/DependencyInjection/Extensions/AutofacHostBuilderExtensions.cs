using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.Core;
using Hx.Sdk.Extensions;
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
            // 判断是否安装了 DependencyInjection 程序集
            var diAssembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(AppExtend.DependencyInjection));
            if (diAssembly != null)
            {
                // 加载 Autofac ContainerBuilder 拓展类型和拓展方法
                var hostBuilderExtensionsType = diAssembly.GetType($"Microsoft.Extensions.Hosting.HostBuilderExtensions");
                var injectContainerBuilderMethod = hostBuilderExtensionsType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(u => u.Name == "InjectContainerBuilder" && u.GetParameters().First().ParameterType == typeof(IHostBuilder));
                return injectContainerBuilderMethod.Invoke(null, new object[] { hostBuilder}) as IHostBuilder;
            }

            return hostBuilder;
        }
    }
}
