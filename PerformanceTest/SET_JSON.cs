using Northwind.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTest
{
    [System.ComponentModel.Category("Redis.Set")]
    public class BeetleX_Set:BeetleX_Base
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
            await RedisDB.StringSetAsync(item.OrderID.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(item));
        }
    }
    [System.ComponentModel.Category("Redis.Set")]
    public class StackExchange_SyncSet:StackExchangeBase
    {
        public override Task Execute()
        {
            var item = OrderHelper.GetOrder();
            RedisDB.StringSet(item.OrderID.ToString(), Newtonsoft.Json.JsonConvert.SerializeObject(item));
            return base.Execute();
        }
    }
}
