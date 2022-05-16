using BeetleX.Redis.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Redis
{
    public static class RedisDBEx
    {
        public static RedisDB Instance(this RedisDB db)
        {
            return db ?? RedisDB.Default;
        }
    }


    public class DefaultRedis
    {


        public static RedisDB Instance
        {
            get
            {
                return RedisDB.Default;
            }

        }

        public static Subscriber Subscribe()
        {
            return Instance.Subscribe();
        }

        public static RedisHashTable CreateHashTable(string key)
        {
            return Instance.CreateHashTable(key);
        }

        public static RedisList<T> CreateList<T>(string key)
        {
            return Instance.CreateList<T>(key);
        }

        public static async ValueTask<float> IncrbyFloat(string key, float increment)
        {
            return await Instance.IncrbyFloat(key, increment);
        }

        public static async ValueTask<long> Incrby(string key, long increment)
        {
            return await Instance.Incrby(key, increment);

        }
        public static async ValueTask<long> Incr(string key)
        {
            return await Instance.Incr(key);
        }

        public static async ValueTask<T> GetSet<T>(string key, object data)
        {
            return await Instance.GetSet<T>(key, data);
        }

        public static async ValueTask<long> Expireat(string key, long timestamp)
        {
            return await Instance.Expireat(key, timestamp);
        }

        public static async ValueTask<long> Expire(string key, int seconds, EXPIREType? type=null)
        {
            return await Instance.Expire(key, seconds, type);
        }

        public static async ValueTask<long> Exists(params string[] key)
        {
            return await Instance.Exists(key);
        }

        public static async ValueTask<long> Decrby(string key, int decrement)
        {
            return await Instance.Decrby(key, decrement);
        }

        public static async ValueTask<long> Decr(string key)
        {
            return await Instance.Decr(key);
        }

        public static async ValueTask<long> Del(params string[] key)
        {
            return await Instance.Del(key);
        }

        public static async ValueTask<(T, T1, T2, T3, T4)> Get<T, T1, T2, T3, T4>(string key, string key1, string key2, string key3, string key4)
        {
            return await Instance.MGet<T, T1, T2, T3, T4>(key, key1, key2, key3, key4);
        }

        public static async ValueTask<(T, T1, T2, T3)> Get<T, T1, T2, T3>(string key, string key1, string key2, string key3)
        {
            return await Instance.MGet<T, T1, T2, T3>(key, key1, key2, key3);
        }

        public static async ValueTask<(T, T1, T2)> Get<T, T1, T2>(string key, string key1, string key2)
        {
            return await Instance.MGet<T, T1, T2>(key, key1, key2);
        }

        public static async ValueTask<(T, T1)> Get<T, T1>(string key, string key1)
        {
            return await Instance.MGet<T, T1>(key, key1);
        }

        public static async ValueTask<T> Get<T>(string key)
        {
            return await Instance.Get<T>(key);
        }


        public static async ValueTask<string> Set(string key, object data)
        {
            return await Instance.Set(key, data);
        }

        public static async ValueTask<string> Set(params (string, object)[] datas)
        {

            return await Instance.MSet(datas);
        }

        public static async ValueTask<long> SetNX(string key, object data)
        {
            return await Instance.SetNX(key, data);
        }

        public static async ValueTask<long> SetNX(params (string, object)[] datas)
        {

            return await Instance.MSetNX(datas);
        }

        public static async ValueTask<long> Move(string key, int db)
        {
            return await Instance.Move(key, db);
        }
        public static async ValueTask<string> PSetEX(string key, long milliseconds, object value)
        {
            return await Instance.PSetEX(key, milliseconds, value);
        }

        public static async ValueTask<long> Persist(string key)
        {
            return await Instance.Persist(key);
        }

        public static async ValueTask<long> Pexpire(string key, long milliseconds)
        {
            return await Instance.Pexpire(key, milliseconds);
        }

        public static async ValueTask<long> Pexpireat(string key, long timestamp)
        {
            return await Instance.Pexpireat(key, timestamp);
        }

        public static async ValueTask<long> Ttl(string key)
        {
            return await Instance.Ttl(key);
        }

        public static async ValueTask<long> PTtl(string key)
        {
            return await Instance.PTtl(key);
        }

        public static async ValueTask<long> Publish(string channel, object data)
        {
            return await Instance.Publish(channel, data);
        }

        public static async ValueTask<string> Rename(string key, string newkey)
        {
            return await Instance.Rename(key, newkey);
        }

        public static async ValueTask<long> RRenamenx(string key, string newkey)
        {
            return await Instance.Renamenx(key, newkey);
        }

        public static async ValueTask<string> SetEX(string key, int seconds, object value)
        {
            return await Instance.SetEX(key, seconds, value);
        }
    }
}
