﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Common.Extensions
{
    /// <summary>
    /// object扩展类
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 对象tostring方法，对象为空返回空字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ObjToString(this object value)
        {
            if (value != null) return value.ToString().Trim();
            return string.Empty;
        }
    }
}
