using Hx.Sdk.ConfigureOptions;
using Hx.Sdk.Core;
using Hx.Sdk.DatabaseAccessor;
using Hx.Sdk.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据库访问器服务拓展类
    /// </summary>
    [SkipScan]
    public static class DatabaseAccessorServiceCollectionExtensions
    {
        /// <summary>
        /// MiniProfiler 插件路径
        /// </summary>
        private const string MiniProfilerRouteBasePath = "/index-mini-profiler";
        /// <summary>
        /// 添加数据库上下文
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configure">配置</param>
        /// <param name="migrationAssemblyName">迁移类库名称</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddDatabaseAccessor(this IServiceCollection services, Action<IServiceCollection> configure = null, string migrationAssemblyName = default)
        {
            // 设置迁移类库名称
            if (!string.IsNullOrWhiteSpace(migrationAssemblyName)) Db.MigrationAssemblyName = migrationAssemblyName;
            //// 注册MiniProfiler 组件
            if (App.Settings!=null && App.Settings.InjectMiniProfiler == true)
            {
                services.AddMiniProfiler(options =>
                {
                    options.RouteBasePath = MiniProfilerRouteBasePath;
                }).AddRelationalDiagnosticListener();
            }
            // 配置数据库上下文
            configure?.Invoke(services);

            // 注册数据库上下文池
            services.TryAddScoped<IDbContextPool, DbContextPool>();

            // 注册 Sql 仓储
            services.TryAddScoped(typeof(ISqlRepository<>), typeof(SqlRepository<>));

            // 注册 Sql 非泛型仓储
            services.TryAddScoped<ISqlRepository, SqlRepository>();

            // 注册多数据库上下文仓储
            services.TryAddScoped(typeof(IRepository<,>), typeof(EFCoreRepository<,>));

            // 注册泛型仓储
            services.TryAddScoped(typeof(IRepository<>), typeof(EFCoreRepository<>));

            // 注册主从库仓储
            services.TryAddScoped(typeof(IMSRepository<,>), typeof(MSRepository<,>));
            services.TryAddScoped(typeof(IMSRepository<,,>), typeof(MSRepository<,,>));

            // 注册非泛型仓储
            services.TryAddScoped<IRepository, EFCoreRepository>();

            // 注册多数据库仓储
            services.TryAddScoped(typeof(IDbRepository<>), typeof(DbRepository<>));

            // 解析数据库上下文
            services.AddTransient(provider =>
            {
                DbContext dbContextResolve(Type locator, ITransientDependency transient)
                {
                    // 判断定位器是否绑定了数据库上下文
                    var isRegistered = Penetrates.DbContextWithLocatorCached.TryGetValue(locator, out var dbContextType);
                    if (!isRegistered) throw new InvalidOperationException($"The DbContext for locator  `{locator.FullName}` binding was not found.");

                    // 动态解析数据库上下文，创建新的对象
                    var dbContext = provider.GetService(dbContextType) as DbContext;

                    // 实现动态数据库上下文功能，刷新 OnModelCreating
                    var dbContextAttribute = DbProvider.GetAppDbContextAttribute(dbContextType);
                    if (dbContextAttribute?.Mode == DbContextMode.Dynamic)
                    {
                        DynamicModelCacheKeyFactory.RebuildModels();
                    }

                    // 添加数据库上下文到池中
                    var httpContext = App.HttpContext;
                    var dbContextPool = (httpContext != null ? httpContext.RequestServices : provider).GetService<IDbContextPool>();
                    dbContextPool?.AddToPool(dbContext);

                    return dbContext;
                }
                return (Func<Type, ITransientDependency, DbContext>)dbContextResolve;
            });

            services.AddScoped(provider =>
            {
                DbContext dbContextResolve(Type locator, IScopedDependency scoped)
                {
                    // 判断定位器是否绑定了数据库上下文
                    var isRegistered = Penetrates.DbContextWithLocatorCached.TryGetValue(locator, out var dbContextType);
                    if (!isRegistered) throw new InvalidOperationException($"The DbContext for locator `{locator.FullName}` binding was not found.");

                    // 动态解析数据库上下文
                    var dbContext = provider.GetService(dbContextType) as DbContext;

                    // 实现动态数据库上下文功能，刷新 OnModelCreating
                    var dbContextAttribute = DbProvider.GetAppDbContextAttribute(dbContextType);
                    if (dbContextAttribute?.Mode == DbContextMode.Dynamic)
                    {
                        DynamicModelCacheKeyFactory.RebuildModels();
                    }

                    // 添加数据库上下文到池中
                    var httpContext = App.HttpContext;
                    var dbContextPool = (httpContext != null ? httpContext.RequestServices : provider).GetService<IDbContextPool>();
                    dbContextPool?.AddToPool(dbContext);

                    return dbContext;
                }
                return (Func<Type, IScopedDependency, DbContext>)dbContextResolve;
            });


            // 注册全局工作单元过滤器
            services.AddMvcFilter<UnitOfWorkFilter>();

            return services;
        }

        /// <summary>
        /// 注册默认数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <param name="services">服务提供器</param>
        public static IServiceCollection RegisterDbContext<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            return services.RegisterDbContext<TDbContext, MasterDbContextLocator>();
        }

        /// <summary>
        /// 注册数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="services">服务提供器</param>
        public static IServiceCollection RegisterDbContext<TDbContext, TDbContextLocator>(this IServiceCollection services)
            where TDbContext : DbContext
            where TDbContextLocator : class, IDbContextLocator
        {
            var dbContextLocatorType = (typeof(TDbContextLocator));

            // 将数据库上下文和定位器一一保存起来
            var isSuccess = Penetrates.DbContextWithLocatorCached.TryAdd(dbContextLocatorType, typeof(TDbContext));
            Penetrates.DbContextLocatorTypeCached.TryAdd(dbContextLocatorType.FullName, dbContextLocatorType);

            if (!isSuccess) throw new InvalidOperationException($"The locator `{dbContextLocatorType.FullName}` is bound to another DbContext.");

            // 注册数据库上下文
            services.TryAddScoped<TDbContext>();

            return services;
        }
    }
}