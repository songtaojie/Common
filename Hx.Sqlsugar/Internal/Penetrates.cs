using Hx.Sdk.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;
using System.Text;
using System.Text.RegularExpressions;

namespace Hx.Sdk.Sqlsugar.Internal
{
    /// <summary>
    /// 常量、公共方法配置类
    /// </summary>
    internal static class Penetrates
    {
    
        /// <summary>
        /// 应用有效程序集
        /// </summary>
        internal static readonly IEnumerable<Assembly> Assemblies;

        /// <summary>
        /// 有效程序集类型
        /// </summary>
        internal static readonly IEnumerable<Type> EffectiveTypes;
        /// <summary>
        /// 构造函数
        /// </summary>
        static Penetrates()
        {
            Assemblies = GetAssemblies();

            EffectiveTypes = Assemblies.SelectMany(GetTypes);
        }

        /// <summary>
        /// 加载程序集中的所有类型
        /// </summary>
        /// <param name="ass"></param>
        /// <returns></returns>
        private static IEnumerable<Type> GetTypes(Assembly ass)
        {
            var types = Array.Empty<Type>();

            try
            {
                types = ass.GetTypes();
            }
            catch
            {
                Console.WriteLine($"Error load `{ass.FullName}` assembly.");
            }

            return types.Where(u => u.IsPublic && !u.IsDefined(typeof(SkipScanAttribute), false));
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
            var dependencyContext = DependencyContext.Default;

            // 读取项目程序集或 Hx.Sdk 发布的包，或手动添加引用的dll，或配置特定的包前缀
            var scanAssemblies = dependencyContext.CompileLibraries
                .Where(u =>
                       (u.Type == "project" && !excludeAssemblyNames.Any(j => u.Name.EndsWith(j)))
                       || (u.Type == "package" && u.Name.StartsWith("Hx.Sdk")))    // 判断是否启用引用程序集扫描
                .Select(u => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(u.Name)))
                .ToList();

            return scanAssemblies;
        }

        /// <summary>
        /// 获取 Swagger 配置
        /// </summary>
        /// <returns></returns>
        public static SwaggerSettingsOptions GetSwaggerSettings(IConfiguration config)
        {
            var options = config.GetSection("SwaggerSettings").Get<SwaggerSettingsOptions>();
            options = SwaggerSettingsOptions.SetDefaultSwaggerSettings(options);
            return options;
        }
    }
}
