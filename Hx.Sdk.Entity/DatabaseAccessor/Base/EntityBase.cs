using Hx.Sdk.DependencyInjection;

namespace Hx.Sdk.Entity
{

    /// <summary>
    /// 数据库实体依赖基类
    /// </summary>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    [SkipScan]
    public abstract class EntityBase<TKeyType> : Internal.PrivateEntityBase<TKeyType>
    { }
   
}
