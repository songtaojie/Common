﻿namespace Hx.Sdk.Core
{
    /// <summary>
    /// App扩展
    /// </summary>
    [Attributes.SkipScan]
    internal static class AppExtend
    {
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
