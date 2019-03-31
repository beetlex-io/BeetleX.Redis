using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceTest
{
    public class SET_JSON : TestBase
    {
        public SET_JSON()
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
}
