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

        public async ValueTask<string> Add(object data, string id = null)
        {
            Commands.XADD cmd = new Commands.XADD(Name, id);
            cmd.DataFormater = mDB.DataFormater;
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
                items.Add((StreamDataItem<T>)item.Data);
            }
            return items;
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
                items.Add((StreamDataItem<T>)item.Data);
            }
            return items;
        }
    }
}
