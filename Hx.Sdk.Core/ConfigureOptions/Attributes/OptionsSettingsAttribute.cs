﻿using System;

namespace Hx.Sdk.ConfigurableOptions
{
    /// <summary>
    /// 选项配置特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OptionsSettingsAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OptionsSettingsAttribute()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jsonKey">appsetting.json 对应键</param>
        public OptionsSettingsAttribute(string jsonKey)
        {
            JsonKey = jsonKey;
        }

        /// <summary>
        /// 对应配置文件中的Key
        /// </summary>
        public string JsonKey { get; set; }
    }
}