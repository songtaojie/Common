using Hx.Sdk.Swagger.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;

namespace Hx.Sdk.Swagger
{
    /// <summary>
    /// 规范化文档Swagger配置选项
    /// </summary>
    public sealed class SwaggerSettingsOptions
    {
        /// <summary>
        /// 是否允许启用MiniProfiler
        /// </summary>
        public bool? EnabledMiniProfiler { get; set; }
        /// <summary>
        /// 文档标题
        /// </summary>
        public string DocumentTitle { get; set; }

        /// <summary>
        /// 默认分组名
        /// </summary>
        public string DefaultGroupName { get; set; }

        /// <summary>
        /// 启用授权支持
        /// </summary>
        public bool? EnableAuthorized { get; set; }

        /// <summary>
        /// 格式化为V2版本
        /// </summary>
        public bool? FormatAsV2 { get; set; }

        /// <summary>
        /// 配置规范化文档地址
        /// </summary>
        public string RoutePrefix { get; set; }

        /// <summary>
        /// 配置虚拟目录
        /// </summary>
        public string VirtualPath { get; set; }

        /// <summary>
        /// 文档展开设置
        /// </summary>
        public DocExpansion? DocExpansionState { get; set; }

        /// <summary>
        /// XML 描述文件
        /// </summary>
        public string[] XmlComments { get; set; }

        /// <summary>
        /// 分组信息
        /// </summary>
        public SwaggerOpenApiInfo[] GroupOpenApiInfos { get; set; }

        /// <summary>
        /// 安全定义
        /// </summary>
        public SwaggerOpenApiSecurityScheme[] SecurityDefinitions { get; set; }

        /// <summary>
        /// 配置 Servers
        /// </summary>
        public OpenApiServer[] Servers { get; set; }

        /// <summary>
        /// 隐藏 Servers
        /// </summary>
        public bool? HideServers { get; set; }

        /// <summary>
        /// 默认 swagger.json 路由模板
        /// </summary>
        public string RouteTemplate { get; set; }

        /// <summary>
        /// 设置默认值
        /// </summary>
        /// <param name="options"></param>
        public static SwaggerSettingsOptions SetDefaultSwaggerSettings(SwaggerSettingsOptions options)
        {
            options.EnabledMiniProfiler = true;
            options.DocumentTitle ??= "Specification Api Document";
            options.DefaultGroupName ??= "Default";
            options.FormatAsV2 ??= false;
            options.RoutePrefix ??= "swagger";
            options.DocExpansionState ??= DocExpansion.List;
            options.XmlComments ??= Penetrates.Assemblies.Where(u => !u.GetName().Name.Contains("Hx.Sdk")).Select(t => t.GetName().Name).ToArray();
            options.GroupOpenApiInfos ??= new SwaggerOpenApiInfo[]
            {
                new SwaggerOpenApiInfo()
                {
                    Group=options.DefaultGroupName
                }
            };

            options.EnableAuthorized ??= true;
            if (options.EnableAuthorized == true)
            {
                options.SecurityDefinitions ??= new SwaggerOpenApiSecurityScheme[]
                {
                    new SwaggerOpenApiSecurityScheme
                    {
                        Id="oauth2",
                        Type= SecuritySchemeType.Http,
                        Name="Authorization",
                        Description="JWT Authorization header using the Bearer scheme.Enter Bearer {Token} in box below (note space between two)",
                        BearerFormat="JWT",
                        Scheme="Bearer",
                        In= ParameterLocation.Header,
                    }
                };
            }

            options.Servers ??= Array.Empty<OpenApiServer>();
            options.HideServers ??= false;
            options.RouteTemplate ??= "swagger/{documentName}/swagger.json";
            return options;
        }
    }
}