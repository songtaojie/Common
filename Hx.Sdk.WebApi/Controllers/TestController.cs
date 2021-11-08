using DotNetCore.CAP;
using Hx.Sdk.EventBus;
using Hx.Sdk.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Sdk.WebApi.Controllers
{
    public class TestController: BaseAdminController
    {
        private IEventBus _capPublisher;
        /// <summary>
        /// 控制器
        /// </summary>
        /// <param name="capPublisher"></param>
        public TestController(IEventBus capPublisher)
        {
            _capPublisher = capPublisher;
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
