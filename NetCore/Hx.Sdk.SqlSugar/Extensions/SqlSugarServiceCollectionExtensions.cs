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
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, Action<DbSettingsOptions> configAction = default,Action<SqlSugarScope> buildAction = default)
        {
            var dbSettingsOptions = new DbSettingsOptions();
            configAction?.Invoke(dbSettingsOptions);
            var dbSettings = dbSettingsOptions.ConnectionConfigs;
            var connectionConfigs = dbSettings.Select(r => r.ToConnectionConfig()).ToList();
            var sqlSugarScope = new SqlSugarScope(connectionConfigs);
            buildAction?.Invoke(sqlSugarScope);
            // 注册 SqlSugar 客户端
            services.AddSingleton<ISqlSugarClient>(sqlSugarScope);

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
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, IConfiguration configuration, Action<SqlSugarScope> buildAction = default)
        {
            var dbOptions = GetDbSettingsOptions(configuration);
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
        private static DbSettingsOptions GetDbSettingsOptions(IConfiguration configuration)
        {
            var result = DbSettingsOptions.GetDefaultSettings();
            var dbSettingsSection = configuration.GetSection("DbSettings");
            if (dbSettingsSection != null)
            {
                var options = dbSettingsSection.Get<DbSettingsOptions>();
                if (options != null) return options;
            }
            return result;
        }
    }
}
