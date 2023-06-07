﻿using Hx.Sdk.UnifyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// 规范化结构（请求成功）过滤器
    /// </summary>
    [Hx.Sdk.SkipScan]
    public class UnifyResultFilterAttribute : ResultFilterAttribute
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // 排除 Mvc 视图
            bool isHandleResult = false;
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (typeof(Controller).IsAssignableFrom(actionDescriptor.ControllerTypeInfo))
            {
                base.OnResultExecuting(context);
                return;
            }
            if (!UnifyResultContext.IsSkipUnifyHandlerOnSucceedReturn(actionDescriptor.MethodInfo, out var unifyResultProvider))
            {
                // 处理规范化结果
                // 处理 BadRequestObjectResult 验证结果
                if (context.Result is BadRequestObjectResult badResult)
                {
                    isHandleResult = true;
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
                            .Select(u => u.Value)
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
                    context.Result = new JsonResult(result);
                }
                else
                {
                    var result = unifyResultProvider.OnSucceeded(context);
                    if (result != null)
                    {
                        isHandleResult = true;
                        context.Result = result;
                    }
                }
            }
            if(!isHandleResult) base.OnResultExecuting(context);
        }
    }
}