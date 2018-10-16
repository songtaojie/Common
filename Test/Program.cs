using Common.Helper;
using Common.Log;
using log4net;
using log4net.Config;
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
            ILog log = CustomFileLogger.GetLogger("pay");
            log.Info("asdasdasdas");
            Console.WriteLine("成功");
            Console.ReadLine();
        }
    }
}
