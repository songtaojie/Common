using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Hx.Sdk.ConfigureOptions
{
    /// <summary>
    /// 读取appsettings.json中的配置的类
    /// </summary>
    //[SkipScan]
    public sealed class AppSettings
    {
        /// <summary>
        /// 全局配置选项
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 应用服务
        /// </summary>
        internal static IServiceCollection InternalServices;

        internal static IServiceProvider ServiceProvider => HttpContextLocal.Current()?.RequestServices?? InternalServices.BuildServiceProvider();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置</param>
        internal AppSettings(IConfiguration config)
        {
            Configuration = config;
        }


        /// <summary>
        /// 获取请求生命周期的服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="scoped"></param>
        /// <returns></returns>
        private static TService GetService<TService>(IServiceProvider scoped = default)
            where TService : class
        {
            return (scoped ?? ServiceProvider).GetService<TService>();
        }

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
            return GetService<IOptions<TOptions>>(scoped)?.Value;
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
            return GetService<IOptionsMonitor<TOptions>>(scoped)?.CurrentValue;
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
            return GetService<IOptionsSnapshot<TOptions>>(scoped)?.Value;
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
