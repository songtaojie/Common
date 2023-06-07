using CSRedis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hx.Sdk.NetCore.Test
{
    static class Program
    {
        static void Main(string[] args)
        {
            //var lockKey = "lockTest";
            //var redisClient = new CSRedisClient("118.31.119.35:6379,defaultDatabase=5,poolsize=50,ssl=false,writeBuffer=10240");
            //RedisHelper.Initialization(redisClient);
            //var iHour = 0;
            //var iMinute = 0;
            //var Interval = 300;
            //var time = "02:00".Split(':');
            //iHour = int.Parse(time[0]);
            //iMinute = int.Parse(time[1]);
            //var now = DateTime.Now;
            //if (now.Hour < iHour) Console.Write("未到上报时间");
            //var oneOClock = DateTime.Today.AddHours(iHour).AddMinutes(iMinute);
            //int msUntilFour = (int)((oneOClock - now).TotalSeconds);
            //if (msUntilFour < 0)
            //{
            //    Interval = 3600 * 12 - msUntilFour;
            //}
            //else
            //{
            //    Interval = msUntilFour;
            //}
            //var cacheValue = RedisHelper.Get("HuiMinCardAPI:UserCard:TransactionRecord");//设置值。默认永不过期
            //var jObject = JObject.Parse(cacheValue);
            //var lastRefundTime = jObject.Value<string>("lastReChargeTime1");

            //Console.WriteLine(lastRefundTime?.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
            //string str1 = "2016191601360271";
            //string str2 = "2016191599808576";
            //Console.WriteLine(str2.CompareTo(str2));

            //Console.WriteLine(Math.Round((decimal)0.27/2,2));
            //Console.WriteLine(Math.Ceiling(value));
            //Console.WriteLine(Compress(Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString()));

            //DateTime.TryParse("2022-01-01", out DateTime dateTime);
            //var endDate = dateTime.AddYears(1).AddDays(-1).ToShortDateString();
            var now = DateTime.UtcNow;
            Console.WriteLine(now.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
            Console.ReadLine();
        }

        public static string Compress(string value)
        {
            try
            {
                string data = string.Empty;
                byte[] byteArray = Encoding.Default.GetBytes(value);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (GZipStream sw = new GZipStream(ms, CompressionMode.Compress))
                    {
                        sw.Write(byteArray, 0, byteArray.Length);
                    }
                    data = Convert.ToBase64String(ms.ToArray());
                }
                return data;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static void Set(int dbid)
        {
            using (var sentinel = new RedisSentinelManager(false, new string[] { "127.0.0.1:26379" }))   //设置哨兵
            {
                sentinel.Connect("mymaster"); // 打开主数据库连接，参数是主数据库别称，在设置哨兵的时候有设置
                var test2 = sentinel.Call(t => t.Select(dbid)); // 使用Call方法可以运行当前主数据的操作，select方法是选择数据库0-14号进行操作
                sentinel.Call(t => t.Set("haha", DateTime.Now.ToString()));//执行方法
            }
        }

        private static bool RedisLock(string lockKey,int delyInterval = 120)
        {
            
            var now = new DateTimeOffset(DateTime.Now);
            var currentTime = now.ToUnixTimeSeconds();
            var isSetNx = RedisHelper.SetNx(lockKey, currentTime + delyInterval);
            if (isSetNx)
            {
                //设置过期时间
                RedisHelper.Expire(lockKey, delyInterval);
                return true;
            }
            else
            { 
                //未找到锁，继续判断，判断时间戳是否可以重置并获取锁
                var localValue = RedisHelper.Get<long>(lockKey);
                if (currentTime > localValue)
                {
                    var getSetResult = RedisHelper.GetSet<long>(lockKey, currentTime + delyInterval);
                    if (getSetResult == localValue)
                    {
                        //设置过期时间
                        RedisHelper.Expire(lockKey, delyInterval);
                        return true;
                    }
                }
            }
            return false;
        }
        
        private static bool DelLock(string lockKey)
        {
            RedisHelper.Del(lockKey);
            return true;
        }

        public static bool IsAssignableFromGenericType(this Type genericType, Type givenType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return genericType.IsAssignableFromGenericType(baseType);
        }
    }

   
}
