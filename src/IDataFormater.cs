using BeetleX.Buffers;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis
{
    public interface IDataFormater
    {
        void SerializeObject(object data, RedisClient client, PipeStream stream);

        object DeserializeObject(Type type, byte[] data);
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

        public object DeserializeObject(Type type, byte[] data)
        {
            using (SerializerExpand jsonExpend = SerializerExpand.Pop())
            {
                return jsonExpend.DeserializeJsonObject(data, type);
            }
        }

        public void SerializeObject(object data, RedisClient client, PipeStream stream)
        {
            using (SerializerExpand jsonExpend = client.SerializerExpand)
            {
                var buffer = jsonExpend.SerializeJsonObject(data);
                var hdata = Command.GetBodyHeaderLenData(buffer.Count);
                if (hdata != null)
                {
                    stream.Write(hdata, 0, hdata.Length);
                }
                else
                {
                    var headerstr = $"${buffer.Count}\r\n";
                    stream.Write(headerstr);
                }
                stream.Write(buffer.Array, buffer.Offset, buffer.Count);
            }
        }
    }

    public class MessagePackFormater : IDataFormater
    {
        public object DeserializeObject(Type type, RedisClient client, PipeStream stream, int length)
        {
            var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(length);
            try
            {
                stream.Read(buffer, 0, length);
                return MessagePackSerializer.Deserialize(type, new ReadOnlyMemory<byte>(buffer, 0, length));
            }
            finally
            {
                System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public object DeserializeObject(Type type, byte[] data)
        {
            return MessagePackSerializer.Deserialize(type, new ReadOnlyMemory<byte>(data, 0, data.Length));
        }

        public void SerializeObject(object data, RedisClient client, PipeStream stream)
        {
            // MessagePackSerializer.NonGeneric.Serialize(data.GetType(), stream, data);
            using (SerializerExpand jsonExpend = client.SerializerExpand)
            {

                var buffer = jsonExpend.SerializeMessagePack(data);
                var hdata = Command.GetBodyHeaderLenData(buffer.Count);
                if (hdata != null)
                {
                    stream.Write(hdata, 0, hdata.Length);
                }
                else
                {
                    var headerstr = $"${buffer.Count}\r\n";
                    stream.Write(headerstr);
                }
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

        public object DeserializeObject(Type type, byte[] data)
        {
            return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(type, new ReadOnlySpan<byte>(data));
        }

        public void SerializeObject(object data, RedisClient client, PipeStream stream)
        {
            using (SerializerExpand jsonExpend = client.SerializerExpand)
            {

                var buffer = jsonExpend.SerializeProtobufObject(data);
                var hdata = Command.GetBodyHeaderLenData(buffer.Count);
                if (hdata != null)
                {
                    stream.Write(hdata, 0, hdata.Length);
                }
                else
                {
                    var headerstr = $"${buffer.Count}\r\n";
                    stream.Write(headerstr);
                }
                stream.Write(buffer.Array, buffer.Offset, buffer.Count);
            }
        }
    }
}
