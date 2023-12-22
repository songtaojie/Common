using Hx.CorsAccessor;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 跨域访问服务拓展类
    /// </summary>
    [SkipScan]
    public static class CorsAccessorServiceCollectionExtensions
    {
        /// <summary>
        /// 默认跨域导出响应头 Key
        /// </summary>
        /// <remarks>解决 ajax，XMLHttpRequest，axios 不能获取请求头问题</remarks>
        private static readonly string[] _defaultExposedHeaders = new[]
        {
            "access-token",
            "x-access-token"
        };
        /// <summary>
        /// 配置跨域
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="corsOptionsHandler"></param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddCorsAccessor(this IServiceCollection services, Action<CorsOptions> corsOptionsHandler = default)
        {
            // 添加跨域配置选项
            services.AddConfigureOptions<CorsAccessorSettingsOptions>();
            // 添加跨域服务
            services.AddCors(options =>
            {
                // 获取选项
                var corsAccessorSettings = App.GetOptions<CorsAccessorSettingsOptions>();
                // 添加策略跨域
                options.AddPolicy(name: corsAccessorSettings.PolicyName, builder =>
                {
                    // 判断是否设置了来源，因为 AllowAnyOrigin 不能和 AllowCredentials一起公用
                    var isNotSetOrigins = corsAccessorSettings.WithOrigins == null || corsAccessorSettings.WithOrigins.Length == 0;

                    var enabledSignalR = corsAccessorSettings.EnabledSignalR == true;

                    // 如果没有配置来源，则允许所有来源
                    if (isNotSetOrigins)
                    {
                        // 解决 SignarlR  不能配置允许所有源问题
                        if (!enabledSignalR) builder.AllowAnyOrigin();
                    }
                    else builder.WithOrigins(corsAccessorSettings.WithOrigins)
                                      .SetIsOriginAllowedToAllowWildcardSubdomains();

                    // 如果没有配置请求标头，则允许所有表头
                    if (corsAccessorSettings.WithHeaders == null || corsAccessorSettings.WithHeaders.Length == 0) builder.AllowAnyHeader();
                    else builder.WithHeaders(corsAccessorSettings.WithHeaders);

                    // 如果没有配置任何请求谓词，则允许所有请求谓词
                    if (corsAccessorSettings.WithMethods == null || corsAccessorSettings.WithMethods.Length == 0)
                    {
                        builder.AllowAnyMethod();
                    }
                    else
                    {
                        // 解决 SignarlR 必须允许 GET POST 问题
                        if (enabledSignalR)
                        {
                            builder.WithMethods(corsAccessorSettings.WithMethods.Concat(new[] { "GET", "POST" }).Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
                        }
                        else
                        {
                            builder.WithMethods(corsAccessorSettings.WithMethods);
                        }
                    }

                    // 配置跨域凭据
                    if ((corsAccessorSettings.AllowCredentials == true && !isNotSetOrigins) || enabledSignalR)
                    {
                        builder.AllowCredentials();
                    }

                    // 配置响应头，如果前端不能获取自定义的 header 信息，必须配置该项，默认配置了 access-token 和 x-access-token，可取消默认行为
                    IEnumerable<string> exposedHeaders = corsAccessorSettings.FixedToken == true
                        ? _defaultExposedHeaders
                        : Array.Empty<string>();
                    if (corsAccessorSettings.WithExposedHeaders != null && corsAccessorSettings.WithExposedHeaders.Length > 0)
                        exposedHeaders.Concat(corsAccessorSettings.WithExposedHeaders).Distinct(StringComparer.OrdinalIgnoreCase);
                    if (exposedHeaders.Any()) builder.WithExposedHeaders(exposedHeaders.ToArray());
                    // 设置预检过期时间
                    builder.SetPreflightMaxAge(TimeSpan.FromSeconds(corsAccessorSettings.SetPreflightMaxAge ?? 24 * 60 * 60));
                });
                // 添加自定义配置
                corsOptionsHandler?.Invoke(options);
            });
            return services;
        }
    }
}