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

        private GEO GetGEO()
        {
            return DB.GetGEO();
        }

        [Fact]
        public async void GEOAdd()
        {
            var len = await GetGEO().GEOAdd("Sicily", (13.361389, 38.115556, "Palermo"), (15.087269, 37.502669, "Catania"));

        }
        [Fact]
        public async void GEOPOS()
        {
            var items = await GetGEO().GEOPos("Sicily", "Palermo", "Catania", "NonExisting");
            Write(items);
        }
        [Fact]
        public async void GEODist()
        {
            var len = await GetGEO().GEOAdd("Sicily", (13.361389, 38.115556, "Palermo"), (15.087269, 37.502669, "Catania"));
            var value = await GetGEO().GEODist("Sicily", "Palermo", "Catania");

            value = await GetGEO().GEODist("Sicily", "Palermo", "Catania", Commands.GEODISTUnit.km);

            value = await GetGEO().GEODist("Sicily", "Palermo", "Catania", Commands.GEODISTUnit.mi);

            value = await GetGEO().GEODist("Sicily", "Foo", "Bar");
            Assert.Equal(-1, value);
        }
        [Fact]
        public async void GEORADIUS()
        {
            var len = await GetGEO().GEOAdd("Sicily", (13.361389, 38.115556, "Palermo"), (15.087269, 37.502669, "Catania"));
            var items = await GetGEO().GEORadius("Sicily", 15, 37, 200, Commands.GEODISTUnit.km);
            Write(items);
        }
        [Fact]
        public async void GEORADIUSBYMEMBER()
        {
            await GetGEO().GEOAdd("Sicily", (13.361389, 38.115556, "Palermo"), (15.087269, 37.502669, "Catania"));
            await GetGEO().GEOAdd("Sicily", (13.583333, 37.316667, "Agrigento"));
            var items = await GetGEO().GEORadiusByMember("Sicily", "Agrigento", 92, Commands.GEODISTUnit.km);
            Write(items);
        }
    }
}
