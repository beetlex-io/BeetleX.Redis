using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace BeetleX.Redis.XUnitTest
{
    public class HashTableJsonTester
    {

        readonly ITestOutputHelper Console;

        public HashTableJsonTester(ITestOutputHelper output)
        {
            this.Console = output;
            DB.Host.AddWriteHost("localhost");
        }

        private RedisDB DB = new RedisDB(0, new JsonFormater());

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

        private void Line()
        {
            Write("-------------------------------------------------------------");
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
        public async void HDel()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("emp1", GetEmployee(1))));
            Write(await table.Del("emp1"));
            Write(await table.Del("emp2"));
            Write(await table.Len());
        }
        [Fact]
        public async void HExists()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("emp1", GetEmployee(1))));
            Write(await table.Exists("emp1"));
            Write(await table.Exists("emp2"));
        }
        [Fact]
        public async void HGet()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("emp1", GetEmployee(1))));
            Write(await table.Get<Employee>("emp1"));
            Write(await table.Get<Employee>("emp2"));
        }

        [Fact]
        public async void HKeys()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("field1", GetEmployee(1)), ("field2", GetEmployee(2))));
            Write(await table.Keys());
        }
        [Fact]
        public async void HLen()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(table.MSet(("field1", GetCustomer(1)), ("field2", GetCustomer(2))));
            Write(await table.Len());
        }
        [Fact]
        public async void HMGet()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(
            ("emp", GetEmployee(1)),
            ("order", GetOrder(1)),
            ("customer", GetCustomer(1))
            ));
            var values = await table.Get<Employee, Order, Customer>("emp", "order", "customer");
            Write(values.Item1);
            Write(values.Item2);
            Write(values.Item3);
        }
        [Fact]
        public async void HMGet2()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("emp", GetEmployee(1)),
            ("order", GetOrder(1))
            ));
            var values = await table.Get<Employee, Order>("emp", "order");
            Write(values.Item1);
            Write(values.Item2);

        }

        [Fact]
        public async void HMGet4()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("emp", GetEmployee(1)),
            ("order", GetOrder(1)),
            ("customer", GetCustomer(1)), ("orderbase", GetOrderBase(1))
            ));
            var values = await table.Get<Employee, Order, Customer, OrderBase>("emp", "order", "customer", "orderbase");
            Write(values.Item1);
            Write(values.Item2);
            Write(values.Item3);
            Write(values.Item4);
        }

        [Fact]
        public async void HMGet5()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("emp", GetEmployee(1)),
            ("order", GetOrder(1)),
            ("customer", GetCustomer(1)), ("orderbase", GetOrderBase(1)),
            ("emp2", GetEmployee(2))
            ));
            var values = await table.Get<Employee, Order, Customer, OrderBase, Employee>("emp", "order", "customer", "orderbase", "emp2");
            Write(values.Item1);
            Write(values.Item2);
            Write(values.Item3);
            Write(values.Item4);
            Write(values.Item5);
        }
        [Fact]
        public async void HMGetMultiNull()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(
            ("emp", GetEmployee(1)),
            ("customer", GetCustomer(1)),
            ("orderbase", GetOrderBase(1))
            ));
            var values = await table.Get<Employee, Order, Customer, OrderBase, Employee>("emp", "order", "customer", "orderbase", "emp2");
            Write(values.Item1);
            Write(values.Item2);
            Write(values.Item3);
            Write(values.Item4);
            Write(values.Item5);
        }


        [Fact]
        public async void HMSet()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("field1", GetEmployee(1)), ("field2", GetCustomer(1))));
            Write(await table.Get<Employee>("field1"));
            Write(await table.Get<Customer>("field2"));
        }
        [Fact]
        public async void HSet()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.Set("field1", GetEmployee(1)));
            Write(await table.Get<Employee>("field1"));
        }
        [Fact]
        public async void HSetNX()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.SetNX("field", GetEmployee(1)));
            Write(await table.SetNX("field", GetEmployee(2)));
            Write(await table.Get<Employee>("field"));
        }
        // [Fact]
        public async void HStrLen()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("f1", "helloworld"), ("f2", 99), ("f3", -256)));
            Write(await table.StrLen("f1"));
            Write(await table.StrLen("f2"));
            Write(await table.StrLen("f3"));
        }
    }
}
