﻿using System;

namespace Hx.Sdk.DependencyInjection.Extensions
{
    /// <summary>
    /// 依赖注入拓展
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// 解析服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="obj"></param>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static TService GetService<TService>(this object obj, IServiceProvider scoped = default)
            where TService : class
        {
            return App.GetService<TService>(scoped);
        }

        /// <summary>
        /// 解析服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="obj"></param>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static TService GetRequiredService<TService>(this object obj, IServiceProvider scoped = default)
            where TService : class
        {
            return App.GetRequiredService<TService>(scoped);
        }
    }
}