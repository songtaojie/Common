using Hx.Sdk.EntityFrameworkCore.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Hx.Sdk.EntityFrameworkCore.Convention
{

    /// <summary>
    /// 值值转换器的属性约定
    /// </summary>
    internal class ValueConverterAttributeConvention : PropertyAttributeConventionBase<ValueConverterAttribute>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencies"></param>
        public ValueConverterAttributeConvention(ProviderConventionSetBuilderDependencies dependencies) : base(dependencies)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyBuilder"></param>
        /// <param name="attribute"></param>
        /// <param name="clrMember"></param>
        /// <param name="context"></param>
        protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, ValueConverterAttribute attribute, MemberInfo clrMember, IConventionContext context)
        {
            if (attribute.ValueConverter != null)
            {
                propertyBuilder.HasConversion(attribute.ValueConverter, true);
            }
        }
    }
}
