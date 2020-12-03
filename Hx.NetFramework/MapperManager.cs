using AutoMapper;
using Hx.NetFramework.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.NetFramework
{
    /// <summary>
    /// 数据模型映射帮助方法
    /// </summary>
    public class MapperManager
    {
        /// <summary>
        /// 一个委托在Build之前会执行当前委托
        /// </summary>
        public event Action<IMapperConfigurationExpression> Config;
        private static IMapper _mapper;
        public MapperManager()
        {
            GetMapper();
        }
        private IMapper GetMapper()
        {
            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.AddProfile<MyMapperProfile>();
                    Config?.Invoke(cfg);
                });
                _mapper = config.CreateMapper();
            }
            return _mapper;
        }
        /// <summary>
        /// 数据映射
        /// </summary>
        /// <typeparam name="TDestination">目标数据类型</typeparam>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 映射数据
        /// </summary>
        /// <typeparam name="TSource">元数据类型</typeparam>
        /// <typeparam name="TDestination">目标数据类型</typeparam>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }
    }
}
