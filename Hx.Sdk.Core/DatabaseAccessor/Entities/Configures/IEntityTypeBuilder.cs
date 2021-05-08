﻿using Hx.Sdk.DatabaseAccessor.Internal;
using Hx.Sdk.Entity;
using Hx.Sdk.Entity.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 数据库实体类型配置依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IEntityTypeBuilder<TEntity> : IEntityTypeBuilder<TEntity, MasterDbContextLocator>
        where TEntity : class, IPrivateEntity, new()
    {
    }

    /// <summary>
    /// 数据库实体类型配置依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    public interface IEntityTypeBuilder<TEntity, TDbContextLocator1> : IPrivateEntityTypeBuilder<TEntity>
        where TEntity : class, IPrivateEntity, new()
        where TDbContextLocator1 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 数据库实体类型配置依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    /// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
    public interface IEntityTypeBuilder<TEntity, TDbContextLocator1, TDbContextLocator2> : IPrivateEntityTypeBuilder<TEntity>
        where TEntity : class, IPrivateEntity, new()
        where TDbContextLocator1 : class, IDbContextLocator
        where TDbContextLocator2 : class, IDbContextLocator
    {
    }
}