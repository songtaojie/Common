using Hx.Sdk.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hx.Sdk.EntityFrameworkCore.Attributes
{

    /// <summary>
    /// 值转换器【ValueConverter】的特性属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ValueConverterAttribute : Attribute
    {

        #region Field
        private ValueConverter  _valueConverter;
        #endregion

        #region Construct
        public ValueConverterAttribute(Type type):this(type,null)
        {
        }

        public ValueConverterAttribute(Type type, object param):this(type,param,null)
        {
        }
        public ValueConverterAttribute(Type type, object param1,object param2)
        {
            ConventionType = type;
            if (param2 != null)
            {
                Params = new object[2] { param1, param2 };
            }
            else if (param1 != null)
            {
                Params = new object[] { param1 };
            }
            if (!ValidateType(type)) throw new TypeAccessException("Type does not implement the class ValueConverter");
        }

        #endregion

        #region Property
        /// <summary>
        /// 值转换器的类型
        /// </summary>
        public Type ConventionType
        {
            get; set;
        }

        /// <summary>
        /// 参数值，如果值值转换器构造函数中需要传递值时使用该参数
        /// 构造函数中的ConverterMappingHints参数为null，不能设置
        /// 如需使用ConverterMappingHints参数，则使用Fluent API，不要使用注解
        /// </summary>
        public object[] Params { get; set; }

        /// <summary>
        /// 值转换器
        /// </summary>
        internal ValueConverter ValueConverter
        {
            get
            {
                if(_valueConverter == null) _valueConverter = GetValueConverter();
                return _valueConverter;
            }
        }

        #endregion

        private bool ValidateType(Type type)
        {
            var valueConverterType = typeof(ValueConverter);
            return valueConverterType.IsAssignableFrom(type);
        }

        /// <summary>
        /// 获取当前指定的值转换器
        /// </summary>
        /// <returns></returns>
        private ValueConverter GetValueConverter()
        {
            ConverterMappingHints mappingHints = null;
            object[] newParams = null;
            if (Params != null)
            {
                var list = Params.ToList();
                list.Add(mappingHints);
                newParams = list.ToArray();
            }
            if (ConventionType.IsGenericTypeDefinition)
            {
                Type[] types = ConventionType.GetGenericArguments();
                var genericType = ConventionType.MakeGenericType(types);
                object toIntInstance = Activator.CreateInstance(genericType, Params == null ? new object[] { mappingHints } : newParams);
                return toIntInstance as ValueConverter;
            }
            else
            {
                object toIntInstance = Activator.CreateInstance(ConventionType, Params == null? new object[] { mappingHints } : newParams);
                return toIntInstance as ValueConverter;
                //object toIntInstance = CreateObject(ConventionType, Params ?? (new object[] { mappingHints }));
            }
        }

        public object CreateObject(Type type,params object[] args)
        {
            try
            {
                int lenght = 0;
                if (args != null)
                {
                    lenght = args.Length;
                }

                Type[] paramTypes = new Type[lenght];
                for (int i = 0; i < args.Length; i++)
                {
                    paramTypes[i] = args[i].GetType();
                }

                object[] param = new object[lenght];
                for (int i = 0; i < args.Length; i++)
                {
                    param[i] = args[i];
                }

                object obj = null;
                ConstructorInfo constructorInfoObj = type.GetConstructor(paramTypes);
                if (constructorInfoObj != null)
                {
                    Console.WriteLine("构造函数创建");
                    //调用指定参数的构造函数
                    obj = constructorInfoObj.Invoke(param);
                }
                else
                {
                    Console.WriteLine("非构造函数创建");
                    obj = Activator.CreateInstance(type, null);
                }
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
