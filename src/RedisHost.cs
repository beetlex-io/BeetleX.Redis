using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace BeetleX.Redis
{
    public class RedisHost : IDisposable
    {
        public RedisHost(bool ssl, int db, string host, int port = 6379)
        {
            SSL = ssl;
            Host = host;
            Port = port;
            DB = db;
            Available = true;
            Master = true;
            mPingClient = new RedisClient(SSL, Host, Port);
        }

        private RedisClient mPingClient;

        private int mDisposed = 0;

        private int mCount = 0;


        private Queue<TaskCompletionSource<RedisClient>> mQueue = new Queue<TaskCompletionSource<RedisClient>>();

        private Stack<RedisClient> mPool = new Stack<RedisClient>();

        public int QueueMaxLength { get; set; } = 256;

        public int MaxConnections { get; set; } = 30;

        public int DB { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Password { get; set; }

        public bool Master { get; set; }

        public bool SSL { get; set; } = false;

        public Task<RedisClient> Pop()
        {
            lock (mPool)
            {
                TaskCompletionSource<RedisClient> result = new TaskCompletionSource<RedisClient>();
                if (!mPool.TryPop(out RedisClient client))
                {
                    mCount++;
                    if (mCount <= MaxConnections)
                    {
                        client = new RedisClient(SSL, Host, Port);
                        result.SetResult(client);
                    }
                    else
                    {
                        if (mQueue.Count > QueueMaxLength)
                        {
                            result.SetResult(null);
                        }
                        else
                        {
                            mQueue.Enqueue(result);
                        }
                    }
                }
                else
                {
                    result.SetResult(client);
                }
                return result.Task;
            }
        }

        public RedisClient Create()
        {
            var client = new RedisClient(SSL, Host, Port);
            var result = Connect(null, client);
            if (result.IsError)
            {
                client.TcpClient.DisConnect();
                throw new RedisException(result.Messge);
            }
            return client;
        }

        public bool Available { get; set; }

        public Result Connect(RedisDB db, RedisClient client)
        {
            if (!client.TcpClient.IsConnected)
            {
                bool isNew;
                if (client.TcpClient.Connect(out isNew))
                {
                    this.Available = true;
                    if (!string.IsNullOrEmpty(Password))
                    {
                        Commands.AUTH auth = new Commands.AUTH(Password);
                        RedisRequest request = new RedisRequest(null, client, auth, typeof(string));
                        var task = request.Execute(db);
                        task.Wait();
                        if (task.Result.ResultType == ResultType.DataError ||
                            task.Result.ResultType == ResultType.Error
                            || task.Result.ResultType == ResultType.NetError)
                            return task.Result;
                    }

                    Commands.SELECT select = new Commands.SELECT(DB);
                    var req = new RedisRequest(null, client, select, typeof(string));
                    var t = req.Execute(db);
                    t.Wait();
                    return t.Result;
                }
                else
                {
                    this.Available = false;
                    return new Result { ResultType = ResultType.NetError, Messge = client.TcpClient.LastError.Message };
                }
            }
            return new Result { ResultType = ResultType.Simple, Messge = "Connected" };
        }

        public void Push(RedisClient client)
        {
            TaskCompletionSource<RedisClient> item = null;
            lock (mPool)
            {
                if (mDisposed > 0)
                {
                    client.TcpClient.DisConnect();
                }
                else
                {
                    if (!mQueue.TryDequeue(out item))
                    {
                        mPool.Push(client);
                    }

                }
            }
            if (item != null)
            {
                Task.Run(() => item.SetResult(client));
            }
        }

        public void Dispose()
        {
            if (System.Threading.Interlocked.CompareExchange(ref mDisposed, 1, 0) == 0)
            {
                while (mPool.TryPop(out RedisClient item))
                {
                    item.TcpClient.DisConnect();
                }
                while (mQueue.TryDequeue(out TaskCompletionSource<RedisClient> t))
                {
                    t.SetCanceled();
                }
            }
        }

        private int mPingStatus = 0;

        public async void Ping()
        {
            if (System.Threading.Interlocked.CompareExchange(ref mPingStatus, 1, 0) == 0)
            {

                try
                {
                    Connect(null, mPingClient);
                    Commands.PING ping = new Commands.PING(null);
                    var request = new RedisRequest(null, mPingClient, ping, typeof(string));
                    var result = await request.Execute(null);
                }
                catch
                {

                }
                finally
                {
                    System.Threading.Interlocked.Exchange(ref mPingStatus, 0);

                }
            }
        }


    }
}
