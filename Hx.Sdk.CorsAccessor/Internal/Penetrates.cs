using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.CorsAccessor.Internal
{
    /// <summary>
    /// 常量、公共方法配置类
    /// </summary>
    internal static class Penetrates
    {
        /// <summary>
        /// 应用服务
        /// </summary>
        internal static IServiceCollection InternalServices;

        /// <summary>
        /// 有效程序集类型
        /// </summary>
        private static IServiceProvider _serviceProvider;
        /// <summary>
        /// 服务提供器
        /// </summary>
        internal static IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider ?? InternalServices.BuildServiceProvider();
            }
            set
            {
                _serviceProvider = value;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        static Penetrates()
        {
        }
       
    }
}
