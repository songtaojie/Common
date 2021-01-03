using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.EntityFrameworkCore.Attributes
{
    /// <summary>
    /// 枚举转换器特性，可以通过注解的形势吧枚举转换成int，string，long
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class EnumConverterAttribute : Attribute
    {

        #region Field
        #endregion

        #region Construct
        public EnumConverterAttribute(Type type)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conventionType"></param>
        public EnumConverterAttribute(ValueConverter conventionType)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conventionType"></param>
        //public EnumToObjectAttribute(EnumConventionType conventionType)
        //{
        //    ConventionType = conventionType;
        //}
        #endregion

        #region Property
        /// <summary>
        /// 类型
        /// </summary>
        public EnumConventionType ConventionType { get; set; } = EnumConventionType.EnumToString;

        public ValueConverter ValueConverter { get; set; }

        #endregion
    }

    /// <summary>
    /// 枚举转成的类型
    /// </summary>
    public enum EnumConventionType
    {
        /// <summary>
        /// 枚举转成字符串
        /// </summary>
        EnumToString,
        /// <summary>
        /// 枚举转成整形
        /// </summary>
        EnumToInt,
        /// <summary>
        /// 枚举转成长整型
        /// </summary>
        EnumToLong
    }
}
