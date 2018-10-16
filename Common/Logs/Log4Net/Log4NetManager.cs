using log4net;
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
    /// 如：
    /// <log4net>
    ///  <appender name = "pay" type="Common.Log.Log4Net.ReadParamAppender">
    ///  <param name = "file" value="log\\pay.txt"/>
    /// <param name = "appendToFile" value="true"/>
    /// <param name = "maxSizeRollBackups" value="100"/>
    /// <param name = "maximumFileSize" value="2MB"/>
    /// <param name = "datePattern" value="yyyyMMdd'.txt'"/>
    /// <param name = "level" value="debug"/>
    /// <layout type = "log4net.Layout.PatternLayout" >
    /// < conversionPattern value="%d - %m%n"/>
    /// </layout>
    /// </appender>
    /// <root>
    /// <level value = "INFO" />
    /// < appender -ref ref="pay"/>
    /// </root>
    ///</log4net>
    /// </summary>
    internal static class Log4NetManager
    {
        private static readonly ConcurrentDictionary<string, ILog> loggerContainer = new ConcurrentDictionary<string, ILog>();

        private static readonly Dictionary<string, ReadParamAppender> appenderContainer = new Dictionary<string, ReadParamAppender>();
        private static object lockObj = new object();

        //默认配置
        private const int MAX_SIZE_ROLL_BACKUPS = 20;
        private const string LAYOUT_PATTERN = "%date [%t] %-5level %message%newline";
        private const string DATE_PATTERN = "yyyyMMdd\".txt\"";
        private const string MAXIMUM_FILE_SIZE = "256MB";
        private const string LEVEL = "INFO";

        //读取配置文件并缓存
        static Log4NetManager()
        {
            IAppender[] appenders = LogManager.GetRepository().GetAppenders();
            for (int i = 0; i < appenders.Length; i++)
            {
                if (appenders[i] is ReadParamAppender)
                {
                    ReadParamAppender appender = (ReadParamAppender)appenders[i];
                    if (appender.MaxSizeRollBackups == 0)
                    {
                        appender.MaxSizeRollBackups = MAX_SIZE_ROLL_BACKUPS;
                    }
                    if (appender.Layout != null && appender.Layout is log4net.Layout.PatternLayout)
                    {
                        appender.LayoutPattern = ((log4net.Layout.PatternLayout)appender.Layout).ConversionPattern;
                    }
                    if (string.IsNullOrEmpty(appender.LayoutPattern))
                    {
                        appender.LayoutPattern = LAYOUT_PATTERN;
                    }
                    if (string.IsNullOrEmpty(appender.DatePattern))
                    {
                        appender.DatePattern = DATE_PATTERN;
                    }
                    if (string.IsNullOrEmpty(appender.MaximumFileSize))
                    {
                        appender.MaximumFileSize = MAXIMUM_FILE_SIZE;
                    }
                    if (string.IsNullOrEmpty(appender.Level))
                    {
                        appender.Level = LEVEL;
                    }
                    lock (lockObj)
                    {
                        appenderContainer[appenders[i].Name] = appender;
                    }
                }
            }
        }
        /// <summary>
        /// 使用添加器的名字获取log4net对象，如果没有配置文件
        /// </summary>
        /// <param name="appenderName">即配置文件标签【appender name="pay" type="Common.Log.ReadParamAppender">】中的pay</param>
        /// <param name="category">
        ///     文件的上层文件夹，即类别,当使用默认配置时：
        ///     如果有值，则生成的日志路径为Log\{category}\Application.{appenderName}.txt；
        ///     如果没值，则声称的路径为Log\Application.{appenderName}.txt
        /// </param>
        /// <param name="additivity">该值指示子记录器是否继承其父级的appender。</param>
        /// <returns></returns>
        internal static ILog GetLogger(string appenderName, string category = null, bool additivity = false)
        {
            return loggerContainer.GetOrAdd(appenderName, delegate (string name)
            {
                RollingFileAppender newAppender = null;
                ReadParamAppender appender = null;
                string file = null;
                if (appenderContainer.ContainsKey(appenderName))
                {
                    appender = appenderContainer[appenderName];
                    newAppender = GetNewFileApender(appender,category);
                    // file = string.IsNullOrEmpty(appender.File) ? GetFile(category, appenderName) : appender.File;
                    //newAppender = GetNewFileApender(appenderName, file, appender.MaxSizeRollBackups,
                    //    appender.AppendToFile, true, appender.MaximumFileSize, RollingFileAppender.RollingMode.Composite, appender.DatePattern, appender.LayoutPattern);
                }
                else
                {
                    file = GetFile(category, appenderName);
                    newAppender = GetNewFileApender(appenderName,file , MAX_SIZE_ROLL_BACKUPS, true, true, MAXIMUM_FILE_SIZE, RollingFileAppender.RollingMode.Composite,
                        DATE_PATTERN, LAYOUT_PATTERN);
                }
                Hierarchy repository = (Hierarchy)LogManager.GetRepository();
                Logger logger = repository.LoggerFactory.CreateLogger(repository, appenderName);
                logger.Hierarchy = repository;
                logger.Parent = repository.Root;
                logger.Level = GetLoggerLevel(appender == null ? LEVEL : appender.Level);
                logger.Additivity = additivity;
                logger.AddAppender(newAppender);
                logger.Repository.Configured = true;
                return new LogImpl(logger);
            });
        }

        //如果没有指定文件路径则在运行路径下建立 Log\Application..{loggerName}.txt
        private static string GetFile(string category, string appenderName)
        {
            if (string.IsNullOrEmpty(category))
            {
                return string.Format(@"Log\Application.{0}.txt", appenderName);
            }
            else
            {
                return string.Format(@"Log\{0}\Application.{1}.txt", category, appenderName);
            }
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
        /// <summary>
        /// 获取一个新的添加器
        /// </summary>
        /// <returns></returns>
        private static RollingFileAppender GetNewFileApender(string appenderName, string file, int maxSizeRollBackups, bool appendToFile = true, bool staticLogFileName = false, string maximumFileSize = "5MB", RollingFileAppender.RollingMode rollingMode = RollingFileAppender.RollingMode.Composite, string datePattern = "yyyyMMdd\".txt\"", string layoutPattern = "%date [%t] %-5level %message%newline")
        {
            RollingFileAppender appender = new RollingFileAppender
            {
                LockingModel = new FileAppender.MinimalLock(),
                Name = appenderName,
                File = file,
                AppendToFile = appendToFile,
                MaxSizeRollBackups = maxSizeRollBackups,
                MaximumFileSize = maximumFileSize,
                StaticLogFileName = staticLogFileName,
                RollingStyle = rollingMode,
                DatePattern = datePattern
            };
            PatternLayout layout = new PatternLayout(layoutPattern);
            appender.Layout = layout;
            layout.ActivateOptions();
            appender.ActivateOptions();
            return appender;
        }
        /// <summary>
        /// 获取一个新的添加器
        /// </summary>
        /// <param name="appender"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        private static RollingFileAppender GetNewFileApender(ReadParamAppender appender,string category = null)
        {
            if (appender.Layout==null && !string.IsNullOrEmpty(appender.LayoutPattern))
            {
                PatternLayout layout = new PatternLayout(appender.LayoutPattern);
                appender.Layout = layout;
                layout.ActivateOptions();
            }
            string file = string.IsNullOrEmpty(appender.File) ? GetFile(category, appender.Name) : appender.File;
            appender.File = file;
            appender.ActivateOptions();
            return appender;
        }
    }
}
