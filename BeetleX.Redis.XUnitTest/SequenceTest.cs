using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace BeetleX.Redis.XUnitTest
{
    public class SequenceTest
    {
        readonly ITestOutputHelper Console;

        public SequenceTest(ITestOutputHelper output)
        {
            this.Console = output;
            DB.Host.AddWriteHost("localhost");
            DB.KeyPrefix = "BeetleX";
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
        public async void ZAdd()
        {
            await DB.Del("seq2");
            var sequeue = DB.CreateSequence("seq2");
            var count = await sequeue.ZAdd((123.1231, "123"), (123.1231, "234"));
            var value = await sequeue.ZRange(0, -1,true);
            Assert.Equal<long>(count, 2);
            Write(count);
        }
        [Fact]
        public async void ZSCORE()
        {
            await DB.Del("seq2");
            var sequeue = DB.CreateSequence("seq2");
            var count = await sequeue.ZAdd((4.14, "bca"));
            var value = await sequeue.ZScore("bca");
            Assert.Equal<double>(value.Value, 4.14);

            var unknownMemberValue = await sequeue.ZScore("unknownseq");
            Assert.Equal<double?>(unknownMemberValue, null);

        }
        [Fact]
        public async void ZINCRBY()
        {
            await DB.Del("seq2");
            string member = "ken";
            var sequeue = DB.CreateSequence("seq2");
            var count = await sequeue.ZAdd((4.14, member));
            var value = await sequeue.ZScore(member);
            Assert.Equal<double>(value.Value, 4.14);
            await sequeue.ZIncrby(5, member);
            value = await sequeue.ZScore(member);
            Assert.Equal<double>(value.Value, 9.14);
        }
        [Fact]
        public async void ZCARD()
        {
            await DB.Del("seq2");
            string member = "ken";
            var sequeue = DB.CreateSequence("seq2");
            await sequeue.ZAdd((4.14, member));
            var count = await sequeue.ZCard();
            Assert.Equal<long>(count, 1);
            await sequeue.ZAdd((4.14, "ken1"));
            count = await sequeue.ZCard();
            Assert.Equal<long>(count, 2);
        }
        [Fact]
        public async void ZCOUNT()
        {
            await DB.Del("seq2");
            var sequeue = DB.CreateSequence("seq2");
            await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
            var count = await sequeue.ZCount(200, 300);
            Assert.Equal<long>(count, 2);
        }

        [Fact]
        public async void ZRANGE()
        {
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
        }

        [Fact]
        public async void ZREVRANGE()
        {
            await DB.Del("seq2");
            var sequeue = DB.CreateSequence("seq2");
            await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
            var items = await sequeue.ZRevRange(0, -1, true);
            Assert.Equal<int>(items.Count, 4);
            Assert.Equal<string>(items[0].Member, "A4");
            Assert.Equal<string>(items[1].Member, "A3");
            Assert.Equal<string>(items[2].Member, "A2");
            Assert.Equal<string>(items[3].Member, "A1");

            Assert.Equal<double>(items[0].Score, 400);
            Assert.Equal<double>(items[1].Score, 300);
            Assert.Equal<double>(items[2].Score, 200);
            Assert.Equal<double>(items[3].Score, 100);
        }

        [Fact]
        public async void ZRANGEBYSCORE()
        {
            await DB.Del("seq2");
            var seq = DB.CreateSequence("seq2");
            await seq.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
            var items = await seq.ZRangeByScore("200", "300", true);
            Assert.Equal<int>(items.Count, 2);
            Assert.Equal<string>(items[0].Member, "A2");
            Assert.Equal<double>(items[0].Score, 200);
        }
        [Fact]
        public async void ZRevRangeByScore()
        {
            await DB.Del("seq2");
            var sequeue = DB.CreateSequence("seq2");
            await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
            var items = await sequeue.ZRevRangeByScore("300", "200", true);
            Assert.Equal<int>(items.Count, 2);
            Assert.Equal<string>(items[0].Member, "A3");
            Assert.Equal<double>(items[0].Score, 300);
            Assert.Equal<string>(items[1].Member, "A2");
            Assert.Equal<double>(items[1].Score, 200);
        }
        [Fact]
        public async void ZRANK()
        {
            await DB.Del("seq2");
            var sequeue = DB.CreateSequence("seq2");
            await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
            var value = await sequeue.ZRank("A4");
            Assert.Equal<long>(value.Value, 3);


            var unknownMemberValue = await sequeue.ZRank("unknownseq");
            Assert.Equal<long?>(unknownMemberValue, null);
        }
        [Fact]
        public async void ZREVRANK()
        {
            await DB.Del("seq2");
            var sequeue = DB.CreateSequence("seq2");
            await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
            var value = await sequeue.ZRevRank("A4");
            Assert.Equal<long>(value, 0);
        }
        [Fact]
        public async void ZREM()
        {
            await DB.Del("seq2");
            var sequeue = DB.CreateSequence("seq2");
            await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
            var count = await sequeue.ZCard();
            Assert.Equal<long>(count, 4);
            await sequeue.ZRem("A1", "A2");
            count = await sequeue.ZCard();
            Assert.Equal<long>(count, 2);
        }
        [Fact]
        public async void ZREMRANGEBYRANK()
        {
            await DB.Del("seq2");
            var sequeue = DB.CreateSequence("seq2");
            await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
            var count = await sequeue.ZCard();
            Assert.Equal<long>(count, 4);
            await sequeue.ZRemRangeByRank(0, 1);
            count = await sequeue.ZCard();
            Assert.Equal<long>(count, 2);
        }
        [Fact]
        public async void ZREMRANGEBYSCORE()
        {
            await DB.Del("seq2");
            var sequeue = DB.CreateSequence("seq2");
            await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
            var count = await sequeue.ZCard();
            Assert.Equal<long>(count, 4);
            await sequeue.ZRemRangeByScore(200, 400);
            count = await sequeue.ZCard();
            Assert.Equal<long>(count, 1);
        }
        [Fact]
        public async void ZRANGEBYLEX()
        {

            await DB.Del("seq2");
            var sequeue = DB.CreateSequence("seq2");
            await sequeue.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));
            var count = await sequeue.ZCard();
            Assert.Equal<long>(count, 4);
            var items = await sequeue.ZRangeByLex("[A2", null);
            Assert.Equal<int>(items.Count, 2);

        }
        [Fact]
        public async void ZUNIONSTORE()
        {
            await DB.Del("seq2", "seq3", "seq4");
            var seq2 = DB.CreateSequence("seq2");
            await seq2.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));

            var seq3 = DB.CreateSequence("seq2");
            await seq3.ZAdd((500, "B1"), (600, "B2"), (700, "B3"), (800, "B4"));

            var seq4 = DB.CreateSequence("seq4");
            await seq4.ZUnionsStore("seq2", "seq3");

            var count = await seq4.ZCard();
            Assert.Equal<long>(count, 8);
        }

        [Fact]
        public async void ZUNIONSTORE_WEIGHTS()
        {
            await DB.Del("seq2", "seq3", "seq4");
            var seq2 = DB.CreateSequence("seq2");
            await seq2.ZAdd((100, "A1"), (200, "A2"), (300, "A3"), (400, "A4"));

            var seq3 = DB.CreateSequence("seq3");
            await seq3.ZAdd((500, "B1"), (600, "B2"), (700, "B3"), (800, "B4"));

            var seq4 = DB.CreateSequence("seq4");
            await seq4.ZUnionsStore(("seq2", 1), ("seq3", 2));

            var count = await seq4.ZCard();
            Assert.Equal<long>(count, 8);

            var items = await seq4.ZRevRange(0, -1, true);
            Assert.Equal<double>(1600, items[0].Score);
            Assert.Equal<double>(1400, items[1].Score);
            Assert.Equal<double>(1200, items[2].Score);
            Assert.Equal<double>(1000, items[3].Score);
        }
        [Fact]
        public async void ZINTERSTORE()
        {
            await DB.Del("seq2", "seq3", "seq4");
            var seq2 = DB.CreateSequence("seq2");
            await seq2.ZAdd((70, "Li Lei"), (70, "Han Meimei"), (99.5, "Tom"));

            var seq3 = DB.CreateSequence("seq3");
            await seq3.ZAdd((88, "Li Lei"), (75, "Han Meimei"), (99.5, "Tom"));

            var seq4 = DB.CreateSequence("seq4");
            await seq4.ZInterStore("seq2", "seq3");

            var items = await seq4.ZRange(0, -1, true);

            Assert.Equal<double>(145, items[0].Score);
            Assert.Equal<double>(158, items[1].Score);
            Assert.Equal<double>(199, items[2].Score);
        }
    }
}
