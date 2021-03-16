using Hx.Sdk.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.EntityFrameworkCore.EntityConfiguration
{
    /// <summary>
    /// 基础的配置,带状态
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    public class BaseStatusEntityTypeConfiguration<T,TKeyType>: BaseEntityTypeConfiguration<T, TKeyType>
         where T : BaseStatusEntity<TKeyType>
    {
        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Delete).IsRequired().HasColumnType("char(1)")
                .HasDefaultValue(StatusEntityEnum.No).HasComment("是否删除");
            builder.Property(x => x.Disable).IsRequired().HasColumnType("char(1)")
                .HasDefaultValue(StatusEntityEnum.No).HasComment("是否禁用");
        }
    }
}
