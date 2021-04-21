﻿using Hx.Sdk.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.Sdk.DatabaseAccessor
{
    /// <summary>
    /// 实体执行组件
    /// </summary>
    public sealed partial class EntityExecutePart<TEntity>
        where TEntity : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 获取实体同类（族群）
        /// </summary>
        /// <returns>DbSet{TEntity}</returns>
        public DbSet<TEntity> Ethnics()
        {
            return GetRepository().Entities;
        }

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理的实体</returns>
        public EntityEntry<TEntity> Insert(bool? ignoreNullValues = null)
        {
            return GetRepository().Insert(Entity, ignoreNullValues);
        }

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>代理的实体</returns>
        public Task<EntityEntry<TEntity>> InsertAsync(bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertAsync(Entity, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertNow(bool? ignoreNullValues = null)
        {
            return GetRepository().InsertNow(Entity, ignoreNullValues);
        }

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">接受所有提交更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertNow(bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null)
        {
            return GetRepository().InsertNow(Entity, acceptAllChangesOnSuccess, ignoreNullValues);
        }

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertNowAsync(bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertNowAsync(Entity, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">接受所有提交更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertNowAsync(bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertNowAsync(Entity, acceptAllChangesOnSuccess, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> Update(bool? ignoreNullValues = null)
        {
            return GetRepository().Update(Entity, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateAsync(bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateAsync(Entity, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateNow(bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateNow(Entity, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateNow(bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateNow(Entity, acceptAllChangesOnSuccess, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateNowAsync(bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateNowAsync(Entity, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateNowAsync(bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateNowAsync(Entity, acceptAllChangesOnSuccess, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateInclude(string[] propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateInclude(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateInclude(Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateInclude(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateInclude(IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateInclude(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateInclude(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateInclude(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeAsync(string[] propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeAsync(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeAsync(Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeAsync(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="propertyNames">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeAsync(IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeAsync(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeAsync(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateIncludeNow(string[] propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeNow(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateIncludeNow(string[] propertyNames, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeNow(Entity, propertyNames, acceptAllChangesOnSuccess, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中实体</returns>
        public EntityEntry<TEntity> UpdateIncludeNow(Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeNow(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateIncludeNow(Expression<Func<TEntity, object>>[] propertyPredicates, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeNow(Entity, propertyPredicates, acceptAllChangesOnSuccess, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateIncludeNow(IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeNow(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateIncludeNow(IEnumerable<string> propertyNames, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeNow(Entity, propertyNames, acceptAllChangesOnSuccess, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateIncludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeNow(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateIncludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeNow(Entity, propertyPredicates, acceptAllChangesOnSuccess, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(string[] propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(string[] propertyNames, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyNames, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(string[] propertyNames, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyNames, acceptAllChangesOnSuccess, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyPredicates, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(Expression<Func<TEntity, object>>[] propertyPredicates, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyPredicates, acceptAllChangesOnSuccess, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(IEnumerable<string> propertyNames, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyNames, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(IEnumerable<string> propertyNames, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyNames, acceptAllChangesOnSuccess, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyPredicates, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyPredicates, acceptAllChangesOnSuccess, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateExclude(string[] propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExclude(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateExclude(Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExclude(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateExclude(IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExclude(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateExclude(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExclude(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeAsync(string[] propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeAsync(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeAsync(Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeAsync(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="propertyNames">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeAsync(IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeAsync(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeAsync(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateExcludeNow(string[] propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeNow(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateExcludeNow(string[] propertyNames, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeNow(Entity, propertyNames, acceptAllChangesOnSuccess, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中实体</returns>
        public EntityEntry<TEntity> UpdateExcludeNow(Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeNow(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateExcludeNow(Expression<Func<TEntity, object>>[] propertyPredicates, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeNow(Entity, propertyPredicates, acceptAllChangesOnSuccess, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateExcludeNow(IEnumerable<string> propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeNow(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateExcludeNow(IEnumerable<string> propertyNames, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeNow(Entity, propertyNames, acceptAllChangesOnSuccess, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateExcludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeNow(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateExcludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeNow(Entity, propertyPredicates, acceptAllChangesOnSuccess, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(string[] propertyNames, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyNames, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(string[] propertyNames, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyNames, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(string[] propertyNames, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyNames, acceptAllChangesOnSuccess, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyPredicates, ignoreNullValues);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(Expression<Func<TEntity, object>>[] propertyPredicates, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyPredicates, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(Expression<Func<TEntity, object>>[] propertyPredicates, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyPredicates, acceptAllChangesOnSuccess, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(IEnumerable<string> propertyNames, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyNames, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(IEnumerable<string> propertyNames, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyNames, acceptAllChangesOnSuccess, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyPredicates, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyPredicates, acceptAllChangesOnSuccess, ignoreNullValues, cancellationToken);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> Delete()
        {
            return GetRepository().Delete(Entity);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> DeleteAsync()
        {
            return GetRepository().DeleteAsync(Entity);
        }

        /// <summary>
        /// 删除一条记录并立即提交
        /// </summary>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> DeleteNow()
        {
            return GetRepository().DeleteNow(Entity);
        }

        /// <summary>
        /// 删除一条记录并立即提交
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <returns></returns>
        public EntityEntry<TEntity> DeleteNow(bool acceptAllChangesOnSuccess)
        {
            return GetRepository().DeleteNow(Entity, acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 删除一条记录并立即提交
        /// </summary>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> DeleteNowAsync(CancellationToken cancellationToken = default)
        {
            return GetRepository().DeleteNowAsync(Entity, cancellationToken);
        }

        /// <summary>
        /// 删除一条记录并立即提交
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> DeleteNowAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return GetRepository().DeleteNowAsync(Entity, acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条记录
        /// </summary>
        /// <param name="ignoreNullValues"></param>
        /// <param name="checkProperty"></param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdate(bool? ignoreNullValues = null, Expression<Func<TEntity, object>> checkProperty = null)
        {
            return GetRepository().InsertOrUpdate(Entity, ignoreNullValues, checkProperty);
        }

        /// <summary>
        /// 新增或更新一条记录
        /// </summary>
        /// <param name="ignoreNullValues"></param>
        /// <param name="checkProperty"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateAsync(bool? ignoreNullValues = null, Expression<Func<TEntity, object>> checkProperty = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateAsync(Entity, ignoreNullValues, checkProperty, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条记录并立即执行
        /// </summary>
        /// <param name="ignoreNullValues"></param>
        /// <param name="checkProperty"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateNow(bool? ignoreNullValues = null, Expression<Func<TEntity, object>> checkProperty = null)
        {
            return GetRepository().InsertOrUpdateNow(Entity, ignoreNullValues, checkProperty);
        }

        /// <summary>
        /// 新增或更新一条记录并立即执行
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="checkProperty"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateNow(bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, Expression<Func<TEntity, object>> checkProperty = null)
        {
            return GetRepository().InsertOrUpdateNow(Entity, acceptAllChangesOnSuccess, ignoreNullValues, checkProperty);
        }

        /// <summary>
        /// 新增或更新一条记录并立即执行
        /// </summary>
        /// <param name="ignoreNullValues"></param>
        /// <param name="checkProperty"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateNowAsync(bool? ignoreNullValues = null, Expression<Func<TEntity, object>> checkProperty = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateNowAsync(Entity, ignoreNullValues, checkProperty, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条记录并立即执行
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="ignoreNullValues"></param>
        /// <param name="checkProperty"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateNowAsync(bool acceptAllChangesOnSuccess, bool? ignoreNullValues = null, Expression<Func<TEntity, object>> checkProperty = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateNowAsync(Entity, acceptAllChangesOnSuccess, ignoreNullValues, checkProperty, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateInclude(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateInclude(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateInclude(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateInclude(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateInclude(IEnumerable<string> propertyNames)
        {
            return GetRepository().InsertOrUpdateInclude(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateInclude(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().InsertOrUpdateInclude(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateIncludeAsync(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(string[] propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateIncludeAsync(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(Expression<Func<TEntity, object>>[] propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(string[] propertyNames, bool acceptAllChangesOnSuccess)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyNames, acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(Expression<Func<TEntity, object>>[] propertyPredicates, bool acceptAllChangesOnSuccess)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyPredicates, acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(IEnumerable<string> propertyNames)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(IEnumerable<string> propertyNames, bool acceptAllChangesOnSuccess)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyNames, acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool acceptAllChangesOnSuccess)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyPredicates, acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(string[] propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(string[] propertyNames, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyNames, acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(Expression<Func<TEntity, object>>[] propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(Expression<Func<TEntity, object>>[] propertyPredicates, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyPredicates, acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(IEnumerable<string> propertyNames, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyNames, acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyPredicates, acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExclude(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateExclude(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExclude(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExclude(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExclude(IEnumerable<string> propertyNames)
        {
            return GetRepository().InsertOrUpdateExclude(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExclude(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExclude(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateExcludeAsync(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(string[] propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExcludeAsync(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(Expression<Func<TEntity, object>>[] propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(string[] propertyNames, bool acceptAllChangesOnSuccess)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyNames, acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(Expression<Func<TEntity, object>>[] propertyPredicates, bool acceptAllChangesOnSuccess)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyPredicates, acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(IEnumerable<string> propertyNames)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(IEnumerable<string> propertyNames, bool acceptAllChangesOnSuccess)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyNames, acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool acceptAllChangesOnSuccess)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyPredicates, acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(string[] propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(string[] propertyNames, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyNames, acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(Expression<Func<TEntity, object>>[] propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(Expression<Func<TEntity, object>>[] propertyPredicates, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyPredicates, acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(IEnumerable<string> propertyNames, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyNames, acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyPredicates, acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <returns></returns>
        public EntityEntry<TEntity> FakeDelete()
        {
            return GetRepository().FakeDelete(Entity);
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <returns></returns>
        public Task<EntityEntry<TEntity>> FakeDeleteAsync()
        {
            return GetRepository().FakeDeleteAsync(Entity);
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <returns></returns>
        public EntityEntry<TEntity> FakeDeleteNow()
        {
            return GetRepository().FakeDeleteNow(Entity);
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <returns></returns>
        public EntityEntry<TEntity> FakeDeleteNow(bool acceptAllChangesOnSuccess)
        {
            return GetRepository().FakeDeleteNow(Entity, acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns></returns>
        public Task<EntityEntry<TEntity>> FakeDeleteNowAsync(CancellationToken cancellationToken = default)
        {
            return GetRepository().FakeDeleteNowAsync(Entity, cancellationToken);
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">接受所有更改</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns></returns>
        public Task<EntityEntry<TEntity>> FakeDeleteNowAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return GetRepository().FakeDeleteNowAsync(Entity, acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// 获取实体仓储
        /// </summary>
        /// <returns></returns>
        private IPrivateRepository<TEntity> GetRepository()
        {
            return App.GetService(typeof(IRepository<,>).MakeGenericType(typeof(TEntity), DbContextLocator), ContextScoped) as IPrivateRepository<TEntity>;
        }
    }
}