using Hx.Sdk.DependencyInjection;
using System;

namespace Hx.Sdk.FriendlyException
{
    /// <summary>
    /// 用户友好的异常提示
    /// </summary>
    [SkipScan]
    public class UserFriendlyException : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserFriendlyException() : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        public UserFriendlyException(string message, object errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        /// <param name="innerException"></param>
        public UserFriendlyException(string message, object errorCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public object ErrorCode { get; set; }
    }
}
