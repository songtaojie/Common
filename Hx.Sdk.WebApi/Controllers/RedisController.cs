using Hx.Sdk.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Sdk.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IRedisCache _redisCache;
        public RedisController(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }

        [HttpPost]
        public string SetRedisValue()
        {
            _redisCache.StringSet("test", "testvalue2", TimeSpan.FromSeconds(30));
            return "成功";
        }

        [HttpGet]
        public string GetRedisValue()
        {
            var testValue = _redisCache.Get<string>("test");
            return testValue;
        }

        [HttpPost]
        public string SetRedisByte()
        {
            var strBytes = System.Text.Encoding.UTF8.GetBytes("testvaluebyte");
            _redisCache.Set(":testbyte", strBytes, new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            { 
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(3)
            });
            return "成功";
        }

        [HttpPost]
        public string GetRedisByte()
        {
            var strBytes = _redisCache.Get(":testbyte");
            return System.Text.Encoding.UTF8.GetString(strBytes);
        }

        [HttpPost]
        public string SetRedisHash()
        {
            _redisCache.HashSet(":testHash", "testvalueHash");
            return "成功";
        }

        [HttpPost]
        public string GetRedisHash()
        {
            var value = _redisCache.HashGet(":testHash");
            return value;
        }
    }
}
