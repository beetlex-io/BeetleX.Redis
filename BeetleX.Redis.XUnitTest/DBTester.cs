using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace BeetleX.Redis.XUnitTest
{
    public class DBTester
    {
        readonly ITestOutputHelper Console;

        public DBTester(ITestOutputHelper output)
        {
            this.Console = output;
            DB.Host.AddWriteHost("127.0.0.1");
            DB.KeyPrefix = "BeetleX";
            DB.AutoPing = false;
        }

        private RedisDB DB = new RedisDB(0);


        private void Write(object result)
        {
            if (result is System.Collections.IEnumerable && !(result is string))
            {
                foreach (var item in (System.Collections.IEnumerable)result)
                {
                    Console.WriteLine($">>{item}");
                }
            }
            else
            {
                Console.WriteLine($">>{result}");
            }
        }
        [Fact]
        public async void DBDisposed()
        {
            using (RedisDB db = new RedisDB())
            {
                db.Host.AddWriteHost("192.168.2.19");
                await db.Ping();
            }
        }

        [Fact]
        public async void Info()
        {
            var info = await DB.Info();
            Write(info);
        }

        [Fact]
        public async void PF()
        {
            await DB.PFAdd("abc", "a", "b", "c", "d");
            var count = await DB.PFCount("abc");
            Assert.Equal<long>(count, 4l);
            await DB.PFAdd("abc1", "d", "e", "f", "g");
            count = await DB.PFCount("abc1");
            Assert.Equal<long>(count, 4l);
            await DB.PFMerge("abc2", "abc", "abc1");
            count = await DB.PFCount("abc2");
            Assert.Equal<long>(count, 7l);

        }


        [Fact]
        public async void GetSetBytes()
        {
            var data = Encoding.UTF8.GetBytes("henryfan@msn.com");
            await DB.Set("bytes", new ArraySegment<byte>(data));
            var result = await DB.Get<ArraySegment<byte>>("bytes");
            Assert.Equal<string>(Encoding.UTF8.GetString(result.Array, 0, result.Count), "henryfan@msn.com");
        }

        [Fact]
        public async void Set()
        {
            var result = await DB.Set("test", "henryfan1");
            Write(result);

            var value = await DB.Get<string>("test");
            Write(value);
            Assert.Equal<string>(value, "henryfan1");

            var emptyResult = await DB.Set("test", "");
            Write(emptyResult);

            var emptyValue = await DB.Get<string>("test");
            Write(emptyValue);
            Assert.Equal<string>(emptyValue, null);


            var nullResult = await DB.Set("test", null);
            Write(nullResult);

            var nullValue = await DB.Get<string>("test");
            Write(nullValue);
            Assert.Equal<string>(nullValue, null);

        }

        [Fact]
        public async void Ping()
        {
            var result = await DB.Ping();
            Console.WriteLine(result.ToString());
        }
        [Fact]
        public async void Del()
        {
            var add = await DB.Set("test", "henryfan");
            Write(add);
            var del = await DB.Del("test");
            Write(del);
        }
        [Fact]
        public async void Dump()
        {
            var add = await DB.Set("test", "henryfan");
            Write(add);
            var result = await DB.Dump("test");
            Write(result);
        }
        [Fact]
        public async void Exists()
        {
            var add = await DB.Set("aa", "sfsdfsd");
            Write(add);
            var count = await DB.Exists("aa");
            Write(count);
            count = await DB.Exists("sdfsdf", "aa");
            Write(count);
            count = await DB.Exists("sdfsdf", "sdfsdfdsaa");
            Write(count);
        }
        [Fact]
        public async void Expire()
        {
            var add = await DB.Set("mykey", "hello");
            Write(add);
            var expire = await DB.Expire("mykey", 10);
            Write(expire);
            var ttl = await DB.Ttl("mykey");
            Write(ttl);
            add = await DB.Set("mykey", "hello world");
            Write(add);
            ttl = await DB.Ttl("mykey");
            Write(ttl);
        }
        [Fact]
        public async void Expireat()
        {
            var set = await DB.Set("mykey", "hello");
            Write(set);
            var extist = await DB.Exists("mykey");
            Write(extist);
            var expireat = await DB.Expireat("mykey", 1293840000);
            Write(expireat);
            extist = await DB.Exists("mykey");
            Write(extist);
        }
        [Fact]
        public async void MSet()
        {
            var result = await DB.MSet(("key1", "hello"), ("key2", "world"));
            Write(result);
            var get = await DB.Get<string>("key1");
            Write(get);
            get = await DB.Get<string>("key2");
            Write(get);
        }
        [Fact]
        public async void Keys()
        {
            await DB.Flushall();
            var mset = await DB.MSet(("one", 1), ("tow", 2), ("three", 2), ("four", 4));
            Write(mset);
            var keys = await DB.Keys("*");
            Write(keys);
            keys = await DB.Keys("t??");
            Write(keys);
            keys = await DB.Keys("*");
            Write(keys);
        }
        [Fact]
        public async void Move()
        {
            var move = await DB.Move("one", 9);
            Write(move);
        }
        [Fact]
        public async void Persist()
        {
            var set = await DB.Set("mykey", "hello");
            Write(set);
            var expire = await DB.Expire("mykey", 10);
            Write(expire);
            var ttl = await DB.Ttl("mykey");
            Write(ttl);
            var persist = await DB.Persist("mykey");
            Write(persist);
            ttl = await DB.Ttl("mykey");
            Write(ttl);
        }
        [Fact]
        public async void Pexpire()
        {
            var set = await DB.Set("mykey", "hello");
            Write(set);
            var pexpire = await DB.Pexpire("mykey", 1500);
            Write(pexpire);
            var ttl = await DB.Ttl("mykey");
            Write(ttl);
            ttl = await DB.PTtl("mykey");
            Write(ttl);
        }

        [Fact]
        public async void Pexpireat()
        {
            var set = await DB.Set("mykey", "hello");
            Write(set);
            var pexpireat = await DB.Pexpireat("mykey", 1555555555005);
            Write(pexpireat);
            var ttl = await DB.Ttl("mykey");
            Write(ttl);
            var pttl = await DB.PTtl("mykey");
            Write(pttl);
        }
        [Fact]
        public async void Pttl()
        {
            var set = await DB.Set("mykey", "hello");
            Write(set);
            var expire = await DB.Expire("mykey", 1);
            Write(expire);
            var pttl = await DB.PTtl("mykey");
            Write(pttl);
        }
        [Fact]
        public async void Randomkey()
        {
            var key = await DB.Randomkey();
            Write(key);
        }
        [Fact]
        public async void Rename()
        {
            var set = await DB.Set("mykey", "hello");
            Write(set);
            var ren = await DB.Rename("mykey", "myotherkey");
            Write(set);
            var get = await DB.Get<string>("myotherkey");
            Write(get);
        }
        [Fact]
        public async void Renamenx()
        {
            await DB.Flushall();
            var set = await DB.Set("mykey", "hello");
            Write(set);
            set = await DB.Set("myotherkey", "World");
            Write(set);
            var ren = await DB.Renamenx("mykey", "myotherkey");
            Write(ren);
            var get = await DB.Get<string>("myotherkey");
            Write(get);
        }
        // [Fact]
        public async void Touch()
        {
            await DB.Flushall();
            var set = await DB.Set("key1", "hello");
            Write(set);
            set = await DB.Set("key2", "hello");
            Write(set);
            var touch = await DB.Touch("key1", "key2");
            Write(touch);
        }
        [Fact]
        public async void Type()
        {
            await DB.Flushall();
            var set = await DB.Set("key1", "hello");
            Write(set);
            set = await DB.Set("key2", "word");
            Write(set);
            var type = await DB.Type("key1");
            Write(type);
            type = await DB.Type("key2");
            Write(type);
        }
        // [Fact]
        public async void Unlink()
        {
            await DB.Flushall();
            var set = await DB.Set("key1", "hello");
            Write(set);
            set = await DB.Set("key2", "word");
            Write(set);
            var ulink = await DB.Unlink("key1", "key2", "key3");
            Write(ulink);
            var get = await DB.Get<string>("key1");
            Write(get);
        }
        [Fact]
        public async void Decr()
        {
            await DB.Flushall();
            var set = await DB.Set("mykey", "10");
            Write(set);
            var decr = await DB.Decr("mykey");
            Write(decr);
        }
        [Fact]
        public async void Decrby()
        {
            await DB.Flushall();
            var set = await DB.Set("mykey", "10");
            Write(set);
            var decrby = await DB.Decrby("mykey", 5);
            Write(decrby);
        }
        [Fact]
        public async void Get()
        {
            await DB.Flushall();
            var get = await DB.Get<string>("nonexisting");
            Write(get);
            var set = await DB.Set("mykey", "hello");
            Write(set);
            get = await DB.Get<string>("mykey");
            Write(get);
        }
        [Fact]
        public async void GetBit()
        {
            await DB.Flushall();
            var set = await DB.SetBit("mykey", 7, true);
            Write(set);
            var get = await DB.GetBit("mykey", 0);
            Write(get);
            get = await DB.GetBit("mykey", 7);
            Write(get);
            get = await DB.GetBit("mykey", 100);
            Write(get);
        }
        [Fact]
        public async void GetRange()
        {
            var set = await DB.Set("mykey", "This is a string");
            Write(set);
            var region = await DB.GetRange("mykey", 0, 3);
            Write(region);
            region = await DB.GetRange("mykey", -3, -1);
            Write(region);
            region = await DB.GetRange("mykey", 0, -1);
            Write(region);
            region = await DB.GetRange("mykey", 10, 100);
            Write(region);
        }
        [Fact]
        public async void GetSet()
        {
            await DB.Flushall();
            var set = await DB.GetSet<string>("aaa", "aaa");
            Write(set);
            set = await DB.GetSet<string>("aaa", "bbb");
            Write(set);
            var incr = await DB.Incr("mycounter");
            Write(incr);
            var getset = await DB.GetSet<string>("mycounter", 0);
            Write(getset);
            var get = await DB.Get<string>("mycounter");
            Write(get);

        }
        [Fact]
        public async void Incr()
        {
            await DB.Flushall();
            var set = await DB.Set("mykey", 10000000);
            Write(set);
            var incr = await DB.Incr("mykey");
            Write(incr);
            var get = await DB.Get<string>("mykey");
            Write(get);
        }
        [Fact]
        public async void Incrby()
        {
            await DB.Flushall();
            var set = await DB.Set("mykey", 10000000);
            Write(set);
            var incr = await DB.Incrby("mykey", 10);
            Write(incr);
            var get = await DB.Get<string>("mykey");
            Write(get);
        }
        [Fact]
        public async void IncrbyFloat()
        {
            await DB.Flushall();
            var set = await DB.Set("mykey", "10.5");
            Write(set);
            var incr = await DB.IncrbyFloat("mykey", 0.1f);
            Write(incr);
            set = await DB.Set("mykey", "5.0e3");
            Write(set);
            incr = await DB.IncrbyFloat("mykey", 200f);
            Write(incr);
        }

        [Fact]
        public async void MGetNull()
        {
            await DB.Flushall();
            var mget = await DB.MGet<string, string>("key1", "key2");
            Write($"key1:{mget.Item1}");
            Write($"key2:{mget.Item2}");

            var mset = await DB.MSet(("key1", "value1"), ("key2", "value2"));
            Write(mset);
            var mget3 = await DB.MGet<string, string, string>("key1", "aaa", "key2");
            Write($"key1:{mget3.Item1}");
            Write($"aaa:{mget3.Item2}");
            Write($"key2:{mget3.Item3}");
        }


        [Fact]
        public async void MGet2()
        {
            await DB.Flushall();
            var mset = await DB.MSet(("key1", "value1"), ("key2", "value2"));
            Write(mset);
            var mget = await DB.MGet<string, string>("key1", "key2");
            Write($"key1:{mget.Item1}");
            Write($"key2:{mget.Item2}");

        }

        [Fact]
        public async void MGet3()
        {
            await DB.Flushall();
            var mset = await DB.MSet(
                ("key1", "value1"),
                ("key2", "value2"),
                ("key3", "value3"));
            Write(mset);
            var mget = await DB.MGet<string, string, string>("key1", "key2", "key3");
            Write($"key1:{mget.Item1}");
            Write($"key2:{mget.Item2}");
            Write($"key3:{mget.Item3}");
        }
        [Fact]
        public async void MGet4()
        {
            await DB.Flushall();
            var mset = await DB.MSet(
                ("key1", "value1"),
                ("key2", "value2"),
                ("key3", "value3"),
                ("key4", "value4"));
            Write(mset);
            var mget = await DB.MGet<string, string, string, string>("key1", "key2", "key3", "key4");
            Write($"key1:{mget.Item1}");
            Write($"key2:{mget.Item2}");
            Write($"key3:{mget.Item3}");
            Write($"key4:{mget.Item4}");
        }

        [Fact]
        public async void MGet5()
        {
            await DB.Flushall();
            var mset = await DB.MSet(
                ("key1", "value1"),
                ("key2", "value2"),
                ("key3", "value3"),
                ("key4", "value4"),
                ("key5", "value5"));
            Write(mset);
            var mget = await DB.MGet<string, string, string, string, string>("key1", "key2", "key3", "key4", "key5");
            Write($"key1:{mget.Item1}");
            Write($"key2:{mget.Item2}");
            Write($"key3:{mget.Item3}");
            Write($"key4:{mget.Item4}");
            Write($"key5:{mget.Item5}");
        }

        [Fact]
        public async void MSetNX()
        {
            await DB.Flushall();
            var msetnx = await DB.MSetNX(("key1", "hello"), ("key2", "there"));
            Write(msetnx);
            msetnx = await DB.MSetNX(("key3", "world"), ("key2", "there"));
            Write(msetnx);
            var mget = await DB.MGet<string, string, string>("key1", "key2", "key3");
            Write(mget.Item1);
            Write(mget.Item2);
            Write(mget.Item3);
        }
        [Fact]
        public async void PSetEX()
        {
            await DB.Flushall();
            var psetex = await DB.PSetEX("mykey", 1000, "hello");
            Write(psetex);
            var pttl = await DB.PTtl("mykey");
            Write(pttl);
            var get = await DB.Get<string>("mykey");
            Write(get);
        }
        [Fact]
        public async void SetBit()
        {
            await DB.Flushall();
            var setbit = await DB.SetBit("mykey", 7, true);
            Write(setbit);
            var get = await DB.Get<string>("mykey");
            Write(get);
            setbit = await DB.SetBit("mykey", 7, false);
            Write(setbit);
            get = await DB.Get<string>("mykey");
            Write(get);
        }
        [Fact]
        public async void SetEX()
        {
            await DB.Flushall();
            var setex = await DB.SetEX("mykey", 10, "hello");
            Write(setex);
            var ttl = await DB.Ttl("mykey");
            Write(ttl);
            var get = await DB.Get<string>("mykey");
            Write(get);
        }
        [Fact]
        public async void SetNX()
        {
            await DB.Flushall();
            var setnx = await DB.SetNX("mykey", "hello");
            Write(setnx);
            setnx = await DB.SetNX("mykey", "World");
            Write(setnx);
            var get = await DB.Get<string>("mykey");
            Write(get);
        }
        [Fact]
        public async void SetRange()
        {
            await DB.Flushall();
            var set = await DB.Set("key1", "hello world");
            Write(set);
            var setrange = await DB.SetRange("key1", 6, "redis");
            Write(setrange);
            var get = await DB.Get<string>("key1");
            Write(get);
            setrange = await DB.SetRange("key2", 6, "redis");
            Write(setrange);
            get = await DB.Get<string>("key2");
            Write(get);
        }

        [Fact]
        public async void Strlen()
        {
            await DB.Flushall();
            var set = await DB.Set("key1", "hello world");
            Write(set);
            var strlen = await DB.Strlen("key1");
            Write(strlen);
            strlen = await DB.Strlen("nonexisting");
            Write(strlen);
        }
        [Fact]
        public async void Publish()
        {
            Write(await DB.Publish("channel", DateTime.Now));
        }

        private RedisDB CreateDB(int db, string host, int port)
        {
            var redisDb = new RedisDB(db);
            redisDb.Host.AddWriteHost(host, port);
            return redisDb;
        }

        private IEnumerable<RedisDB> CreateMultiDB(string host, int port)
        {
            for (int db = 0; db < 12; db++)
            {
                yield return CreateDB(db, host, port);
            }
        }


        [Fact]
        public async void MultiDBInit()
        {
            var redisDbs = CreateMultiDB("192.168.2.19", 6379).ToList();

            var db = redisDbs[0]; //any one

            ConcurrentBag<long?> results = new ConcurrentBag<long?>();
            ConcurrentBag<Exception> exs = new ConcurrentBag<Exception>();
            var parallelResult = Parallel.For(1, 100, async i =>
              {
                  long? result = null;
                  try
                  {
                      result = await db.Exists("anykey");
                  }
                  //throw
                  //connect xxxxxx@6379 timeout! task status:WaitingToRun
                  catch (Exception ex)
                  {
                      exs.Add(ex);
                  }
                  finally
                  {
                      results.Add(result);
                  }
              });

            while (results.Count != 99)
            {
                await Task.Delay(1000);
            }
            Assert.True(exs.Count == 0, exs.ToArray()[0].Message);

        }
    }
}
