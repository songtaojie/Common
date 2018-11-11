using Common.Email;
using Common.Helper;
using Common.Logs;
using Common.Memcached;
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
            //ILoggerFactory factory = new LoggerFactory(LoggerType.NLog);
            ////factory.UseConfig = true;
            ////factory.ConfigPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + (@"\Config\nlog.config");
            //ILogger log = factory.CreateLogger();
            //log.Info("测试测试");
            //Console.WriteLine("成功");
            ////测试邮件发送
            Console.WriteLine("开始异步发送邮件,时间:" + DateTime.Now.ToString());
            SendEmail();
            Console.WriteLine("邮件正在异步发送,时间:" + DateTime.Now.ToString());
            //System.Threading.Thread.Sleep(1000);
            // email.SendAsyncCancel();
            //测试Memcached
            //MemcachedHelper.Set("test", "10");
            //Console.WriteLine(MemcachedHelper.Get("test"));
            //MemcachedHelper.Delete("test");
            //Console.WriteLine(MemcachedHelper.Get("test"));
            Console.ReadLine();
        }
        public static async void SendEmail()
        {
            EmailHelper email = new EmailHelper()
            {
                MailSubject = "欢迎您注册 海星·博客",
                MailBody = EmailHelper.TempBody("宋先生", "请复制打开链接(或者右键新标签中打开)，激活账号", "链接地址"),
                MailToArray = new string[] { "977601042@qq.com" }

            };
            
            await Task.Run(()=> email.SendAsync(emailCompleted));
        }
        static void emailCompleted(bool success,Exception ex)
        {
            //延时1秒
            Console.WriteLine();
            Console.WriteLine("邮件发送结果:\r\n" + (success ? "邮件发送成功" : "邮件发送失败") + ",时间:" + DateTime.Now.ToString());
            //写入日志
        }
    }
}
