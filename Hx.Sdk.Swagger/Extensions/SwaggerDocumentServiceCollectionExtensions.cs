using Hx.Sdk.Swagger;
using Hx.Sdk.Swagger.Internal;
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
        /// <param name="swaggerGenConfigure">自定义配置</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddSwaggerDocuments(this IServiceCollection services, Action<SwaggerGenOptions> swaggerGenConfigure = null)
        {
            Penetrates.InternalServices = services;
            // 添加配置
            ConfigureJWTOptions(services);
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
            var services = mvcBuilder.Services;
            Penetrates.InternalServices = services;
            // 添加配置
            ConfigureJWTOptions(services);
            // 添加Swagger生成器服务
            services.AddSwaggerGen(options => SwaggerDocumentBuilder.BuildSwaggerGen(options, swaggerGenConfigure));

            return mvcBuilder;
        }

        /// <summary>
        /// 添加 JWT 授权
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureJWTOptions(IServiceCollection services)
        {
            // 配置验证
            services.AddOptions<SwaggerSettingsOptions>()
                    .BindConfiguration("SwaggerSettings")
                    .ValidateDataAnnotations()
                    .PostConfigure(options =>
                    {
                        _ = SwaggerSettingsOptions.SetDefaultSwaggerSettings(options);
                    });
        }
    }
}