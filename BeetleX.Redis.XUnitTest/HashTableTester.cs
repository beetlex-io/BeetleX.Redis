using System;
using Xunit;
using Xunit.Abstractions;

namespace BeetleX.Redis.XUnitTest
{
    public class HashTableTester
    {
        readonly ITestOutputHelper Console;

        public HashTableTester(ITestOutputHelper output)
        {
            this.Console = output;
            DB.Host.AddWriteHost("localhost");
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
        public async void HDel()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            var mset = await table.MSet(("field1", "foo"));
            Write(mset);
            var del = await table.Del("field1");
            Write(del);
            del = await table.Del("field2");
            Write(del);
        }
        [Fact]
        public async void HExists()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            var mset = await table.MSet(("field1", "foo"));
            Write(mset);
            var exists = await table.Exists("field1");
            Write(exists);
            exists = await table.Exists("field2");
            Write(exists);
        }
        [Fact]
        public async void HGet()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            var mset = await table.MSet(("field1", "foo"));
            Write(mset);
            var get = await table.Get<string>("field1");
            Write(get);
            get = await table.Get<string>("field2");
            Write(get);
        }
        [Fact]
        public async void HIncrby()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            var set = await table.Set("field", 5);
            Write(set);
            var value = await table.Incrby("field", 1);
            Write(value);
            value = await table.Incrby("field", -1);
            Write(value);
            value = await table.Incrby("field", -10);
            Write(value);
        }

        [Fact]
        public async void HincrbyFloat()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            var set = await table.Set("field", 10.50);
            Write(set);
            var f = await table.IncrbyFloat("field", 0.1f);
            Write(f);
            set = await table.Set("field", 5000f);
            Write(set);
            f = await table.IncrbyFloat("field", 200);
            Write(f);
        }
        [Fact]
        public async void HKeys()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("field1", "hello"), ("field2", "world")));
            Write(await table.Keys());
        }
        [Fact]
        public async void HLen()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(table.MSet(("field1", "hello"), ("field2", "world")));
            Write(await table.Len());
        }
        [Fact]
        public async void HMGet()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("field1", "hello"), ("field2", "world")));
            var values = await table.Get<string, string, string>("field1", "field2", "nofield");
            Write(values.Item1);
            Write(values.Item2);
            Write(values.Item3);
        }
        [Fact]
        public async void HMSet()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.MSet(("field1", "hello"), ("field2", "world")));
            Write(await table.Get<string>("field1"));
            Write(await table.Get<string>("field2"));
        }
        [Fact]
        public async void HSet()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.Set("field1", "hello"));
            Write(await table.Get<string>("field1"));
        }
        [Fact]
        public async void HSetNX()
        {
            await DB.Flushall();
            var table = DB.CreateHashTable("myhash");
            Write(await table.SetNX("field", "hello"));
            Write(await table.SetNX("field", "world"));
            Write(await table.Get<string>("field"));
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
