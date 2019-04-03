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


    public class Redis
    {


        public static RedisDB Default
        {
            get
            {
                return RedisDB.Default;
            }

        }

        public static Subscriber Subscribe()
        {
            return Default.Subscribe();
        }

        public static RedisHashTable CreateHashTable(string key)
        {
            return Default.CreateHashTable(key);
        }

        public static RedisList<T> CreateList<T>(string key)
        {
            return Default.CreateList<T>(key);
        }

        public static async ValueTask<float> IncrbyFloat(string key, float increment)
        {
            return await Default.IncrbyFloat(key, increment);
        }

        public static async ValueTask<long> Incrby(string key, int increment)
        {
            return await Default.Incrby(key, increment);

        }
        public static async ValueTask<long> Incr(string key)
        {
            return await Default.Incr(key);
        }

        public static async ValueTask<T> GetSet<T>(string key, object data)
        {
            return await Default.GetSet<T>(key, data);
        }

        public static async ValueTask<long> Expireat(string key, long timestamp)
        {
            return await Default.Expireat(key, timestamp);
        }

        public static async ValueTask<long> Expire(string key, int seconds)
        {
            return await Default.Expire(key, seconds);
        }

        public static async ValueTask<long> Exists(params string[] key)
        {
            return await Default.Exists(key);
        }

        public static async ValueTask<long> Decrby(string key, int decrement)
        {
            return await Default.Decrby(key, decrement);
        }

        public static async ValueTask<long> Decr(string key)
        {
            return await Default.Decr(key);
        }

        public static async ValueTask<long> Del(params string[] key)
        {
            return await Default.Del(key);
        }

        public static async ValueTask<(T, T1, T2, T3, T4)> Get<T, T1, T2, T3, T4>(string key, string key1, string key2, string key3, string key4)
        {
            return await Default.MGet<T, T1, T2, T3, T4>(key, key1, key2, key3, key4);
        }

        public static async ValueTask<(T, T1, T2, T3)> Get<T, T1, T2, T3>(string key, string key1, string key2, string key3)
        {
            return await Default.MGet<T, T1, T2, T3>(key, key1, key2, key3);
        }

        public static async ValueTask<(T, T1, T2)> Get<T, T1, T2>(string key, string key1, string key2)
        {
            return await Default.MGet<T, T1, T2>(key, key1, key2);
        }

        public static async ValueTask<(T, T1)> Get<T, T1>(string key, string key1)
        {
            return await Default.MGet<T, T1>(key, key1);
        }

        public static async ValueTask<T> Get<T>(string key)
        {
            return await Default.Get<T>(key);
        }


        public static async ValueTask<string> Set(string key, object data)
        {
            return await Default.Set(key, data);
        }

        public static async ValueTask<string> Set(params (string, object)[] datas)
        {

            return await Default.MSet(datas);
        }

        public static async ValueTask<long> SetNX(string key, object data)
        {
            return await Default.SetNX(key, data);
        }

        public static async ValueTask<long> SetNX(params (string, object)[] datas)
        {

            return await Default.MSetNX(datas);
        }

        public static async ValueTask<long> Move(string key, int db)
        {
            return await Default.Move(key, db);
        }
        public static async ValueTask<string> PSetEX(string key, long milliseconds, object value)
        {
            return await Default.PSetEX(key, milliseconds, value);
        }

        public static async ValueTask<long> Persist(string key)
        {
            return await Default.Persist(key);
        }

        public static async ValueTask<long> Pexpire(string key, long milliseconds)
        {
            return await Default.Pexpire(key, milliseconds);
        }

        public static async ValueTask<long> Pexpireat(string key, long timestamp)
        {
            return await Default.Pexpireat(key, timestamp);
        }

        public static async ValueTask<long> Ttl(string key)
        {
            return await Default.Ttl(key);
        }

        public static async ValueTask<long> PTtl(string key)
        {
            return await Default.PTtl(key);
        }

        public static async ValueTask<long> Publish(string channel, object data)
        {
            return await Default.Publish(channel, data);
        }

        public static async ValueTask<string> Rename(string key, string newkey)
        {
            return await Default.Rename(key, newkey);
        }

        public static async ValueTask<long> RRenamenx(string key, string newkey)
        {
            return await Default.Renamenx(key, newkey);
        }

        public static async ValueTask<string> SetEX(string key, int seconds, object value)
        {
            return await Default.SetEX(key, seconds, value);
        }
    }
}
