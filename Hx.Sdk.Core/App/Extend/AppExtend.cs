using Hx.Sdk.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// App扩展
    /// </summary>
    [SkipScan]
    internal static class AppExtend
    {
        /// <summary>
        /// 是否使用Autofac依赖注入接管原生的依赖注入
        /// </summary>
        internal static bool InjectAutofac = true;

        /// <summary>
        /// Hx.Sdk.DependencyInjection依赖注入包
        /// </summary>
        internal const string DependencyInjection = "Hx.Sdk.DependencyInjection";

        /// <summary>
        /// Hx.Sdk.DatabaseAccessor数据访问注入包
        /// </summary>
        internal const string DatabaseAccessor = "Hx.Sdk.DatabaseAccessor";

        /// <summary>
        /// Hx.Sdk.Swagger文档包
        /// </summary>
        internal const string Swagger = "Hx.Sdk.Swagger";
    }
}
