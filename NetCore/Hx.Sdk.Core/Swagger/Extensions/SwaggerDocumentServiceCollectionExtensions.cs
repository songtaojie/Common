﻿using Hx.Sdk.Core;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 规范化接口服务拓展类
    /// </summary>
    [SkipScan]
    internal static class SwaggerDocumentServiceCollectionExtensions
    {
        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="config"></param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddSwaggerDocuments(this IServiceCollection services,IConfiguration config)
        {
            // 判断是否安装了 Hx.Sdk.Swagger 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.Swagger));
            if (diAssembly == null) return services;
            // 加载 SwaggerBuilder 拓展类型和拓展方法
            var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.Extensions.DependencyInjection.SwaggerDocumentServiceCollectionExtensions");
            if (swaggerBuilderExtensionsType == null) return services;
            var addSwaggerDocumentsMethod = swaggerBuilderExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(FirstMethod);
            ConsoleHelper.WriteInfoLine("Add the SwaggerDocuments service");
            addSwaggerDocumentsMethod?.Invoke(null, new object[] { services, config,null, null });
            return services;
            static bool FirstMethod(MethodInfo methodInfo)
            {
                var parameter = methodInfo.GetParameters();
                if (parameter.Length < 2) return false;
                return methodInfo.Name == "AddSwaggerDocuments" 
                    && parameter.First().ParameterType == typeof(IServiceCollection) 
                    && parameter[1].ParameterType == typeof(IConfiguration);
            }
        }
    }
}