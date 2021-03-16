using Hx.Sdk.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.EntityFrameworkCore.EntityConfiguration
{
    /// <summary>
    /// EntityType配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KeyType"></typeparam>
    public class EntityTypeConfiguration<T, KeyType> : IEntityTypeConfiguration<T>
        where T : class, IEntity<KeyType>
    {
        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="builder"></param>
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            Type genericType = typeof(T);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().HasMaxLength(36).HasComment("主键");
            builder.ToTable(genericType.Name);
        }
    }
}
