using Hx.Sqlsugar.Repositories;
using Hx.Sqlsugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// SqlSugar 拓展类
    /// </summary>
    public static class SqlSugarServiceCollectionExtensions
    {
        private const string DbSettings_Key = "DbSettings";
        private static bool _isInitialized = false;
        /// <summary>
        /// 添加 SqlSugar 拓展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, Action<DbSettingsOptions>? configAction = default,Action<ISqlSugarClient, IServiceProvider>? buildAction = default)
        {
            AddSqlSugarCore(services, configAction);
            //注册SqlSugar用AddScoped
            services.AddScoped<ISqlSugarClient>(provider =>
            {
                var sugarClient = InitSqlSugarClient(provider);
                buildAction?.Invoke(sugarClient, provider);
                _isInitialized = true;
                return sugarClient;
            });
            return services;
        }


        private static ISqlSugarClient InitSqlSugarClient(IServiceProvider provider)
        {
            var logger = provider.GetRequiredService<ILogger<ISqlSugarClient>>();
            var options = provider.GetRequiredService<IOptions<DbSettingsOptions>>().Value;
            //options.ConnectionConfigs SetDbConfig
            if (!_isInitialized)
            {
                foreach (var item in options.ConnectionConfigs!)
                {
                    SqlSugarConfigProvider.SetDbConfig(item);
                }
                RepositoryExtension.ConnectionConfigs = options.ConnectionConfigs;
            }

            var connectionConfigs = options.ConnectionConfigs!.Select(r => r.ToConnectionConfig()).ToList();
            SqlSugarClient sugarClient = new SqlSugarClient(connectionConfigs, db =>
            {
                foreach (var dbConnectionConfig in options.ConnectionConfigs!)
                {
                    var dbProvider = db.GetConnectionScope(dbConnectionConfig.ConfigId);
                    if (dbConnectionConfig.EnableSqlLog) SqlSugarConfigProvider.SetAopLog(dbProvider, logger);
                    SqlSugarConfigProvider.SetDataExecuting(dbProvider);
                    //每次上下文都会执行
                    SqlSugarConfigProvider.InitDatabase(dbProvider, dbConnectionConfig);
                }
            });
            return sugarClient;
        }

        private static void AddSqlSugarCore(IServiceCollection services, Action<DbSettingsOptions>? configAction = default)
        {
            services.AddOptions<DbSettingsOptions>()
               .BindConfiguration(DbSettings_Key)
               .Configure(options =>
               {
                   options.Configure(options);
                   configAction?.Invoke(options);
               });
            // 注册非泛型仓储
            services.AddScoped<ISqlSugarRepository, SqlSugarRepository>();

            // 注册 SqlSugar 仓储
            services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));
        }

        /// <summary>
        /// 添加 SqlSugar 拓展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configs"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, ConnectionConfig[] configs, Action<ISqlSugarClient, IServiceProvider>? buildAction = default)
        {
            // 注册 SqlSugar 客户端
            services.AddScoped<ISqlSugarClient>(provider =>
            {
                var sugarClient = new SqlSugarClient(configs.ToList());
                buildAction?.Invoke(sugarClient, provider);

                return sugarClient;
            });

            // 注册非泛型仓储
            services.AddScoped<ISqlSugarRepository, SqlSugarRepository>();

            // 注册 SqlSugar 仓储
            services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));

            return services;
        }
    }
}
