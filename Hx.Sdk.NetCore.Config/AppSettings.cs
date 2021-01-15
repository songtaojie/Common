using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using System;

namespace Hx.Sdk.NetCore.Config
{
    /// <summary>
    /// 读取appsettings.json中的配置的类
    /// </summary>
    public class AppSettings
    {
        static IConfiguration Configuration { get; set; }
        static string ContentPath { get; set; }
        /// <summary>
        /// appsettings.json
        /// </summary>
        static readonly string Path = "appsettings.json";
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="env">环境对象</param>
        internal AppSettings(IHostEnvironment env)
        {
            ContentPath = env.ContentRootPath;
            //可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
            Configuration = new ConfigurationBuilder()
                .SetBasePath(ContentPath)
                .AddJsonFile(Path,true,true)
                .AddJsonFile(option => 
                {
                    option.Path = $"appsettings.{env.EnvironmentName}.json";
                    option.ReloadOnChange = true;
                    option.Optional = true;
                })
                .Build();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置</param>
        internal AppSettings(IConfiguration config)
        {
            Configuration = config;
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
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">配置节点</param>
        /// <returns></returns>
        public static string Get(params string[] sections)
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

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">要映射的类</typeparam>
        /// <param name="section">配置节点</param>
        /// <returns></returns>
        public static T Get<T>(string section)
            where T : class, new()
        {
            T config = new T();
            try
            {
                config = Configuration.GetSection(section).Get<T>();
            }
            catch
            {
            }
            return config;
        }
    }
}
