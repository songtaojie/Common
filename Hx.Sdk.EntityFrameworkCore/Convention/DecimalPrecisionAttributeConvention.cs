using Hx.Sdk.EntityFrameworkCore.Attributes;
using Hx.Sdk.EntityFrameworkCore.Extensions;
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
    /// decimal标记属性约定
    /// </summary>
    internal class DecimalPrecisionAttributeConvention : PropertyAttributeConventionBase<DecimalPrecisionAttribute>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dependencies"></param>
        public DecimalPrecisionAttributeConvention(ProviderConventionSetBuilderDependencies dependencies) : base(dependencies)
        {

        }
        /// <summary>
        /// 属性添加时
        /// </summary>
        /// <param name="propertyBuilder"></param>
        /// <param name="attribute"></param>
        /// <param name="clrMember"></param>
        /// <param name="context"></param>
        protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, DecimalPrecisionAttribute attribute, MemberInfo clrMember, IConventionContext context)
        {
            //Console.WriteLine("开始ProcessPropertyAdded");
            propertyBuilder.HasPrecision(attribute.Precision, attribute.Scale);
        }
    }

    
}
