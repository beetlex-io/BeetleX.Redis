# BeetleX.Redis
A high-performance async/non-blocking  redis client components for dotnet core,default support json and protobuf data format


## Performance test
code:[https://github.com/IKende/BeetleX.Redis/tree/master/PerformanceTest](https://github.com/IKende/BeetleX.Redis/tree/master/PerformanceTest)
```
{"OrderID":10255,"CustomerID":"RICSU","EmployeeID":9,"OrderDate":"1996-07-12T00:00:00","RequiredDate":"1996-08-09T00:00:00","ShippedDate":"1996-07-15T00:00:00","ShipVia":3,"Freight":148.33,"ShipName":"Richter Supermarkt","ShipAddress":"Starenweg 5","ShipCity":"Genève","ShipPostalCode":"1204","ShipCountry":"Switzerland"}
```
## get/set (BeetleX vs  StackExchange)
```
[2019/4/1 8:45:25]BeetleX_GET_JSON 		using 1 threads run 100000 [use time 6.18s 16,179.44/sec]
[2019/4/1 8:45:39]StackExchange_GET_JSON 	using 1 threads run 100000 [use time 13.27s 7,536.65/sec]
[2019/4/1 8:45:40]BeetleX_GET_JSON		using 4 threads run 100000 [use time 1.96s 50,979.21/sec]
[2019/4/1 8:45:43]StackExchange_GET_JSON 	using 4 threads run 100000 [use time 2.48s 40,289.81/sec]
[2019/4/1 8:45:49]BeetleX_SET_JSON 		using 1 threads run 100000 [use time 5.86s 17,070.36/sec]
[2019/4/1 8:46:02]StackExchange_SET_JSON 	using 1 threads run 100000 [use time 12.82s 7,801.39/sec]
[2019/4/1 8:46:04]BeetleX_SET_JSON 		using 4 threads run 100000 [use time 1.92s 51,977.16/sec]
[2019/4/1 8:46:06]StackExchange_SET_JSON 	using 4 threads run 100000 [use time 2.24s 44,628.63/sec]

```

## nuget
https://www.nuget.org/packages/BeetleX.Redis/

## New db client
```
RedisDB DB = new RedisDB();
DB.AddWriteHost("192.168.2.19");
// set password
DB.AddWriteHost("192.168.2.19").Password=123456;
```
### Json db
```
RedisDB DB = new RedisDB(0, new JsonFormater());
```
### Protobuf db
```
RedisDB DB = new RedisDB(0, new ProtobufFormater());
```
### Basic operations
```
await DB.Decr("mykey")
await DB.Decrby("mykey", 5);
await DB.Del("mykey");
await DB.Dump("mykey");
await DB.Exists("mykey", "order");
await DB.Expire("mykey", 10);
await DB.Expireat("mykey", 1293840000);
await DB.Get<string>("mykey");
await DB.GetBit("mykey", 0);
await DB.GetRange("mykey", -3, -1);
await DB.GetSet<string>("mycounter", 0);
await DB.Incr("mykey");
await DB.Incrby("mykey", 10);
await DB.IncrbyFloat("mykey", 0.1f);
await DB.Keys("t??");
await DB.MGet<string, string>("key1", "key2");
await DB.MGet<string, string, string>("key1", "aaa", "key2");
await DB.MSet(m => m["key1", "hello"]["key2", "world"]);
await DB.MSetNX(m => m["key1", "hello"]["key2", "there"]);
await DB.Move("one", 9);
await DB.PSetEX("mykey", 1000, "hello");
await DB.Persist("mykey");
await DB.Pexpire("mykey", 1500);
await DB.Pexpireat("mykey", 1555555555005);
await DB.Ping();
await DB.PTtl("mykey");
await DB.Randomkey();
await DB.Rename("mykey", "myotherkey");
await DB.Renamenx("mykey", "myotherkey");
await DB.Set("test", "henryfan");
await DB.SetBit("mykey", 7, false);
await DB.SetEX("mykey", 10, "hello");
await DB.SetNX("mykey", "hello");
await DB.SetRange("key1", 6, "redis");
await DB.Strlen("key1");
await DB.Type("key2");
```
## List
create list
```
 var list = DB.CreateList<Employee>("employees");
```
### operations
```
await list.BLPop();
await list.BRPop();
await list.BRPopLPush("List2");
await list.Index(0);
await list.Insert(true, GetEmployee(2), GetEmployee(3));
await list.Len();
await list.Pop();
await list.Push(GetEmployee(1));
await list.Push(GetEmployee(1), GetEmployee(2));
await myotherlist.PushX(GetEmployee(2));
await list.Rem(-2, GetEmployee(1));
await list.Set(-2, GetEmployee(5));
await list.Trim(1, -1);
await list.RPop();
await list.RPopLPush("myotherlist");
await list.RPush(GetEmployee(3));
await list.RPush(GetEmployee(1), GetEmployee(2));
await list.RPushX(GetEmployee(2));
await list.Range(-3, 2);
```

## HashTable
create HashTable
```
 var table = DB.CreateHashTable("myhash");
```
### operations
```
await table.Del("emp1");
await table.Exists("emp1");
await table.Get<Employee>("emp1");
await table.Keys();
await table.Len();
await table.Get<Employee, Order>("emp", "order");
await table.Get<Employee, Order, Customer>("emp", "order", "customer");
await table.MSet(m => m["field1", GetEmployee(1)]["field2", GetCustomer(1)]);
await table.Set("field1", GetEmployee(1));
await table.SetNX("field", GetEmployee(1));
```
## Subscribe
## Create subscriber
```
            var sub = db.Subscribe();
            sub.Register<Employee>("test1");
            sub.Receive = (o, e) =>
            {
                Console.WriteLine($"[{DateTime.Now}]{e.Channel}-{e.Type}:{e.Data}");
            };
            sub.Listen();
```
### Publish
```
await DB.Publish("test1", GetEmployee(i));
```



