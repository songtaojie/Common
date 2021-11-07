using Hx.Sdk.Core;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 规范化文档swagger中间件拓展
    /// </summary>
    [Hx.Sdk.Attributes.SkipScan]
    internal static class SwaggerDocumentApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加规范化文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        internal static IApplicationBuilder UseSwaggerDocuments(this IApplicationBuilder app)
        {
            // 判断是否安装了 DependencyInjection 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.Swagger));
            if (diAssembly == null) return app;
            // 加载 SwaggerBuilder 拓展类型和拓展方法
            var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.AspNetCore.Builder.SwaggerDocumentApplicationBuilderExtensions");
            if (swaggerBuilderExtensionsType == null) return app;
            var useSwaggerDocuments = swaggerBuilderExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(u => u.Name == "UseSwaggerDocuments" && u.GetParameters().First().ParameterType == typeof(IApplicationBuilder));
            ConsoleHelper.WriteInfoLine("Use the SwaggerDocument ApplicationBuilder");
            useSwaggerDocuments?.Invoke(null, new object[] { app, null, null, null });
            return app;
        }
    }
}