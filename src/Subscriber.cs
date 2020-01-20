using BeetleX.Buffers;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Redis
{
    public class Subscriber : IDisposable
    {

        public Subscriber(RedisDB db)
        {
            mDB = db;
        }

        private RedisDB mDB;

        private int mDisposed = 0;

        private ConcurrentDictionary<string, ChannelRegister> mChannels = new ConcurrentDictionary<string, ChannelRegister>();

        private RedisClient redisClient;

        private SubscribeRequest subscribeRequest;

        private Commands.SUBSCRIBE mCommand;

        public Subscriber Register<T>(string channel, Action<T> action)
        {
            Register(channel, typeof(T), action);
            return this;
        }

        protected Subscriber Register(string channel, Type msgType, Delegate action)
        {
            mChannels[channel] = new ChannelRegister { MessageType = msgType, Action = action };
            return this;
        }

        public void UnRegister(string channel)
        {
            if (mChannels.TryRemove(channel, out ChannelRegister type))
            {
                Commands.UNSUBSCRIBE cmd = new Commands.UNSUBSCRIBE(channel);
                subscribeRequest?.SendCommmand(cmd);
            }
        }

        protected void OnReceive(SubscribeMessage e)
        {
            try
            {
                Receive?.Invoke(this, e);
            }
            catch { }
        }

        public EventHandler<SubscribeMessage> Receive { get; set; }

        private async void OnReadMessave()
        {

            while (true)
            {
                var result = await subscribeRequest.Read();
                if (result.ResultType == ResultType.NetError || result.ResultType == ResultType.DataError || result.ResultType == ResultType.Error)
                {
                    OnReceive(new SubscribeMessage { Type = SubscribeMessageType.Error, Data = result.Messge });
                    if (redisClient.TcpClient.IsConnected)
                    {
                        var pipeStream = redisClient.TcpClient.Stream.ToPipeStream();
                        pipeStream.ReadFree((int)pipeStream.Length);
                    }
                    else
                    {
                        ReOnSubscrib();
                        break;
                    }
                }
                else
                {
                    ProcessReceiveData(result);
                }
            }

        }

        private void ProcessReceiveData(Result result)
        {
            var items = result.Data;

            for (int i = 0; i < items.Count; i = i + 3)
            {
                SubscribeMessage subscribeMessage = new SubscribeMessage();
                if ((string)items[i].Data == "subscribe")
                {
                    subscribeMessage.Type = SubscribeMessageType.Subscribe;
                }
                else
                {
                    subscribeMessage.Type = SubscribeMessageType.Message;
                }
                subscribeMessage.Channel = (string)items[i + 1].Data;
                subscribeMessage.Data = items[i + 2].Data;
                if (subscribeMessage.Type == SubscribeMessageType.Message)
                {
                    if (mChannels.TryGetValue(subscribeMessage.Channel, out ChannelRegister register))
                    {
                        try
                        {
                            register.Action.DynamicInvoke(subscribeMessage.Data);
                        }
                        catch (Exception e_)
                        {
                            subscribeMessage.Type = SubscribeMessageType.Error;
                            subscribeMessage.Data = $"invoke {subscribeMessage.Channel} error {e_.Message}";
                            OnReceive(subscribeMessage);
                        }
                    }
                    else
                    {
                        OnReceive(subscribeMessage);
                    }
                }
                else
                {
                    OnReceive(subscribeMessage);
                }
            }
        }

        private bool OnBlockingRead(Result r, PipeStream stream, RedisClient c)
        {
            int index = r.ReadCount % 3;
            if (index == 0 || index == 1)
            {

                var item = new ResultItem { Type = ResultType.String, Data = stream.ReadString(r.BodyLength.Value) };
                r.Data.Add(item);
                return false;
            }
            else
            {
                string channel = (string)r.Data[r.ArrayReadCount - 1].Data;
                mChannels.TryGetValue(channel, out ChannelRegister register);
                if (register.MessageType == null || mDB.DataFormater == null)
                {
                    var item = new ResultItem { Type = ResultType.String, Data = stream.ReadString(r.BodyLength.Value) };
                    r.Data.Add(item);
                }
                else
                {
                    try
                    {
                        var item = new ResultItem { Type = ResultType.Object, Data = mDB.DataFormater.DeserializeObject(register.MessageType, c, stream, r.BodyLength.Value) };
                        r.Data.Add(item);
                    }
                    catch (Exception e_)
                    {
                        throw new RedisException($"{channel} channel {mDB.DataFormater.GetType().Name} {e_.Message}", e_);
                    }
                }
                return false;
            }

        }

        private System.Threading.Timer mTimer;

        private void ReOnSubscrib()
        {
            if (mDisposed == 0)
                mTimer = new System.Threading.Timer(OnSubscrib, null, 2000, 2000);
        }

        private async void OnSubscrib(object state)
        {
            try
            {
                if (mTimer != null)
                {
                    mTimer.Dispose();
                    mTimer = null;
                }
                if (redisClient != null)
                    redisClient.TcpClient.DisConnect();
                Result result;
                RedisHost host = mDB.Host.GetWriteHost();
                if (host == null)
                {
                    result = new Result() { ResultType = ResultType.NetError, Messge = "redis server is not available" };
                }
                else
                {
                    redisClient = host.Create();
                    mCommand = new Commands.SUBSCRIBE(mDB.DataFormater, mChannels.Keys.ToArray());
                    mCommand.Reader = OnBlockingRead;
                    subscribeRequest = new SubscribeRequest(host, redisClient, mCommand, typeof(string));
                    result = await subscribeRequest.Execute();
                }
                if (result.ResultType == ResultType.NetError || result.ResultType == ResultType.DataError || result.ResultType == ResultType.Error)
                {
                    OnReceive(new SubscribeMessage { Type = SubscribeMessageType.Error, Data = result.Messge });
                    if (redisClient.TcpClient.IsConnected)
                    {
                        var pipeStream = redisClient.TcpClient.Stream.ToPipeStream();
                        pipeStream.ReadFree((int)pipeStream.Length);
                    }
                    else
                    {
                        ReOnSubscrib();
                    }
                }
                else
                {
                    ProcessReceiveData(result);
                    OnReadMessave();

                }
            }
            catch (Exception e_)
            {
                OnReceive(new SubscribeMessage { Type = SubscribeMessageType.Error, Data = e_.Message });
            }
        }

        public void Listen()
        {
            OnSubscrib(null);
        }

        public void Dispose()
        {
            if (System.Threading.Interlocked.CompareExchange(ref mDisposed, 1, 0) == 0)
            {
                Commands.UNSUBSCRIBE cmd = new Commands.UNSUBSCRIBE(null);
                subscribeRequest?.SendCommmand(cmd);
            }
        }

        class ChannelRegister
        {
            public Type MessageType { get; set; }
            public Delegate Action { get; set; }
        }

    }

    public class SubscribeMessage : System.EventArgs
    {

        public string Channel { get; set; }

        public SubscribeMessageType Type { get; set; }

        public object Data { get; set; }

    }

    public enum SubscribeMessageType
    {
        Subscribe,
        Error,
        Message
    }

}
