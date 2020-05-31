using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Redis
{
    public class RedisStreamGroup<T>
    {

        public RedisStreamGroup(RedisStream<T> stream, string name,string start)
        {
            Stream = stream;
            Name = name;
            Start = start;
        }

        public string Start { get; private set; }

        public string Name { get; private set; }

        private StreamDataReceive<T> mReceiver;

        public RedisStream<T> Stream { get; private set; }

        public async ValueTask<bool> SetID(string id)
        {
            Commands.XGROUP_SETID cmd = new Commands.XGROUP_SETID(Stream.Name, Name, id);
            var result = await Stream.DB.Execute(cmd, typeof(string));
            result.Throw();
            return true;
        }

        public async ValueTask<List<StreamDataItem<T>>> ReadWait(string consumer,int timeout=0)
        {
            return await Read(consumer, timeout, null, null);
        }

        public ValueTask<List<StreamDataItem<T>>> Read(string consumer,string start = null)
        {
            return Read(consumer, null, null, start);
        }

        public async ValueTask<List<StreamDataItem<T>>> Read(string consumer, int? block, int? count, string start = null)
        {
            StreamDataReceive<T> receiver = new StreamDataReceive<T>();
            mReceiver = receiver;
            Commands.XREADGROUP cmd = new Commands.XREADGROUP(Stream.Name, Name, consumer, start);
            cmd.NetworkReceive = receiver.Receive;
            cmd.DataFormater = Stream.DB.DataFormater;
            cmd.Block = block;
            cmd.Count = count;
            var result = await Stream.DB.Execute(cmd, typeof(T));
            result.Throw();
            List<StreamDataItem<T>> items = new List<StreamDataItem<T>>();
            foreach (var item in result.Data)
            {
                ((StreamDataItem<T>)item.Data).Group = this;
                items.Add((StreamDataItem<T>)item.Data);
            }
            return items;
        }

        public async ValueTask<long> Ack(params string[] id)
        {
            Commands.XACK cmd = new Commands.XACK(Stream.Name, Name, id);
            var result = await Stream.DB.Execute(cmd, typeof(T));
            result.Throw();
            return (long)result.Value;
        }
        
    }
}
