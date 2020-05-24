using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace BeetleX.Redis.XUnitTest
{
    public class ListJsonTester
    {
        readonly ITestOutputHelper Console;

        public ListJsonTester(ITestOutputHelper output)
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
        public async void BLPop()
        {

            var list = DB.CreateList<Employee>("employees");
            Write(await list.BLPop(30));
        }
        [Fact]
        public async void BRPop()
        {

            var list = DB.CreateList<Employee>("employees");
            Write(await list.BRPop());
        }
        [Fact]
        public async void BRPopLPush()
        {

            var list = DB.CreateList<Employee>("employees");
            Write(await list.BRPopLPush("employees1"));
        }
        [Fact]
        public async void LIndex()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.Push(GetEmployee(1)));
            Write(await list.Push(GetEmployee(2)));
            Write(await list.Index(0));
            Write(await list.Index(-1));
            Write(await list.Index(3));
        }
        [Fact]
        public async void LInsert()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.Insert(true, GetEmployee(2), GetEmployee(3)));
            Write(await list.Range(0, -1));
        }

        [Fact]
        public async void LLen()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(list.Len());
        }
        [Fact]
        public async void LPop()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.RPush(GetEmployee(3)));
            Write(await list.RPush(GetEmployee(4)));
            Write(await list.Pop());
            Write(await list.Range(0, -1));
            Write(await list.Len());
        }



        [Fact]
        public async void LPush()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.Push(GetEmployee(1)));
            Write(await list.Push(GetEmployee(2)));
            Write(await list.RPush(GetEmployee(3)));
            Write(await list.RPush(GetEmployee(4)));
            Write(await list.Range(0, -1));
        }


        [Fact]
        public async void LPushMulti()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.Push(GetEmployee(1), GetEmployee(2)));
            Write(await list.Range(0, -1));
        }

        [Fact]
        public async void LPushX()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.Push(GetEmployee(1)));
            Write(await list.PushX(GetEmployee(2)));
            var myotherlist = DB.CreateList<Employee>("myotherlist ");
            Write(await myotherlist.PushX(GetEmployee(2)));
            Write(await list.Range(0, -1));
            Write(await myotherlist.Range(0, -1));

        }
        [Fact]
        public async void Range()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.RPush(GetEmployee(3)));
            Write(await list.Range(0, 0));
            Line();
            Write(await list.Range(-3, 2));
            Line();
            Write(await list.Range(-100, 100));
            Line();
            Write(await list.Range(5, 10));
        }
        [Fact]
        public async void LRem()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.Rem(-2, GetEmployee(1)));
            Write(await list.Range(0, -1));
        }

        [Fact]
        public async void LSet()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.RPush(GetEmployee(3)));
            Write(await list.Set(0, GetEmployee(4)));
            Write(await list.Set(-2, GetEmployee(5)));
            Write(await list.Range(0, -1));
        }

        [Fact]
        public async void LTrim()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.RPush(GetEmployee(3)));
            Write(await list.Trim(1, -1));
            Write(await list.Range(0, -1));
        }

        [Fact]
        public async void RPop()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.RPush(GetEmployee(3)));
            Write(await list.RPop());
            Line();
            Write(await list.Range(0, -1));
        }
        [Fact]
        public async void RPopLPush()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.RPush(GetEmployee(3)));
            Write(await list.RPopLPush("myotherlist"));
            Write(await list.Range(0, -1));
            var otherList = DB.CreateList<Employee>("myotherlist");
            Write(await otherList.Range(0, -1));
        }
        [Fact]
        public async void RPush()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.Range(0, -1));
        }
        [Fact]
        public async void RPushMulti()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            Write(await list.RPush(GetEmployee(1), GetEmployee(2)));
            Write(await list.Range(0, -1));
        }

        [Fact]
        public async void RPushX()
        {
            await DB.Flushall();
            var list = DB.CreateList<Employee>("employees");
            var otherlist = DB.CreateList<Employee>("othrelist");
            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPushX(GetEmployee(2)));
            Write(await otherlist.RPushX(GetEmployee(2)));
            Write(await list.Range(0, -1));
            Write(await otherlist.Range(0, -1));
        }
    }
}
