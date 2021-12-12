using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Hx.Sdk.UnifyResult
{
    /// <summary>
    /// RESTful 风格返回值
    /// </summary>
    [Attributes.SkipScan, UnifyResultModel(typeof(RESTfulResult<>))]
    public class RESTfulResultProvider : IUnifyResultProvider
    {
        /// <summary>
        /// 异常返回值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IActionResult OnException(ExceptionContext context)
        {
            // 解析异常信息
            var (StatusCode, _, Message) = UnifyResultContext.GetExceptionMetadata(context);

            return new JsonResult(new RESTfulResult<object>
            {
                StatusCode = StatusCode,
                Succeeded = false,
                Data = null,
                Message = Message,
                Extras = UnifyResultContext.Take(),
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
        }

        /// <summary>
        /// 成功返回值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IActionResult OnSucceeded(ResultExecutingContext context)
        {
            object data;
            // 处理内容结果
            if (context.Result is ContentResult contentResult) data = contentResult.Content;
            // 处理对象结果
            else if (context.Result is ObjectResult objectResult) data = objectResult.Value;
            else if (context.Result is EmptyResult) data = null;
            else return null;

            return new JsonResult(new RESTfulResult<object>
            {
                StatusCode = context.Result is EmptyResult ? StatusCodes.Status204NoContent : StatusCodes.Status200OK,  // 处理没有返回值情况 204
                Succeeded = true,
                Data = data,
                Extras = UnifyResultContext.Take(),
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
        }


        /// <summary>
        /// 拦截返回状态码
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public async Task OnResponseStatusCodes(HttpContext context, int statusCode)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            switch (statusCode)
            {
                // 处理 401 状态码
                case StatusCodes.Status401Unauthorized:
                    context.Response.ContentType = "application/json";
                    //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new RESTfulResult<object>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Succeeded = false,
                        Data = null,
                        Message = "401 Unauthorized",
                        Extras = UnifyResultContext.Take(),
                        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    }, setting));
                    break;
                // 处理 403 状态码
                case StatusCodes.Status403Forbidden:
                    context.Response.ContentType = "application/json";
                    //context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new RESTfulResult<object>
                    {
                        StatusCode = StatusCodes.Status403Forbidden,
                        Succeeded = false,
                        Data = null,
                        Message = "403 Forbidden",
                        Extras = UnifyResultContext.Take(),
                        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    }, setting));
                    break;

                default:
                    break;
            }
        }
    }
}