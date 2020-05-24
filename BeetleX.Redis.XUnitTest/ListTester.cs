using System;
using Xunit;
using Xunit.Abstractions;

namespace BeetleX.Redis.XUnitTest
{
    public class ListTester
    {
        readonly ITestOutputHelper Console;

        public ListTester(ITestOutputHelper output)
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
        public async void BLPop()
        {

            var item = await DB.CreateList<String>("list1").BLPop();
            Write(item);
        }
        [Fact]
        public async void BRPop()
        {

            var item = await DB.CreateList<String>("list1").BRPop();
            Write(item);
        }
        [Fact]
        public async void BRPopLPush()
        {

            var item = await DB.CreateList<String>("list1").BRPopLPush("List2");
            Write(item);
        }
        [Fact]
        public async void LIndex()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var push = await list.Push("World");
            Write(push);
            push = await list.Push("Hello");
            Write(push);
            var lindex = await list.Index(0);
            Write(lindex);
            lindex = await list.Index(-1);
            Write(lindex);
            lindex = await list.Index(3);
            Write(lindex);

        }
        [Fact]
        public async void LInsert()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var rpush = await list.RPush("Hello");
            Write(rpush);
            rpush = await list.RPush("World");
            Write(rpush);
            var linsert = await list.Insert(true, "World", "There");
            Write(linsert);
            var lrange = await list.Range(0, -1);
            Write(lrange);
        }

        [Fact]
        public async void LLen()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var rpush = await list.RPush("Hello");
            Write(rpush);
            rpush = await list.RPush("World");
            Write(rpush);
            var len = await list.Len();
            Write(len);
        }
        [Fact]
        public async void LPop()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var rpush = await list.RPush("one");
            Write(rpush);
            rpush = await list.RPush("tow");
            Write(rpush);
            rpush = await list.RPush("three");
            Write(rpush);
            rpush = await list.RPush("four");
            Write(rpush);
            var lpop = await list.Pop();
            Write(lpop);
            var lrange = await list.Range(0, -1);
            Write(lrange);
        }
        [Fact]
        public async void LPush()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var lpush = await list.Push("world");
            Write(lpush);
            lpush = await list.Push("hello");
            Write(lpush);
            lpush = await list.Push("hello");
            Write(lpush);
            lpush = await list.Push("hello");
            Write(lpush);
            var lrange = await list.Range(0, -1);
            Write(lrange);

        }
        [Fact]
        public async void LPushX()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var lpush = await list.Push("world");
            Write(lpush);
            lpush = await list.Push("hello");
            Write(lpush);
            var myothrelist = DB.CreateList<string>("myothrelist");
            lpush = await myothrelist.PushX("hello");
            Write(lpush);
            var lrange = await list.Range(0, -1);
            Write(lrange);
            lrange = await myothrelist.Range(0, -1);
            Write(lrange);
        }
        [Fact]
        public async void Range()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var rpush = await list.RPush("one");
            Write(rpush);
            rpush = await list.RPush("two");
            Write(rpush);
            rpush = await list.RPush("three");
            Write(rpush);
            var lrange = await list.Range(0, 0);
            Write(lrange);
            lrange = await list.Range(-3, 2);
            Write(lrange);

            lrange = await list.Range(-100, 100);
            Write(lrange);

            lrange = await list.Range(5, 10);
            Write(lrange);
        }
        [Fact]
        public async void LRem()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var rpush = await list.RPush("hello");
            Write(rpush);
            rpush = await list.RPush("hello");
            Write(rpush);
            rpush = await list.RPush("foo");
            Write(rpush);
            rpush = await list.RPush("hello");
            Write(rpush);
            var rem = list.Rem(-2, "hello");
            Write(rem);
            var lrange = await list.Range(0, -1);
            Write(lrange);
        }

        [Fact]
        public async void LSet()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var rpush = await list.RPush("one");
            Write(rpush);
            rpush = await list.RPush("tow");
            Write(rpush);
            rpush = await list.RPush("three");
            Write(rpush);
            var lset = await list.Set(0, "four");
            Write(lset);
            lset = await list.Set(-2, "five");
            Write(lset);
            var lrange = await list.Range(0, -1);
            Write(lrange);
        }
        [Fact]
        public async void LTrim()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var rpush = await list.RPush("one");
            Write(rpush);
            rpush = await list.RPush("tow");
            Write(rpush);
            rpush = await list.RPush("three");
            Write(rpush);
            var ltrim = await list.Trim(1, -1);
            Write(ltrim);
            var lrange = await list.Range(0, -1);
            Write(lrange);
        }

        [Fact]
        public async void RPop()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var rpush = await list.RPush("one");
            Write(rpush);
            rpush = await list.RPush("tow");
            Write(rpush);
            rpush = await list.RPush("three");
            Write(rpush);
            var rpop = await list.RPop();
            Write(rpop);
            var lrange = await list.Range(0, -1);
            Write(lrange);
        }
        [Fact]
        public async void RPopLPush()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var rpush = await list.RPush("one");
            Write(rpush);
            rpush = await list.RPush("tow");
            Write(rpush);
            rpush = await list.RPush("three");
            Write(rpush);
            var rpopLpush = await list.RPopLPush("myotherlist");
            Write(rpopLpush);
            var myothrelist = DB.CreateList<string>("myotherlist");
            var lrange = await list.Range(0, -1);
            Write(lrange);
            lrange = await myothrelist.Range(0, -1);
            Write(lrange);
        }
        [Fact]
        public async void RPush()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var rpush = await list.RPush("hello");
            Write(rpush);
            rpush = await list.RPush("world");
            Write(rpush);
            var lrange = await list.Range(0, -1);
            Write(lrange);
        }
        [Fact]
        public async void RPushX()
        {
            await DB.Flushall();
            var list = DB.CreateList<string>("list1");
            var rpush = await list.RPush("hello");
            Write(rpush);
            rpush = await list.RPush("world");
            Write(rpush);
            var mlist = DB.CreateList<string>("myotherlist");
            var rpuxhx = mlist.RPushX("world");
            Write(rpuxhx);
            var lrange = await list.Range(0, -1);
            Write(lrange);
            lrange = await mlist.Range(0, -1);
            Write(lrange);
        }
    }
}
