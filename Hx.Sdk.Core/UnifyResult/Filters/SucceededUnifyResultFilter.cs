using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Hx.Sdk.UnifyResult
{
    /// <summary>
    /// 规范化结构（请求成功）过滤器
    /// </summary>
    [SkipScan]
    public class SucceededUnifyResultFilter : IAsyncActionFilter, IOrderedFilter
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
            if (actionExecutedContext.Exception == null && !UnifyContext.IsSkipUnifyHandlerOnSucceedReturn(actionDescriptor.MethodInfo, out var unifyResult))
            {
                // 处理规范化结果
                if (unifyResult != null)
                {
                    // 处理 BadRequestObjectResult 验证结果
                    if (actionExecutedContext.Result is BadRequestObjectResult badRequestObjectResult)
                    {
                        // 解析验证消息
                        var (validationResults, validateFaildMessage, modelState) = ValidatorContext.OutputValidationInfo(badRequestObjectResult.Value);

                        var result = unifyResult.OnValidateFailed(context, modelState, validationResults, validateFaildMessage);
                        if (result != null) actionExecutedContext.Result = result;

                        // 打印验证失败信息
                        App.PrintToMiniProfiler("validation", "Failed", $"Validation Failed:\r\n{validateFaildMessage}", true);
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