﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hx.Sdk.Common.Extensions
{
    /// <summary>
    /// 类型扩展类
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 判断该类型上是否有指定的属性
        /// </summary>
        /// <typeparam name="T">属性的类型</typeparam>
        /// <param name="type">当前类型</param>
        /// <returns></returns>
        public static bool HasAttribute<T>(this Type type) where T : Attribute
        {
            if (type == null) return false;
            T attribute = type.GetCustomAttribute<T>();
            return attribute != null;
        }
        /// <summary>
        /// 根据键得到值，如果没有返回默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            dictionary.TryGetValue(key, out value);
            return value;
        }
        /// <summary>
        /// 根据名字获取方法的元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MethodInfo GetInheritedMethod(this Type type, string name)
        {
            return GetMember(type, name) as MethodInfo;
        }
        /// <summary>
        /// 得到指定名字的字段或者属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MemberInfo GetFieldOrProperty(this Type type, string name)
        {
            var memberInfo = GetMember(type, name);
            if (memberInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Cannot find a field or property named " + name);
            }
            return memberInfo;
        }
        /// <summary>
        /// 得到类型中指定名称的成员
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static MemberInfo GetMember(Type type, string name)
        {
            return
                new[] { type }.Concat(type.GetTypeInfo().ImplementedInterfaces)
                .SelectMany(i => i.GetMember(name))
                .FirstOrDefault();
        }
        /// <summary>
        /// 是否是Nullable类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType(typeof(Nullable<>));
        }

        /// <summary>
        /// 获取Nullable类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetTypeOfNullable(this Type type)
        {
            return type.GetTypeInfo().GenericTypeArguments[0];
        }
        /// <summary>
        /// 是否是集合类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCollectionType(this Type type)
        {
            return type.ImplementsGenericInterface(typeof(ICollection<>));
        }

        /// <summary>
        /// 是否是枚举类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerableType(this Type type)
        {
            return typeof(IEnumerable<>).IsAssignableFrom(type);
        }
        /// <summary>
        /// 是否是IQueryable类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsQueryableType(this Type type)
        {
            return typeof(IQueryable).IsAssignableFrom(type);
        }
        /// <summary>
        /// 是否是List类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsListType(this Type type)
        {
            return typeof(IList<>).IsAssignableFrom(type);
        }
        /// <summary>
        /// 是否是List或者Dictionary类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsListOrDictionaryType(this Type type)
        {
            return type.IsListType() || type.IsDictionaryType();
        }
        /// <summary>
        /// 是否是Dictionary类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDictionaryType(this Type type)
        {
            return type.ImplementsGenericInterface(typeof(IDictionary<,>));
        }
        /// <summary>
        /// 判断当前类是否实现了泛型接口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool ImplementsGenericInterface(this Type type, Type interfaceType)
        {
            if (type.IsGenericType(interfaceType))
            {
                return true;
            }
            foreach (var @interface in type.GetTypeInfo().ImplementedInterfaces)
            {
                if (@interface.IsGenericType(interfaceType))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 是否是某个泛型类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static bool IsGenericType(this Type type, Type genericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }

        /// <summary>
        /// 获取IEnumerable的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetIEnumerableType(this Type type)
        {
            return type.GetGenericInterface(typeof(IEnumerable<>));
        }

        /// <summary>
        /// 获取Dictionary的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetDictionaryType(this Type type)
        {
            return type.GetGenericInterface(typeof(IDictionary<,>));
        }

        /// <summary>
        /// 获取泛型接口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericInterface"></param>
        /// <returns></returns>
        public static Type GetGenericInterface(this Type type, Type genericInterface)
        {
            if (type.IsGenericType(genericInterface))
            {
                return type;
            }
            return type.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(t => t.IsGenericType(genericInterface));
        }
        /// <summary>
        /// 得到所有的泛型接口类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericInterface"></param>
        /// <returns></returns>
        public static List<Type> GetGenericInterfaces(this Type type, Type genericInterface)
        {
            List<Type> typeList = new List<Type>();
            var types = type.GetTypeInfo().ImplementedInterfaces.Where(t => t.IsGenericType(genericInterface));
            if (types != null)
            {
                typeList = types.ToList();
            }
            return typeList;
        }

        /// <summary>
        /// 获取ElementType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetGenericElementType(this Type type)
        {
            if (type.HasElementType)
                return type.GetElementType();
            return type.GetTypeInfo().GenericTypeArguments[0];
        }

        /// <summary>
        /// 确定是否可以将指定类型的实例分配给当前泛型类型的变量。
        /// </summary>
        /// <param name="genericType">当前泛型类型</param>
        /// <param name="givenType">指定类型</param>
        /// <returns></returns>
        public static bool IsAssignableFromGenericType(this Type genericType, Type givenType)
        {
            if (givenType.IsGenericType(genericType))  return true;
            var interfaceTypes = givenType.GetInterfaces();
            foreach (var it in interfaceTypes)
            {
                
                if (it.IsGenericType(genericType))
                    return true;
            }
            Type baseType = givenType.BaseType;
            if (baseType == null) return false;
            return genericType.IsAssignableFromGenericType(baseType);
        }
    }
}