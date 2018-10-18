﻿using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: XmlConfigurator(Watch = true)]
namespace Common.Logs.Log4Net
{
    /// <summary>
    /// 内置默认配置，引用dll后不需要添加或修改任何配置文件也可以使用,
    /// 也可以在app.config或者web.config中添加配置文件
    /// </summary>
    internal static class Log4NetManager
    {
        private static readonly ConcurrentDictionary<string, ILog> loggerContainer = new ConcurrentDictionary<string, ILog>();

        // private static readonly Dictionary<string, IAppender> appenderContainer = new Dictionary<string, IAppender>();
        private static object lockObj = new object();

        //默认配置
        private const int MAX_SIZE_ROLL_BACKUPS = 20;
        private const string LAYOUT_PATTERN = "%date [%t] %-5level %message%newline";
        private const string DATE_PATTERN = "yyyy-MM-dd\".log\"";
        private const string MAXIMUM_FILE_SIZE = "5MB";
        public static bool UseConfig
        {
            get; set;
        } = false;
        //读取配置文件并缓存
        static Log4NetManager()
        {
            if (UseConfig)
            {

            }
            //IAppender[] appenders = LogManager.GetRepository().GetAppenders();
            //for (int i = 0; i < appenders.Length; i++)
            //{
            //    IAppender appender = appenders[i];
            //    lock (lockObj)
            //    {
            //        appenderContainer[appender.Name] = appender;
            //    }
            //}
        }
        /// <summary>
        /// 使用日志的名字获取log4net对象，如果没有配置文件，则使用默认的配置创建对象并返回
        /// </summary>
        /// <param name="loggerName">如果使用的是配置文件则是&lt;logger name="loggerName">标签中的
        /// name属性的值</param>
        /// <param name="category">
        ///     文件的上层文件夹，即类别,当使用默认配置时：
        ///     如果有值，则生成的日志路径为Log\{category}\{短时间}.log；
        ///     如果没值，则声称的路径为Log\{短时间}.log
        /// </param>
        /// <param name="additivity">该值指示子记录器是否继承其父级的appender。</param>
        /// <returns></returns>
        internal static ILog GetLogger(string loggerName, string category = null, bool additivity = false)
        {
            return loggerContainer.GetOrAdd(loggerName, delegate (string name)
            {
                ILog log= LogManager.Exists(loggerName);
                if (log == null)
                {
                    IAppender newAppender = GetNewFileApender(null, loggerName, category);
                    Hierarchy repository = (Hierarchy)LogManager.GetRepository();
                    Logger logger = repository.LoggerFactory.CreateLogger(repository, loggerName);
                    logger.Hierarchy = repository;
                    logger.Parent = repository.Root;
                    logger.Level = Level.Info;
                    logger.Additivity = additivity;
                    logger.AddAppender(newAppender);
                    logger.Repository.Configured = true;
                    log = new LogImpl(logger);
                }
                return log;
            });
        }

        //如果没有指定文件路径则在运行路径下建立 Log\2018-10-10.log
        private static string GetFile(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return string.Format(@"Log\");
            }
            else
            {
                return string.Format(@"Log\{0}\", category);
            }
        }
        /// <summary>
        /// 获取一个新的添加器
        /// </summary>
        /// <param name="appender"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        private static IAppender GetNewFileApender(IAppender appender,string appenderName, string category = null)
        {
            if (appender == null)
            {
                RollingFileAppender rollAppender = new RollingFileAppender
                {
                    LockingModel = new FileAppender.MinimalLock(),
                    Name = appenderName,
                    File = GetFile(category),
                    AppendToFile = true,
                    MaxSizeRollBackups = MAX_SIZE_ROLL_BACKUPS,
                    MaximumFileSize = MAXIMUM_FILE_SIZE,
                    StaticLogFileName = false,
                    RollingStyle = RollingFileAppender.RollingMode.Date,
                    DatePattern = DATE_PATTERN
                };
                PatternLayout layout = new PatternLayout(LAYOUT_PATTERN);
                rollAppender.Layout = layout;
                layout.ActivateOptions();
                rollAppender.ActivateOptions();
                return rollAppender;
            }

            return appender;
        }

        /// <summary>
        /// 获取日志的级别
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static Level GetLoggerLevel(string level)
        {
            if (!string.IsNullOrEmpty(level))
            {
                switch (level.ToLower().Trim())
                {
                    case "debug":
                        return Level.Debug;

                    case "info":
                        return Level.Info;

                    case "warn":
                        return Level.Warn;

                    case "error":
                        return Level.Error;

                    case "fatal":
                        return Level.Fatal;
                }
            }
            return Level.Debug;
        }
    }
}
