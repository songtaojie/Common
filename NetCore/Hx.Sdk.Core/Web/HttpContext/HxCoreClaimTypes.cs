using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// 自定义声明
    /// </summary>
    internal static class HxClaimTypes
    {
        /// <summary>
        /// IdentityServer4的name claim
        /// </summary>
        public const string Ids4Name = "name";
        /// <summary>
        /// http://tools.ietf.org/html/rfc7519#section-4    
        /// </summary>
        public const string Jti = "jti";
    }

    /// <summary>
    /// Claim的常量值
    /// </summary>
    internal static class HxClaimValues
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        public const string SuperAdmin = "SuperAdmin";

        /// <summary>
        /// 管理员的值
        /// </summary>
        public const string Admin = "Admin";
    }
}
