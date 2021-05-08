using Hx.Sdk.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// 自定义包
    /// </summary>
    [SkipScan]
    internal static class AppExtend
    {
        /// <summary>
        /// 依赖注入包
        /// </summary>
        internal const string DependencyInjection = "Hx.Sdk.DependencyInjection";
    }
}
