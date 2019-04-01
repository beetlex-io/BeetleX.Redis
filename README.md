# BeetleX.Redis
A high-performance async/non-blocking  redis client components for dotnet core,default support json and protobuf data format

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
## Performance test
code:[https://github.com/IKende/BeetleX.Redis/tree/master/PerformanceTest](https://github.com/IKende/BeetleX.Redis/tree/master/PerformanceTest)
```
{"OrderID":10255,"CustomerID":"RICSU","EmployeeID":9,"OrderDate":"1996-07-12T00:00:00","RequiredDate":"1996-08-09T00:00:00","ShippedDate":"1996-07-15T00:00:00","ShipVia":3,"Freight":148.33,"ShipName":"Richter Supermarkt","ShipAddress":"Starenweg 5","ShipCity":"Genève","ShipPostalCode":"1204","ShipCountry":"Switzerland"}
```
## get/set/mget (BeetleX vs  StackExchange)
**10Gb network**
```
|Name                          | Round| Threads|     Count| Use time(s)|   Sec|     cpu(ms)|
--------------------------------------------------------------------------------------------
|BeetleX_SET                   |     1|       1|    100000|       13.02|  7680|     6162.04|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_SET        |     1|       1|    100000|       13.35|  7491|    14439.70|
--------------------------------------------------------------------------------------------
|BeetleX_GET                   |     1|       1|    100000|        12.8|  7810|    21240.74|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_GET        |     1|       1|    100000|       14.46|  6914|    29221.99|
--------------------------------------------------------------------------------------------
|BeetleX_MGET                  |     1|       1|    100000|       15.31|  6532|    37456.24|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_MGET       |     1|       1|    100000|       17.55|  5697|    47140.31|
--------------------------------------------------------------------------------------------
|Name                          | Round| Threads|     Count| Use time(s)|   Sec|     cpu(ms)|
--------------------------------------------------------------------------------------------
|BeetleX_SET                   |     1|       2|    100000|        6.35| 15744|    53179.54|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_SET        |     1|       2|    100000|        8.94| 11187|    60223.39|
--------------------------------------------------------------------------------------------
|BeetleX_GET                   |     1|       2|    100000|        6.64| 15062|    67129.63|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_GET        |     1|       2|    100000|        9.27| 10790|    73568.28|
--------------------------------------------------------------------------------------------
|BeetleX_MGET                  |     1|       2|    100000|       10.01|  9995|    84077.94|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_MGET       |     1|       2|    100000|       11.42|  8757|    93501.40|
--------------------------------------------------------------------------------------------
|Name                          | Round| Threads|     Count| Use time(s)|   Sec|     cpu(ms)|
--------------------------------------------------------------------------------------------
|BeetleX_SET                   |     1|       4|    200000|        7.84| 25525|   102328.86|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_SET        |     1|       4|    200000|       11.12| 17980|   116947.35|
--------------------------------------------------------------------------------------------
|BeetleX_GET                   |     1|       4|    200000|        8.02| 24942|   128934.43|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_GET        |     1|       4|    200000|        11.2| 17854|   139920.50|
--------------------------------------------------------------------------------------------
|BeetleX_MGET                  |     1|       4|    200000|       10.07| 19858|   156285.41|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_MGET       |     1|       4|    200000|       12.58| 15901|   173208.91|
--------------------------------------------------------------------------------------------
|Name                          | Round| Threads|     Count| Use time(s)|   Sec|     cpu(ms)|
--------------------------------------------------------------------------------------------
|BeetleX_SET                   |     1|       8|    500000|       12.71| 39346|   207118.33|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_SET        |     1|       8|    500000|       14.91| 33537|   235376.91|
--------------------------------------------------------------------------------------------
|BeetleX_GET                   |     1|       8|    500000|       13.53| 36960|   276075.98|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_GET        |     1|       8|    500000|       15.38| 32513|   306656.77|
--------------------------------------------------------------------------------------------
|BeetleX_MGET                  |     1|       8|    500000|       15.26| 32762|   354142.67|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_MGET       |     1|       8|    500000|       16.11| 31035|   387026.09|
--------------------------------------------------------------------------------------------
|Name                          | Round| Threads|     Count| Use time(s)|   Sec|     cpu(ms)|
--------------------------------------------------------------------------------------------
|BeetleX_SET                   |     1|      16|    500000|        7.55| 66241|   413754.26|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_SET        |     1|      16|    500000|        8.19| 61013|   436199.20|
--------------------------------------------------------------------------------------------
|BeetleX_GET                   |     1|      16|    500000|        7.87| 63504|   466716.80|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_GET        |     1|      16|    500000|        7.99| 62558|   491383.75|
--------------------------------------------------------------------------------------------
|BeetleX_MGET                  |     1|      16|    500000|        7.99| 62581|   531888.62|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_MGET       |     1|      16|    500000|        8.46| 59128|   563731.22|
--------------------------------------------------------------------------------------------
|Name                          | Round| Threads|     Count| Use time(s)|   Sec|     cpu(ms)|
--------------------------------------------------------------------------------------------
|BeetleX_SET                   |     1|      24|    500000|         6.3| 79421|   590660.79|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_SET        |     1|      24|    500000|         6.2| 80618|   612248.13|
--------------------------------------------------------------------------------------------
|BeetleX_GET                   |     1|      24|    500000|        6.15| 81252|   642794.72|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_GET        |     1|      24|    500000|        5.74| 87070|   666946.88|
--------------------------------------------------------------------------------------------
|BeetleX_MGET                  |     1|      24|    500000|        6.82| 73320|   708263.14|
--------------------------------------------------------------------------------------------
|StackExchange_Sync_MGET       |     1|      24|    500000|        7.55| 66207|   742491.96|
--------------------------------------------------------------------------------------------

```


