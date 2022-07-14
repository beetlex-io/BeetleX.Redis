using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Redis
{
    public class RedisStream<T>
    {
        internal RedisStream(RedisDB db, string name)
        {
            mDB = db;
            Name = name;
        }

        public string Name { get; private set; }

        private RedisDB mDB;

        public RedisDB DB => mDB;

        public ValueTask<string> Add(object data, string id = null)
        {
            return Add(data, null, id);
        }

        public ValueTask<string> Add(Dictionary<string, string> properties, string id = null)
        {
            return Add(properties, null, id);
        }

        public ValueTask<string> Add(params Tuple<string, string>[] properties)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (var item in properties)
                data[item.Item1] = item.Item1;
            return Add(data);
        }

        public async ValueTask<string> Add(object data, int? maxlen, string id)
        {
            Commands.XADD cmd = new Commands.XADD(Name, id);
            cmd.DataFormater = mDB.DataFormater;
            cmd.MAXLEN = maxlen;
            cmd.Data = data;
            var result = await mDB.Execute(cmd, typeof(string));
            result.Throw();
            return (string)result.Value;
        }

        public async ValueTask<long> Del(params string[] id)
        {
            Commands.XDEL cmd = new Commands.XDEL(Name, id);
            var result = await mDB.Execute(cmd, typeof(string));
            result.Throw();
            return (long)result.Value;
        }

        public async ValueTask<long> Len()
        {
            Commands.XLEN cmd = new Commands.XLEN(Name);
            var result = await mDB.Execute(cmd, typeof(string));
            result.Throw();
            return (long)result.Value;
        }

        public ValueTask<List<StreamDataItem<T>>> RangeAll(int? count = null)
        {
            return Range("0", null, count);
        }

        public ValueTask<List<StreamDataItem<T>>> Range(int? count = null)
        {
            return Range(null, null, count);
        }

        public async ValueTask<List<StreamDataItem<T>>> Range(string start, string stop, int? count = null)
        {
            StreamDataItemReceive<T> receiver = new StreamDataItemReceive<T>();
            Commands.XRANGE cmd = new Commands.XRANGE(Name, start, stop);
            cmd.NetworkReceive = receiver.Receive;
            cmd.DataFormater = mDB.DataFormater;
            cmd.Count = count;
            var result = await mDB.Execute(cmd, typeof(T));
            result.Throw();
            List<StreamDataItem<T>> items = new List<StreamDataItem<T>>();
            foreach (var item in result.Data)
            {
                ((StreamDataItem<T>)item.Data).Stream = this;
                ((StreamDataItem<T>)item.Data).StreamName = Name;
                items.Add((StreamDataItem<T>)item.Data);
            }
            return items;
        }

        public ValueTask<List<StreamDataItem<T>>> RevRangeAll(int? count = null)
        {
            return RevRange(null, "0", count);
        }

        public ValueTask<List<StreamDataItem<T>>> RevRange(int? count = null)
        {
            return RevRange(null, null, count);
        }

        public async ValueTask<List<StreamDataItem<T>>> RevRange(string start, string stop, int? count = null)
        {
            StreamDataItemReceive<T> receiver = new StreamDataItemReceive<T>();
            Commands.XREVRANGE cmd = new Commands.XREVRANGE(Name, start, stop);
            cmd.NetworkReceive = receiver.Receive;
            cmd.DataFormater = mDB.DataFormater;
            cmd.Count = count;
            var result = await mDB.Execute(cmd, typeof(T));
            result.Throw();
            List<StreamDataItem<T>> items = new List<StreamDataItem<T>>();
            foreach (var item in result.Data)
            {
                ((StreamDataItem<T>)item.Data).Stream = this;
                ((StreamDataItem<T>)item.Data).StreamName = Name;
                items.Add((StreamDataItem<T>)item.Data);
            }
            return items;
        }



        public ValueTask<List<StreamDataItem<T>>> Read(int? block = null)
        {
            return Read(block, null, null);
        }

        public async ValueTask<List<StreamDataItem<T>>> Read(int? block, int? count, string start = null)
        {
            StreamDataReceive<T> receiver = new StreamDataReceive<T>();
            Commands.XREAD cmd = new Commands.XREAD(Name);
            cmd.NetworkReceive = receiver.Receive;
            cmd.DataFormater = mDB.DataFormater;
            cmd.Block = block;
            cmd.Count = count;
            cmd.Start = start;
            var result = await mDB.Execute(cmd, typeof(T));
            result.Throw();
            List<StreamDataItem<T>> items = new List<StreamDataItem<T>>();
            foreach (var item in result.Data)
            {
                ((StreamDataItem<T>)item.Data).Stream = this;
                items.Add((StreamDataItem<T>)item.Data);
            }
            return items;
        }

        public async ValueTask<RedisStreamGroup<T>> GetGroup(string name, string start = null)
        {
            Commands.XGROUP_CREATE cmd = new Commands.XGROUP_CREATE(Name, name);
            if (!string.IsNullOrEmpty(start))
                cmd.Start = start;
            var result = await mDB.Execute(cmd, typeof(T));
            if (result.IsError)
            {
                if (result.Messge.IndexOf("name already exists", StringComparison.OrdinalIgnoreCase) == -1)
                    result.Throw();
            }
            return new RedisStreamGroup<T>(this, name, start);
        }

        public async ValueTask<long> DestroyGroup(string name)
        {
            Commands.XGROUP_DESTROY cmd = new Commands.XGROUP_DESTROY(Name, name);
            var result = await mDB.Execute(cmd, typeof(T));
            result.Throw();
            return (long)result.Value;
        }


    }
}
