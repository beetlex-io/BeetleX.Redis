using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace BeetleX.Redis.XUnitTest
{
    public class GEOTester
    {
        readonly ITestOutputHelper Console;

        public GEOTester(ITestOutputHelper output)
        {
            this.Console = output;
            DefaultRedis.Instance.Host.AddWriteHost("localhost");
            DefaultRedis.Instance.DataFormater = new JsonFormater();
            DB = DefaultRedis.Instance;
        }

        private RedisDB DB;

        public DataHelper Data => DataHelper.Defalut;


        private Employee GetEmployee(int id)
        {
            return Data.Employees[id];
        }

        private Order GetOrder(int id)
        {
            return Data.Orders[id];
        }

        private OrderBase GetOrderBase(int id)
        {
            return Data.OrderBases[id];
        }

        private Customer GetCustomer(int id)
        {
            return Data.Customers[id];
        }


        private void Write(object result)
        {
            if (result is System.Collections.IEnumerable && !(result is string))
            {

                foreach (var item in (System.Collections.IEnumerable)result)
                {
                    Console.WriteLine($">>{item?.ToJson()}");
                }
            }
            else
            {
                Console.WriteLine($">>{result?.ToJson()}");
            }
        }
        [Fact]
        public async void GEOAdd()
        {
            var len = await DB.GEOAdd("Sicily", (13.361389, 38.115556, "Palermo"), (15.087269, 37.502669, "Catania"));
            
        }
        [Fact]
        public async void GEOPOS()
        {
            var items = await DB.GEOPos("Sicily", "Palermo", "Catania", "NonExisting");
            Write(items);
        }
        [Fact]
        public async void GEODist()
        {
            var len = await DB.GEOAdd("Sicily", (13.361389, 38.115556, "Palermo"), (15.087269, 37.502669, "Catania"));
            var value = await DB.GEODist("Sicily", "Palermo", "Catania");

            value = await DB.GEODist("Sicily", "Palermo", "Catania", Commands.GEODISTUnit.km);

            value = await DB.GEODist("Sicily", "Palermo", "Catania", Commands.GEODISTUnit.mi);

            value = await DB.GEODist("Sicily", "Foo", "Bar");
            Assert.Equal(-1, value);
        }
        [Fact]
        public async void GEORADIUS()
        {
            var len = await DB.GEOAdd("Sicily", (13.361389, 38.115556, "Palermo"), (15.087269, 37.502669, "Catania"));
            var items = await DB.GEORadius("Sicily", 15, 37, 200, Commands.GEODISTUnit.km);
            Write(items);
        }
        [Fact]
        public async void GEORADIUSBYMEMBER()
        {
            await DB.GEOAdd("Sicily", (13.361389, 38.115556, "Palermo"), (15.087269, 37.502669, "Catania"));
            await DB.GEOAdd("Sicily", (13.583333, 37.316667, "Agrigento"));
            var items = await DB.GEORadiusByMember("Sicily", "Agrigento", 92, Commands.GEODISTUnit.km);
            Write(items);
        }
    }
}
