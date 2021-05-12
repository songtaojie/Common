using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Entity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// 友好异常拦截器
    /// </summary>
    [SkipScan]
    public sealed class FriendlyExceptionFilter : IAsyncExceptionFilter
    {
        /// <summary>
        /// 服务提供器
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostEnvironment _env;
        private readonly ILogger<FriendlyExceptionFilter> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">服务提供器</param>
        /// <param name="env">环境</param>
        /// <param name="logger">日志</param>
        public FriendlyExceptionFilter(IServiceProvider serviceProvider, IHostEnvironment env, ILogger<FriendlyExceptionFilter> logger)
        {
            _serviceProvider = serviceProvider;
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// 异步异常的处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }

       
        /// <summary>
        /// 异常拦截
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            WriteLog(context);
            context.ExceptionHandled = true;
            AjaxResult result = new AjaxResult { Success = false, Message = context.Exception.Message };
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;
            if (context.Exception is UserFriendlyException)
            {
                result.Code = response.StatusCode = StatusCodes.Status200OK;
                context.Result = new JsonResult(result);
            }
            else if (context.Exception is NoAuthorizeException)
            {
                result.Code = response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = new JsonResult(result);
            }
            else if (context.Exception is NotFoundException)
            {
                result.Code = response.StatusCode = StatusCodes.Status404NotFound;
                context.Result = new JsonResult(result);
            }
            else
            {
                result.Code = response.StatusCode = StatusCodes.Status500InternalServerError;
                if (!_env.IsDevelopment())
                {
                    result.Message = "服务器端错误!";
                }
                context.Result = new JsonResult(result);
            }

            //解析异常处理服务，实现自定义异常额外操作，如记录日志等
            var globalExceptionHandler = _serviceProvider.GetService<IGlobalExceptionHandler>();
            if (globalExceptionHandler != null)
            {
                await globalExceptionHandler.OnExceptionAsync(context);
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
            if (UnifyContext.IsSkipUnifyHandler(actionDescriptor.MethodInfo, out var unifyResult))
            {
                // 解析异常信息
                var (StatusCode, _, Errors) = UnifyContext.GetExceptionMetadata(context);

                // 解析 JSON 序列化提供器
                var jsonSerializer = _serviceProvider.GetService<IJsonSerializerProvider>();

                context.Result = new ContentResult
                {
                    Content = jsonSerializer.Serialize(Errors),
                    StatusCode = StatusCode
                };
            }
            else context.Result = unifyResult.OnException(context);

            // 处理验证异常，打印验证失败信息
            if (isValidationMessage)
            {
                App.PrintToMiniProfiler("validation", "Failed", $"Exception Validation Failed:\r\n{errorMessage}", true);
            }
            // 打印错误到 MiniProfiler 中
            else Oops.PrintToMiniProfiler(context.Exception);
        }


        /// <summary>
        /// 写入日志
        /// <param name="context">提供使用</param>
        /// </summary>
        private void WriteLog(ExceptionContext context)
        {
            if (context == null)
                return;
            _logger.LogError(context.Exception, context.Exception.Message);
        }

    }
}
