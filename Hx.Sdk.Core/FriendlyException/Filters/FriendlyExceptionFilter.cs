using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Entity;
using Hx.Sdk.FriendlyException;
using Hx.Sdk.UnifyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// 友好异常拦截器
    /// </summary>
    [SkipScan]
    public sealed class FriendlyExceptionFilter : IAsyncExceptionFilter, IExceptionFilter
    {
        /// <summary>
        /// 服务提供器
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">服务提供器</param>
        public FriendlyExceptionFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            //解析异常处理服务，实现自定义异常额外操作，如记录日志等
            var globalExceptionHandler = _serviceProvider.GetService<IGlobalExceptionHandler>();
            if (globalExceptionHandler != null)
            {
                globalExceptionHandler.OnException(context);
            }

            // 排除 Mvc 控制器处理
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (typeof(Controller).IsAssignableFrom(actionDescriptor.ControllerTypeInfo)) return;

            // 如果异常在其他地方被标记了处理，那么这里不再处理
            if (context.ExceptionHandled) return;

            // 标识异常已经被处理（改代码已取消，导致其他自定义异常过滤器无法拦截）
            // context.ExceptionHandled = true;

            // 设置异常结果
            var exception = context.Exception;

            // 解析验证异常
            var validationFlag = "[Validation]";
            var isValidationMessage = exception.Message.StartsWith(validationFlag);
            var errorMessage = isValidationMessage ? exception.Message[validationFlag.Length..] : exception.Message;

            // 判断是否跳过规范化结果
            if (UnifyResultContext.IsSkipUnifyHandler(actionDescriptor.MethodInfo, out var unifyResult))
            {
                // 解析异常信息
                var (StatusCode, _, Message) = UnifyResultContext.GetExceptionMetadata(context);
                context.Result = new ContentResult
                {
                    Content = Message,
                    StatusCode = StatusCode
                };
            }
            else context.Result = unifyResult.OnException(context);

            // 处理验证异常，打印验证失败信息
            if (isValidationMessage)
            {
                App.PrintToMiniProfiler("validation", "Failed", $"Exception Validation Failed:\r\n{errorMessage}", true);
            }
        }

        /// <summary>
        /// 异步异常的处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            await Task.CompletedTask;
        }
    }
}
