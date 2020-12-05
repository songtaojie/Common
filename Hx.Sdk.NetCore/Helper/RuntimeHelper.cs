using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.Extensions.DependencyModel;

namespace Hx.Sdk.NetCore.Helper
{
    /// <summary>
    /// 运行时的帮助类
    /// </summary>
    public class RuntimeHelper
    {
        private static IEnumerable<Assembly> _allAssemblies;
        /// <summary>
        /// 获取所有的程序集
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAllAssembly()
        {
            if (_allAssemblies == null)
            {
                var list = new List<Assembly>();
                var deps = DependencyContext.Default;
                ////排除所有的系统程序集、Nuget下载包//只取项目程序
                var libs = deps.CompileLibraries.Where(lib => lib.Type == "project" && !lib.Serviceable && lib.Type != "package");
                foreach (var lib in libs)
                {
                    try
                    {
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                        list.Add(assembly);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                _allAssemblies = list;
            }

            return _allAssemblies;
        }

        /// <summary>
        /// 获取指定程序集名称的程序集
        /// </summary>
        /// <param name="assemblyNames">程序集的全名</param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssembly(params string[] assemblyNames)
        {
            var allAssemblies = GetAllAssembly();
            var result = new List<Assembly>();
            foreach (var assemblyName in assemblyNames)
            {
                var assembly = allAssemblies.FirstOrDefault(assembly => assembly.FullName.Contains(assemblyName));
                if (assembly != null) result.Add(assembly);
            }
            return result;
        }
    }
}
