using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Sdk.UnifyResult
{
    /// <summary>
    /// 规范化结构（请求成功）过滤器
    /// </summary>
    [SkipScan]
    public class SucceedUnifyResultFilter : IAsyncActionFilter, IOrderedFilter
    {

        /// <summary>
        /// 排序属性
        /// </summary>
        public int Order => FilterOrder.SucceededUnifyResultFilterOrder;

        /// <summary>
        /// 处理规范化结果
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 排除 Mvc 视图
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (typeof(Controller).IsAssignableFrom(actionDescriptor.ControllerTypeInfo))
            {
                await next();
                return;
            }

            var actionExecutedContext = await next();

            // 如果没有异常再执行
            if (actionExecutedContext.Exception == null && !UnifyResultContext.IsSkipUnifyHandlerOnSucceedReturn(actionDescriptor.MethodInfo, out var unifyResult))
            {
                // 处理规范化结果
                if (unifyResult != null)
                {
                   
                    // 处理 BadRequestObjectResult 验证结果
                    if (actionExecutedContext.Result is BadRequestObjectResult badResult)
                    {
                        var result = new RESTfulResult<object>
                        {
                            StatusCode = badResult.StatusCode.HasValue ? badResult.StatusCode.Value : StatusCodes.Status500InternalServerError,  // 处理没有返回值情况 204
                            Succeeded = false,
                            Extras = UnifyResultContext.Take(),
                            Message = "Internal Server Error",
                            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                        };

                        // 如果是模型验证字典类型
                        if (badResult.Value is ModelStateDictionary modelState)
                        {
                            // 将验证错误信息转换成字典并序列化成 Json
                            var validationResult = modelState.Where(u => modelState[u.Key].ValidationState == ModelValidationState.Invalid)
                                .Select(u=>u.Value)
                                .FirstOrDefault();
                            result.Message = validationResult?.Errors.FirstOrDefault().ErrorMessage;
                        }
                        // 如果是 ValidationProblemDetails 特殊类型
                        else if (badResult.Value is ValidationProblemDetails validation)
                        {
                            var error = validation.Errors.FirstOrDefault().Value;
                            result.Message = error.FirstOrDefault();
                        }
                        // 解析验证消息
                        if (result != null) actionExecutedContext.Result = new JsonResult(result);

                        // 打印验证失败信息
                        App.PrintToMiniProfiler("validation", "Failed", $"Validation Failed:\r\n{result.Message}", true);
                    }
                    else
                    {
                        var result = unifyResult.OnSucceeded(actionExecutedContext);
                        if (result != null) actionExecutedContext.Result = result;
                    }
                }
            }
        }
    }
}