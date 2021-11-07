using Hx.Sdk.UnifyResult;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 状态码中间件拓展
    /// </summary>
    [Hx.Sdk.Attributes.SkipScan]
    public static class UnifyResultMiddlewareExtensions
    {
        /// <summary>
        /// 添加状态码拦截中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseUnifyResultStatusCodes(this IApplicationBuilder builder)
        {
            // 提供配置
            builder.UseMiddleware<UnifyResultStatusCodesMiddleware>();
            Hx.Sdk.Core.ConsoleHelper.WriteInfoLine("Use the UnifyResultStatusCodes ApplicationBuilder");
            return builder;
        }
    }
}