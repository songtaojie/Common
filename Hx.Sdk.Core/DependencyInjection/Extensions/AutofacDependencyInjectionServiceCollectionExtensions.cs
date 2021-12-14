using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Hx.Sdk.DependencyInjection;
using Hx.Sdk.DependencyInjection.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 使用Autofac进行依赖注入的扩展类
    /// </summary>
    public static class AutofacDependencyInjectionServiceCollectionExtensions
    {
        #region 使用Aotofac注入
        /// <summary>
        /// 使用autofac进行依赖注入
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="effectiveTypes">要注入的类型集合</param>
        /// <param name="aopTypes">Aop切面类型</param>
        public static void AddAutofacDependencyInjection(this ContainerBuilder builder, IEnumerable<Type> effectiveTypes, IEnumerable<Type> aopTypes = null)
        {
            try
            {
                builder.AddScanDependencyInjection(effectiveTypes, aopTypes);
            }
            catch (Exception ex)
            {
                throw new Exception("Autofac injection failed，" + ex.Message + "\n" + ex.InnerException);
            }
        }
        #endregion


        /// <summary>
        /// 添加扫描注入
        /// </summary>
        /// <param name="builder">服务集合</param>
        /// <param name="effectiveTypes">有效的程序集</param>
        /// <param name="aopTypes">Aop类型集合</param>
        /// <returns>服务集合</returns>
        private static ContainerBuilder AddScanDependencyInjection(this ContainerBuilder builder, IEnumerable<Type> effectiveTypes, IEnumerable<Type> aopTypes = null)
        {
            // 查找所有需要依赖注入的类型
            var injectTypes = effectiveTypes
                .Where(u => typeof(IPrivateDependency).IsAssignableFrom(u) && u.IsClass && !u.IsInterface && !u.IsAbstract)
                .OrderBy(u => GetOrder(u));
            // 注册aop服务
            if (aopTypes != null && aopTypes.Count() > 0)
            {
                builder.RegisterTypes(aopTypes.ToArray());
            }
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
                                //&& projectAssemblies.Contains(u.Assembly)
                                && (
                                    (!type.IsGenericType && !u.IsGenericType)
                                    || (type.IsGenericType && u.IsGenericType && type.GetGenericArguments().Length == u.GetGenericArguments().Length))
                                );
                IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registerBuilder = null;
                // 注册暂时服务
                if (typeof(ITransientDependency).IsAssignableFrom(type))
                {
                    registerBuilder = RegisterService(builder, DependencyInjectionType.Transient, type, injectionAttribute, canInjectInterfaces);
                }
                // 注册作用域服务
                else if (typeof(IScopedDependency).IsAssignableFrom(type))
                {
                    registerBuilder = RegisterService(builder, DependencyInjectionType.Scoped, type, injectionAttribute, canInjectInterfaces);
                }
                // 注册单例服务
                else if (typeof(ISingletonDependency).IsAssignableFrom(type))
                {
                    registerBuilder = RegisterService(builder, DependencyInjectionType.Singleton, type, injectionAttribute, canInjectInterfaces);
                }
                if (injectionAttribute.Pattern == InjectionPatterns.Self)
                {
                    registerBuilder.EnableClassInterceptors();
                }
                else
                {
                    registerBuilder.EnableInterfaceInterceptors();
                }
                //AOp
                if (aopTypes != null && aopTypes.Count() > 0)
                {
                    registerBuilder.InterceptedBy(aopTypes.ToArray());
                }
                //缓存类型注册
                var typeNamed = injectionAttribute.Named ?? type.Name;
                TypeNamedCollection.TryAdd(typeNamed, type);
            }

            // 注册命名服务
            RegisterNamed(builder);

            return builder;
        }


        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="registerType">类型作用域</param>
        /// <param name="type">类型</param>
        /// <param name="injectionAttribute">注入特性</param>
        /// <param name="canInjectInterfaces">能被注册的接口</param>
        private static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterService(ContainerBuilder builder, DependencyInjectionType registerType, Type type, InjectionAttribute injectionAttribute, IEnumerable<Type> canInjectInterfaces)
        {
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registerBuilder = null;
            // 注册自己
            if (injectionAttribute.Pattern is InjectionPatterns.Self)
            {
                registerBuilder = RegisterType(builder, registerType, type, injectionAttribute);
            }
            if (!canInjectInterfaces.Any()) return registerBuilder;
            // 只注册第一个接口
            if (injectionAttribute.Pattern is InjectionPatterns.FirstInterface)
            {
                var first = canInjectInterfaces.First();
                registerBuilder = RegisterType(builder, registerType, type, injectionAttribute, new List<Type> { first });
            }
            // 注册多个接口
            else if (injectionAttribute.Pattern is InjectionPatterns.ImplementedInterfaces)
            {
                registerBuilder = RegisterType(builder, registerType, type, injectionAttribute, canInjectInterfaces);
            }
            return registerBuilder;
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="builder">服务</param>
        /// <param name="registerType">注册类型</param>
        /// <param name="type">类型</param>
        /// <param name="injectionAttribute">注入特性</param>
        /// <param name="canInjectInterfaces">接口</param>
        private static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType(ContainerBuilder builder,
            DependencyInjectionType registerType, Type type, InjectionAttribute injectionAttribute, IEnumerable<Type> canInjectInterfaces = null)
        {
            var fixedType = FixedGenericType(type);
            if (registerType == DependencyInjectionType.Transient)
                return RegisterTransientType(builder, fixedType, injectionAttribute, canInjectInterfaces);
            if (registerType == DependencyInjectionType.Scoped)
                return RegisterScopeType(builder, fixedType, injectionAttribute, canInjectInterfaces);
            if (registerType == DependencyInjectionType.Singleton)
                return RegisterSingletonType(builder, fixedType, injectionAttribute, canInjectInterfaces);
            throw new Exception("The unknown registerType");
        }

        /// <summary>
        /// 注册瞬时接口实例类型
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type">类型</param>
        /// <param name="injectionAttribute">注入特性</param>
        /// <param name="canInjectInterfaces">接口</param>
        private static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterTransientType(ContainerBuilder builder, Type type, InjectionAttribute injectionAttribute, IEnumerable<Type> canInjectInterfaces = null)
        {
            switch (injectionAttribute.Action)
            {
                case InjectionActions.Add:
                    if (canInjectInterfaces == null || canInjectInterfaces.Count() == 0)
                    {
                        return builder.RegisterType(type)
                            .AsSelf()
                            .InstancePerDependency();
                    }
                    return builder.RegisterType(type)
                        .As(canInjectInterfaces.ToArray())
                        .InstancePerDependency();

                case InjectionActions.TryAdd:
                    if (canInjectInterfaces == null)
                    {
                        return builder.RegisterType(type)
                                .AsSelf()
                                .InstancePerDependency()
                                .PreserveExistingDefaults();
                    }
                        
                    return builder.RegisterType(type)
                             .As(canInjectInterfaces.ToArray())
                             .InstancePerDependency()
                             .PreserveExistingDefaults();
                default: throw new Exception("The unknown InjectionActions");
            }
        }

        /// <summary>
        /// 注册作用域接口实例类型
        /// </summary>
        /// <param name="builder">服务</param>
        /// <param name="type">类型</param>
        /// <param name="injectionAttribute">注入特性</param>
        /// <param name="canInjectInterfaces">接口</param>
        private static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterScopeType(ContainerBuilder builder, Type type, InjectionAttribute injectionAttribute, IEnumerable<Type> canInjectInterfaces = null)
        {
            switch (injectionAttribute.Action)
            {
                case InjectionActions.Add:
                    if (canInjectInterfaces == null || canInjectInterfaces.Count() == 0)
                    {
                        return builder.RegisterType(type).AsSelf().InstancePerLifetimeScope();
                    }
                    return builder.RegisterType(type)
                        .As(canInjectInterfaces.ToArray())
                        .InstancePerLifetimeScope();
                case InjectionActions.TryAdd:
                    if (canInjectInterfaces == null || canInjectInterfaces.Count() == 0)
                    {
                        return builder.RegisterType(type)
                                .InstancePerLifetimeScope()
                                .PreserveExistingDefaults();
                    }
                    return builder.RegisterType(type)
                                   .As(canInjectInterfaces.ToArray())
                                   .InstancePerLifetimeScope()
                                   .PreserveExistingDefaults()
                                   .EnableInterfaceInterceptors();
                default: throw new Exception("The unknown InjectionActions");
            }
        }

        /// <summary>
        /// 注册单例接口实例类型
        /// </summary>
        /// <param name="builder">服务</param>
        /// <param name="type">类型</param>
        /// <param name="injectionAttribute">注入特性</param>
        /// <param name="canInjectInterfaces">接口</param>
        private static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterSingletonType(ContainerBuilder builder, Type type, InjectionAttribute injectionAttribute, IEnumerable<Type> canInjectInterfaces = null)
        {
            switch (injectionAttribute.Action)
            {
                case InjectionActions.Add:
                    if (canInjectInterfaces == null || canInjectInterfaces.Count() == 0)
                    {
                        return builder.RegisterType(type)
                           .AsSelf()
                           .SingleInstance();
                    }
                    return builder.RegisterType(type)
                            .As(canInjectInterfaces.ToArray())
                            .SingleInstance();
                case InjectionActions.TryAdd:
                    if (canInjectInterfaces == null || canInjectInterfaces.Count() == 0)
                    {
                        return builder.RegisterType(type)
                            .AsSelf()
                            .SingleInstance()
                            .PreserveExistingDefaults();
                    }
                    return builder.RegisterType(type)
                        .As(canInjectInterfaces.ToArray())
                        .SingleInstance()
                        .PreserveExistingDefaults();
                default: throw new Exception("The unknown InjectionActions");
            }
        }

        /// <summary>
        /// 注册命名服务
        /// </summary>
        /// <param name="builder">服务集合</param>
        private static void RegisterNamed(ContainerBuilder builder)
        {
            // 注册暂时命名服务
            builder.Register((c) =>
            {
                object ResolveService(string named, ITransientDependency transient)
                {
                    var isRegister = TypeNamedCollection.TryGetValue(named, out var serviceType);
                    return isRegister ? c.Resolve(serviceType) : null;
                }
                return (Func<string, ITransientDependency, object>)ResolveService;
            }).InstancePerDependency();

            // 注册作用域命名服务
            builder.Register(c =>
            {

                object ResolveService(string named, IScopedDependency transient)
                {
                    var isRegister = TypeNamedCollection.TryGetValue(named, out var serviceType);
                    return isRegister ? c.Resolve(serviceType) : null;
                }
                return (Func<string, IScopedDependency, object>)ResolveService;
            }).InstancePerLifetimeScope();

            // 注册单例命名服务
            builder.Register(c =>
            {

                object ResolveService(string named, ISingletonDependency transient)
                {
                    var isRegister = TypeNamedCollection.TryGetValue(named, out var serviceType);
                    return isRegister ? c.Resolve(serviceType) : null;
                }
                return (Func<string, ISingletonDependency, object>)ResolveService;
            }).SingleInstance();
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
        static AutofacDependencyInjectionServiceCollectionExtensions()
        {
            TypeNamedCollection = new ConcurrentDictionary<string, Type>();
        }
    }
}
