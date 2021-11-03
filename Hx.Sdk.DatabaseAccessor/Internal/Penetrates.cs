using Microsoft.Extensions.DependencyInjection;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.Sdk.DatabaseAccessor.Internal
{
    /// <summary>
    /// 常量、公共方法配置类
    /// </summary>
    internal static class Penetrates
    {
        /// <summary>
        /// 应用服务
        /// </summary>
        internal static IServiceCollection InternalServices;

        /// <summary>
        /// 有效程序集类型
        /// </summary>
        private static IServiceProvider _serviceProvider;

        /// <summary>
        /// 服务提供器
        /// </summary>
        internal static IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider ?? InternalServices.BuildServiceProvider();
            }
            set
            {
                _serviceProvider = value;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        static Penetrates()
        {
        }

        /// <summary>
        /// 打印验证信息到 MiniProfiler
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="message">消息</param>
        /// <param name="isError">是否为警告消息</param>
        public static void PrintToMiniProfiler(string category, string state, string message = null, bool isError = false)
        {
            // 判断是否启用了 MiniProfiler 组件
            if (App.Settings.EnabledMiniProfiler != true) return;

            // 打印消息
            string titleCaseategory = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(category);
            var customTiming = MiniProfiler.Current.CustomTiming(category, string.IsNullOrWhiteSpace(message) ? $"{titleCaseategory} {state}" : message, state);
            if (customTiming == null) return;

            // 判断是否是警告消息
            if (isError) customTiming.Errored = true;
        }
    }
}
