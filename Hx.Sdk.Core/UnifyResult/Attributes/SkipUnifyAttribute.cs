using Hx.Sdk.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// 禁止规范化处理
    /// </summary>
    [SkipScan, AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class SkipUnifyAttribute : Attribute
    {
    }
}