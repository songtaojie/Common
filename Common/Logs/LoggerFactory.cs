using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logs
{
    public class LoggerFactory : ILoggerFactory
    {
        private LoggerType type;
        /// <summary>
        /// 使用指定的日志框架初始化对象，默认使用的是log4net框架
        /// </summary>
        /// <param name="type"></param>
        public LoggerFactory(LoggerType type)
        {
            this.type = type;
        }
        public LoggerFactory()
        {
            this.type = LoggerType.Log4Net;
        }
        //public Common.Logs.ILogger CreateLogger<T>(string category = null)
        //{
        //    return this.CreateLogger(typeof(T).FullName, category);
        //}
        /// <summary>
        /// 使用指定的日志名称创建日志对象
        /// </summary>
        /// <param name="loggerName"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public Common.Logs.ILogger CreateLogger(string loggerName = "Default", string category = null)
        {
            Common.Logs.ILogger log = null;
            if (this.type == LoggerType.Log4Net)
            {
                log = new Log4Net.Log4NetLogger(loggerName, category);
            }
            else if (this.type == LoggerType.NLog)
            {
                log = new NLogs.NLogLogger(loggerName, category);
            }
            return log;
        }
    }
    /// <summary>
    /// 日志的类型，用于指定要创建给予哪个框架的服务接口
    /// </summary>
    public enum LoggerType
    {
        /// <summary>
        /// 使用Log4net框架
        /// </summary>
        Log4Net,
        /// <summary>
        /// 使用NLog框架
        /// </summary>
        NLog
    }
}
