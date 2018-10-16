using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logs.NLogs
{
    internal static class NLogManager
    {
        /// <summary>
        /// 获取NLoger对象，如果没有添加配置文件，则使用默认的配置创建对象
        /// </summary>
        /// <param name="loggerName">日志名称，对应配置文件中rules-logger标签中的name属性的值</param>
        /// <param name="category">日志文件的上一层目录，一般用来分类</param>
        /// <returns></returns>
        internal static NLog.ILogger GetLogger(string loggerName, string category = null)
        {
            if (string.IsNullOrEmpty(loggerName))
                loggerName = "Default";
            LoggingConfiguration config = LogManager.Configuration;
            if (config == null)
            {
                config = new LoggingConfiguration();
                // 创建Target，并添加成员变量
                //ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget()
                //{
                //    Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}"
                //};
                //config.AddTarget("console", consoleTarget);
                string filename = string.IsNullOrEmpty(category) ? "" : category+"/";
                FileTarget fileTarget = new FileTarget()
                {
                    FileName = "${basedir}/Log/"+filename+"Application."+ loggerName + ".txt",
                    Layout = "${message}"
                };
                config.AddTarget("file", fileTarget);
                SimpleLayout layout = new SimpleLayout();
                // 定义规则
                // LoggingRule rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
                // config.LoggingRules.Add(rule1);
                LoggingRule rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
                config.LoggingRules.Add(rule2);

                // 设置配置

                LogManager.Configuration = config;
            }
            Logger logger = LogManager.GetLogger(loggerName);
            return logger;
        }
    }
}
