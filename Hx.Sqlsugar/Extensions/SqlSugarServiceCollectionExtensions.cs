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
        public static IServiceCollection AddSqlSugar(this IServiceCollection services,  ConnectionConfig[] configs, Action<ISqlSugarClient> buildAction = default)
        {
            ConfigureDbConnectionSettings(services);
            // 注册 SqlSugar 客户端
            services.AddSingleton<ISqlSugarClient>(provider =>
            {
                var dbOptions = provider.GetService<IOptionsMonitor<DbSettingsOptions>>();
                var dbSettings = dbOptions.CurrentValue.ConnectionConfigs;
                var cacheService =  provider.GetService<ICacheService>();
                var logger = provider.GetService<ILogger<ISqlSugarClient>>();
                foreach (var configs in dbSettings)
                {
                    SetDbConfig(configs, cacheService);
                }
                var connectionConfigs = dbSettings.Select(r => r.ToConnectionConfig()).ToList();
                SqlSugarScope sqlSugar = new SqlSugarScope(connectionConfigs, db =>
                {
                    foreach (var config in dbSettings)
                    {
                        var dbProvider = db.GetConnectionScope(config.ConfigId);
                        SetDbAop(config,dbProvider, logger);
                        SetDbDiffLog(dbProvider, config);
                    }
                });
                //var sqlSugarClient = new SqlSugarClient(options.ToList());
                //buildAction?.Invoke(sqlSugarClient);

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
        /// 配置连接属性
        /// </summary>
        /// <param name="config"></param>
        private static void SetDbConfig(DbConnectionConfig config, ICacheService? cacheService = null)
        {
            var configureExternalServices = new ConfigureExternalServices
            {
                EntityNameService = (type, entity) => // 处理表
                {
                    if (config.EnableUnderLine && !entity.DbTableName.Contains('_'))
                        entity.DbTableName = UtilMethods.ToUnderLine(entity.DbTableName); // 驼峰转下划线
                },
                EntityService = (type, column) => // 处理列
                {
                    if (column.IsPrimarykey == false && type.PropertyType.IsGenericType 
                        && type.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        column.IsNullable = true;
                    }
                    if (config.EnableUnderLine && !column.IsIgnore && !column.DbColumnName.Contains('_'))
                        column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线

                    if (config.DbType == SqlSugar.DbType.Oracle)
                    {
                        if (type.PropertyType == typeof(long) || type.PropertyType == typeof(long?))
                            column.DataType = "number(18)";
                        if (type.PropertyType == typeof(bool) || type.PropertyType == typeof(bool?))
                            column.DataType = "number(1)";
                    }
                }
            };
            if (cacheService != null)
            {
                configureExternalServices.DataInfoCacheService= cacheService;
            }
            config.ConfigureExternalServices = configureExternalServices;
            config.InitKeyType = InitKeyType.Attribute;
            config.IsAutoCloseConnection = true;
            config.MoreSettings = new ConnMoreSettings
            {
                IsAutoRemoveDataCache = true,
                SqlServerCodeFirstNvarchar = true // 采用Nvarchar
            };
        }

        /// <summary>
        /// 配置Aop
        /// </summary>
        /// <param name="db"></param>
        public static void SetDbAop(DbConnectionConfig dbConfig, SqlSugarScopeProvider db, ILogger logger)
        {
            var config = db.CurrentConnectionConfig;

            // 设置超时时间
            db.Ado.CommandTimeOut = 30;

            // 打印SQL语句
            if (dbConfig.EnableSqlLog)
            {
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    logger.LogInformation($"【{DateTime.Now}——执行SQL】\r\n{UtilMethods.GetSqlString(config.DbType, sql, pars)}\r\n");
                };
                db.Aop.OnError = ex =>
                {
                    if (ex.Parametres == null) return;
                    logger.LogError($"【{DateTime.Now}——错误SQL】\r\n {UtilMethods.GetSqlString(config.DbType, ex.Sql, (SugarParameter[])ex.Parametres)} \r\n");
                };
            }
           
            

            // 数据审计
            db.Aop.DataExecuting = (oldValue, entityInfo) =>
            {
                // 演示环境判断
                if (IsInit && entityInfo.EntityColumnInfo.IsPrimarykey)
                {
                    if (entityInfo.EntityName != nameof(SysJobDetail) && entityInfo.EntityName != nameof(SysJobTrigger) &&
                        entityInfo.EntityName != nameof(SysLogOp) && entityInfo.EntityName != nameof(SysLogVis) &&
                        entityInfo.EntityName != nameof(SysOnlineUser))
                    {
                        var isDemoEnv = App.GetService<SysConfigService>().GetConfigValue<bool>(CommonConst.SysDemoEnv).GetAwaiter().GetResult();
                        if (isDemoEnv)
                            throw Oops.Oh(ErrorCodeEnum.D1200);
                    }
                }

                if (entityInfo.OperationType == DataFilterType.InsertByObject)
                {
                    // 主键(long类型)且没有值的---赋值雪花Id
                    if (entityInfo.EntityColumnInfo.IsPrimarykey && entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long))
                    {
                        var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                        if (id == null || (long)id == 0)
                            entityInfo.SetValue(YitIdHelper.NextId());
                    }
                    if (entityInfo.PropertyName == "CreateTime")
                        entityInfo.SetValue(DateTime.Now);
                    if (App.User != null)
                    {
                        if (entityInfo.PropertyName == "TenantId")
                        {
                            var tenantId = ((dynamic)entityInfo.EntityValue).TenantId;
                            if (tenantId == null || tenantId == 0)
                                entityInfo.SetValue(App.User.FindFirst(ClaimConst.TenantId)?.Value);
                        }
                        if (entityInfo.PropertyName == "CreateUserId")
                        {
                            var createUserId = ((dynamic)entityInfo.EntityValue).CreateUserId;
                            if (createUserId == 0 || createUserId == null)
                                entityInfo.SetValue(App.User.FindFirst(ClaimConst.UserId)?.Value);
                        }

                        if (entityInfo.PropertyName == "CreateOrgId")
                        {
                            var createOrgId = ((dynamic)entityInfo.EntityValue).CreateOrgId;
                            if (createOrgId == 0 || createOrgId == null)
                                entityInfo.SetValue(App.User.FindFirst(ClaimConst.OrgId)?.Value);
                        }
                    }
                }
                if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                {
                    if (entityInfo.PropertyName == "UpdateTime")
                        entityInfo.SetValue(DateTime.Now);
                    if (entityInfo.PropertyName == "UpdateUserId")
                        entityInfo.SetValue(App.User?.FindFirst(ClaimConst.UserId)?.Value);
                }
            };

            // 超管时排除各种过滤器
            if (App.User?.FindFirst(ClaimConst.AccountType)?.Value == ((int)AccountTypeEnum.SuperAdmin).ToString())
                return;

            // 配置实体假删除过滤器
            db.QueryFilter.AddTableFilter<IDeletedFilter>(u => u.IsDelete == false);
            // 配置租户过滤器
            var tenantId = App.User?.FindFirst(ClaimConst.TenantId)?.Value;
            if (!string.IsNullOrWhiteSpace(tenantId))
                db.QueryFilter.AddTableFilter<ITenantIdFilter>(u => u.TenantId == long.Parse(tenantId));

            // 配置用户机构（数据范围）过滤器
            SqlSugarFilter.SetOrgEntityFilter(db);
            // 配置自定义过滤器
            SqlSugarFilter.SetCustomEntityFilter(db);
        }

        /// <summary>
        /// 开启库表差异化日志
        /// </summary>
        /// <param name="db"></param>
        /// <param name="config"></param>
        private static void SetDbDiffLog(SqlSugarScopeProvider db, DbConnectionConfig config)
        {
            if (!config.EnableDiffLog) return;

            db.Aop.OnDiffLogEvent = async u =>
            {
                var logDiff = new SysLogDiff
                {
                    // 操作后记录（字段描述、列名、值、表名、表描述）
                    AfterData = JSON.Serialize(u.AfterData),
                    // 操作前记录（字段描述、列名、值、表名、表描述）
                    BeforeData = JSON.Serialize(u.BeforeData),
                    // 传进来的对象
                    BusinessData = JSON.Serialize(u.BusinessData),
                    // 枚举（insert、update、delete）
                    DiffType = u.DiffType.ToString(),
                    Sql = UtilMethods.GetSqlString(config.DbType, u.Sql, u.Parameters),
                    Parameters = JSON.Serialize(u.Parameters),
                    Elapsed = u.Time == null ? 0 : (long)u.Time.Value.TotalMilliseconds
                };
                await db.Insertable(logDiff).ExecuteCommandAsync();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(DateTime.Now + $"\r\n*****差异日志开始*****\r\n{Environment.NewLine}{JSON.Serialize(logDiff)}{Environment.NewLine}*****差异日志结束*****\r\n");
            };
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="db"></param>
        /// <param name="config"></param>
        private static void InitDatabase(SqlSugarScope db, DbConnectionConfig config)
        {
            if (!config.EnableInitDb) return;

            SqlSugarScopeProvider dbProvider = db.GetConnectionScope(config.ConfigId);

            // 创建数据库
            if (config.DbType != SqlSugar.DbType.Oracle)
                dbProvider.DbMaintenance.CreateDatabase();

            // 获取所有实体表-初始化表结构
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false)).ToList();
            if (!entityTypes.Any()) return;
            foreach (var entityType in entityTypes)
            {
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if (tAtt != null && tAtt.configId.ToString() != config.ConfigId) continue;
                if (tAtt == null && config.ConfigId != SqlSugarConst.ConfigId) continue;

                var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
                if (splitTable == null)
                    dbProvider.CodeFirst.InitTables(entityType);
                else
                    dbProvider.CodeFirst.SplitTables().InitTables(entityType);
            }

            if (!config.EnableInitSeed) return;

            // 获取所有种子配置-初始化数据
            var seedDataTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && u.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(ISqlSugarEntitySeedData<>)))).ToList();
            if (!seedDataTypes.Any()) return;
            foreach (var seedType in seedDataTypes)
            {
                var instance = Activator.CreateInstance(seedType);

                var hasDataMethod = seedType.GetMethod("HasData");
                var seedData = ((IEnumerable)hasDataMethod?.Invoke(instance, null))?.Cast<object>();
                if (seedData == null) continue;

                var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if (tAtt != null && tAtt.configId.ToString() != config.ConfigId) continue;
                if (tAtt == null && config.ConfigId != SqlSugarConst.ConfigId) continue;

                var entityInfo = dbProvider.EntityMaintenance.GetEntityInfo(entityType);
                if (entityInfo.Columns.Any(u => u.IsPrimarykey))
                {
                    // 按主键进行批量增加和更新
                    var storage = dbProvider.StorageableByObject(seedData.ToList()).ToStorage();
                    storage.AsInsertable.ExecuteCommand();
                    var ignoreUpdate = hasDataMethod.GetCustomAttribute<IgnoreUpdateAttribute>();
                    if (ignoreUpdate == null) storage.AsUpdateable.ExecuteCommand();
                }
                else
                {
                    // 无主键则只进行插入
                    if (!dbProvider.Queryable(entityInfo.DbTableName, entityInfo.DbTableName).Any())
                        dbProvider.InsertableByObject(seedData.ToList()).ExecuteCommand();
                }
            }
        }

        /// <summary>
        /// 初始化租户业务数据库
        /// </summary>
        /// <param name="iTenant"></param>
        /// <param name="config"></param>
        public static void InitTenantDatabase(ITenant iTenant, DbConnectionConfig config)
        {
            SetDbConfig(config);

            iTenant.AddConnection(config);
            var db = iTenant.GetConnectionScope(config.ConfigId);
            db.DbMaintenance.CreateDatabase();

            // 获取所有实体表-初始化租户业务表
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && u.IsDefined(typeof(SugarTable), false) && !u.IsDefined(typeof(SystemTableAttribute), false)).ToList();
            if (!entityTypes.Any()) return;
            foreach (var entityType in entityTypes)
            {
                var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
                if (splitTable == null)
                    db.CodeFirst.InitTables(entityType);
                else
                    db.CodeFirst.SplitTables().InitTables(entityType);
            }
        }

        /// <summary>
        /// 添加 Redis 配置
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
