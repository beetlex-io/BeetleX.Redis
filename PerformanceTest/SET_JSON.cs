using Northwind.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceTest
{
    public class BeetleX_SET_JSON : TestBase
    {
        public BeetleX_SET_JSON()
        {
            RedisDB = new BeetleX.Redis.RedisDB(0, new BeetleX.Redis.JsonFormater());
            RedisDB.AddWriteHost("localhost");
        }

        private BeetleX.Redis.RedisDB RedisDB;

        protected override void OnTest()
        {
            base.OnTest();
            RunTest();
        }

        private async void RunTest()
        {
            while (true)
            {
                for (int i = 0; i < DataHelper.Defalut.Orders.Count; i++)
                {
                    var item = DataHelper.Defalut.Orders[i];
                    await RedisDB.Set(item.OrderID.ToString(), item);
                    if (!Increment())
                        return;
                }
            }
        }
    }

    public class StackExchange_SET_JSON : TestBase
    {
        public StackExchange_SET_JSON()
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
                for (int i = 0; i < DataHelper.Defalut.Orders.Count; i++)
                {
                    var item = DataHelper.Defalut.Orders[i];
                    await RedisDB.StringSetAsync(item.OrderID.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(item));
                    if (!Increment())
                        return;
                }
            }
        }
    }
}
