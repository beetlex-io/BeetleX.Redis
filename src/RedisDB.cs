using BeetleX.Redis.Commands;
using BeetleX.Tracks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Redis
{
    public class RedisDB : IHostHandler, IDisposable
    {
        public RedisDB(int db = 0, IDataFormater dataFormater = null, IHostHandler hostHandler = null)
        {
            DB = db;
            DataFormater = dataFormater;
            if (hostHandler == null)
            {
                mDetectionTime = new System.Threading.Timer(OnDetection, null, 1000, 1000);
                this.Host = this;
            }
            else
            {
                this.Host = hostHandler;
            }
        }

        private System.Threading.Timer mDetectionTime;

        private static RedisDB mDefault = new RedisDB();

        internal static RedisDB Default => mDefault;

        public bool AutoPing { get; set; } = true;

        public IHostHandler Host { get; set; }

        private void OnDetection(object state)
        {
            mDetectionTime?.Change(-1, -1);
            if (AutoPing)
            {
                var wHosts = mWriteActives;
                foreach (var item in wHosts)
                    item.Ping();
                var rHost = mReadActives;
                foreach (var item in rHost)
                    item.Ping();
            }
            mDetectionTime?.Change(1000, 1000);

        }

        public IDataFormater DataFormater { get; set; }

        private List<RedisHost> mWriteHosts = new List<RedisHost>();

        private List<RedisHost> mReadHosts = new List<RedisHost>();

        private RedisHost[] mWriteActives = new RedisHost[0];

        private RedisHost[] mReadActives = new RedisHost[0];

        private bool OnClientPush(RedisClient client)
        {
            return true;
        }

        public int DB { get; set; }

        public string KeyPrefix { get; set; }

        public Sets GetSets()
        {
            return new Sets(this);
        }

        public GEO GetGEO()
        {
            return new GEO(this);
        }

        public RedisStream<T> GetStream<T>(string name)
        {
            if (DataFormater == null)
                throw new RedisException("RedisDB data formater property cannot be empty!");
            return new RedisStream<T>(this, name);
        }

        public RedisDB Cloneable(IDataFormater dataFormater = null)
        {
            var result = new RedisDB(this.DB, dataFormater, this);
            result.KeyPrefix = this.KeyPrefix;
            return result;
        }

        RedisHost IHostHandler.AddWriteHost(string host, int port = 6379)
        {
            return ((IHostHandler)this).AddWriteHost(host, port, false);
        }

        RedisHost IHostHandler.AddReadHost(string host, int port = 6379)
        {
            return ((IHostHandler)this).AddReadHost(host, port, false);
        }

        RedisHost IHostHandler.AddWriteHost(string host, int port, bool ssl)
        {
            if (port == 0)
                port = 6379;
            RedisHost redisHost = new RedisHost(ssl, DB, host, port);
            mWriteHosts.Add(redisHost);
            mWriteActives = mWriteHosts.ToArray();
            return redisHost;
        }

        RedisHost IHostHandler.AddReadHost(string host, int port, bool ssl)
        {
            if (port == 0)
                port = 6379;
            RedisHost redisHost = new RedisHost(ssl, DB, host, port);
            mReadHosts.Add(redisHost);
            mReadActives = mReadHosts.ToArray();
            return redisHost;
        }

        RedisHost IHostHandler.GetWriteHost()
        {
            var items = mWriteActives;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Available)
                    return items[i];
            }
            return null;
        }

        RedisHost IHostHandler.GetReadHost()
        {
            var items = mReadActives;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Available)
                    return items[i];
            }
            return Host.GetWriteHost();
        }

        public Subscriber Subscribe()
        {
            Subscriber result = new Subscriber(this);
            return result;
        }

        public async Task<Result> Execute(Command cmd, params Type[] types)
        {
            var host = cmd.Read ? Host.GetReadHost() : Host.GetWriteHost();
            if (host == null)
            {
                return new Result() { ResultType = ResultType.NetError, Messge = "redis server is not available" };
            }
            var client = await host.Pop();
            if (client == null)
                return new Result() { ResultType = ResultType.NetError, Messge = "exceeding maximum number of connections" };
            try
            {
                var result = await host.Connect(this, client);
                if (result.IsError)
                {
                    return result;
                }
                using (var tarck = CodeTrackFactory.Track(cmd.Name, CodeTrackLevel.Module, null, "Redis", client.Host))
                {
                    if (tarck.Enabled)
                    {
                        tarck.Activity?.AddTag("tag", "BeetleX Redis");
                    }
                    cmd.Activity = tarck.Activity;
                    RedisRequest request = new RedisRequest(host, client, cmd, types);
                    request.Activity = tarck.Activity;
                    result = await request.Execute(this);
                    return result;
                }
            }
            finally
            {
                if (client != null)
                    host.Push(client);
            }
        }

        public async ValueTask<string> Flushall()
        {
            Commands.FLUSHALL cmd = new Commands.FLUSHALL();
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (string)result.Value;
        }

        public async ValueTask<bool> Ping()
        {
            Commands.PING ping = new Commands.PING(null);
            var result = await Execute(ping, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return true;
        }

        public async ValueTask<long> Del(params string[] key)
        {
            Commands.DEL del = new Commands.DEL(key);
            var result = await Execute(del, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<string> Set(string key, object value)
        {
            return await Set(key, value, null, null);
        }


        public async ValueTask<string> Set(string key, object value, int? extime)
        {
            return await Set(key, value, extime, null);
        }

        public async ValueTask<string> Set(string key, object value, int? extime, bool? nx)
        {
            Commands.SET set = new Commands.SET(key, value, DataFormater);
            if (extime != null)
            {
                set.ExTime = extime.Value;
            }
            set.NX = nx;
            var result = await Execute(set, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (string)result.Value;

        }

        public async ValueTask<string> Dump(string key)
        {
            Commands.DUMP dump = new Commands.DUMP(key);
            var result = await Execute(dump, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (string)result.Value;
        }

        public async ValueTask<long> Exists(params string[] key)
        {
            Commands.EXISTS exists = new Commands.EXISTS(key);
            var result = await Execute(exists, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> Expire(string key, int seconds, EXPIREType? type=null)
        {
            Commands.EXPIRE expire = new Commands.EXPIRE(key, seconds);
            expire.Type = type;
            var result = await Execute(expire, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> Ttl(string key)
        {
            Commands.TTL cmd = new Commands.TTL(key);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> PTtl(string key)
        {
            Commands.PTTL cmd = new Commands.PTTL(key);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> Expireat(string key, long timestamp)
        {
            Commands.EXPIREAT cmd = new Commands.EXPIREAT(key, timestamp);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<string> MSet(params (string, object)[] datas)
        {
            Commands.MSET cmd = new Commands.MSET(DataFormater);
            foreach (var item in datas)
            {
                cmd = cmd[item.Item1, item.Item2];
            }
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (string)result.Value;
        }

        public async ValueTask<long> MSetNX(params (string, object)[] datas)
        {
            Commands.MSETNX cmd = new Commands.MSETNX(DataFormater);
            foreach (var item in datas)
            {
                cmd = cmd[item.Item1, item.Item2];
            }
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public static T ChangeValue<T>(object data)
        {
            if (data == null && typeof(T).IsValueType)
                return default(T);
            return (T)data;
        }

        public async ValueTask<T> Get<T>(string key)
        {
            Commands.GET cmd = new Commands.GET(key, DataFormater);
            var result = await Execute(cmd, typeof(T));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return ChangeValue<T>(result.Value);
        }

        public async ValueTask<string[]> Keys(string pattern)
        {
            Commands.KEYS cmd = new Commands.KEYS(pattern);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (from a in result.Data select (string)a.Data).ToArray();
        }
        public async ValueTask<Commands.ScanResult> Scan(int cursor, int count = 20, string pattern = null)
        {
            List<string> items = new List<string>();
            Commands.SCAN cmd = new Commands.SCAN();
            cmd.Cursor = cursor;
            cmd.Count = count;
            cmd.Pattern = pattern;
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (Commands.ScanResult)result.Value;
        }
        public async ValueTask<long> Move(string key, int db)
        {
            Commands.MOVE cmd = new Commands.MOVE(key, db);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> Persist(string key)
        {
            Commands.PERSIST cmd = new Commands.PERSIST(key);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> Pexpire(string key, long milliseconds)
        {
            Commands.PEXPIRE cmd = new Commands.PEXPIRE(key, milliseconds);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> Pexpireat(string key, long timestamp)
        {
            Commands.PEXPIREAT cmd = new Commands.PEXPIREAT(key, timestamp);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<string> Randomkey()
        {
            Commands.RANDOMKEY cmd = new Commands.RANDOMKEY();
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (string)result.Value;
        }

        public async ValueTask<string> Rename(string key, string newkey)
        {
            Commands.RENAME cmd = new Commands.RENAME(key, newkey);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (string)result.Value;
        }

        public async ValueTask<long> Renamenx(string key, string newkey)
        {
            Commands.RENAMENX cmd = new Commands.RENAMENX(key, newkey);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> Touch(params string[] keys)
        {
            Commands.TOUCH cmd = new Commands.TOUCH(keys);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<string> Type(string key)
        {
            Commands.TYPE cmd = new Commands.TYPE(key);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (string)result.Value;
        }

        public async ValueTask<long> Unlink(params string[] keys)
        {
            Commands.UNLINK cmd = new Commands.UNLINK(keys);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> Decr(string key)
        {
            Commands.DECR cmd = new Commands.DECR(key);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> Decrby(string key, int decrement)
        {
            Commands.DECRBY cmd = new Commands.DECRBY(key, decrement);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> SetBit(string key, int offset, bool value)
        {
            Commands.SETBIT cmd = new Commands.SETBIT(key, offset, value);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> GetBit(string key, int offset)
        {
            Commands.GETBIT cmd = new Commands.GETBIT(key, offset);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<string> GetRange(string key, int start, int end)
        {
            Commands.GETRANGE cmd = new Commands.GETRANGE(key, start, end);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (string)result.Value;
        }

        public async ValueTask<T> GetSet<T>(string key, object value)
        {
            Commands.GETSET cmd = new Commands.GETSET(key, value, DataFormater);
            var result = await Execute(cmd, typeof(T));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (T)result.Value;
        }

        public async ValueTask<long> Incr(string key)
        {
            Commands.INCR cmd = new Commands.INCR(key);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> Incrby(string key, long increment)
        {
            Commands.INCRBY cmd = new Commands.INCRBY(key, increment);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<float> IncrbyFloat(string key, float increment)
        {
            Commands.INCRBYFLOAT cmd = new Commands.INCRBYFLOAT(key, increment);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return float.Parse((string)result.Value, CultureInfo.InvariantCulture);
        }

        public async ValueTask<(T, T1)> MGet<T, T1>(string key1, string key2)
        {
            string[] keys = { key1, key2 };
            Type[] types = { typeof(T), typeof(T1) };
            var items = await MGet(keys, types);
            return ((T)items[0], (T1)items[1]);
        }

        public async ValueTask<(T, T1, T2)> MGet<T, T1, T2>(string key1, string key2, string key3)
        {
            string[] keys = { key1, key2, key3 };
            Type[] types = { typeof(T), typeof(T1), typeof(T2) };
            var items = await MGet(keys, types);
            return ((T)items[0], (T1)items[1], (T2)items[2]);
        }

        public async ValueTask<(T, T1, T2, T3)> MGet<T, T1, T2, T3>(string key1, string key2, string key3, string key4)
        {
            string[] keys = { key1, key2, key3, key4 };
            Type[] types = { typeof(T), typeof(T1), typeof(T2), typeof(T3) };
            var items = await MGet(keys, types);
            return ((T)items[0], (T1)items[1], (T2)items[2], (T3)items[3]);
        }

        public async ValueTask<(T, T1, T2, T3, T4)> MGet<T, T1, T2, T3, T4>(string key1, string key2, string key3, string key4, string key5)
        {
            string[] keys = { key1, key2, key3, key4, key5 };
            Type[] types = { typeof(T), typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
            var items = await MGet(keys, types);
            return ((T)items[0], (T1)items[1], (T2)items[2], (T3)items[3], (T4)items[4]);
        }

        public async ValueTask<object[]> MGet(string[] keys, Type[] types)
        {
            Commands.MGET cmd = new Commands.MGET(DataFormater, keys);
            var result = await Execute(cmd, types);
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (from a in result.Data select a.Data).ToArray();

        }

        public async ValueTask<string> PSetEX(string key, long milliseconds, object value)
        {
            Commands.PSETEX cmd = new Commands.PSETEX(key, milliseconds, value, DataFormater);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (string)result.Value;
        }

        public async ValueTask<string> SetEX(string key, int seconds, object value)
        {
            Commands.SETEX cmd = new Commands.SETEX(key, seconds, value, DataFormater);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (string)result.Value;
        }

        public async ValueTask<long> SetNX(string key, object value)
        {
            Commands.SETNX cmd = new Commands.SETNX(key, value, DataFormater);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> SetRange(string key, int offset, string value)
        {
            Commands.SETRANGE cmd = new Commands.SETRANGE(key, offset, value);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }



        public async ValueTask<long> Strlen(string key)
        {
            Commands.STRLEN cmd = new Commands.STRLEN(key);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public Sequence CreateSequence(string key)
        {
            return new Sequence(this, key);
        }

        public RedisHashTable CreateHashTable(string key)
        {
            return new RedisHashTable(this, key, DataFormater);
        }

        public RedisList<T> CreateList<T>(string key)
        {
            return new RedisList<T>(this, key, DataFormater);
        }

        public async ValueTask<long> Publish(string channel, object data)
        {
            Commands.PUBLISH cmd = new Commands.PUBLISH(channel, data, DataFormater);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> PFCount(params string[] keys)
        {
            Commands.PFCount cmd = new Commands.PFCount(keys);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<long> PFAdd(string key, params string[] items)
        {
            Commands.PFAdd cmd = new Commands.PFAdd(key, items);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async Task PFMerge(string key, params string[] items)
        {
            Commands.PFMerge cmd = new Commands.PFMerge(key, items);
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
        }




        public async ValueTask<InfoResult> Info(InfoSection? section = null)
        {
            Commands.INFO cmd = new INFO();
            cmd.Section = section;
            var result = await Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (InfoResult)result.Value;

        }

        private bool mIsDisposed = false;

        public void Dispose()
        {
            if (!mIsDisposed)
            {
                mIsDisposed = true;
                if (mDetectionTime != null)
                {
                    mDetectionTime.Dispose();
                    mDetectionTime = null;
                }
                foreach (var item in mReadHosts)
                    item.Dispose();
                foreach (var item in mWriteHosts)
                    item.Dispose();
            }
        }
    }
}
