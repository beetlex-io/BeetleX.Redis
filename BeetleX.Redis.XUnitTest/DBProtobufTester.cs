using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace BeetleX.Redis.XUnitTest
{
    public class DBProtobufTester
    {
        readonly ITestOutputHelper Console;

        public DBProtobufTester(ITestOutputHelper output)
        {
            this.Console = output;
            DB.Host.AddWriteHost("192.168.2.19");
        }

        private RedisDB DB = new RedisDB(0, new ProtobufFormater());

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
        public async void Get()
        {
            await DB.Flushall();
            Write(await DB.Get<Employee>("nonexisting"));
            Write(await DB.Set("emp3", GetEmployee(3)));
            Write(await DB.Get<Employee>("emp3"));
        }
        [Fact]
        public async void GetSet()
        {
            await DB.Flushall();
            Write(await DB.GetSet<Employee>("emp1", GetEmployee(1)));
            Write(await DB.Set("emp1", GetEmployee(2)));
            Write(await DB.Get<Employee>("emp1"));
            Write(await DB.GetSet<Employee>("emp1", GetEmployee(1)));
            Write(await DB.Get<Employee>("emp1"));
        }

        [Fact]
        public async void Publish()
        {

            for (int i = 0; i < 5; i++)
            {
                Write(await DB.Publish("test1", GetEmployee(i)));
            }
            System.Threading.Thread.Sleep(1);

        }

        [Fact]
        public async void MGet()
        {
            await DB.Flushall();
            Write(await DB.Set("emp1", GetEmployee(1)));
            Write(await DB.Set("emp2", GetEmployee(2)));
            Write(await DB.MGet<Employee, Employee>("emp2", "emp1"));

        }

        [Fact]
        public async void MGetNull1()
        {
            await DB.Flushall();
            Write(await DB.Set("emp1", GetEmployee(1)));
            Write(await DB.Set("emp2", GetEmployee(2)));
            Write(await DB.MGet<Employee, Employee, OrderBase, Employee>("emp2", "aaa", "sdfds", "emp1"));

        }

        [Fact]
        public async void MGetMultiTypes1()
        {
            Write(await DB.Set("emp1", GetEmployee(1)));
            Write(await DB.Set("order1", GetOrder(1)));
            Write(await DB.Set("customer1", GetCustomer(1)));
            Write(await DB.MGet<Employee, Order, Customer>("emp1", "order1", "customer1"));
        }

        [Fact]
        public async void MGetMultiTypes()
        {
            Write(await DB.Set("emp1", GetEmployee(1)));
            Write(await DB.Set("order1", GetOrder(1)));
            Write(await DB.MGet<Employee, Order>("emp1", "order1"));
        }

        [Fact]
        public async void MGetNull()
        {
            await DB.Flushall();
            Write(await DB.MGet<Employee, Employee>("emp2", "emp1"));

        }



        [Fact]
        public async void MSet()
        {
            await DB.Flushall();
            Write(await DB.MSet(
            ("emp1", GetEmployee(1)),
            ("emp2", GetEmployee(2))
            ));
            Write(await DB.Get<Employee>("emp1"));
            Write(await DB.Get<Employee>("emp2"));
        }
        [Fact]
        public async void MSetNX()
        {
            await DB.Flushall();
            Write(await DB.MSetNX(("key1", GetEmployee(1)),("key2", GetEmployee(2))));
            Write(await DB.MSetNX(("key2", GetEmployee(2)),("key3", GetEmployee(3))));
            var items = await DB.MGet<Employee, Employee, Employee>("key1", "key2", "key3");
            Write(items.Item1);
            Write(items.Item2);
            Write(items.Item3);
        }
        [Fact]
        public async void PSetEX()
        {
            await DB.Flushall();
            Write(await DB.PSetEX("key1", 1000, GetEmployee(1)));
            Write(await DB.PTtl("key1"));
            Write(await DB.Get<Employee>("key1"));
        }
        [Fact]
        public async void Set()
        {
            await DB.Flushall();
            Write(await DB.Set("key1", GetEmployee(4)));
            Write(await DB.Get<Employee>("key1"));
        }
        [Fact]
        public async void SetEX()
        {
            await DB.Flushall();
            Write(await DB.SetEX("key1", 10, GetEmployee(1)));
            Write(await DB.Ttl("key1"));
            Write(await DB.Get<Employee>("key1"));
        }
        [Fact]
        public async void SetNX()
        {
            await DB.Flushall();
            Write(await DB.SetNX("key1", GetEmployee(1)));
            Write(await DB.SetNX("key1", GetEmployee(2)));
            Write(await DB.Get<Employee>("key1"));
        }
    }
}

