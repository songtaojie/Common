using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Common
{
    /// <summary>
    /// 每个请求一个实例
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class ScopedDependencyAttribute : Attribute
    {
    }
}
