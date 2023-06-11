using DotNetCore.CAP;
using Hx.Sdk.EventBus;
using Hx.Sdk.SqlSugar;
using Hx.Sdk.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Sdk.WebApi.Controllers
{
    public class TestController: BaseAdminController
    {
        private IEventBus _capPublisher;
        private DbSettingsOptions dbConnectionOptions;
        /// <summary>
        /// 控制器
        /// </summary>
        /// <param name="capPublisher"></param>
        public TestController(IOptions<DbSettingsOptions> options)
        {
            //_capPublisher = capPublisher;
            dbConnectionOptions = options.Value;
        }

        [HttpPost]
        public string PublishTest(BasePush basePush)
        {
            _capPublisher.Publish("Hx.Sdk.Cap.Test", basePush);
            return "ok";
        }

        [HttpPost]
        [CapSubscribe("Hx.Sdk.Cap.Test")]
        public string SubscribeTest(BasePush basePush)
        {
            //_capPublisher.Publish("Hx.Sdk.Cap.Test", basePush);
            return "ok";
        }
    }
}
