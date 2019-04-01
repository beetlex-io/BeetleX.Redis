using BeetleX.Redis;
using Northwind.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceTest
{
    public class BeetleX_GET : TestBase
    {
        public BeetleX_GET()
        {
            RedisDB = new RedisDB(0, new JsonFormater());
            RedisDB.AddWriteHost(Program.Host);
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

    public class BeetleX_MGET : TestBase
    {
        public BeetleX_MGET()
        {
            RedisDB = new RedisDB(0, new JsonFormater());
            RedisDB.AddWriteHost(Program.Host);
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
                for (int i = 10248; i <= 11077; i = i + 2)
                {
                    var item = await RedisDB.MGet<Northwind.Data.Order, Northwind.Data.Order>(i.ToString(), (i + 1).ToString());
                    if (!this.Increment())
                    {
                        return;
                    }
                }
            }
        }
    }

    public class StackExchange_GET : TestBase
    {
        public StackExchange_GET()
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


    public class StackExchange_MGET : TestBase
    {
        public StackExchange_MGET()
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
                for (int i = 10248; i <= 11077; i = i + 2)
                {

                    var values = await RedisDB.StringGetAsync(new RedisKey[] { i.ToString(), (i + 1).ToString() });
                    object item1, item2;
                    if (!values[0].IsNullOrEmpty)
                        item1 = Newtonsoft.Json.JsonConvert.DeserializeObject(values[0], typeof(Northwind.Data.Order));
                    if (!values[1].IsNullOrEmpty)
                        item2 = Newtonsoft.Json.JsonConvert.DeserializeObject(values[1], typeof(Northwind.Data.Order));

                    if (!this.Increment())
                    {
                        return;
                    }
                }
            }
        }
    }


    public class StackExchange_Sync_GET : TestBase
    {
        public StackExchange_Sync_GET()
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
                for (int i = 10248; i <= 11077; i++)
                {
                    var data = RedisDB.StringGet(i.ToString());
                    var item = Newtonsoft.Json.JsonConvert.DeserializeObject<Northwind.Data.Order>(data);
                    if (!this.Increment())
                    {
                        return;
                    }
                }
            }
        }
    }

    public class StackExchange_Sync_MGET : TestBase
    {
        public StackExchange_Sync_MGET()
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
                for (int i = 10248; i <= 11077; i++)
                {
                    var values = RedisDB.StringGet(new RedisKey[] { i.ToString(), (i + 1).ToString() });
                    object item1, item2;
                    if (!values[0].IsNullOrEmpty)
                        item1 = Newtonsoft.Json.JsonConvert.DeserializeObject(values[0], typeof(Northwind.Data.Order));
                    if (!values[1].IsNullOrEmpty)
                        item2 = Newtonsoft.Json.JsonConvert.DeserializeObject(values[1], typeof(Northwind.Data.Order));
                    if (!this.Increment())
                    {
                        return;
                    }
                }
            }
        }
    }

}
