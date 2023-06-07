using Hx.Sdk.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 规范化接口服务拓展类
    /// </summary>
    [Hx.Sdk.Attributes.SkipScan]
    internal static class EventBusServiceCollectionExtensions
    {
        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="config"></param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddCapRabbitMQForMySql(this IServiceCollection services, IConfiguration config)
        {
            // 判断是否安装了 DependencyInjection 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.EventBus));
            if (diAssembly == null) return services;
            // 加载 SwaggerBuilder 拓展类型和拓展方法
            var eventBusBuilderExtensionsType = diAssembly.GetType($"Microsoft.Extensions.DependencyInjection.EventBusServiceCollectionExtensions");
            if (eventBusBuilderExtensionsType == null) return services;
            var addCapRabbitMQForMySqlMethod = eventBusBuilderExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(u =>
                {
                    var parameters = u.GetParameters();
                    if(parameters.Length < 2)return false;
                    var param1 = parameters[0];
                    var param2 = parameters[1];
                    return u.Name == "AddCapRabbitMQForMySql" && param1.ParameterType == typeof(IServiceCollection) && param2.ParameterType == typeof(IConfiguration);  
                });
            ConsoleHelper.WriteInfoLine("Add the AddCapRabbitMQForMySql service");
            addCapRabbitMQForMySqlMethod?.Invoke(null, new object[] { services, config, null, null });
            return services;
        }
    }
}
