using Common.Helper;
using Common.Logs;
//using log4net;
//using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // XmlConfigurator.Configure(new System.IO.FileInfo("~/App.config"));
            //ILog log = CustomFileLogger.GetLogger("pay");
            //log.Info("asdasdasdas");
            //NLog.ILogger log = CustomNLog.GetLogger("Default");
            //log.Info("Nlog日志");
            ILoggerFactory factory = new LoggerFactory();
            ILogger log = factory.CreateLogger("pay");
            log.Info("测试测试");
            Console.WriteLine("成功");
            Console.ReadLine();
        }
    }
}
