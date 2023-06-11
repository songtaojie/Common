using Hx.Sdk.Swagger;
using Hx.Sdk.Swagger.Internal;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 规范化接口服务拓展类
    /// </summary>
    public static class SwaggerDocumentServiceCollectionExtensions
    {
        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="config"></param>
        /// <param name="swaggerSettings">swagger配置</param>
        /// <param name="swaggerGenConfigure">自定义配置</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddSwaggerDocuments(this IServiceCollection services,IConfiguration config, Action<SwaggerSettingsOptions> swaggerSettings = null, Action<SwaggerGenOptions> swaggerGenConfigure = null)
        {
            // 添加Swagger生成器服务
            services.AddSwaggerGen(options =>
            {
                SwaggerDocumentBuilder.Init(config);
                SwaggerDocumentBuilder.BuildSwaggerGen(options, swaggerGenConfigure);
            });
            return services;
        }

        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="mvcBuilder">Mvc 构建器</param>
        /// <param name="config">配置对象</param>
        /// <param name="swaggerSettings">swagger配置</param>
        /// <param name="swaggerGenConfigure">自定义配置</param>
        /// <returns>服务集合</returns>
        public static IMvcBuilder AddSwaggerDocuments(this IMvcBuilder mvcBuilder,IConfiguration config, Action<SwaggerSettingsOptions> swaggerSettings = null, Action<SwaggerGenOptions> swaggerGenConfigure = null)
        {
            var services = mvcBuilder.Services;
            services.AddSwaggerDocuments(config, swaggerSettings, swaggerGenConfigure);
            return mvcBuilder;
        }
    }
}