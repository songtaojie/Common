using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.Core;
using Hx.Sdk.Extensions;
using System.Linq;
using System.Reflection;

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
    }
}
