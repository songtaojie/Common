using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using StackExchange.Profiling;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Hx.Sdk.ConfigureOptions
{
    /// <summary>
    /// 全局应用类
    /// </summary>
    public class App
    {
        /// <summary>
        /// 全局配置选项
        /// </summary>
        public static IConfiguration Configuration;

        /// <summary>
        /// 获取泛型主机环境，如，是否是开发环境，生产环境等
        /// </summary>
        public static IHostEnvironment HostEnvironment;

        /// <summary>
        /// 应用有效程序集
        /// </summary>
        public static  IEnumerable<Assembly> Assemblies;

        /// <summary>
        /// 有效程序集类型
        /// </summary>
        public static  IEnumerable<Type> EffectiveTypes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        /// <param name="config"></param>
        internal App(IHostEnvironment env, IConfiguration config)
        {
            Configuration = config;
            HostEnvironment = env;
            Assemblies = GetAssemblies();
            EffectiveTypes = Assemblies.SelectMany(u => u.GetTypes()
                .Where(u => u.IsPublic && !u.IsDefined(typeof(SkipScanAttribute), false)));
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        static App()
        {
            // 编译配置
            Configuration = InternalApp.ConfigurationBuilder.Build();

           
        }

        /// <summary>
        /// 私有设置，避免重复解析
        /// </summary>
        private static AppSettingsOptions _settings;

        /// <summary>
        /// 应用全局配置
        /// </summary>
        public static AppSettingsOptions Settings => _settings ??= GetOptions<AppSettingsOptions>();

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
        /// 打印验证信息到 MiniProfiler
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="message">消息</param>
        /// <param name="isError">是否为警告消息</param>
        public static void PrintToMiniProfiler(string category, string state, string message = null, bool isError = false)
        {
            // 判断是否注入 MiniProfiler 组件
            if (Settings.InjectMiniProfiler != true) return;

            // 打印消息
            var customTiming = MiniProfiler.Current.CustomTiming(category, string.IsNullOrWhiteSpace(message) ? $"{category.ToTitleCase()} {state}" : message, state);
            if (customTiming == null) return;

            // 判断是否是警告消息
            if (isError) customTiming.Errored = true;
        }

        /// <summary>
        /// 保存文件夹的监听
        /// </summary>
        private static readonly ConcurrentDictionary<string, IFileProvider> FileProviders =
            new ConcurrentDictionary<string, IFileProvider>();

        /// <summary>
        /// 插件上下文的弱引用
        /// </summary>
        private static readonly ConcurrentDictionary<string, WeakReference> PLCReferences =
            new ConcurrentDictionary<string, WeakReference>();

        /// <summary>
        /// 获取应用有效程序集
        /// </summary>
        /// <returns>IEnumerable</returns>
        private static IEnumerable<Assembly> GetAssemblies()
        {
            // 读取应用配置
            var settings = GetConfig<AppSettingsOptions>("AppSettings") ?? new AppSettingsOptions { };
            var supportPackageNamePrefixs = settings.SupportPackageNamePrefixs ?? Array.Empty<string>(); ;

            var dependencyContext = DependencyContext.Default;

            // 读取项目程序集或 Furion 官方发布的包，或手动添加引用的dll，或配置特定的包前缀
            var scanAssemblies = dependencyContext.CompileLibraries
                .Where(u =>
                       (u.Type == "project") ||
                       (u.Type == "package" && (u.Name.StartsWith(nameof(Hx.Sdk)) || supportPackageNamePrefixs.Any(p => u.Name.StartsWith(p)))) ||
                       (settings.EnabledReferenceAssemblyScan == true && (u.Type == "reference" || u.Type == "referenceassembly")))    // 判断是否启用引用程序集扫描
                .Select(u => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(u.Name)))
                .ToList();
            return scanAssemblies;
        }

    }
}
