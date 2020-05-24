# BeetleX.Redis
A high-performance async/non-blocking  redis client components for dotnet core,default support json and protobuf data format
,support ssl
## Support commands
[AUTH](https://redis.io/commands/AUTH)|
[BLPOP](https://redis.io/commands/BLPOP)|
[BRPOP](https://redis.io/commands/BRPOP)|
[BRPOPLPUSH](https://redis.io/commands/BRPOPLPUSH)|
[DECR](https://redis.io/commands/DECR)|
[DECRBY](https://redis.io/commands/DECRBY)|
[DEL](https://redis.io/commands/DEL)|
[DUMP](https://redis.io/commands/DUMP)|
[EXISTS](https://redis.io/commands/EXISTS)|
[EXPIRE](https://redis.io/commands/EXPIRE)|
[EXPIREAT](https://redis.io/commands/EXPIREAT)|
[FLUSHALL](https://redis.io/commands/FLUSHALL)|
[GET](https://redis.io/commands/GET)|
[GETBIT](https://redis.io/commands/GETBIT)|
[GETRANGE](https://redis.io/commands/GETRANGE)|
[GETSET](https://redis.io/commands/GETSET)|
[HDEL](https://redis.io/commands/HDEL)|
[HEXISTS](https://redis.io/commands/HEXISTS)|
[HGET](https://redis.io/commands/HGET)|
[HGETALL](https://redis.io/commands/HGETALL)|
[HINCRBY](https://redis.io/commands/HINCRBY)|
[HINCRBYFLOAT](https://redis.io/commands/HINCRBYFLOAT)|
[HKEYS](https://redis.io/commands/HKEYS)|
[HLEN](https://redis.io/commands/HLEN)|
[HMGET](https://redis.io/commands/HMGET)|
[HMSET](https://redis.io/commands/HMSET)|
[HSET](https://redis.io/commands/HSET)|
[HSETNX](https://redis.io/commands/HSETNX)|
[HSTRLEN](https://redis.io/commands/HSTRLEN)|
[HVALS](https://redis.io/commands/HVALS)|
[INCR](https://redis.io/commands/INCR)|
[INCRBY](https://redis.io/commands/INCRBY)|
[INCRBYFLOAT](https://redis.io/commands/INCRBYFLOAT)|
[KEYS](https://redis.io/commands/KEYS)|
[LINDEX](https://redis.io/commands/LINDEX)|
[LINSERT](https://redis.io/commands/LINSERT)|
[LLEN](https://redis.io/commands/LLEN)|
[LPOP](https://redis.io/commands/LPOP)|
[LPUSH](https://redis.io/commands/LPUSH)|
[LPUSHX](https://redis.io/commands/LPUSHX)|
[LRANGE](https://redis.io/commands/LRANGE)|
[LREM](https://redis.io/commands/LREM)|
[LSET](https://redis.io/commands/LSET)|
[LTRIM](https://redis.io/commands/LTRIM)|
[MGET](https://redis.io/commands/MGET)|
[MOVE](https://redis.io/commands/MOVE)|
[MSET](https://redis.io/commands/MSET)|
[MSETNX](https://redis.io/commands/MSETNX)|
[OBJECT](https://redis.io/commands/OBJECT)|
[PERSIST](https://redis.io/commands/PERSIST)|
[PEXPIRE](https://redis.io/commands/PEXPIRE)|
[PEXPIREAT](https://redis.io/commands/PEXPIREAT)|
[PING](https://redis.io/commands/PING)|
[PSETEX](https://redis.io/commands/PSETEX)|
[PTTL](https://redis.io/commands/PTTL)|
[PUBLISH](https://redis.io/commands/PUBLISH)|
[RANDOMKEY](https://redis.io/commands/RANDOMKEY)|
[RENAME](https://redis.io/commands/RENAME)|
[RENAMENX](https://redis.io/commands/RENAMENX)|
[RPOP](https://redis.io/commands/RPOP)|
[RPOPLPUSH](https://redis.io/commands/RPOPLPUSH)|
[RPUSH](https://redis.io/commands/RPUSH)|
[RPUSHX](https://redis.io/commands/RPUSHX)|
[SCAN](https://redis.io/commands/SCAN)|
[SELECT](https://redis.io/commands/SELECT)|
[SET](https://redis.io/commands/SET)|
[SETBIT](https://redis.io/commands/SETBIT)|
[SETEX](https://redis.io/commands/SETEX)|
[SETNX](https://redis.io/commands/SETNX)|
[SETRANGE](https://redis.io/commands/SETRANGE)|
[STRLEN](https://redis.io/commands/STRLEN)|
[SUBSCRIBE](https://redis.io/commands/SUBSCRIBE)|
[TOUCH](https://redis.io/commands/TOUCH)|
[TTL](https://redis.io/commands/TTL)|
[TYPE](https://redis.io/commands/TYPE)|
[UNLINK](https://redis.io/commands/UNLINK)|
[UNSUBSCRIBE](https://redis.io/commands/UNSUBSCRIBE)|
[WAIT](https://redis.io/commands/WAIT)|
[ZADD](https://redis.io/commands/ZADD)|
[ZCARD](https://redis.io/commands/ZCARD)|
[ZCOUNT](https://redis.io/commands/ZCOUNT)|
[ZINCRBY](https://redis.io/commands/ZINCRBY)|
[ZINTERSTORE](https://redis.io/commands/ZINTERSTORE)|
[ZLEXCOUNT](https://redis.io/commands/ZLEXCOUNT)|
[ZRANGE](https://redis.io/commands/ZRANGE)|
[ZRANGEBYLEX](https://redis.io/commands/ZRANGEBYLEX)|
[ZRANGEBYSCORE](https://redis.io/commands/ZRANGEBYSCORE)|
[ZRANK](https://redis.io/commands/ZRANK)|
[ZREM](https://redis.io/commands/ZREM)|
[ZREMRANGEBYLEX](https://redis.io/commands/ZREMRANGEBYLEX)|
[ZREMRANGEBYRANK](https://redis.io/commands/ZREMRANGEBYRANK)|
[ZREMRANGEBYSCORE](https://redis.io/commands/ZREMRANGEBYSCORE)|
[ZREVRANGE](https://redis.io/commands/ZREVRANGE)|
[ZREVRANGEBYSCORE](https://redis.io/commands/ZREVRANGEBYSCORE)|
[ZREVRANK](https://redis.io/commands/ZREVRANK)|
[ZSCORE](https://redis.io/commands/ZSCORE)|
[ZUNIONSTORE](https://redis.io/commands/ZUNIONSTORE)|
[PFCount](https://redis.io/commands/PFCount)|
[PFAdd](https://redis.io/commands/PFAdd)|
[PFMerge](https://redis.io/commands/PFMerge)|
[INFO](https://redis.io/commands/INFO)|
## nuget
https://www.nuget.org/packages/BeetleX.Redis/

## Setting
``` csharp
Redis.Default.DataFormater = new JsonFormater();
Redis.Default.Host.AddWriteHost("localhost");
//ssl
Redis.Default.Host.AddWriteHost("localhost",6378,true);
//password 
Redis.Default.Host.AddWriteHost("localhost").Password="123456"
```
## Create db
``` csharp
RedisDB DB = new RedisDB(1);
DB.DataFormater = new JsonFormater();
DB.Host.AddWriteHost("localhost");
  //SSL
DB.Host.AddWriteHost("localhost",6378,true);
  //password 
DB.Host.AddWriteHost("localhost").Password="123456"
```

## SET/SET
``` csharp
await Redis.Get<Employee>("nonexisting");
await Redis.Set("emp3", GetEmployee(3));
await Redis.Get<Employee>("emp3");
```
## MSET/MGET
``` csharp
await Redis.Set(("field1", GetEmployee(1)), ("field2", GetEmployee(2)));
await Redis.Get<Employee, Order, Customer>("emp1", "order1", "customer1");
```
## List
``` csharp
var list = Redis.CreateList<Employee>("employees");
await list.Push(GetEmployee(1));
await list.Insert(true, GetEmployee(2), GetEmployee(3));
await list.Range(0, -1);
```

## Sequence
``` csharp
var sequeue = DB.CreateSequence("seq2");
await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
var items = await sequeue.ZRange(0, -1, true);
```
## Hash
``` csharp
var table = Redis.CreateHashTable("myhash");
await table.MSet(("field1", GetEmployee(1)), ("field2", GetEmployee(2)));
await table.Get<Employee, Employee>("field1", "field2");
await table.Del("emp2");
await table.Keys();
```
## Subscribe
``` csharp
var subscribe = Redis.Subscribe();
subscribe.Register<Employee>("employees");
subscribe.Receive = (o, e) =>
{
       Console.WriteLine($"{e.Type} {e.Channel} {e.Data}");
};
subscribe.Listen();
```
``` csharp
await Redis.Publish("employees", GetEmployee(1));
```


## New db client
``` csharp
RedisDB DB = new RedisDB();
DB.Host.AddWriteHost("192.168.2.19");
// set password
DB.Host.AddWriteHost("192.168.2.19").Password=123456;
```
### Json db
``` csharp
RedisDB DB = new RedisDB(0, new JsonFormater());
```
### Protobuf db
``` csharp
RedisDB DB = new RedisDB(0, new ProtobufFormater());
```
### Basic operations
``` csharp
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
``` csharp
 var list = DB.CreateList<Employee>("employees");
```
### operations
``` csharp
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
## Sequence
``` csharp
await DB.Del("seq2");
var sequeue = DB.CreateSequence("seq2");
await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
var items = await sequeue.ZRange(0, -1, true);
Assert.Equal<int>(items.Count, 4);
Assert.Equal<string>(items[0].Member, "A1");
Assert.Equal<string>(items[1].Member, "A2");
Assert.Equal<string>(items[2].Member, "A3");
Assert.Equal<string>(items[3].Member, "A4");

Assert.Equal<double>(items[0].Score, 100);
Assert.Equal<double>(items[1].Score, 200);
Assert.Equal<double>(items[2].Score, 300);
Assert.Equal<double>(items[3].Score, 400);
//--------------------------------------------------------------------
await DB.Del("seq2");
var sequeue = DB.CreateSequence("seq2");
await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
var value = await sequeue.ZRevRank("A4");
Assert.Equal<long>(value, 0);

//------------------------------------------------------------------------
await DB.Del("seq2","seq3","seq4");
var seq2 = DB.CreateSequence("seq2");
await seq2.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));

var seq3 = DB.CreateSequence("seq2");
await seq3.ZAdd((500, "B1"), (600, "B2"), (700, "B3"), (800, "B4"));

var seq4 = DB.CreateSequence("seq4");
await seq4.ZUnionsStore("seq2", "seq3");

var count = await seq4.ZCard();
Assert.Equal<long>(count, 8);
```
## HashTable
create HashTable
``` csharp
 var table = DB.CreateHashTable("myhash");
```
### operations
``` csharp
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
``` csharp
            var sub = db.Subscribe();
            sub.Register<Employee>("test1");
            sub.Receive = (o, e) =>
            {
                Console.WriteLine($"[{DateTime.Now}]{e.Channel}-{e.Type}:{e.Data}");
            };
            sub.Listen();
```
### Publish
``` csharp
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


