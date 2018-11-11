using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Map
{
    /// <summary>
    /// AutoMapper的帮助类，对属性进行映射
    /// </summary>
    public class MapperHelper
    {
        /// <summary>
        /// 把指定的数据对象映射到TDestination类型的数据中
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination Map<TDestination>(object source) where
            TDestination:class,new()
        {
            TDestination destination = null;
            if (source == null)
            {
                destination = new TDestination();
            }
            else
            {
                Type sourceType = source.GetType();
                Mapper.Initialize(c => c.CreateMap(sourceType, typeof(TDestination)));
                destination = Mapper.Map<TDestination>(source);
            }
            return destination;
        }
    }
}
