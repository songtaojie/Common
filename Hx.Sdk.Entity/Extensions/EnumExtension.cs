﻿using Hx.Sdk.DependencyInjection;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Hx.Sdk.Extensions
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    [SkipScan]
    public static class EnumExtension
    {
        /// <summary>
        ///  获取枚举的中文描述
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum obj)
        {
            string objName = obj.ToString();
            Type t = obj.GetType();
            FieldInfo fi = t.GetField(objName);
            DescriptionAttribute[] arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return arrDesc[0].Description;
        }
    }
}
