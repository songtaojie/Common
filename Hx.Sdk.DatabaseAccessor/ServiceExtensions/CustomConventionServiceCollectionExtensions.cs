using Hx.Sdk.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 自定义约束扩展类
    /// </summary>
    public static class CustomConventionServiceCollectionExtensions
    {
        /// <summary>
        /// 添加自定义约定
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder AddCustomConvention(this DbContextOptionsBuilder optionsBuilder)
        {
            var extension = GetOrCreateExtension(optionsBuilder);
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
            return optionsBuilder;
        }

        private static IDbContextOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
        {
            IDbContextOptionsExtension optionsExtension = optionsBuilder.Options.FindExtension<CustomDbContextOptionsExtension>();
            if (optionsExtension == null)
            {
                return new CustomDbContextOptionsExtension();
            }
            return optionsExtension;
        }

    }
}
