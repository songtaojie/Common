using Microsoft.EntityFrameworkCore;
using System;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 实体数据改变监听依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IEntityChangedListener<TEntity> : IEntityChangedListener<TEntity, MasterDbContextLocator>
        where TEntity : class, IPrivateEntity, new()
    {
    }

    /// <summary>
    /// 实体数据改变监听依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    public interface IEntityChangedListener<TEntity, TDbContextLocator1> : IPrivateEntityChangedListener<TEntity>
        where TEntity : class, IPrivateEntity, new()
        where TDbContextLocator1 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 实体数据改变监听依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    /// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
    public interface IEntityChangedListener<TEntity, TDbContextLocator1, TDbContextLocator2> : IPrivateEntityChangedListener<TEntity>
        where TEntity : class, IPrivateEntity, new()
        where TDbContextLocator1 : class, IDbContextLocator
        where TDbContextLocator2 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 实体数据改变监听依赖接口（禁止外部继承）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IPrivateEntityChangedListener<TEntity> : IPrivateModelBuilder
        where TEntity : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 监听数据改变之前（仅支持EFCore操作）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <param name="state"></param>
        void OnChanging(TEntity entity, DbContext dbContext, Type dbContextLocator, EntityState state) { }

        /// <summary>
        /// 监听数据改变之后（仅支持EFCore操作）
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="oldEntity"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <param name="state"></param>
        void OnChanged(TEntity newEntity, TEntity oldEntity, DbContext dbContext, Type dbContextLocator, EntityState state);

        /// <summary>
        /// 监听数据改变失败（仅支持EFCore操作）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <param name="state"></param>
        void OnChangeFailed(TEntity entity, DbContext dbContext, Type dbContextLocator, EntityState state) { }
    }
}