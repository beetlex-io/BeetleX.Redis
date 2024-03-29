﻿using BeetleX.Buffers;
using BeetleX.Clients;
using BeetleX.Redis.Commands;
using BeetleX.Tracks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace BeetleX.Redis
{
    public class RedisRequest
    {
        public RedisRequest(RedisHost host, RedisClient client, Command cmd, params Type[] types)
        {
            Client = client;
            Client.TcpClient.DataReceive = OnReceive;
            Client.TcpClient.ClientError = OnError;
            Command = cmd;
            Host = host;
            Types = types;
            Host = host;
        }

        internal XActivity Activity { get; set; }

        public RedisHost Host { get; set; }

        private void OnError(IClient c, ClientErrorArgs e)
        {
            if (e.Error is BeetleX.BXException || e.Error is System.Net.Sockets.SocketException ||
                e.Error is System.ObjectDisposedException)
            {
                c.DisConnect();
                OnCompleted(ResultType.NetError, e.Error.Message);
            }
            else
            {
                OnCompleted(ResultType.DataError, e.Error.Message);
            }
        }

        public Type[] Types { get; private set; }

        private int mFreeLength;

        private void FreeLine(PipeStream stream)
        {
            if (stream.Length >= 2)
            {
                stream.ReadFree(2);
                mFreeLength = 0;
            }
            else
            {
                mFreeLength = 2;
            }
        }

        private CodeTrack mReceiveTrack;

        private void OnReceive(IClient c, ClientReceiveArgs reader)
        {
            mReceiveTrack = CodeTrackFactory.Track("Read", CodeTrackLevel.Function, Activity?.Id, "Redis", "Protocol");
            if (Command.NetworkReceive != null)
            {
                var result = Command.NetworkReceive(this, reader.Stream.ToPipeStream());
                if (result != null)
                {
                    Result = result;
                    OnCompleted(ResultType.Object, null);
                }
            }
            else
            {

                ResultType resultType;
                string msg;
                PipeStream pipeStream = reader.Stream.ToPipeStream();
                if (mFreeLength > 0)
                {
                    pipeStream.ReadFree(mFreeLength);
                    mFreeLength = 0;
                }
                if (Result.Status == ResultStatus.None)
                {
                    if (pipeStream.TryReadLine(out string line))
                    {
                        char type = line[0];
                        switch (type)
                        {
                            case '+':
                                resultType = ResultType.Simple;
                                msg = line.Substring(1, line.Length - 1);
                                OnCompleted(resultType, msg);
                                return;
                            case '-':
                                resultType = ResultType.Error;
                                msg = line.Substring(1, line.Length - 1);
                                OnCompleted(resultType, msg);
                                return;
                            case ':':
                                Result.Data.Add(new ResultItem { Type = ResultType.Integers, Data = long.Parse(line.Substring(1, line.Length - 1)) });
                                Result.Status = ResultStatus.Completed;
                                OnCompleted(ResultType.Integers, null);
                                return;
                            case '$':
                                Result.ResultType = ResultType.Bulck;
                                Result.ArrayCount = 1;
                                Result.BodyLength = int.Parse(line.Substring(1, line.Length - 1));
                                Result.Status = ResultStatus.Loading;
                                break;
                            case '*':
                                Result.ResultType = ResultType.Arrays;
                                Result.ArrayCount = int.Parse(line.Substring(1, line.Length - 1));
                                Result.Status = ResultStatus.Loading;
                                break;
                        }
                    }
                }
                if (Result.Status == ResultStatus.Loading)
                {
                    if (Result.ResultType == ResultType.Arrays)
                    {
                        LoadArray(pipeStream);
                    }
                    else if (Result.ResultType == ResultType.Bulck)
                    {
                        LoadBulck(pipeStream);
                    }
                }
            }
        }

        private void LoadBulck(PipeStream pipeStream)
        {
            if (Result.BodyLength == -1)
            {
                Result.Data.Add(new ResultItem { Type = ResultType.NotFound, Data = null });
                OnCompleted(ResultType.Bulck, null);
            }
            else if (Result.BodyLength == 0)
            {
                if (pipeStream.Length >= 2)
                {
                    Result.Data.Add(new ResultItem { Type = ResultType.Null, Data = null });
                    pipeStream.ReadFree(2);
                    OnCompleted(ResultType.Bulck, null);
                }
            }
            else
            {
                if (pipeStream.Length >= Result.BodyLength + 2)
                {
                    try
                    {
                        object value = null;
                        if (Command.DataFormater == null)
                        {
                            if (Types.Length == 1 && Types[0] == typeof(ArraySegment<byte>))
                            {
                                var len = Result.BodyLength.Value;
                                var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(len);
                                pipeStream.Read(buffer, 0, len);
                                value = new ArraySegment<byte>(buffer, 0, len);
                            }
                            else
                            {
                                value = pipeStream.ReadString(Result.BodyLength.Value);
                            }
                        }
                        else
                        {
                            var type = Types[Result.Data.Count % Types.Length];
                            if (type == typeof(ArraySegment<byte>))
                            {
                                var len = Result.BodyLength.Value;
                                var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(len);
                                pipeStream.Read(buffer, 0, len);
                                value = new ArraySegment<byte>(buffer, 0, len);
                            }
                            else
                            {
                                value = Command.DataFormater.DeserializeObject(type, Client, pipeStream, Result.BodyLength.Value);
                            }
                        }
                        Result.Data.Add(new ResultItem { Type = ResultType.Object, Data = value });
                        pipeStream.ReadFree(2);
                        OnCompleted(ResultType.Bulck, null);
                    }
                    catch (Exception e_)
                    {
                        OnCompleted(ResultType.DataError, e_.Message);
                    }
                }
            }
        }

        private void LoadArray(PipeStream pipeStream)
        {
            START:
            if (Result.ArrayCount == -1 || Result.ArrayCount == 0)
            {
                OnCompleted(ResultType.Arrays, null);
                return;
            }
            else
            {
                if (Result.BodyLength == null)
                {
                    if (pipeStream.TryReadLine(out string line))
                    {
                        if (line[0] == ':')
                        {
                            var item = new ResultItem { Type = ResultType.Integers, Data = long.Parse(line.Substring(1, line.Length - 1)) };
                            Result.Data.Add(item);
                            Result.ArrayReadCount++;
                            Result.ReadCount++;
                        }
                        else if (line[0] == '+')
                        {
                            var item = new ResultItem { Type = ResultType.Simple, Data = line.Substring(1, line.Length - 1) };
                            Result.Data.Add(item);
                            Result.ArrayReadCount++;
                            Result.ReadCount++;
                        }
                        else if (line[0] == '-')
                        {
                            var item = new ResultItem { Type = ResultType.Error, Data = line.Substring(1, line.Length - 1) };
                            Result.Data.Add(item);
                            Result.ArrayReadCount++;
                            Result.ReadCount++;
                        }
                        else
                        {
                            Result.BodyLength = int.Parse(line.Substring(1, line.Length - 1));
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                if (Result.BodyLength != null)
                {
                    if ((Result.BodyLength == -1))
                    {
                        if (Command.Reader == null || Command.Reader(Result, pipeStream, Client))
                        {
                            var item = new ResultItem { Type = ResultType.NotFound, Data = null };
                            Result.Data.Add(item);
                        }
                        Result.BodyLength = null;
                        Result.ArrayReadCount++;
                        Result.ReadCount++;
                    }
                    else if (Result.BodyLength == 0)
                    {
                        if (pipeStream.Length >= 2)
                        {
                            if (Command.Reader == null || Command.Reader(Result, pipeStream, Client))
                            {
                                var item = new ResultItem { Type = ResultType.Null, Data = null };
                                Result.Data.Add(item);
                            }
                            FreeLine(pipeStream);
                            //pipeStream.ReadFree(2);
                            Result.BodyLength = null;
                            Result.ArrayReadCount++;
                            Result.ReadCount++;
                        }
                    }
                    else if (pipeStream.Length >= Result.BodyLength)
                    {
                        object value = null;
                        if (Command.Reader == null || Command.Reader(Result, pipeStream, Client))
                        {
                            if (Command.DataFormater == null)
                            {

                                Type type = Types[Result.Data.Count % Types.Length];
                                if (type == typeof(ArraySegment<byte>))
                                {
                                    var bytes = System.Buffers.ArrayPool<byte>.Shared.Rent(Result.BodyLength.Value);
                                    pipeStream.Read(bytes, 0, Result.BodyLength.Value);
                                    value = new ArraySegment<byte>(bytes, 0, Result.BodyLength.Value);
                                }
                                else
                                {
                                    value = pipeStream.ReadString(Result.BodyLength.Value);
                                    value = Convert.ChangeType(value, type);
                                }
                            }
                            else
                            {
                                value = Command.DataFormater.DeserializeObject(Types[Result.Data.Count % Types.Length], Client, pipeStream, Result.BodyLength.Value);
                            }
                            var item = new ResultItem
                            {
                                Type = ResultType.Object,
                                Data = value
                            };
                            Result.Data.Add(item);
                        }
                        FreeLine(pipeStream);
                        //pipeStream.ReadFree(2);
                        Result.ReadCount++;
                        Result.BodyLength = null;
                        Result.ArrayReadCount++;
                    }
                    else
                    {
                        return;
                    }
                }
                if (Result.ArrayReadCount == Result.ArrayCount)
                {
                    OnCompleted(ResultType.Arrays, null);
                    return;
                }
            }
            if (pipeStream.Length > 0)
                goto START;
        }

        public Action<RedisRequest> Completed { get; set; }

        protected Result Result { get; set; } = new Result();

        public Command Command { get; private set; }

        public RedisClient Client { get; private set; }

        public TaskCompletionSource<Result> TaskCompletionSource { get; protected set; }

        internal void SendCommmand(RedisDB db, Command cmd)
        {
            try
            {
                Client.Send(db, cmd);
                if (!Client.TcpClient.IsConnected)
                {
                    OnCompleted(ResultType.NetError, "Connection is closed!");
                }
            }
            catch (Exception e_)
            {
                OnCompleted(ResultType.DataError, e_.Message);
            }
        }

        public Task<Result> Execute(RedisDB db)
        {

            TaskCompletionSource = new TaskCompletionSource<Result>();
            SendCommmand(db, Command);
            return TaskCompletionSource.Task;

        }

        private int mCompletedStatus = 0;

        public virtual void OnCompleted(ResultType type, string message)
        {
            if (System.Threading.Interlocked.CompareExchange(ref mCompletedStatus, 1, 0) == 0)
            {
                Result.Status = ResultStatus.Completed;
                Client.TcpClient.DataReceive = null;
                Client.TcpClient.ClientError = null;
                Result.ResultType = type;
                Result.Messge = message;
                Completed?.Invoke(this);
                mReceiveTrack?.Dispose();
                mReceiveTrack = null;
                // TaskCompletion();
                if (Command.GetType() == typeof(SELECT) || Command.GetType() == typeof(AUTH))
                {
                    Task.Run(() => TaskCompletion());
                }
                else
                {
                    if (ResultDispatch.UseDispatch)
                        ResultDispatch.DispatchCenter.Enqueue(this, 3);
                    else
                        TaskCompletion();
                }


            }

        }

        internal void TaskCompletion()
        {
            TaskCompletionSource.TrySetResult(Result);
        }
    }

    public class SubscribeRequest : RedisRequest
    {
        public SubscribeRequest(RedisHost host, RedisClient client, Command cmd, params Type[] types)
            : base(host, client, cmd, types)
        {

        }

        public Task<Result> Read()
        {
            TaskCompletionSource = new TaskCompletionSource<Result>();
            Result = new Result();
            return TaskCompletionSource.Task;
        }

        public override void OnCompleted(ResultType type, string message)
        {
            Result.Status = ResultStatus.Completed;
            Result.ResultType = type;
            Result.Messge = message;
            TaskCompletionSource.TrySetResult(Result);
        }
    }

}
