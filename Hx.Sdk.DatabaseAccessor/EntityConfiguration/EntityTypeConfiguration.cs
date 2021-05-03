using Hx.Sdk.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.DatabaseAccessor.EntityConfiguration
{
    /// <summary>
    /// EntityType配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KeyType"></typeparam>
    public class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
        where T : class, Hx.Sdk.Entity.IEntity
    {
        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="builder"></param>
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
        }
    }
}
