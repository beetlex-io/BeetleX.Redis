using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace BeetleX.Redis.XUnitTest
{
    public class SetsTester
    {
        readonly ITestOutputHelper Console;

        public SetsTester(ITestOutputHelper output)
        {
            this.Console = output;
            DefaultRedis.Instance.Host.AddWriteHost("localhost");
            DefaultRedis.Instance.DataFormater = new JsonFormater();
            DB = DefaultRedis.Instance.Cloneable();
            DB.KeyPrefix = "BeetleX";
        }

        private RedisDB DB;

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

        private Sets GetSets()
        {
            return DB.GetSets();
        }

        [Fact]
        public async void SADD()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("bbs", "discuz.net");
            Assert.Equal(1, count);
            count = await sets.SAdd("bbs", "discuz.net");
            Assert.Equal(0, count);
            count = await sets.SAdd("bbs", "tianya.cn", "groups.google.com");
            Assert.Equal(2, count);
            var items = await sets.SMembers("bbs");

        }
        [Fact]
        public async void SISMEMBER()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("joe's_movies", "hi, lady", "Fast Five", "2012");
            Assert.Equal(3, count);

            var has = await sets.SisMember("joe's_movies", "bet man");
            Assert.Equal(false, has);
            has = await sets.SisMember("joe's_movies", "Fast Five");
            Assert.Equal(true, has);
        }
        [Fact]
        public async void SPOP()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("db", "MySQL", "MongoDB", "Redis");
            Assert.Equal(3, count);
            var item = await sets.SPop("db");
            var items = await sets.SMembers("db");
            Assert.Equal(2, items.Count);

            item = await sets.SPop("db");
            items = await sets.SMembers("db");
            Assert.Equal(1, items.Count);
        }
        [Fact]
        public async void SRANDMEMBER()
        {
            await DB.Flushall();
            var sets = GetSets();
            await sets.SAdd("fruit", "apple", "banana", "cherry");

            var items = await sets.SRandMember("fruit", 1);
            Assert.Equal(1, items.Count);
            var items1 = await sets.SRandMember("fruit", 1);
            Assert.Equal(1, items1.Count);
            var items2 = await sets.SRandMember("fruit", 3);
            Assert.Equal(3, items2.Count);
        }
        [Fact]
        public async void SREM()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("languages", "c", "lisp", "python", "ruby");
            var len = await sets.SRem("languages", "ruby");
            Assert.Equal(1, len);

            len = await sets.SRem("languages", "non-exists-language");
            Assert.Equal(0, len);

            len = await sets.SRem("languages", "c", "lisp", "python");
            Assert.Equal(3, len);

            var items = await sets.SMembers("languages");
            Assert.Equal(0, items.Count);
        }

        [Fact]
        public async void SMOVE()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("songs", "Billie Jean", "Believe Me");
            await sets.SMove("songs", "my_songs", "Believe Me");
            var items = await sets.SMembers("songs");
            Assert.Equal(1, items.Count);
            items = await sets.SMembers("my_songs");
            Assert.Equal(1, items.Count);
        }

        [Fact]
        public async void SCARD()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("tool", "pc", "printer", "phone");
            var len = await sets.SCard("tool");
            Assert.Equal(3, len);
            await DB.Del("tool");
            len = await sets.SCard("tool");
            Assert.Equal(0, len);
        }
        [Fact]
        public async void SMEMBERS()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("language", "Ruby", "Python", "Clojure");
            var items = await sets.SMembers("language");
            Assert.Equal(3, items.Count);
        }
        [Fact]
        public async void SSCAN()
        {
            await DB.Flushall();
            var sets = GetSets();
            List<string> data = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                data.Add("rec" + i);
            }
            var count = await sets.SAdd("sscan", data.ToArray());
            var items = await sets.SScan("sscan", 0, 20, "rec*");
        }
        [Fact]
        public async void SINTER()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("group_1", "LI LEI", "TOM", "JACK");
            count = await sets.SAdd("group_2", "HAN MEIMEI", "JACK", "Clojure");
            var items = await sets.SInter("group_1", "group_2");
            Assert.Equal("JACK", items[0]);
        }
        [Fact]
        public async void SINTERSTORE()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("group_1", "LI LEI", "TOM", "JACK");
            count = await sets.SAdd("group_2", "HAN MEIMEI", "JACK", "Clojure");
            await sets.SInterStore("dest", "group_1", "group_2");
            var items = await sets.SMembers("dest");
            Assert.Equal(1, items.Count);
        }
        [Fact]
        public async void SUNION()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("group_1", "LI LEI", "TOM", "JACK");
            count = await sets.SAdd("group_2", "HAN MEIMEI", "JACK", "Clojure");
            var items = await sets.SUnion("group_1", "group_2");
            Assert.Equal(5, items.Count);
        }

        [Fact]
        public async void SUNIONSTORE()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("group_1", "LI LEI", "TOM", "JACK");
            count = await sets.SAdd("group_2", "HAN MEIMEI", "JACK", "Clojure");
            await sets.SUnionStore("dest", "group_1", "group_2");
            var items = await sets.SMembers("dest");
            Assert.Equal(5, items.Count);
        }

        [Fact]
        public async void SDIFF()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("group_1", "LI LEI", "TOM", "JACK");
            count = await sets.SAdd("group_2", "HAN MEIMEI", "JACK", "Clojure");
            var items = await sets.SDiff("group_1", "group_2");
            Assert.Equal(2, items.Count);
        }
        [Fact]
        public async void SDIFFSTORE()
        {
            await DB.Flushall();
            var sets = GetSets();
            var count = await sets.SAdd("group_1", "LI LEI", "TOM", "JACK");
            count = await sets.SAdd("group_2", "HAN MEIMEI", "JACK", "Clojure");
            await sets.SDiffStore("dest", "group_1", "group_2");
            var items = await sets.SMembers("dest");
            Assert.Equal(2, items.Count);
        }
    }
}
