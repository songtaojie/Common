using Hx.Sdk.Entity;
using Hx.Sdk.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hx.Sdk.SqlSugar.Internal
{
    /// <summary>
    /// 常量、公共方法配置类
    /// </summary>
    internal static class Penetrates
    {
        /// <summary>
        /// 应用有效程序集
        /// </summary>
        internal static IEnumerable<Assembly> Assemblies;
        private static IEnumerable<Type> _effectiveTypes;
        /// <summary>
        /// 有效程序集类型
        /// </summary>
        internal static IEnumerable<Type> EffectiveTypes
        {
            get
            {
                if (_effectiveTypes == null && Assemblies != null)
                {
                    _effectiveTypes = Assemblies.SelectMany(GetTypes);
                }
                else
                {
                    _effectiveTypes = Array.Empty<Type>();
                }
                return _effectiveTypes;
            }
        }

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        //static Penetrates()
        //{
        //    Assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(r=>r.FullName.StartsWith("Hx"));
        //    EffectiveTypes = Assemblies?.SelectMany(GetTypes);
        //}

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


        #region  Sqlsugar
        /// <summary>
        /// 配置连接属性
        /// </summary>
        /// <param name="config"></param>
        internal static void SetDbConfig(DbConnectionConfig config, ICacheService? cacheService = null)
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

                    if (config.DbType == DbType.Oracle)
                    {
                        if (type.PropertyType == typeof(long) || type.PropertyType == typeof(long?))
                            column.DataType = "number(18)";
                        if (type.PropertyType == typeof(bool) || type.PropertyType == typeof(bool?))
                            column.DataType = "number(1)";
                    }
                }
            };
            if (config.EnableDataInfoCache && cacheService != null)
            {
                configureExternalServices.DataInfoCacheService = cacheService;
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
        internal static void SetDbAop(DbConnectionConfig dbConfig, SqlSugarScopeProvider db, ILogger logger)
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
        }

        /// <summary>
        /// 开启库表差异化日志
        /// </summary>
        /// <param name="db"></param>
        /// <param name="config"></param>
        internal static void SetDbDiffLog(SqlSugarScopeProvider db, DbConnectionConfig config, ILogger logger)
        {
            if (!config.EnableDiffLog) return;

            db.Aop.OnDiffLogEvent = u =>
            {
                var logDiff = new
                {
                    // 操作后记录（字段描述、列名、值、表名、表描述）
                    AfterData = JsonConvert.SerializeObject(u.AfterData),
                    // 操作前记录（字段描述、列名、值、表名、表描述）
                    BeforeData = JsonConvert.SerializeObject(u.BeforeData),
                    // 传进来的对象
                    BusinessData = JsonConvert.SerializeObject(u.BusinessData),
                    // 枚举（insert、update、delete）
                    DiffType = u.DiffType.ToString(),
                    Sql = UtilMethods.GetSqlString(config.DbType, u.Sql, u.Parameters),
                    Parameters = JsonConvert.SerializeObject(u.Parameters),
                    Elapsed = u.Time == null ? 0 : (long)u.Time.Value.TotalMilliseconds
                };
                logger.LogInformation($"*****差异日志开始记录，时间：{DateTime.Now}*****{Environment.NewLine}{JsonConvert.SerializeObject(logDiff)}{Environment.NewLine}*****差异日志结束*****{Environment.NewLine}");
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
            if (config.DbType != DbType.Oracle)
                dbProvider.DbMaintenance.CreateDatabase();

            // 获取所有实体表-初始化表结构
            var entityTypes = EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false)).ToList();
            if (!entityTypes.Any()) return;
            foreach (var entityType in entityTypes)
            {
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if (tAtt != null && tAtt.configId.ToString() != config.ConfigId) continue;

                var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
                if (splitTable == null)
                    dbProvider.CodeFirst.InitTables(entityType);
                else
                    dbProvider.CodeFirst.SplitTables().InitTables(entityType);
            }

            if (!config.EnableInitSeed) return;

            // 获取所有种子配置-初始化数据
            var seedDataTypes = EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
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
                //if (tAtt == null && config.ConfigId != SqlSugarConst.ConfigId) continue;

                var entityInfo = dbProvider.EntityMaintenance.GetEntityInfo(entityType);
                if (entityInfo.Columns.Any(u => u.IsPrimarykey))
                {
                    // 按主键进行批量增加和更新
                    var storage = dbProvider.StorageableByObject(seedData.ToList()).ToStorage();
                    storage.AsInsertable.ExecuteCommand();
                    var ignoreUpdate = hasDataMethod.GetCustomAttribute<IgnoreSeedUpdateAttribute>();
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
            var entityTypes = EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && u.IsDefined(typeof(SugarTable), false) && !u.IsDefined(typeof(TenantAttribute), false)).ToList();
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
        #endregion 
    }
}
