using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logs
{
    public interface ILoggerFactory
    {
        /// <summary>
        /// 创建ILogger对象,如果没有添加配置文件，则使用默认的配置创建对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="category">日志文件的上一层目录，一般用来分类</param>
        /// <returns></returns>
        Common.Logs.ILogger CreateLogger<T>(string category = null);
        /// <summary>
        /// 创建ILogger对象,如果没有添加配置文件，则使用默认的配置创建对象
        /// </summary>
        /// <param name="loggerName">日志名称，对应NLog配置文件中rules->logger标签中的name属性的值;
        /// 对应log4net置文件标签【appender name="pay" type="Common.Log.ReadParamAppender">】中的name的值</param>
        /// <param name="category">日志文件的上一层目录，一般用来分类</param>
        /// <returns></returns>
        Common.Logs.ILogger CreateLogger(string loggerName, string category = null);
    }
}
