﻿using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Hx.Sdk.FriendlyException
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public interface IGlobalExceptionHandler
    {
        /// <summary>
        /// 异常拦截
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task OnException(ExceptionContext context);
    }
}