using Northwind.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceTest
{
    public class BeetleX_SET : TestBase
    {
        public BeetleX_SET()
        {
            RedisDB = new BeetleX.Redis.RedisDB(0, new BeetleX.Redis.JsonFormater());
            RedisDB.Host.AddWriteHost(Program.Host);
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
                    {
                        return;
                    }
                }
            }
        }
    }

    public class StackExchange_SET : TestBase
    {
        public StackExchange_SET()
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(Program.Host);
            options.AsyncTimeout = 20 * 1000;
            options.SyncTimeout = 20 * 1000;
            Redis = ConnectionMultiplexer.Connect(options);

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
                    {
                        return;
                    }
                }
            }
        }
    }

    public class StackExchange_Sync_SET : TestBase
    {
        public StackExchange_Sync_SET()
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(Program.Host);
            options.AsyncTimeout = 20 * 1000;
            options.SyncTimeout = 20 * 1000;
            Redis = ConnectionMultiplexer.Connect(options);
            RedisDB = Redis.GetDatabase(0);
        }

        private ConnectionMultiplexer Redis;

        private IDatabase RedisDB;

        protected override void OnTest()
        {
            base.OnTest();
            RunTest();
        }

        private void RunTest()
        {
            while (true)
            {
                for (int i = 0; i < DataHelper.Defalut.Orders.Count; i++)
                {
                    var item = DataHelper.Defalut.Orders[i];
                    RedisDB.StringSet(item.OrderID.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(item));
                    if (!Increment())
                    {
                        return;
                    }

                }
            }
        }
    }
}
