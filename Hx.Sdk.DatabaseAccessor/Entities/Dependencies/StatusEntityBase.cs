using Hx.Sdk.DependencyInjection;
namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 数据库实体依赖基类（使用默认的数据库上下文定位器）
    /// </summary>
    [SkipScan]
    public abstract class StatusEntityBase : StatusEntityBase<string>
    {
    }

    /// <summary>
    /// 数据库实体依赖基类（使用默认的数据库上下文定位器）
    /// </summary>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    [SkipScan]
    public abstract class StatusEntityBase<TKeyType> : StatusEntityBase<TKeyType, MasterDbContextLocator>
    {
    }

    /// <summary>
    /// 数据库实体依赖基类
    /// </summary>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    [SkipScan]
    public abstract class StatusEntityBase<TKeyType, TDbContextLocator1> : Hx.Sdk.Entity.StatusEntityBase<TKeyType>
        where TDbContextLocator1 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 数据库实体依赖基类
    /// </summary>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    /// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
    [SkipScan]
    public abstract class StatusEntityBase<TKeyType, TDbContextLocator1, TDbContextLocator2> : Hx.Sdk.Entity.StatusEntityBase<TKeyType>
        where TDbContextLocator1 : class, IDbContextLocator
        where TDbContextLocator2 : class, IDbContextLocator
    {
    }

   
}