using BeetleX.Buffers;
using BeetleX.Clients;
using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BeetleX.Redis
{
    public class RedisClient
    {
        public RedisClient(bool ssl, string host, int port = 6379)
        {
            Host = host;
            if (ssl)
            {

                TcpClient = BeetleX.SocketFactory.CreateSslClient<AsyncTcpClient>(host, port, "beetlex");
                TcpClient.CertificateValidationCallback = (o, e, f, d) =>
                {
                    return true;
                };
            }
            else
            {
                TcpClient = BeetleX.SocketFactory.CreateClient<AsyncTcpClient>(host, port);
            }
        }

        public string Host { get; set; }

        public AsyncTcpClient TcpClient { get; private set; }

        internal SerializerExpand SerializerExpand
        {
            get
            {
                return SerializerExpand.Pop();
            }
        }

        public void Send(Command cmd)
        {
            PipeStream stream = TcpClient.Stream.ToPipeStream();

            cmd.Execute(this, stream);

            TcpClient.Stream.Flush();
        }

    }

    public class JsonMemoryStream : MemoryStream
    {
        public JsonMemoryStream(int size) : base(size) { }

        protected override void Dispose(bool disposing)
        {

        }
    }

    class SerializerExpand : IDisposable
    {

        public SerializerExpand()
        {
            Memory = new JsonMemoryStream(1024 * 32);
            InitStream();
        }

        private System.IO.StreamReader StreamReader;

        private System.IO.StreamWriter StreamWriter;

        private void InitStream()
        {
            StreamReader = new StreamReader(Memory);

            StreamWriter = new StreamWriter(Memory);

        }

        private const int BufferSize = 1024 * 4;

        private byte[] mBuffer = new byte[BufferSize];

        public object DeserializeJsonObject(System.IO.Stream steram, int length, Type type)
        {
            try
            {
                while (length > 0)
                {
                    var readcount = BufferSize;
                    if (length < BufferSize)
                        readcount = length;
                    var len = steram.Read(mBuffer, 0, readcount);
                    Memory.Write(mBuffer, 0, len);
                    length -= len;
                }
                Memory.Position = 0;
                var task = System.Text.Json.JsonSerializer.DeserializeAsync(Memory, type);
                return task.Result;

            }
            catch (Exception e_)
            {
                InitStream();
                throw new RedisException($"json deserialize error {e_.Message}", e_);
            }
        }



        public ArraySegment<byte> SerializeJsonObject(Object data)
        {
            System.Text.Json.JsonSerializer.SerializeAsync(Memory, data);
            return GetBuffer();
        }

        public object DeserializeProtobufObject(System.IO.Stream stream, int length, Type type)
        {
            return ProtoBuf.Meta.RuntimeTypeModel.Default.Deserialize(stream, null, type, length);
        }

        public ArraySegment<byte> SerializeProtobufObject(Object data)
        {
            ProtoBuf.Meta.RuntimeTypeModel.Default.Serialize(Memory, data);
            return GetBuffer();
        }

        public ArraySegment<byte> SerializeMessagePack(object data)
        {
            MessagePackSerializer.Serialize(data.GetType(), Memory, data);
            return GetBuffer();
        }




        public System.IO.MemoryStream Memory { get; private set; }

        public ArraySegment<byte> GetBuffer()
        {
            var result = new ArraySegment<byte>(Memory.GetBuffer(), 0, (int)Memory.Position);
            return result;
        }

        public void Reset()
        {

            Memory.SetLength(0);
            Memory.Position = 0;
        }

        public void Dispose()
        {
            Reset();
            Push(this);
        }

        private static System.Collections.Concurrent.ConcurrentStack<SerializerExpand> mPools = new System.Collections.Concurrent.ConcurrentStack<SerializerExpand>();

        public static SerializerExpand Pop()
        {
            if (mPools.TryPop(out SerializerExpand result))
            {
                return result;
            }
            return new SerializerExpand();
        }

        public static void Push(SerializerExpand jsonWriterExpand)
        {
            mPools.Push(jsonWriterExpand);
        }
    }

}
