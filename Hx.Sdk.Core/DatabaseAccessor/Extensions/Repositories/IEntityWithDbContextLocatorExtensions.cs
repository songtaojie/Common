﻿using Hx.Sdk.DatabaseAccessor;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.Entity;
using Hx.Sdk.Entity.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.Sdk.Extensions
{
    /// <summary>
    /// 实体多数据库上下文拓展类
    /// </summary>
    [SkipScan]
    public static class IEntityWithDbContextLocatorExtensions
    {
        /// <summary>
        /// 获取实体同类（族群）
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <returns>DbSet{TEntity}</returns>
        public static DbSet<TEntity> Ethnics<TEntity, TDbContextLocator>(this TEntity entity)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().Ethnics();
        }

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理的实体</returns>
        public static EntityEntry<TEntity> Insert<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().Insert(ignoreNullValues);
        }

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>代理的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertAsync<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertAsync(ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> InsertNow<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertNow(ignoreNullValues);
        }

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertNowAsync<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertNowAsync(ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> Update<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().Update(ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateAsync<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null)
             where TEntity : class, IPrivateEntity, new()
             where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateAsync(ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> UpdateNow<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateNow(ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateNowAsync<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateNowAsync(ignoreNullValues, cancellationToken);
        }


        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> UpdateInclude<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateInclude(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> UpdateInclude<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateInclude(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> UpdateInclude<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateInclude(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> UpdateInclude<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateInclude(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateIncludeAsync<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeAsync(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateIncludeAsync<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeAsync(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateIncludeAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeAsync(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateIncludeAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeAsync(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> UpdateIncludeNow<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeNow(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中实体</returns>
        public static EntityEntry<TEntity> UpdateIncludeNow<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeNow(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> UpdateIncludeNow<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeNow(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> UpdateIncludeNow<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeNow(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeNowAsync(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeNowAsync(propertyNames, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeNowAsync(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeNowAsync(propertyPredicates, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeNowAsync(propertyNames, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateIncludeNowAsync(propertyPredicates, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> UpdateExclude<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExclude(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> UpdateExclude<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExclude(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> UpdateExclude<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExclude(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> UpdateExclude<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExclude(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateExcludeAsync<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeAsync(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateExcludeAsync<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeAsync(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateExcludeAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeAsync(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateExcludeAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeAsync(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> UpdateExcludeNow<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeNow(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中实体</returns>
        public static EntityEntry<TEntity> UpdateExcludeNow<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeNow(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> UpdateExcludeNow<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeNow(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> UpdateExcludeNow<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeNow(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeNowAsync(propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeNowAsync(propertyNames, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeNowAsync(propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeNowAsync(propertyPredicates, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeNowAsync(propertyNames, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> UpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().UpdateExcludeNowAsync(propertyPredicates, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> Delete<TEntity, TDbContextLocator>(this TEntity entity)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().Delete();
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> DeleteAsync<TEntity, TDbContextLocator>(this TEntity entity)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().DeleteAsync();
        }

        /// <summary>
        /// 删除一条记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> DeleteNow<TEntity, TDbContextLocator>(this TEntity entity)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().DeleteNow();
        }

        /// <summary>
        /// 删除一条记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> DeleteNowAsync<TEntity, TDbContextLocator>(this TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().DeleteNowAsync(cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="checkProperty"></param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdate<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null, Expression<Func<TEntity, object>> checkProperty = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdate(ignoreNullValues, checkProperty);
        }

        /// <summary>
        /// 新增或更新一条记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="checkProperty"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateAsync<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null, Expression<Func<TEntity, object>> checkProperty = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateAsync(ignoreNullValues, checkProperty, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条记录并立即执行
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="checkProperty"></param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateNow<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null, Expression<Func<TEntity, object>> checkProperty = null)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateNow(ignoreNullValues, checkProperty);
        }

        /// <summary>
        /// 新增或更新一条记录并立即执行
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="checkProperty"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateNowAsync<TEntity, TDbContextLocator>(this TEntity entity, bool? ignoreNullValues = null, Expression<Func<TEntity, object>> checkProperty = null, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateNowAsync(ignoreNullValues, checkProperty, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateInclude<TEntity, TDbContextLocator>(this TEntity entity, params string[] propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateInclude(propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateInclude<TEntity, TDbContextLocator>(this TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateInclude(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateInclude<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateInclude(propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateInclude<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateInclude(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync<TEntity, TDbContextLocator>(this TEntity entity, params string[] propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeAsync(propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeAsync(propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync<TEntity, TDbContextLocator>(this TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeAsync(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeAsync(propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeAsync(propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeAsync(propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateIncludeNow<TEntity, TDbContextLocator>(this TEntity entity, params string[] propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeNow(propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateIncludeNow<TEntity, TDbContextLocator>(this TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeNow(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateIncludeNow<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeNow(propertyNames);
        }

        
        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateIncludeNow<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeNow(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, params string[] propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeNowAsync(propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeNowAsync(propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeNowAsync(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeNowAsync(propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeNowAsync(propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateIncludeNowAsync(propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateExclude<TEntity, TDbContextLocator>(this TEntity entity, params string[] propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExclude(propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateExclude<TEntity, TDbContextLocator>(this TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExclude(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateExclude<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExclude(propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateExclude<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExclude(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync<TEntity, TDbContextLocator>(this TEntity entity, params string[] propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeAsync(propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeAsync(propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync<TEntity, TDbContextLocator>(this TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeAsync(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeAsync(propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeAsync(propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeAsync(propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateExcludeNow<TEntity, TDbContextLocator>(this TEntity entity, params string[] propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeNow(propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateExcludeNow<TEntity, TDbContextLocator>(this TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeNow(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateExcludeNow<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeNow(propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public static EntityEntry<TEntity> InsertOrUpdateExcludeNow<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeNow(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, params string[] propertyNames)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeNowAsync(propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, string[] propertyNames, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeNowAsync(propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeNowAsync(propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, Expression<Func<TEntity, object>>[] propertyPredicates, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeNowAsync(propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeNowAsync(propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public static Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync<TEntity, TDbContextLocator>(this TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().InsertOrUpdateExcludeNowAsync(propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static EntityEntry<TEntity> FakeDelete<TEntity, TDbContextLocator>(this TEntity entity)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().FakeDelete();
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static Task<EntityEntry<TEntity>> FakeDeleteAsync<TEntity, TDbContextLocator>(this TEntity entity)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().FakeDeleteAsync();
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static EntityEntry<TEntity> FakeDeleteNow<TEntity, TDbContextLocator>(this TEntity entity)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().FakeDeleteNow();
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns></returns>
        public static Task<EntityEntry<TEntity>> FakeDeleteNowAsync<TEntity, TDbContextLocator>(this TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return new EntityExecutePart<TEntity>().SetEntity(entity).Change<TDbContextLocator>().FakeDeleteNowAsync(cancellationToken);
        }
    }
}