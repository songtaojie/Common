﻿using Hx.Sdk.Extensions;
using Hx.Sdk.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Security.Claims;
using System.Text;

namespace Hx.Sdk.Core
{
    public class App2
    {

        /// <summary>
        /// 私有设置，避免重复解析
        /// </summary>
        private static AppSettingsOptions _settings;

        /// <summary>
        /// 应用全局配置
        /// </summary>
        public static AppSettingsOptions Settings => _settings ??= GetOptions<AppSettingsOptions>();

        /// <summary>
        /// 全局配置选项
        /// </summary>
        public static IConfiguration Configuration;

        /// <summary>
        /// 获取Web主机环境，如，是否是开发环境，生产环境等
        /// </summary>
        public static IWebHostEnvironment WebHostEnvironment => InternalApp.WebHostEnvironment;

        /// <summary>
        /// 获取泛型主机环境，如，是否是开发环境，生产环境等
        /// </summary>
        public static IHostEnvironment HostEnvironment;


        /// <summary>
        /// 构造函数
        /// </summary>
        internal App2(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            // 编译配置
            Configuration = configuration;
            HostEnvironment = hostEnvironment;

            Assemblies = GetAssemblies();
            EffectiveTypes = Assemblies.SelectMany(u => u.GetTypes()
                .Where(u => u.IsPublic && !u.IsDefined(typeof(SkipScanAttribute), false)));
        }


        /// <summary>
        /// 应用有效程序集
        /// </summary>
        public static  IEnumerable<Assembly> Assemblies;

        /// <summary>
        /// 有效程序集类型
        /// </summary>
        public static  IEnumerable<Type> EffectiveTypes;

        /// <summary>
        /// 服务提供器
        /// </summary>
        public static IServiceProvider ServiceProvider => HttpContext?.RequestServices ?? InternalApp.InternalServices.BuildServiceProvider();

        /// <summary>
        /// 获取请求上下文
        /// </summary>
        public static HttpContext HttpContext => HttpContextLocal.Current();

        /// <summary>
        /// 获取请求上下文用户
        /// </summary>
        public static ClaimsPrincipal User => HttpContext?.User;

        /// <summary>
        /// 获取请求生命周期的服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static TService GetService<TService>(IServiceProvider scoped = default)
            where TService : class
        {
            return GetService(typeof(TService), scoped) as TService;
        }

        /// <summary>
        /// 获取请求生命周期的服务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static object GetService(Type type, IServiceProvider scoped = default)
        {
            return (scoped ?? ServiceProvider).GetService(type);
        }

        /// <summary>
        /// 获取请求生命周期的服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static TService GetRequiredService<TService>(IServiceProvider scoped = default)
            where TService : class
        {
            return GetRequiredService(typeof(TService), scoped) as TService;
        }

        /// <summary>
        /// 获取请求生命周期的服务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static object GetRequiredService(Type type, IServiceProvider scoped = default)
        {
            return (scoped ?? ServiceProvider).GetRequiredService(type);
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
        /// 获取应用有效程序集
        /// </summary>
        /// <returns>IEnumerable</returns>
        private static IEnumerable<Assembly> GetAssemblies()
        {
            // 需排除的程序集后缀
            var excludeAssemblyNames = new string[] {
                "Database.Migrations"
            };

            // 读取应用配置
            var settings = GetConfig<AppSettingsOptions>("AppSettings") ?? new AppSettingsOptions { };
            var supportPackageNamePrefixs = settings.SupportPackageNamePrefixs ?? Array.Empty<string>(); ;

            var dependencyContext = DependencyContext.Default;

            // 读取项目程序集或 Hx.Sdk 发布的包，或手动添加引用的dll，或配置特定的包前缀
            var scanAssemblies = dependencyContext.CompileLibraries
                .Where(u =>
                       (u.Type == "project" && !excludeAssemblyNames.Any(j => u.Name.EndsWith(j))) ||
                       (u.Type == "package" && (u.Name.StartsWith(nameof(Hx.Sdk)) || supportPackageNamePrefixs.Any(p => u.Name.StartsWith(p)))) ||
                       (settings.EnabledReferenceAssemblyScan == true && (u.Type == "reference" || u.Type == "referenceassembly")))    // 判断是否启用引用程序集扫描
                .Select(u => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(u.Name)))
                .ToList();

            return scanAssemblies;
        }
    }
}
