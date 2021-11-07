using System;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// 禁止规范化处理
    /// </summary>
    [Hx.Sdk.Attributes.SkipScan, AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class SkipUnifyAttribute : Attribute
    {
    }
}