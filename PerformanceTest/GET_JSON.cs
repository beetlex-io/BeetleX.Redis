using BeetleX.Redis;
using Northwind.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceTest
{
    public class BeetleX_GET_JSON : TestBase
    {
        public BeetleX_GET_JSON()
        {
            RedisDB = new RedisDB(0, new JsonFormater());
            RedisDB.AddWriteHost("localhost");
        }

        private RedisDB RedisDB;


        protected override void OnTest()
        {
            base.OnTest();
            RunTest();
        }

        private async void RunTest()
        {
            while (true)
            {
                for (int i = 10248; i <= 11077; i++)
                {
                    await RedisDB.Get<Northwind.Data.Order>(i.ToString());
                    if (!this.Increment())
                    {
                        return;
                    }
                }
            }
        }
    }

    public class StackExchange_GET_JSON : TestBase
    {
        public StackExchange_GET_JSON()
        {
            Redis = ConnectionMultiplexer.Connect("localhost");
            RedisDB = Redis.GetDatabase(0);
        }

        private ConnectionMultiplexer Redis;

        private IDatabase RedisDB;

        protected override void OnTest()
        {
            base.OnTest();
            RunTest();
        }

        private async void RunTest()
        {
            while (true)
            {
                for (int i = 10248; i <= 11077; i++)
                {
                    var data = await RedisDB.StringGetAsync(i.ToString());
                    var item = Newtonsoft.Json.JsonConvert.DeserializeObject<Northwind.Data.Order>(data);
                    if (!this.Increment())
                    {
                        return;
                    }
                }
            }
        }
    }

}
