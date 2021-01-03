using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.EntityFrameworkCore.Extensions
{
    /// <summary>
    /// IConventionPropertyBuilder扩展类
    /// </summary>
    internal static class IConventionPropertyBuilderExtensions
    {
        public static IConventionPropertyBuilder HasPrecision(this IConventionPropertyBuilder propertyBuilder, int precision, int scale)
        {
            propertyBuilder.HasColumnType($"decimal({precision},{scale})");
            return propertyBuilder;
        }
    }

}
