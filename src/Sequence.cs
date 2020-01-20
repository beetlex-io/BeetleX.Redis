using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BeetleX.Redis.Commands;
namespace BeetleX.Redis
{
    public class Sequence
    {
        internal Sequence(RedisDB db, string key)
        {
            DB = db.Cloneable(null);
            Key = key;
        }

        public string Key { get; private set; }

        internal RedisDB DB { get; set; }

        public async ValueTask<long> ZAdd(params (double, string)[] items)
        {
            if (items == null || items.Length == 0)
                return 0;
            ZADD cmd = new ZADD(Key, items);
            var result = await DB.Execute(cmd, typeof(long));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<double> ZScore(string member)
        {
            ZSCORE cmd = new ZSCORE(Key, member);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToDouble(result.Value);
        }

        public async ValueTask<double> ZIncrby(double increment, string member)
        {
            ZINCRBY cmd = new ZINCRBY(Key, increment, member);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToDouble(result.Value);
        }

        public async ValueTask<long> ZCard()
        {
            ZCARD cmd = new ZCARD(Key);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToInt64(result.Value);
        }

        public async ValueTask<long> ZCount(double min, double max)
        {
            ZCOUNT cmd = new ZCOUNT(Key, min, max);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToInt64(result.Value);
        }
        public async ValueTask<List<(double Score, string Member)>> ZRange(int start, int stop, bool withscores = false)
        {
            ZRANGE cmd = new ZRANGE(Key, start, stop, withscores);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return GetItems(result, withscores);
        }

        private List<(double Score, string Member)> GetItems(Result result, bool withscores)
        {
            List<(double Score, string Member)> items = new List<(double Score, string Member)>();
            for (int i = 0; i < result.Data.Count; i = i + (withscores ? 2 : 1))
            {
                (double Score, string Member) item;
                item.Member = result.Data[i].Data.ToString();
                item.Score = 0;
                if (withscores)
                    item.Score = System.Convert.ToDouble(result.Data[i + 1].Data);
                items.Add(item);
            }
            return items;
        }

        public async ValueTask<List<(double Score, string Member)>> ZRevRange(int start, int stop, bool withscores = false)
        {
            ZREVRANGE cmd = new ZREVRANGE(Key, start, stop, withscores);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return GetItems(result, withscores);
        }

        public async ValueTask<List<(double Score, string Member)>> ZRangeByScore(string min, string max, bool withscores = false)
        {
            ZRANGEBYSCORE cmd = new ZRANGEBYSCORE(Key, min, max, withscores);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return GetItems(result, withscores);
        }

        public async ValueTask<List<(double Score, string Member)>> ZRevRangeByScore(string max, string min, bool withscores = false)
        {
            ZREVRANGEBYSCORE cmd = new ZREVRANGEBYSCORE(Key, max, min, withscores);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return GetItems(result, withscores);
        }

        public async ValueTask<long> ZRank(string member)
        {
            ZRANK cmd = new ZRANK(Key, member);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToInt64(result.Value);
        }

        public async ValueTask<long> ZRevRank(string member)
        {
            ZREVRANK cmd = new ZREVRANK(Key, member);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToInt64(result.Value);
        }

        public async ValueTask<long> ZRem(params string[] members)
        {
            if (members == null || members.Length == 0)
                return 0;
            ZREM cmd = new ZREM(Key, members);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToInt64(result.Value);
        }

        public async ValueTask<long> ZRemRangeByRank(int start, int stop)
        {
            ZREMRANGEBYRANK cmd = new ZREMRANGEBYRANK(Key, start, stop);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToInt64(result.Value);
        }

        public async ValueTask<long> ZRemRangeByScore(double min, double max)
        {
            ZREMRANGEBYSCORE cmd = new ZREMRANGEBYSCORE(Key, min, max);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToInt64(result.Value);
        }

        public async ValueTask<List<string>> ZRangeByLex(string min, string max, bool negative = true)
        {
            ZRANGEBYLEX cmd = new ZRANGEBYLEX(Key, min, max, negative);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            List<string> items = new List<string>();
            foreach (var item in result.Data)
            {
                items.Add(item.Data.ToString());
            }
            return items;
        }

        public async ValueTask<long> ZLexCount(string min, string max, bool negative = true)
        {
            ZLEXCOUNT cmd = new ZLEXCOUNT(Key, min, max, negative);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToInt64(result.Value);
        }

        public async ValueTask<List<string>> ZRemRangeByLex(string min, string max)
        {
            ZREMRANGEBYLEX cmd = new ZREMRANGEBYLEX(Key, min, max);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            List<string> items = new List<string>();
            foreach (var item in result.Data)
            {
                items.Add(item.Data.ToString());
            }
            return items;
        }

        public ValueTask<long> ZUnionsStore(params string[] keys)
        {
            if (keys == null || keys.Length == 0)
                return new ValueTask<long>(0);
            (string key, double weight)[] items = new (string key, double weight)[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                items[i].key = keys[i];
                items[i].weight = 1;
            }
            return ZUnionsStore(items);
        }

        public  ValueTask<long> ZUnionsStore(params (string key, double weight)[] items)
        {
            return ZUnionsStore(items, ZUNIONSTORE.AggregateType.SUM);
        }

        public async ValueTask<long> ZUnionsStore((string key, double weight)[] items, ZUNIONSTORE.AggregateType type = ZUNIONSTORE.AggregateType.SUM)
        {
            ZUNIONSTORE cmd = new ZUNIONSTORE(Key, type, items);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToInt64(result.Value);
        }



        public ValueTask<long> ZInterStore(params string[] keys)
        {
            if (keys == null || keys.Length == 0)
                return new ValueTask<long>(0);
            (string key, double weight)[] items = new (string key, double weight)[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                items[i].key = keys[i];
                items[i].weight = 1;
            }
            return ZInterStore(items);
        }

        public ValueTask<long> ZInterStore(params (string key, double weight)[] items)
        {
            return ZInterStore(items, ZINTERSTORE.AggregateType.SUM);
        }

        public async ValueTask<long> ZInterStore((string key, double weight)[] items, ZINTERSTORE.AggregateType type = ZINTERSTORE.AggregateType.SUM)
        {
            ZINTERSTORE cmd = new ZINTERSTORE(Key, type, items);
            var result = await DB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return System.Convert.ToInt64(result.Value);
        }
    }
}
