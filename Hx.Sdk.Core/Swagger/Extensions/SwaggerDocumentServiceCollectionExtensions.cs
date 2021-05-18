using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.Core;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 规范化接口服务拓展类
    /// </summary>
    [SkipScan]
    public static class SwaggerDocumentServiceCollectionExtensions
    {
        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddSwaggerDocuments(this IServiceCollection services)
        {
            // 判断是否安装了 DependencyInjection 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.Swagger));
            if (diAssembly != null)
            {
                // 加载 SwaggerBuilder 拓展类型和拓展方法
                var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.Extensions.DependencyInjection.SwaggerDocumentServiceCollectionExtensions");
                var addObjectMapperMethod = swaggerBuilderExtensionsType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(u => u.Name == "AddSwaggerDocuments" && u.GetParameters().First().ParameterType == typeof(IServiceCollection));
                ConsoleHelper.WriteInfoLine("Add the SwaggerDocuments service");
                return addObjectMapperMethod.Invoke(null, new object[] { services, null}) as IServiceCollection;
            }
            return services;
        }

        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="mvcBuilder">Mvc 构建器</param>
        /// <returns>服务集合</returns>
        public static IMvcBuilder AddSwaggerDocuments(this IMvcBuilder mvcBuilder)
        {
            // 判断是否安装了 Swagger 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.Swagger));
            if (diAssembly != null)
            {
                // 加载 SwaggerBuilder 拓展类型和拓展方法
                var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.Extensions.DependencyInjection.SwaggerDocumentServiceCollectionExtensions");
                var addObjectMapperMethod = swaggerBuilderExtensionsType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(u => u.Name == "AddSwaggerDocuments" && u.GetParameters().First().ParameterType == typeof(IMvcBuilder));
                ConsoleHelper.WriteInfoLine("Add the SwaggerDocuments service");
                return addObjectMapperMethod.Invoke(null, new object[] { mvcBuilder, null }) as IMvcBuilder;
            }
            return mvcBuilder;
        }
    }
}