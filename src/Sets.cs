using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Redis
{
    public class Sets
    {
        internal Sets(RedisDB db)
        {
            mDB = db;
        }

        private RedisDB mDB;

        public async ValueTask<long> SAdd(string key, params string[] members)
        {
            Commands.SETS_SADD cmd = new Commands.SETS_SADD(key, members);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<bool> SisMember(string key, string member)
        {
            Commands.SETS_SISMEMBER cmd = new Commands.SETS_SISMEMBER(key, member);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value == 1;
        }
        public async ValueTask<string> SPop(string key)
        {
            Commands.SETS_SPOP cmd = new Commands.SETS_SPOP(key);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (string)result.Value;
        }

        public async ValueTask<List<string>> SRandMember(string key, int count)
        {
            Commands.SETS_SRANDMEMBER cmd = new Commands.SETS_SRANDMEMBER(key, count);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            List<string> data = new List<string>();
            foreach (var item in result.Data)
            {
                data.Add((string)item.Data);
            }
            return data;
        }
        public async ValueTask<long> SRem(string key, params string[] members)
        {
            Commands.SETS_SREM cmd = new Commands.SETS_SREM(key, members);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<bool> SMove(string source, string dest, string member)
        {
            Commands.SETS_SMOVE cmd = new Commands.SETS_SMOVE(source, dest, member);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value == 1;
        }

        public async ValueTask<long> SCard(string key)
        {
            Commands.SETS_SCARD cmd = new Commands.SETS_SCARD(key);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<List<string>> SMembers(string key)
        {
            Commands.SETS_SMEMBERS cmd = new Commands.SETS_SMEMBERS(key);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            List<string> data = new List<string>();
            foreach (var item in result.Data)
            {
                data.Add((string)item.Data);
            }
            return data;
        }

        public async ValueTask<Commands.ScanResult> SScan(string key, int cursor, int count = 20, string pattern = null)
        {
            List<string> items = new List<string>();
            Commands.SETS_SSCAN cmd = new Commands.SETS_SSCAN();
            cmd.Key = key;
            cmd.Cursor = cursor;
            cmd.Count = count;
            cmd.Pattern = pattern;
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (Commands.ScanResult)result.Value;
        }

        public async ValueTask<List<string>> SInter(params string[] keys)
        {
            Commands.SETS_SINTER cmd = new Commands.SETS_SINTER(keys);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            List<string> data = new List<string>();
            foreach (var item in result.Data)
            {
                data.Add((string)item.Data);
            }
            return data;
        }

        public async ValueTask<long> SInterStore(string dest, params string[] keys)
        {
            Commands.SETS_SINTERSTORE cmd = new Commands.SETS_SINTERSTORE(dest, keys);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<List<string>> SUnion(params string[] keys)
        {
            Commands.SETS_SUNION cmd = new Commands.SETS_SUNION(keys);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            List<string> data = new List<string>();
            foreach (var item in result.Data)
            {
                data.Add((string)item.Data);
            }
            return data;
        }

        public async ValueTask<long> SUnionStore(string dest, params string[] keys)
        {
            Commands.SETS_SUNIONSTORE cmd = new Commands.SETS_SUNIONSTORE(dest, keys);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }

        public async ValueTask<List<string>> SDiff(params string[] keys)
        {
            Commands.SETS_SDIFF cmd = new Commands.SETS_SDIFF(keys);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            List<string> data = new List<string>();
            foreach (var item in result.Data)
            {
                data.Add((string)item.Data);
            }
            return data;
        }

        public async ValueTask<long> SDiffStore(string dest,params string[] keys)
        {
            Commands.SETS_SDIFFSTORE cmd = new Commands.SETS_SDIFFSTORE(dest, keys);
            var result = await mDB.Execute(cmd, typeof(string));
            if (result.IsError)
                throw new RedisException(result.Messge);
            return (long)result.Value;
        }
    }
}
