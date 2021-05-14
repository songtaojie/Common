using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.Core;
using Hx.Sdk.Extensions;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// MiniProfiler服务扩展类
    /// </summary>
    public static class MiniProfilerServiceCollectionExtensions
    {
        /// <summary>
        /// MiniProfiler 插件路径
        /// </summary>
        private const string MiniProfilerRouteBasePath = "/index-mini-profiler";
        /// <summary>
        /// 添加MiniProfiler服务
        /// </summary>
        /// <param name="services">服务</param>
        /// <returns></returns>
        public static IServiceCollection AddMiniProfilerService(this IServiceCollection services)
        {
            ConsoleExtensions.WriteInfoLine("Add the MiniProfiler service");
            // 注册MiniProfiler 组件
            var miniProfilerBuilder = services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = MiniProfilerRouteBasePath;
            });

            // 判断是否安装了 DatabaseAccessor 程序集
            var dbAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.DatabaseAccessor));
            if (dbAssembly != null)
            {
                // 加载 MiniProfilerS 拓展类型和拓展方法
                var miniProfilerServiceCollectionExtensionsType = dbAssembly.GetType($"Microsoft.Extensions.DependencyInjection.MiniProfilerServiceCollectionExtensions");
                var addObjectMapperMethod = miniProfilerServiceCollectionExtensionsType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(u => u.Name == "AddRelationalDiagnosticListener");
                ConsoleExtensions.WriteInfoLine("Add RelationalDiagnosticListener service");
                _ = addObjectMapperMethod.Invoke(null, new object[] { miniProfilerBuilder }) as IMiniProfilerBuilder;
            }
            return services;
        }
    }
}
