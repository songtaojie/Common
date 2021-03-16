using Hx.Sdk.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.EntityFrameworkCore.EntityConfiguration
{
    /// <summary>
    /// 基础的配置
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    public class BaseEntityTypeConfiguration<T, TKeyType> : EntityTypeConfiguration<T, TKeyType>
            where T : BaseEntity<TKeyType>
    {
        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.CreateTime).IsRequired().HasComment("创建时间");
            builder.Property(x => x.CreaterId).HasMaxLength(36).IsRequired().HasComment("创建者id");
            builder.Property(x => x.Creater).HasMaxLength(36).IsRequired().HasComment("创建者姓名");
            builder.Property(x => x.LastModifyTime).IsRequired().HasComment("最后修改时间");
            builder.Property(x => x.LastModifierId).HasMaxLength(36).IsRequired().HasComment("最后修改人的id");
            builder.Property(x => x.LastModifier).HasMaxLength(36).IsRequired().HasComment("最后修改人");
        }
    }
}
