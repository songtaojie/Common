using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Entity.Attributes
{
    /// <summary>
    /// 跳过全局代理
    /// </summary>
    [SkipScan, AttributeUsage(AttributeTargets.Class)]
    public class SkipProxyAttribute : Attribute
    {
    }
}
