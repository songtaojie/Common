using Autofac;
using Hx.Sdk.NetFramework.Autofac;
using Hx.Sdk.NetFramework.Cache;
using Hx.Sdk.NetFramework.Test.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.NetFramework.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ContainerManager manager = new ContainerManager();
            //manager.BeforeBuild += builder =>
            //{
            //    builder.RegisterType<TestServices>().As<ITestService>();
            //};
            manager.Build(null,builder => 
            {
                builder.RegisterType<TestServices>().As<ITestService>();
            });
            var service =  ContainerManager.Resolve<ITestService>();
            service.TestAutofac();
            Console.WriteLine("end");
            Console.ReadLine();
        }
    }
}
