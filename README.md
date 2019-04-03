# BeetleX.Redis
A high-performance async/non-blocking  redis client components for dotnet core,default support json and protobuf data format

## nuget
https://www.nuget.org/packages/BeetleX.Redis/

## Setting
```
  Redis.Default.DataFormater = new JsonFormater();
  Redis.Default.Host.AddWriteHost("localhost");
```
## SET/SET
```
await Redis.Get<Employee>("nonexisting");
await Redis.Set("emp3", GetEmployee(3));
await Redis.Get<Employee>("emp3");
```
## MSET/MGET
```
await Redis.Set(("field1", GetEmployee(1)), ("field2", GetEmployee(2)));
await Redis.Get<Employee, Order, Customer>("emp1", "order1", "customer1");
```
## List
```
var list = Redis.CreateList<Employee>("employees");
await list.Push(GetEmployee(1));
await list.Insert(true, GetEmployee(2), GetEmployee(3));
await list.Range(0, -1);
```
## Hash
```
var table = Redis.CreateHashTable("myhash");
await table.MSet(("field1", GetEmployee(1)), ("field2", GetEmployee(2)));
await table.Get<Employee, Employee>("field1", "field2");
await table.Del("emp2");
await table.Keys();
```
## Subscribe
```
var subscribe = Redis.Subscribe();
subscribe.Register<Employee>("employees");
subscribe.Receive = (o, e) =>
{
       Console.WriteLine($"{e.Type} {e.Channel} {e.Data}");
};
subscribe.Listen();
```
```
await Redis.Publish("employees", GetEmployee(1));
```


## New db client
```
RedisDB DB = new RedisDB();
DB.Host.AddWriteHost("192.168.2.19");
// set password
DB.Host.AddWriteHost("192.168.2.19").Password=123456;
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
await DB.MSet(("key1", "hello"),("key2", "world"));
await DB.MSetNX(("key1", "hello"),("key2", "there"));
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
await table.MSet(("field1", GetEmployee(1)),("field2", GetCustomer(1)));
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
## Performance test
code:[https://github.com/IKende/BeetleX.Redis/tree/master/PerformanceTest](https://github.com/IKende/BeetleX.Redis/tree/master/PerformanceTest)
```
{"OrderID":10255,"CustomerID":"RICSU","EmployeeID":9,"OrderDate":"1996-07-12T00:00:00","RequiredDate":"1996-08-09T00:00:00","ShippedDate":"1996-07-15T00:00:00","ShipVia":3,"Freight":148.33,"ShipName":"Richter Supermarkt","ShipAddress":"Starenweg 5","ShipCity":"Genève","ShipPostalCode":"1204","ShipCountry":"Switzerland"}
```
## get/set/mget (BeetleX vs  StackExchange)
**10Gb network**
```
-------------------------------------------------------------------------------
|Name                          | Round| Threads|     Count| Use time(s)|   Sec|
-------------------------------------------------------------------------------
|BeetleX_SET                   |     1|       1|    100000|        5.22| 19157|
-------------------------------------------------------------------------------
|StackExchange_SET             |     1|       1|    100000|        6.97| 14357|
-------------------------------------------------------------------------------
|StackExchange_Sync_SET        |     1|       1|    100000|        6.62| 15103|
-------------------------------------------------------------------------------
|BeetleX_GET                   |     1|       1|    100000|        5.41| 18487|
-------------------------------------------------------------------------------
|StackExchange_GET             |     1|       1|    100000|        7.48| 13378|
-------------------------------------------------------------------------------
|StackExchange_Sync_GET        |     1|       1|    100000|        7.09| 14105|
-------------------------------------------------------------------------------
|BeetleX_MGET                  |     1|       1|    100000|        7.03| 14216|
-------------------------------------------------------------------------------
|StackExchange_MGET            |     1|       1|    100000|        8.69| 11504|
-------------------------------------------------------------------------------
|StackExchange_Sync_MGET       |     1|       1|    100000|        8.36| 11963|
-------------------------------------------------------------------------------
|BeetleX_SET                   |     1|       2|    100000|        2.55| 39246|
-------------------------------------------------------------------------------
|StackExchange_SET             |     1|       2|    100000|        3.97| 25199|
-------------------------------------------------------------------------------
|StackExchange_Sync_SET        |     1|       2|    100000|        3.56| 28069|
-------------------------------------------------------------------------------
|BeetleX_GET                   |     1|       2|    100000|        2.78| 35946|
-------------------------------------------------------------------------------
|StackExchange_GET             |     1|       2|    100000|         4.1| 24364|
-------------------------------------------------------------------------------
|StackExchange_Sync_GET        |     1|       2|    100000|        3.72| 26907|
-------------------------------------------------------------------------------
|BeetleX_MGET                  |     1|       2|    100000|        3.59| 27871|
-------------------------------------------------------------------------------
|StackExchange_MGET            |     1|       2|    100000|        4.75| 21035|
-------------------------------------------------------------------------------
|StackExchange_Sync_MGET       |     1|       2|    100000|        4.55| 21976|
-------------------------------------------------------------------------------
|BeetleX_SET                   |     1|       4|    100000|        2.04| 48956|
-------------------------------------------------------------------------------
|StackExchange_SET             |     1|       4|    100000|        2.37| 42220|
-------------------------------------------------------------------------------
|StackExchange_Sync_SET        |     1|       4|    100000|        2.15| 46541|
-------------------------------------------------------------------------------
|BeetleX_GET                   |     1|       4|    100000|        2.14| 46822|
-------------------------------------------------------------------------------
|StackExchange_GET             |     1|       4|    100000|        2.58| 38789|
-------------------------------------------------------------------------------
|StackExchange_Sync_GET        |     1|       4|    100000|        2.24| 44619|
-------------------------------------------------------------------------------
|BeetleX_MGET                  |     1|       4|    100000|        2.49| 40238|
-------------------------------------------------------------------------------
|StackExchange_MGET            |     1|       4|    100000|        3.06| 32708|
-------------------------------------------------------------------------------
|StackExchange_Sync_MGET       |     1|       4|    100000|        2.76| 36264|
-------------------------------------------------------------------------------
```


