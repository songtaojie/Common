using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Entity.Dependency
{
    /// <summary>
    /// 单例模式
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class SingletonDependencyAttribute : Attribute
    {
    }
}
