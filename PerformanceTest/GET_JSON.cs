using BeetleX.Redis;
using CodeBenchmark;
using Northwind.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTest
{


    [System.ComponentModel.Category("Redis.Get")]
    public class BeetleX_Get : BeetleX_Base
    {
        public async override Task Execute()
        {
            await RedisDB.Get<Northwind.Data.Order>(OrderHelper.GetOrderID().ToString());
        }
    }

    [System.ComponentModel.Category("Redis.MGet")]
    public class BeetleX_MGet : BeetleX_Base
    {
        public async override Task Execute()
        {
            var id = OrderHelper.GetOrderID();
            await RedisDB.MGet<Northwind.Data.Order, Northwind.Data.Order>
                        (id.ToString(),
                        (id + 1).ToString());
        }
    }


    [System.ComponentModel.Category("Redis.Get")]
    public class StackExchange_AsyncGet : StackExchangeBase
    {
        public async override Task Execute()
        {
            var i = OrderHelper.GetOrderID();
            var data = await RedisDB.StringGetAsync(i.ToString());
            var item = System.Text.Json.JsonSerializer.Deserialize<Northwind.Data.Order>(data);
        }
    }

    [System.ComponentModel.Category("Redis.MGet")]
    public class StackExchange_AsyncMGet : StackExchangeBase
    {
        public async override Task Execute()
        {
            var i = OrderHelper.GetOrderID();
            var values = await RedisDB.StringGetAsync(new RedisKey[] { i.ToString(), (i + 1).ToString() });
            object item1, item2;
            if (!values[0].IsNullOrEmpty)
                item1 = System.Text.Json.JsonSerializer.Deserialize<Northwind.Data.Order>(values[0]);
            if (!values[1].IsNullOrEmpty)
                item2 = System.Text.Json.JsonSerializer.Deserialize<Northwind.Data.Order>(values[1]);
        }

    }

    [System.ComponentModel.Category("Redis.Get")]
    public class StackExchange_SyncGet : StackExchangeBase
    {
        public override Task Execute()
        {
            var i = OrderHelper.GetOrderID();
            var data = RedisDB.StringGet(i.ToString());
            var item = System.Text.Json.JsonSerializer.Deserialize<Northwind.Data.Order>(data);
            return base.Execute();
        }
    }

    [System.ComponentModel.Category("Redis.MGet")]
    public class StackExchange_SyncMGet : StackExchangeBase
    {
        public override Task Execute()
        {
            var i = OrderHelper.GetOrderID();
            var values = RedisDB.StringGet(new RedisKey[] { i.ToString(), (i + 1).ToString() });
            object item1, item2;
            if (!values[0].IsNullOrEmpty)
                item1 = System.Text.Json.JsonSerializer.Deserialize<Northwind.Data.Order>(values[0]);
            if (!values[1].IsNullOrEmpty)
                item2 = System.Text.Json.JsonSerializer.Deserialize<Northwind.Data.Order>(values[1]);
            return base.Execute();
        }
    }


    [System.ComponentModel.Category("Redis.Get")]
    public class FreeRedis_Get : FreeRedisBase
    {
        public override Task Execute()
        {
            var i = OrderHelper.GetOrderID();
            var data = cli.Get(i.ToString());
            var item = System.Text.Json.JsonSerializer.Deserialize<Northwind.Data.Order>(data);
            return base.Execute();
        }
    }

    [System.ComponentModel.Category("Redis.MGet")]
    public class FreeRedis_MGet : FreeRedisBase
    {
        public override Task Execute()
        {
            var i = OrderHelper.GetOrderID();
            var values = cli.MGet(i.ToString(), (i + 1).ToString());
            object item1, item2;
            if (!string.IsNullOrEmpty(values[0]))
                item1 = System.Text.Json.JsonSerializer.Deserialize<Northwind.Data.Order>(values[0]);
            if (!string.IsNullOrEmpty(values[1]))
                item2 = System.Text.Json.JsonSerializer.Deserialize<Northwind.Data.Order>(values[1]);
            return base.Execute();
        }

    }


    [System.ComponentModel.Category("Redis.Get")]
    public class NewLife_Get : NewLifeRedisBase
    {
        public override Task Execute()
        {
            var i = OrderHelper.GetOrderID();
            var data = cli.Get<Northwind.Data.Order>(i.ToString());
            return base.Execute();
        }
    }

    [System.ComponentModel.Category("Redis.MGet")]
    public class NewLife_MGet : NewLifeRedisBase
    {
        public override Task Execute()
        {
            var i = OrderHelper.GetOrderID();
            var values = cli.GetAll<Northwind.Data.Order>(new string[] { i.ToString(), (i + 1).ToString() });
            return base.Execute();
        }

    }

}
