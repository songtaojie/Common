using Hx.Sdk.SqlSugar.Repositories;
using Hx.Sdk.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Reflection;
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
        /// <param name="configAction"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, Action<SqlSugarClient,IServiceProvider> buildAction = default)
        {
            // 注册 SqlSugar 客户端
            services.AddSingleton<ISqlSugarClient>(provider =>
            {
                var dbSettingsOptions = new DbSettingsOptions();
                var options = provider.GetService<IOptions<DbSettingsOptions>>();
                if(options != null) dbSettingsOptions = options.Value;
                var dbSettings = dbSettingsOptions.ConnectionConfigs;
                var connectionConfigs = dbSettings.Select(r => r.ToConnectionConfig()).ToList();
                var sqlSugarScope = new SqlSugarScope(connectionConfigs,db =>
                {
                    buildAction?.Invoke(db, provider);
                });
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
        /// <param name="assemblies">实体所在的程序集</param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services,IEnumerable<Assembly> assemblies = default, Action<ISqlSugarClient, DbConnectionConfig, IServiceProvider> buildAction = default)
        {
            if(assemblies != default)  SqlSugarConfigProvider.SetAssemblies(assemblies);
            ConfigureDbConnectionSettings(services);
            // 注册 SqlSugar 客户端
            services.AddSingleton<ISqlSugarClient>(provider =>
            {
                var dbOptions = provider.GetService<IOptions<DbSettingsOptions>>();
                var dbSettings = dbOptions.Value.ConnectionConfigs;
               
                var connectionConfigs = dbSettings.Select(r =>
                {
                    r = SqlSugarConfigProvider.SetDbConfig(r);
                    return r.ToConnectionConfig();
                }).ToList();
                SqlSugarScope sqlSugar = new SqlSugarScope(connectionConfigs, db =>
                {
                    foreach (var config in dbSettings)
                    {
                        SqlSugarScopeProvider dbProvider = db.GetConnectionScope(config.ConfigId);
                        if(config.EnableSqlLog) SqlSugarConfigProvider.SetAopLog(dbProvider);
                        SqlSugarConfigProvider.InitDatabase(dbProvider, config);
                        buildAction?.Invoke(dbProvider,config, provider);
                    }
                });
                return sqlSugar;
            });
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
