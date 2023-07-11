using Hx.Sdk.Sqlsugar.Repositories;
using Hx.Sdk.Sqlsugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime;
using Microsoft.Extensions.Configuration;

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
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, IConfiguration configuration, IEnumerable<Assembly> assemblies = default, Action<SqlSugarScope> buildAction = default)
        {
            if(assemblies != default)  SqlSugarConfigProvider.SetAssemblies(assemblies);
            var obj = configuration.GetSection("DbSettings:ConnectionConfigs");
            var dbOptions = configuration.GetValue("DbSettings", DbSettingsOptions.GetDefaultSettings());
            foreach (var r in dbOptions.ConnectionConfigs)
            {
                SqlSugarConfigProvider.SetDbConfig(r);
            }
            var sqlSugar = new SqlSugarScope(dbOptions.ConnectionConfigs.Select(r=>r.ToConnectionConfig()).ToList());
            buildAction?.Invoke(sqlSugar);
            foreach (var config in dbOptions.ConnectionConfigs)
            {
                SqlSugarScopeProvider dbProvider = sqlSugar.GetConnectionScope(config.ConfigId);
                if (config.EnableSqlLog) SqlSugarConfigProvider.SetAopLog(dbProvider);
                SqlSugarConfigProvider.InitDatabase(dbProvider, config);
            }
            // 注册 SqlSugar 客户端
            services.AddSingleton<ISqlSugarClient>(sqlSugar);
           
            // 注册非泛型仓储
            services.AddScoped<ISqlSugarRepository, SqlSugarRepository>();

            // 注册 SqlSugar 仓储
            services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));
            return services;
        }
    }
}
