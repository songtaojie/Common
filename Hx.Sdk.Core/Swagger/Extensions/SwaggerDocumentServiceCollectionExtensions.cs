using Hx.Sdk.Core;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

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
        /// <param name="swaggerGenConfigure">自定义配置</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddSwaggerDocuments(this IServiceCollection services, Action<SwaggerGenOptions> swaggerGenConfigure = null)
        {
            // 判断是否启用规范化文档
            if (App.Settings.InjectSwaggerDocument != true) return services;

            ConsoleHelper.WriteInfoLine("Add the Swagger service");
            // 添加配置
            services.AddConfigurableOptions<SwaggerSettingsOptions>();

            // 添加Swagger生成器服务
            services.AddSwaggerGen(options => SwaggerDocumentBuilder.BuildSwaggerGen(options, swaggerGenConfigure));

            return services;
        }

        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="mvcBuilder">Mvc 构建器</param>
        /// <param name="swaggerGenConfigure">自定义配置</param>
        /// <returns>服务集合</returns>
        public static IMvcBuilder AddSwaggerDocuments(this IMvcBuilder mvcBuilder, Action<SwaggerGenOptions> swaggerGenConfigure = null)
        {
            // 判断是否启用规范化文档
            if (App.Settings.InjectSwaggerDocument != true) return mvcBuilder;
            var services = mvcBuilder.Services;

            // 添加配置
            services.AddConfigurableOptions<SwaggerSettingsOptions>();

            // 添加Swagger生成器服务
            services.AddSwaggerGen(options => SwaggerDocumentBuilder.BuildSwaggerGen(options, swaggerGenConfigure));

            return mvcBuilder;
        }
    }
}