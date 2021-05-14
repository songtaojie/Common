using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.Core;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Extensions;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 规范化文档swagger中间件拓展
    /// </summary>
    [SkipScan]
    public static class SwaggerDocumentApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加规范化文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerDocuments(this IApplicationBuilder app)
        {

            // 判断是否安装了 DependencyInjection 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.Swagger));
            if (diAssembly != null)
            {
                // 加载 SwaggerBuilder 拓展类型和拓展方法
                var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.AspNetCore.Builder.SwaggerDocumentApplicationBuilderExtensions");
                var addObjectMapperMethod = swaggerBuilderExtensionsType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(u => u.Name == "UseSwaggerDocuments");
                ConsoleExtensions.WriteInfoLine("Use the SwaggerDocument ApplicationBuilder");
                return addObjectMapperMethod.Invoke(null, new object[] { app, null,null,null }) as IApplicationBuilder;
            }
            return app;
        }
    }
}