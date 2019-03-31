using BeetleX.Redis;
using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceTest
{
    public class GET_JSON : TestBase
    {
        public GET_JSON()
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
                    await RedisDB.Get<Order>(i.ToString());
                    if (!this.Increment())
                    {
                        return;
                    }
                }
            }
        }
    }
}
