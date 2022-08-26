using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Sdk.WebApi.Controllers
{
    public class RedisController : BaseApiController
    {
        public RedisController()
        {
        }

        [HttpPost]
        public string SetRedisValue()
        {
            RedisHelper.Set("test", "testvalue2", TimeSpan.FromSeconds(30));
            return "成功";
        }

        [HttpGet]
        public string GetRedisValue()
        {
            var testValue = RedisHelper.Get("test");
            return testValue;
        }
    }
}
