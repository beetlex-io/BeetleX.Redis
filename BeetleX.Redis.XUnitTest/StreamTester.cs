using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace BeetleX.Redis.XUnitTest
{
    public class StreamTester
    {
        readonly ITestOutputHelper Console;

        public StreamTester(ITestOutputHelper output)
        {
            this.Console = output;
            DB.DataFormater = new JsonFormater();
            DB.Host.AddWriteHost("127.0.0.1");
        }

        private RedisDB DB = new RedisDB(0);

        private void Write(object result)
        {
            if (result is System.Collections.IEnumerable && !(result is string))
            {
                foreach (var item in (System.Collections.IEnumerable)result)
                {
                    Console.WriteLine($">>{Newtonsoft.Json.JsonConvert.SerializeObject(item)}");
                }
            }
            else
            {
                Console.WriteLine($">>{Newtonsoft.Json.JsonConvert.SerializeObject(result)}");
            }
        }

        [Fact]
        public async void XADD()
        {
            RedisStream<Employee> stream = DB.GetStream<Employee>("employees_stream");
            var id = await stream.Add(DataHelper.Defalut.Employees[0]);
            id = await stream.Add(DataHelper.Defalut.Employees[1]);
            id = await stream.Add(DataHelper.Defalut.Employees[2]);
            var len = await stream.Len();
            Write(len);
        }
        [Fact]
        public async void XLEN()
        {
            RedisStream<Employee> stream = DB.GetStream<Employee>("employees_stream");
            var len = await stream.Len();
            Write(len);
        }

        [Fact]
        public async void XRANGE()
        {
            RedisStream<Employee> stream = DB.GetStream<Employee>("employees_stream");
            var items = await stream.Range();
            Write(items);
        }
        [Fact]
        public async void XREVRANGE()
        {
            RedisStream<Employee> stream = DB.GetStream<Employee>("employees_stream");
            var items = await stream.RevRange();
            Write(items);
        }
        [Fact]
        public async void XREAD()
        {
            RedisStream<Employee> stream = DB.GetStream<Employee>("employees_stream");
            var items = await stream.Read(0, null, "0-0");
            Write(items);
        }
        [Fact]
        public async void XDEL()
        {
            RedisStream<Employee> stream = DB.GetStream<Employee>("employees_stream");
            var items = await stream.Read(null, null, "0-0");
            await stream.Del((from item in items select item.ID).ToArray());
            var len = await stream.Len();
            Assert.Equal(len, 0);
        }
        [Fact]
        public async void XGROUP_CREATE()
        {
            RedisStream<Employee> stream = DB.GetStream<Employee>("employees_stream");
            var group = await stream.GetGroup("henry");
        }
        [Fact]
        public async void XGROUP_READ()
        {
            RedisStream<Employee> stream = DB.GetStream<Employee>("employees_stream");
            var group = await stream.GetGroup("g1");
            var items = await group.Read("henry","0");
            Write(items);

        }
        [Fact]
        public async void XACK()
        {
            RedisStream<Employee> stream = DB.GetStream<Employee>("employees_stream");
            var group = await stream.GetGroup("g1");
            var items = await group.Read("henry","0");
            foreach (var item in items)
                await item.Ack();
            //await group.Ack((from a in items select a.ID).ToArray());
            items = await group.Read("henry","0");
            Write(items);
        }
    }
}
