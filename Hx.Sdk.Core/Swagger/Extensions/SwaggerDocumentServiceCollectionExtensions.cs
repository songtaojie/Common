using Hx.Sdk.Core;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 规范化接口服务拓展类
    /// </summary>
    [Hx.Sdk.Attributes.SkipScan]
    internal static class SwaggerDocumentServiceCollectionExtensions
    {
        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddSwaggerDocuments(this IServiceCollection services)
        {
            // 判断是否安装了 DependencyInjection 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.Swagger));
            if (diAssembly == null) return services;
            // 加载 SwaggerBuilder 拓展类型和拓展方法
            var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.Extensions.DependencyInjection.SwaggerDocumentServiceCollectionExtensions");
            if (swaggerBuilderExtensionsType == null) return services;
            var addObjectMapperMethod = swaggerBuilderExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(u => u.Name == "AddSwaggerDocuments" && u.GetParameters().First().ParameterType == typeof(IServiceCollection));
            ConsoleHelper.WriteInfoLine("Add the SwaggerDocuments service");
            addObjectMapperMethod?.Invoke(null, new object[] { services, null, null });
            return services;
        }

        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="mvcBuilder">Mvc 构建器</param>
        /// <returns>服务集合</returns>
        internal static IMvcBuilder AddSwaggerDocuments(this IMvcBuilder mvcBuilder)
        {
            // 判断是否安装了 Swagger 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.Swagger));
            if (diAssembly == null) return mvcBuilder;
            // 加载 SwaggerBuilder 拓展类型和拓展方法
            var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.Extensions.DependencyInjection.SwaggerDocumentServiceCollectionExtensions");
            if (swaggerBuilderExtensionsType == null) return mvcBuilder;
            var addObjectMapperMethod = swaggerBuilderExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(u => u.Name == "AddSwaggerDocuments" && u.GetParameters().First().ParameterType == typeof(IMvcBuilder));
            ConsoleHelper.WriteInfoLine("Add the SwaggerDocuments service");
            addObjectMapperMethod?.Invoke(null, new object[] { mvcBuilder, null, null });
            return mvcBuilder;
        }
    }
}