﻿using Hx.Sdk.Swagger;
using Hx.Sdk.Swagger.Internal;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 规范化文档中间件拓展
    /// </summary>
    public static class SwaggerDocumentApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加规范化文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="routePrefix"></param>
        /// <param name="swaggerConfigure"></param>
        /// <param name="swaggerUIConfigure"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerDocuments(this IApplicationBuilder app, string routePrefix = default, Action<SwaggerOptions> swaggerConfigure = null, Action<SwaggerUIOptions> swaggerUIConfigure = null)
        {
            var config = app.ApplicationServices.GetService<IConfiguration>();
            SwaggerDocumentBuilder.Init(config);
            // 配置 Swagger 全局参数
            app.UseSwagger(options => SwaggerDocumentBuilder.Build(options, swaggerConfigure));
            // 配置 Swagger UI 参数
            app.UseSwaggerUI(options => SwaggerDocumentBuilder.BuildUI(options, routePrefix, swaggerUIConfigure));
            return app;
        }

        /// <summary>
        /// 添加规范化文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="routePrefix"></param>
        /// <param name="swaggerUIConfigure"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseKnife4SwaggerDocuments(this IApplicationBuilder app, string routePrefix = default, Action<Knife4UIOptions> swaggerUIConfigure = null)
        {
            var config = app.ApplicationServices.GetService<IConfiguration>();
            SwaggerDocumentBuilder.Init(config);
            // 配置 Swagger 全局参数
            app.UseSwagger(options => SwaggerDocumentBuilder.Build(options));

            // 配置 Swagger UI 参数
            app.UseKnife4UI(options => SwaggerDocumentBuilder.BuildKnife4UUI(options, routePrefix, swaggerUIConfigure));
            return app;
        }
    }
}