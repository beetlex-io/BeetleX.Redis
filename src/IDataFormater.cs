using BeetleX.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis
{
    public interface IDataFormater
    {
        void SerializeObject(object data, RedisClient client, PipeStream stream);

        object DeserializeObject(Type type, RedisClient client, PipeStream stream, int length);

    }

    public class JsonFormater : IDataFormater
    {
        public object DeserializeObject(Type type, RedisClient client, PipeStream stream, int length)
        {
            using (SerializerExpand jsonExpend = client.SerializerExpand)
            {
                return jsonExpend.DeserializeJsonObject(stream, length, type);
            }
        }

        public void SerializeObject(object data, RedisClient client, PipeStream stream)
        {
            using (SerializerExpand jsonExpend = client.SerializerExpand)
            {

                var buffer = jsonExpend.SerializeJsonObject(data);
                var headerstr = $"${buffer.Count}\r\n";
                stream.Write(headerstr);
                stream.Write(buffer.Array, buffer.Offset, buffer.Count);
            }
        }
    }

    public class ProtobufFormater : IDataFormater
    {
        public object DeserializeObject(Type type, RedisClient client, PipeStream stream, int length)
        {
            return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(stream, null, type, length);
        }

        public void SerializeObject(object data, RedisClient client, PipeStream stream)
        {
            using (SerializerExpand jsonExpend = client.SerializerExpand)
            {

                var buffer = jsonExpend.SerializeProtobufObject(data);
                var headerstr = $"${buffer.Count}\r\n";
                stream.Write(headerstr);
                stream.Write(buffer.Array, buffer.Offset, buffer.Count);
            }
        }
    }
}
