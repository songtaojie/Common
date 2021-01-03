using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Hx.Sdk.EntityFrameworkCore
{
    public static class CustomConventionExtension
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
