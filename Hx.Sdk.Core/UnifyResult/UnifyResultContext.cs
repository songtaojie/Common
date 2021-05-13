﻿using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.FriendlyException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using Hx.Sdk.Extensions;

namespace Hx.Sdk.UnifyResult
{
    /// <summary>
    /// 规范化结果上下文
    /// </summary>
    [SkipScan]
    public static class UnifyResultContext
    {
        /// <summary>
        /// 是否启用规范化结果
        /// </summary>
        internal static bool IsEnabledUnifyHandle = false;

        /// <summary>
        /// 规范化结果类型
        /// </summary>
        internal static Type RESTfulResultType = typeof(RESTfulResult<>);

        /// <summary>
        /// 规范化结果额外数据键
        /// </summary>
        internal static string UnifyResultExtrasKey = "UNIFY_RESULT_EXTRAS";

        /// <summary>
        /// 规范化结果状态码
        /// </summary>
        internal static string UnifyResultStatusCodeKey = "UNIFY_RESULT_STATUS_CODE";

        /// <summary>
        /// 获取异常元数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static (int StatusCode, object ErrorCode, string Message) GetExceptionMetadata(ExceptionContext context)
        {
            // 获取错误码
            var errorCode = context.Exception is UserFriendlyException friendlyException ? friendlyException?.ErrorCode : default;
            // 读取规范化状态码信息
            var statusCode = Get(UnifyResultStatusCodeKey) ?? StatusCodes.Status500InternalServerError;

            var errorMessage = context.Exception.Message;
            var validationFlag = "[Validation]";

            // 处理验证失败异常
            string message = errorMessage;
            if (!string.IsNullOrEmpty(errorMessage) && errorMessage.StartsWith(validationFlag))
            {
                // 处理结果
                message = errorMessage[validationFlag.Length..];
                // 设置为400状态码
                statusCode = StatusCodes.Status400BadRequest;
            }

            return ((int)statusCode, errorCode, message);
        }

        /// <summary>
        /// 填充附加信息
        /// </summary>
        /// <param name="extras"></param>
        public static void Fill(object extras)
        {
            var items = App.HttpContext?.Items;
            if (items.ContainsKey(UnifyResultExtrasKey)) items.Remove(UnifyResultExtrasKey);
            items.Add(UnifyResultExtrasKey, extras);
        }

        /// <summary>
        /// 读取附加信息
        /// </summary>
        public static object Take()
        {
            object extras = null;
            App.HttpContext?.Items?.TryGetValue(UnifyResultExtrasKey, out extras);
            return extras;
        }

        /// <summary>
        /// 设置规范化结果信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        internal static void Set(string key, object value)
        {
            var items = App.HttpContext?.Items;
            if (items != null && items.ContainsKey(key)) items.Remove(key);
            items?.Add(key, value);
        }

        /// <summary>
        /// 读取规范化结果信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static object Get(string key)
        {
            object value = null;
            App.HttpContext?.Items?.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// 是否跳过成功返回结果规范处理（状态码 200~209 ）
        /// </summary>
        /// <param name="method"></param>
        /// <param name="unifyResult"></param>
        /// <param name="isWebRequest"></param>
        /// <returns></returns>
        internal static bool IsSkipUnifyHandlerOnSucceedReturn(MethodInfo method, out IUnifyResultProvider unifyResult, bool isWebRequest = true)
        {
            // 判断是否跳过规范化处理
            var isSkip = !IsEnabledUnifyHandle
                  || method.GetRealReturnType().HasImplementedRawGeneric(RESTfulResultType)
                  || method.CustomAttributes.Any(x => typeof(SkipUnifyAttribute).IsAssignableFrom(x.AttributeType) || typeof(ProducesResponseTypeAttribute).IsAssignableFrom(x.AttributeType) || typeof(IApiResponseMetadataProvider).IsAssignableFrom(x.AttributeType))
                  || method.ReflectedType.IsDefined(typeof(SkipUnifyAttribute), true);

            if (!isWebRequest)
            {
                unifyResult = null;
                return isSkip;
            }

            unifyResult = isSkip ? null : App.GetService<IUnifyResultProvider>();
            return unifyResult == null || isSkip;
        }

        /// <summary>
        /// 是否跳过规范化处理（包括任意状态：成功，失败或其他状态码）
        /// </summary>
        /// <param name="method"></param>
        /// <param name="unifyResult"></param>
        /// <param name="isWebRequest"></param>
        /// <returns></returns>
        internal static bool IsSkipUnifyHandler(MethodInfo method, out IUnifyResultProvider unifyResult, bool isWebRequest = true)
        {
            // 判断是否跳过规范化处理
            var isSkip = !IsEnabledUnifyHandle
                    || method.CustomAttributes.Any(x => typeof(SkipUnifyAttribute).IsAssignableFrom(x.AttributeType))
                    || (
                            !method.CustomAttributes.Any(x => typeof(ProducesResponseTypeAttribute).IsAssignableFrom(x.AttributeType))
                            && method.ReflectedType.IsDefined(typeof(SkipUnifyAttribute), true)
                        );

            if (!isWebRequest)
            {
                unifyResult = null;
                return isSkip;
            }

            unifyResult = isSkip ? null : App.GetService<IUnifyResultProvider>();
            return unifyResult == null || isSkip;
        }

        /// <summary>
        /// 是否跳过特定状态码规范化处理（如，处理 401，403 状态码情况）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="unifyResult"></param>
        /// <param name="isWebRequest"></param>
        /// <returns></returns>
        internal static bool IsSkipUnifyHandler(HttpContext context, out IUnifyResultProvider unifyResult, bool isWebRequest = true)
        {
            // 判断是否跳过规范化处理
            var isSkip = !IsEnabledUnifyHandle || context.GetMetadata<SkipUnifyAttribute>() != null; 

            if (!isWebRequest)
            {
                unifyResult = null;
                return isSkip;
            }

            unifyResult = isSkip ? null : App.GetService<IUnifyResultProvider>();
            return unifyResult == null || isSkip;
        }
    }
}