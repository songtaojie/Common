using Hx.Sdk.Core.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// 读取appsettings.json中的配置的类
    /// </summary>
    [Attributes.SkipScan]
    public static class AppSettings
    {
        /// <summary>
        /// 全局配置选项
        /// </summary>
        public static IConfiguration Configuration => InternalApp.Configuration;
        
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="name">ConnectionStrings节点中子节点名字</param>
        /// <returns></returns>
        public static string GetConnectionString(string name)
        {
            return Configuration.GetConnectionString(name);
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <param name="jsonKey">配置中对应的Key</param>
        /// <returns>TOptions</returns>
        public static TOptions GetConfig<TOptions>(string jsonKey)
        {
            return Configuration.GetSection(jsonKey).Get<TOptions>();
        }

        /// <summary>
        /// 获取选项
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <param name="scoped"></param>
        /// <returns>TOptions</returns>
        public static TOptions GetOptions<TOptions>(IServiceProvider scoped = default)
            where TOptions : class, new()
        {
            return App.GetService<IOptions<TOptions>>(scoped)?.Value;
        }

        /// <summary>
        /// 获取选项
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <param name="scoped"></param>
        /// <returns>TOptions</returns>
        public static TOptions GetOptionsMonitor<TOptions>(IServiceProvider scoped = default)
            where TOptions : class, new()
        {
            return App.GetService<IOptionsMonitor<TOptions>>(scoped)?.CurrentValue;
        }

        /// <summary>
        /// 获取选项
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <param name="scoped"></param>
        /// <returns>TOptions</returns>
        public static TOptions GetOptionsSnapshot<TOptions>(IServiceProvider scoped = default)
            where TOptions : class, new()
        {
            return App.GetService<IOptionsSnapshot<TOptions>>(scoped)?.Value;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">配置节点</param>
        /// <returns></returns>
        public static string GetConfig(params string[] sections)
        {
            try
            {
                if (sections == null || sections.Length == 0) throw new ArgumentNullException(nameof(sections));
                if (sections.Length == 1)
                {
                    return Configuration[sections[0]];
                }
                else
                {
                    string section = string.Join(':', sections);
                    return Configuration[section];
                }
            }
            catch
            {
                return "";
            }
        }
    }

}
