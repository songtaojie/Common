using Hx.Sdk.Sqlsugar.Repositories;
using Hx.Sdk.Sqlsugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.DirectoryServices.ActiveDirectory;
using System.Reflection;
using System.Collections;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Hx.Sdk.Sqlsugar.Internal;
using Hx.Sdk.Extensions;
using System.Runtime;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// SqlSugar 拓展类
    /// </summary>
    public static class SqlSugarServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 SqlSugar 拓展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionConfig"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, ConnectionConfig connectionConfig, Action<ISqlSugarClient> buildAction = default)
        {
            return services.AddSqlSugar(new ConnectionConfig[] { connectionConfig }, buildAction);
        }

        /// <summary>
        /// 添加 SqlSugar 拓展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configs"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, ConnectionConfig[] configs, Action<ISqlSugarClient> buildAction = default)
        {
            // 注册 SqlSugar 客户端
            services.AddSingleton<ISqlSugarClient>(provider =>
            {
                var sqlSugarScope = new SqlSugarScope(configs.ToList());
                buildAction?.Invoke(sqlSugarScope);

                return sqlSugarScope;
            });

            // 注册非泛型仓储
            services.AddScoped<ISqlSugarRepository, SqlSugarRepository>();

            // 注册 SqlSugar 仓储
            services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));

            return services;
        }

        /// <summary>
        /// 添加 SqlSugar 拓展,
        /// 使用配置文件配置，
        /// </summary>
        /// <param name="services"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, Action<ConnectionConfig> configAction = default,  Action<ISqlSugarClient> buildAction = default)
        {
            ConfigureDbConnectionSettings(services);
            // 注册 SqlSugar 客户端
            services.AddSingleton<ISqlSugarClient>(provider =>
            {
                var dbOptions = provider.GetService<IOptionsMonitor<DbSettingsOptions>>();
                var dbSettings = dbOptions.CurrentValue.ConnectionConfigs;
                var cacheService =  provider.GetService<ICacheService>();
                var logger = provider.GetService<ILogger<ISqlSugarClient>>();
                foreach (var config in dbSettings)
                {
                    Penetrates.SetDbConfig(config, cacheService);
                    configAction?.Invoke(config);
                }
                var connectionConfigs = dbSettings.Select(r => r.ToConnectionConfig()).ToList();
                SqlSugarScope sqlSugar = new SqlSugarScope(connectionConfigs, db =>
                {
                    foreach (var config in dbSettings)
                    {
                        var dbProvider = db.GetConnectionScope(config.ConfigId);
                        Penetrates.SetDbAop(config,dbProvider, logger);
                        Penetrates.SetDbDiffLog(dbProvider, config, logger);
                    }
                    buildAction?.Invoke(db);
                });
                return sqlSugar;
            });
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, SqlSugarCache>();
            // 注册非泛型仓储
            services.AddScoped<ISqlSugarRepository, SqlSugarRepository>();

            // 注册 SqlSugar 仓储
            services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));

            return services;
        }
      

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureDbConnectionSettings(IServiceCollection services)
        {
            services.AddOptions<DbSettingsOptions>()
                   .BindConfiguration("DbSettings", options =>
                   {
                       options.BindNonPublicProperties = true; // 绑定私有变量
                   })
                   .PostConfigure(options =>
                   {
                       _ = options.SetDefaultSettings(options);
                   });
        }
    }
}
