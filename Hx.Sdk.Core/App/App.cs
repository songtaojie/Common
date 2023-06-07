using Hx.Sdk.Core.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// 全局应用类
    /// </summary>
    [SkipScan]
    public static partial class App
    {

        /// <summary>
        /// 获取Web主机环境，如，是否是开发环境，生产环境等
        /// </summary>
        public static IWebHostEnvironment WebHostEnvironment => InternalApp.WebHostEnvironment;

        /// <summary>
        /// 获取泛型主机环境，如，是否是开发环境，生产环境等
        /// </summary>
        public static IHostEnvironment HostEnvironment => InternalApp.HostEnvironment;

        /// <summary>
        /// 应用有效程序集
        /// </summary>
        public static readonly IEnumerable<Assembly> Assemblies;

        /// <summary>
        /// 有效程序集类型
        /// </summary>
        public static readonly IEnumerable<Type> EffectiveTypes;

        /// <summary>
        /// 服务提供器
        /// </summary>
        public static IServiceProvider ServiceProvider => InternalApp.ServiceProvider;

        /// <summary>
        /// 上下文
        /// </summary>
        public static HttpContext HttpContext => ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext;

        /// <summary>
        /// 私有设置，避免重复解析
        /// </summary>
        private static AppSettingsOptions _settings;

        /// <summary>
        /// 应用全局配置
        /// </summary>
        public static AppSettingsOptions Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = AppSettings.GetConfig<AppSettingsOptions>("AppSettings") ?? new AppSettingsOptions();
                    _settings.PostConfigure("", _settings);
                }
                return _settings;
            }
        }

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
        /// 构造函数
        /// </summary>
        static App()
        {
            Assemblies = GetAssemblies();
            EffectiveTypes = Assemblies.SelectMany(u => u.GetTypes()
                .Where(u => u.IsPublic && !u.IsDefined(typeof(Attributes.SkipScanAttribute), false)));
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

            var dependencyContext = DependencyContext.Default;

            // 读取项目程序集或 Hx.Sdk 发布的包，或手动添加引用的dll，或配置特定的包前缀
            var scanAssemblies = dependencyContext.CompileLibraries
                .Where(u =>
                       (u.Type == "project" && !excludeAssemblyNames.Any(j => u.Name.EndsWith(j)))
                       || (u.Type == "package" && u.Name.StartsWith("Hx.Sdk")))   
                .Select(u => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(u.Name)))
                .ToList();

            return scanAssemblies;
        }
    }
}
