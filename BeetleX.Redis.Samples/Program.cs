using System;
using Northwind.Data;
using DB = Northwind.Data.DataHelper;
namespace BeetleX.Redis.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Redis.Default.DataFormater = new JsonFormater();
            Redis.Default.Host.AddWriteHost("localhost");
            var subscribe = Redis.Subscribe();
            subscribe.Register<Employee>("employees");
            subscribe.Receive = (o, e) =>
            {
                Console.WriteLine($"{e.Type} {e.Channel} {e.Data}");
            };
            subscribe.Listen();
            System.Threading.Thread.Sleep(1000);
            Test();
            Console.Read();
        }

        static async void Test()
        {
          //  await Redis.Default.Flushall();
            
            Write(await Redis.Get<Employee>("nonexisting"));
            Write(await Redis.Set("emp3", GetEmployee(3)));
            Write(await Redis.Get<Employee>("emp3"));
            Line();
            Write(await Redis.GetSet<Employee>("emp1", GetEmployee(1)));
            Write(await Redis.Set("emp1", GetEmployee(2)));
            Write(await Redis.Get<Employee>("emp1"));
            Write(await Redis.GetSet<Employee>("emp1", GetEmployee(1)));
            Write(await Redis.Get<Employee>("emp1"));
            Line();

            Write(await Redis.Set("emp1", GetEmployee(1)));
            Write(await Redis.Set("emp2", GetEmployee(2)));
            Write(await Redis.Get<Employee, Employee>("emp2", "emp1"));
            Line();
            Write(await Redis.Set("emp1", GetEmployee(1)));
            Write(await Redis.Set("order1", GetOrder(1)));
            Write(await Redis.Set("customer1", GetCustomer(1)));
            Write(await Redis.Get<Employee, Order, Customer>("emp1", "order1", "customer1"));
            Line();
            Write(await Redis.Set(("emp1", GetEmployee(1)), ("emp2", GetEmployee(2))));
            Write(await Redis.Get<Employee>("emp1"));
            Write(await Redis.Get<Employee>("emp2"));
            Line();

            Write(await Redis.SetNX(("key1", GetEmployee(1)), ("key2", GetEmployee(2))));
            Write(await Redis.SetNX(("key2", GetEmployee(2)), ("key3", GetEmployee(3))));
            var items = await Redis.Get<Employee, Employee, Employee>("key1", "key2", "key3");
            Write(items.Item1);
            Write(items.Item2);
            Write(items.Item3);
            Line();

            Write(await Redis.PSetEX("key1", 1000, GetEmployee(1)));
            Write(await Redis.PTtl("key1"));
            Write(await Redis.Get<Employee>("key1"));
            Line();

            Write(await Redis.Set("key1", GetEmployee(4)));
            Write(await Redis.Get<Employee>("key1"));
            Line();

            Write(await Redis.SetEX("key1", 10, GetEmployee(1)));
            Write(await Redis.Ttl("key1"));
            Write(await Redis.Get<Employee>("key1"));
            Line();

            Write(await Redis.SetNX("key1", GetEmployee(1)));
            Write(await Redis.SetNX("key1", GetEmployee(2)));
            Write(await Redis.Get<Employee>("key1"));
            Line();

            Write(await Redis.Set("aa", "sfsdfsd"));
            Write(await Redis.Exists("aa"));
            Write(await Redis.Exists("sdfsdf", "aa"));
            Write(await Redis.Exists("sdfsdf", "sdfsdfdsaa"));
            Line();
            Write(await Redis.Set("mykey", "hello"));
            Write(await Redis.Expire("mykey", 10));
            Write(await Redis.Ttl("mykey"));
            Write(await Redis.Set("mykey", "hello world"));
            Write(await Redis.Ttl("mykey"));
            Line();

            var list = Redis.CreateList<Employee>("employees");
            Write(await list.Push(GetEmployee(1)));
            Write(await list.Push(GetEmployee(2)));
            Write(await list.Index(0));
            Write(await list.Index(-1));
            Write(await list.Index(3));
            Line();

            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.Insert(true, GetEmployee(2), GetEmployee(3)));
            Write(await list.Range(0, -1));
            Line();

            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.Len());
            Line();

            Write(await list.RPush(GetEmployee(1)));
            Write(await list.RPush(GetEmployee(2)));
            Write(await list.RPush(GetEmployee(3)));
            Write(await list.RPush(GetEmployee(4)));
            Write(await list.Pop());
            Write(await list.Range(0, -1));
            Write(await list.Len());
            Line();

            Write(await list.Push(GetEmployee(1)));
            Write(await list.Push(GetEmployee(2)));
            Write(await list.Range(0, -1));
            Line();

          //  await Redis.Default.Flushall();
            var table = Redis.CreateHashTable("myhash");
            Write(await table.MSet(("emp1", GetEmployee(1))));
            Write(await table.Del("emp1"));
            Write(await table.Del("emp2"));
            Write(await table.Len());
            Line();
            Write(await table.MSet(("emp1", GetEmployee(1))));
            Write(await table.Get<Employee>("emp1"));
            Write(await table.Get<Employee>("emp2"));
            Line();
            Write(await table.MSet(("field1", GetEmployee(1)), ("field2", GetEmployee(2))));
            Write(await table.Get<Employee, Employee>("field1", "field2"));
            Write(await table.Keys());
            Line();

            Write(await Redis.Publish("employees", GetEmployee(1)));
            Write(await Redis.Publish("employees", GetEmployee(1)));
            Write(await Redis.Publish("employees", GetEmployee(1)));
        }


        public static DataHelper Data => DataHelper.Defalut;

        private static void Line()
        {
            Console.WriteLine("-----------------------------------------------------------------------------");
        }

        private static Employee GetEmployee(int id)
        {
            return Data.Employees[id];
        }

        private static Order GetOrder(int id)
        {
            return Data.Orders[id];
        }

        private static OrderBase GetOrderBase(int id)
        {
            return Data.OrderBases[id];
        }

        private static Customer GetCustomer(int id)
        {
            return Data.Customers[id];
        }


        private static void Write(object result)
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

    }
    public static class JsonEx
    {
        public static string ToJson(this object st)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(st);
        }
    }
}
