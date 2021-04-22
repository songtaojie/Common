using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Entity;
using System;
using System.Text.Json.Serialization;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 数据库实体依赖基类
    /// </summary>
    [SkipScan]
    public abstract class StatusEntityBase : StatusEntityBase<string, MasterDbContextLocator>
    {
    }

    /// <summary>
    /// 数据库实体依赖基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    [SkipScan]
    public abstract class StatusEntityBase<TKeyType> : StatusEntityBase<TKeyType, MasterDbContextLocator>
    {
    }

    /// <summary>
    /// 数据库实体依赖基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    [SkipScan]
    public abstract class StatusEntityBase<TKeyType, TDbContextLocator1> : PrivateStatusEntityBase<TKeyType>
        where TDbContextLocator1 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 数据库实体依赖基类
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    /// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
    [SkipScan]
    public abstract class StatusEntityBase<TKeyType, TDbContextLocator1, TDbContextLocator2> : PrivateStatusEntityBase<TKeyType>
        where TDbContextLocator1 : class, IDbContextLocator
        where TDbContextLocator2 : class, IDbContextLocator
    {
    }

   
}