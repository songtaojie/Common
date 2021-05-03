using Hx.Sdk.DependencyInjection;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hx.Sdk.Entity
{
    /// <summary>
    ///  该基类封装了删除，禁用等状态字段（默认为字符串主键）
    /// </summary>
    [SkipScan]
    public abstract class StatusEntityBase : StatusEntityBase<string>
    { }

    /// <summary>
    ///  该基类封装了删除，禁用等状态字段
    /// </summary>
    /// <typeparam name="TKeyType">主键的类型</typeparam>
    [SkipScan]
    public abstract class StatusEntityBase<TKeyType> : Internal.PrivateStatusEntityBase<TKeyType>
    { }

    /// <summary>
    /// 状态枚举
    /// </summary>
    [SkipScan]
    public enum StatusEntityEnum
    { 
        /// <summary>
        /// 是
        /// </summary>
        Yes,
        /// <summary>
        /// 否
        /// </summary>
        No
    }
}
