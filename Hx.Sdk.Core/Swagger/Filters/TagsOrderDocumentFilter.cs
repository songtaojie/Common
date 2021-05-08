using Hx.Sdk.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Hx.Sdk.Swagger
{
    /// <summary>
    /// 标签文档排序拦截器
    /// </summary>
    [SkipScan]
    public class TagsOrderDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// 配置拦截
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = swaggerDoc.Tags.OrderBy(u=>u.Name).ToArray();
        }
    }
}