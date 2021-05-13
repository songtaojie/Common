using Hx.Sdk.DependencyInjection;
using Hx.Sdk.UnifyResult;
using Microsoft.AspNetCore.Http;
using System;

namespace System
{
    /// <summary>
    /// 异常拓展
    /// </summary>
    [SkipScan]
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 设置异常状态码
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static Exception SetUnifyResultStatusCode(this Exception exception, int statusCode = StatusCodes.Status500InternalServerError)
        {
            UnifyResultContext.Set(UnifyResultContext.UnifyResultStatusCodeKey, statusCode);
            return exception;
        }
    }
}