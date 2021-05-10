using Hx.Sdk.Core;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Hosting
{
    /// <summary>
    /// 主机构建器拓展类
    /// </summary>
    [SkipScan]
    public static class WebHostBuilderExtensions
    {

        /// <summary>
        /// Web 主机注入
        /// </summary>
        /// <param name="hostBuilder">Web主机构建器</param>
        /// <param name="injectAutofac">是否使用Autofac依赖注入接管原生的依赖注入，
        /// 使用Autofac时需要配置Autofac相关的配置，只需引用Hx.Sdk.DependencyInjection程序集包中泛型主机IHostBuilder的
        /// 扩展方法InjectContainerBuilder即可完成配置</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder InjectHxWebApp(this IWebHostBuilder hostBuilder, bool injectAutofac = false)
        {
            hostBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "Hx.Sdk.Core");
            AppExtend.InjectAutofac = injectAutofac;
            return hostBuilder;
        }
    }
}
