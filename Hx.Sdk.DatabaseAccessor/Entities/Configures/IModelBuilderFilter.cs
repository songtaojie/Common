using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 数据库模型构建筛选器依赖接口
    /// </summary>
    public interface IModelBuilderFilter : IModelBuilderFilter<MasterDbContextLocator>
    {
    }

    /// <summary>
    /// 数据库模型构建筛选器依赖接口
    /// </summary>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    public interface IModelBuilderFilter<TDbContextLocator1> : IPrivateModelBuilderFilter
        where TDbContextLocator1 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 数据库模型构建筛选器依赖接口
    /// </summary>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    /// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
    public interface IModelBuilderFilter<TDbContextLocator1, TDbContextLocator2> : IPrivateModelBuilderFilter
        where TDbContextLocator1 : class, IDbContextLocator
        where TDbContextLocator2 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 数据库模型构建筛选器依赖接口（禁止外部继承）
    /// </summary>
    public interface IPrivateModelBuilderFilter : IPrivateModelBuilder
    {
        /// <summary>
        /// 模型构建之前
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="entityBuilder">实体构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        void OnCreating(ModelBuilder modelBuilder, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator);

        /// <summary>
        /// 模型构建之后
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="entityBuilder">实体构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        void OnCreated(ModelBuilder modelBuilder, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator) { }
    }
}