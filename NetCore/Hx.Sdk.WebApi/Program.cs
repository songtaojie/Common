using Hx.Sdk.Common.Helper;
using Hx.Sdk.Test.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Sdk.WebApi
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");

            //app.Run();
            //var builder = WebApplication.CreateBuilder(args);
            //var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            try
            {
                var seed = args.Contains("/seed");
                if (seed)
                {
                    args = args.Except(new[] { "/seed" }).ToArray();
                }
                var host = CreateHostBuilder(args).Build();
                host.MigrateDbContext<DefaultDbContext>((db, _) =>
                {
                });
                host.Run();
                ConsoleHelper.WriteSuccessLine("program success", true);
                return 1;
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorLine(string.Format("Error Nessage：{0}", ex.Message));
                logger.Error(ex, "stopped program");
                return 1;
            }
            finally
            {
                // 确保在应用程序退出之前刷新和停止内部计时器/线程（避免Linux上的分段错误）
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHxWebHostDefaults(webBuilder => {
                    webBuilder.UseNLog().UseStartup<Startup>();
                });
    }
}
