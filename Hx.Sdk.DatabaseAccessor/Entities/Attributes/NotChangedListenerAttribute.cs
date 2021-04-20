using Hx.Sdk.DependencyInjection;
using System;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 跳过实体监听
    /// </summary>
    [SkipScan, AttributeUsage(AttributeTargets.Class)]
    public sealed class NotChangedListenerAttribute : Attribute
    {
    }
}