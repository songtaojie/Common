using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Log
{
    public  class CustomNLog
    {
        public static NLog.ILogger GetLogger(string loggerName)
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
                FileTarget fileTarget = new FileTarget()
                {
                    FileName = "${basedir}/Log/Application."+ loggerName + ".txt",
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
