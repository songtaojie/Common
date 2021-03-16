using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.Entity
{
    /// <summary>
    /// 主键接口
    /// </summary>
    /// <typeparam name="TKeyType">主键的类型</typeparam>
    public interface IEntity<TKeyType>
    {
        /// <summary>
        /// 主键
        /// </summary>
        TKeyType Id { get; set; }
    }
}
