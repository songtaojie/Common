using Hx.Sdk.DependencyInjection;
using Hx.Sdk.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///使用.Net Core原生的依赖注入进行 依赖注入的拓展类
    /// </summary>
    public static class NativeDependencyInjectionServiceCollectionExtensions
    {
        /// <summary>
        /// 使用.Net Core自带的DI添加依赖注入接口
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="types">依赖注入的类型集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddNativeDependencyInjection(this IServiceCollection services, IEnumerable<Type> types)
        {
            services.AddScanDependencyInjection(types);
            return services;
        }

        /// <summary>
        /// 扫描批量注册类型进行依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IServiceCollection AddRegisterTypes(this IServiceCollection services, IEnumerable<Type> types)
        {
            services.AddScanDependencyInjection(types);
            return services;
        }

        /// <summary>
        /// 添加扫描注入
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="effectiveTypes"></param>
        /// <returns>服务集合</returns>
        private static IServiceCollection AddScanDependencyInjection(this IServiceCollection services, IEnumerable<Type> effectiveTypes)
        {
            // 查找所有需要依赖注入的类型
            var injectTypes = effectiveTypes
                .Where(u => typeof(IPrivateDependency).IsAssignableFrom(u) && u.IsClass && !u.IsInterface && !u.IsAbstract)
                .OrderBy(u => GetOrder(u));

            //var projectAssemblies = App.Assemblies;

            // 执行依赖注入
            foreach (var type in injectTypes)
            {
                // 获取注册方式
                var injectionAttribute = !type.IsDefined(typeof(InjectionAttribute)) ? new InjectionAttribute() : type.GetCustomAttribute<InjectionAttribute>();

                // 获取所有能注册的接口
                var canInjectInterfaces = type.GetInterfaces()
                    .Where(u => !injectionAttribute.ExpectInterfaces.Contains(u)
                                && u != typeof(IPrivateDependency)
                                && !typeof(IPrivateDependency).IsAssignableFrom(u)
                                && (
                                    (!type.IsGenericType && !u.IsGenericType)
                                    || (type.IsGenericType && u.IsGenericType && type.GetGenericArguments().Length == u.GetGenericArguments().Length))
                                );
                // 注册服务
                RegisterService(services,type, injectionAttribute, canInjectInterfaces);
                // 缓存类型注册
                var typeNamed = injectionAttribute.Named ?? type.Name;
                TypeNamedCollection.TryAdd(typeNamed, type);
            }

            // 注册命名服务
            RegisterNamed(services);

            return services;
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="type">类型</param>
        /// <param name="injectionAttribute">注入特性</param>
        /// <param name="canInjectInterfaces">能被注册的接口</param>
        private static void RegisterService(IServiceCollection services, Type type, InjectionAttribute injectionAttribute, IEnumerable<Type> canInjectInterfaces)
        {
            // 获取注册服务的类型
            DependencyInjectionType registerType = typeof(ITransientDependency).IsAssignableFrom(type)
                ? DependencyInjectionType.Transient
                : (typeof(IScopedDependency).IsAssignableFrom(type) ? DependencyInjectionType.Scoped : DependencyInjectionType.Singleton);
            // 注册自己
            if (injectionAttribute.Pattern is InjectionPatterns.Self)
            {
                RegisterType(services, registerType, type);
            }

            if (!canInjectInterfaces.Any()) return;

            // 只注册第一个接口
            if (injectionAttribute.Pattern is InjectionPatterns.FirstInterface)
            {
                RegisterType(services, registerType, type, canInjectInterfaces.First());
            }
            // 注册多个接口
            else if (injectionAttribute.Pattern is InjectionPatterns.ImplementedInterfaces)
            {
                foreach (var inter in canInjectInterfaces)
                {
                    RegisterType(services, registerType, type,inter);
                }
            }
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="registerType">注册类型</param>
        /// <param name="type">类型</param>
        /// <param name="inter">接口</param>
        private static void RegisterType(IServiceCollection services, DependencyInjectionType registerType, Type type, Type inter = null)
        {
            // 修复泛型注册类型
            var fixedType = FixedGenericType(type);
            var fixedInter = inter == null ? null : FixedGenericType(inter);

            if (registerType == DependencyInjectionType.Transient) RegisterTransientType(services, fixedType, fixedInter);
            if (registerType == DependencyInjectionType.Scoped) RegisterScopeType(services, fixedType, fixedInter);
            if (registerType == DependencyInjectionType.Singleton) RegisterSingletonType(services, fixedType, fixedInter);
        }

        /// <summary>
        /// 注册瞬时接口实例类型
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="type">类型</param>
        /// <param name="inter">接口</param>
        private static void RegisterTransientType(IServiceCollection services, Type type, Type inter = null)
        {
            if (inter == null) services.AddTransient(type);
            else
            {
                services.AddTransient(inter, type);
            }
        }

        /// <summary>
        /// 注册作用域接口实例类型
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="type">类型</param>
        /// <param name="inter">接口</param>
        private static void RegisterScopeType(IServiceCollection services, Type type,Type inter = null)
        {
            if (inter == null) services.AddScoped(type);
            else
            {
                services.AddScoped(inter, type);
            }
        }

        /// <summary>
        /// 注册单例接口实例类型
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="type">类型</param>
        /// <param name="inter">接口</param>
        private static void RegisterSingletonType(IServiceCollection services, Type type, Type inter = null)
        {
            if (inter == null) services.AddSingleton(type);
            else
            {
                services.AddSingleton(inter, type);
            }
        }

        /// <summary>
        /// 注册命名服务
        /// </summary>
        /// <param name="services">服务集合</param>
        private static void RegisterNamed(IServiceCollection services)
        {
            // 注册暂时命名服务
            services.AddTransient(provider =>
            {
                object ResolveService(string named, ITransientDependency transient)
                {
                    var isRegister = TypeNamedCollection.TryGetValue(named, out var serviceType);
                    return isRegister ? provider.GetService(serviceType) : null;
                }
                return (Func<string, ITransientDependency, object>)ResolveService;
            });

            // 注册作用域命名服务
            services.AddScoped(provider =>
            {
                object ResolveService(string named, IScopedDependency scoped)
                {
                    var isRegister = TypeNamedCollection.TryGetValue(named, out var serviceType);
                    return isRegister ? provider.GetService(serviceType) : null;
                }
                return (Func<string, IScopedDependency, object>)ResolveService;
            });

            // 注册单例命名服务
            services.AddSingleton(provider =>
            {
                object ResolveService(string named, ISingletonDependency singleton)
                {
                    var isRegister = TypeNamedCollection.TryGetValue(named, out var serviceType);
                    return isRegister ? provider.GetService(serviceType) : null;
                }
                return (Func<string, ISingletonDependency, object>)ResolveService;
            });
        }

        /// <summary>
        /// 修复泛型类型注册类型问题
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static Type FixedGenericType(Type type)
        {
            if (!type.IsGenericType) return type;

            return type.Assembly.GetType($"{type.Namespace}.{type.Name}", true, true);
        }

        /// <summary>
        /// 获取 注册 排序
        /// </summary>
        /// <param name="type">排序类型</param>
        /// <returns>int</returns>
        private static int GetOrder(Type type)
        {
            return !type.IsDefined(typeof(InjectionAttribute), true)
                ? 0
                : type.GetCustomAttribute<InjectionAttribute>(true).Order;
        }

        /// <summary>
        /// 类型名称集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, Type> TypeNamedCollection;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static NativeDependencyInjectionServiceCollectionExtensions()
        {
            TypeNamedCollection = new ConcurrentDictionary<string, Type>();
        }
    }
}