using Hx.Sdk.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 动态表名依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IEntityMutableTable<TEntity> : IEntityMutableTable<TEntity, MasterDbContextLocator>
        where TEntity : class, IPrivateEntity, new()
    {
    }

    /// <summary>
    /// 动态表名依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    public interface IEntityMutableTable<TEntity, TDbContextLocator1> : IPrivateEntityMutableTable<TEntity>
        where TEntity : class, IPrivateEntity, new()
        where TDbContextLocator1 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 动态表名依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    /// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
    public interface IEntityMutableTable<TEntity, TDbContextLocator1, TDbContextLocator2> : IPrivateEntityMutableTable<TEntity>
        where TEntity : class, IPrivateEntity, new()
        where TDbContextLocator1 : class, IDbContextLocator
        where TDbContextLocator2 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 动态表名依赖接口（禁止外部继承）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IPrivateEntityMutableTable<TEntity> : IPrivateModelBuilder
        where TEntity : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        string GetTableName(DbContext dbContext, Type dbContextLocator);
    }
}