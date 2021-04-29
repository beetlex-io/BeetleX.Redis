using Northwind.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTest
{
    [System.ComponentModel.Category("Redis.Set")]
    public class BeetleX_Set : BeetleX_Base
    {
        public async override Task Execute()
        {
            var item = OrderHelper.GetOrder();
            await RedisDB.Set(item.OrderID.ToString(), item);
        }
    }
    [System.ComponentModel.Category("Redis.Set")]
    public class StackExchange_AsyncSet : StackExchangeBase
    {
        public async override Task Execute()
        {
            var item = OrderHelper.GetOrder();
            await RedisDB.StringSetAsync(item.OrderID.ToString(), System.Text.Json.JsonSerializer.Serialize(item));
        }
    }
    [System.ComponentModel.Category("Redis.Set")]
    public class StackExchange_SyncSet : StackExchangeBase
    {
        public override Task Execute()
        {
            var item = OrderHelper.GetOrder();
            RedisDB.StringSet(item.OrderID.ToString(), System.Text.Json.JsonSerializer.Serialize(item));
            return base.Execute();
        }
    }

    [System.ComponentModel.Category("Redis.Set")]
    public class FreeRedis_Set : FreeRedisBase
    {
        public override Task Execute()
        {
            var item = OrderHelper.GetOrder();
            cli.Set(item.OrderID.ToString(), System.Text.Json.JsonSerializer.Serialize(item));
            return base.Execute();
        }
    }

    //[System.ComponentModel.Category("Redis.Set")]
    //public class NewLife_Set : NewLifeRedisBase
    //{
    //    public override Task Execute()
    //    {
    //        var item = OrderHelper.GetOrder();
    //        cli.Set<Northwind.Data.Order>(item.OrderID.ToString(), item);
    //        return base.Execute();
    //    }
    //}
}
