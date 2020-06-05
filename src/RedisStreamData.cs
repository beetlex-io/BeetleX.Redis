using BeetleX.Buffers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.Redis
{
    class StreamDataReceive<T>
    {


        private Result mResult = new Result();

        private StreamDataReader<T> mReader = new StreamDataReader<T>();

        public Result Receive(RedisRequest request, PipeStream stream)
        {
            if (mReader.Read(request, stream))
            {
                foreach (var s in mReader.Items)
                {
                    foreach (var item in s.Items)
                    {
                        item.StreamName = s.Name;
                        mResult.Data.Add(new ResultItem { Type = ResultType.Object, Data = item });
                    }
                }

                return mResult;
            }
            return null;
        }
    }

    class StreamDataItemReceive<T>
    {

        public StreamDataItemReceive()
        {
            mResult.ResultType = ResultType.Arrays;
        }

        private Result mResult = new Result();

        private StreamDataItemReader<T> mReader = new StreamDataItemReader<T>();

        public Result Receive(RedisRequest request, PipeStream stream)
        {
            if (mReader.Read(request, stream))
            {
                foreach (var item in mReader.Items)
                {
                    mResult.Data.Add(new ResultItem { Type = ResultType.Object, Data = item });
                }
                return mResult;
            }
            else
            {
                return null;
            }
        }

    }

    class StreamDataReader<T>
    {
        public List<StreamData<T>> Items { get; set; } = new List<StreamData<T>>();

        private int? mCount = null;

        private string mName;

        private StreamDataItemReader<T> mItem;

        private ReadStatus mStatus = ReadStatus.None;

        public bool Read(RedisRequest request, PipeStream stream)
        {
            string line;
            if (mCount == null)
            {
                if (stream.TryReadLine(out line))
                {
                    if (line[0] == '-')
                    {
                        throw new RedisException(line.Substring(1));
                    }
                    mCount = int.Parse(line.Substring(1));
                    if (mCount <= 0)
                        return true;
                }
            }
        ReRead:
            if (mStatus == ReadStatus.None)
            {
                if (stream.TryReadLine(out line))
                    mStatus = ReadStatus.ReadStream;
            }
            if (mStatus == ReadStatus.ReadStream)
            {
                if (stream.TryReadLine(out line))
                {
                    mName = line;
                    mStatus = ReadStatus.ReadNameLength;
                }
            }
            if (mStatus == ReadStatus.ReadNameLength)
            {
                if (stream.TryReadLine(out line))
                {
                    mName = line;
                    mStatus = ReadStatus.ReadName;
                }
            }
            if (mStatus == ReadStatus.ReadName)
            {
                if (mItem == null)
                    mItem = new StreamDataItemReader<T>();
                if (mItem.Read(request, stream))
                {
                    StreamData<T> item = new StreamData<T>();
                    item.Name = mName;
                    item.Items = mItem.Items;
                    Items.Add(item);
                    mItem = null;
                    mName = null;
                    mStatus = ReadStatus.None;
                    if (mCount != null && Items.Count >= mCount.Value)
                        return true;
                    goto ReRead;
                }
            }
            return false;
        }

        public enum ReadStatus
        {
            None,
            ReadStream,
            ReadNameLength,
            ReadName,
        }

    }

    class StreamDataItemReader<T>
    {

        public StreamDataItemReader()
        {

        }

        public List<StreamDataItem<T>> Items { get; set; } = new List<StreamDataItem<T>>();

        private int? mCount = null;

        private string mID = null;

        private int? mValueLength = 0;

        private int? mFields = null;

        private string mFieldName = null;

        private Dictionary<string, string> mProperty;

        private ItemReadStaus mReadStatus = ItemReadStaus.None;

        public bool Read(RedisRequest request, PipeStream stream)
        {
            string line;
            if (mCount == null)
            {
                if (stream.TryReadLine(out line))
                {
                    if (line[0] == '-')
                    {
                        throw new RedisException(line.Substring(1));
                    }
                    mCount = int.Parse(line.Substring(1));
                    if (mCount <= 0)
                        return true;
                }
            }
        ReRead:
            if (mReadStatus == ItemReadStaus.None)
            {
                if (stream.TryReadLine(out line))
                {
                    mReadStatus = ItemReadStaus.ReadMessage;
                }
            }
            if (mReadStatus == ItemReadStaus.ReadMessage)
            {
                if (stream.TryReadLine(out line))
                {
                    mValueLength = int.Parse(line.Substring(1));
                    mReadStatus = ItemReadStaus.ReadIdLength;
                }
            }
            if (mReadStatus == ItemReadStaus.ReadIdLength)
            {
                if (stream.TryReadLine(out line))
                {
                    mID = line;
                    mReadStatus = ItemReadStaus.ReadIdValue;
                }
            }
            if (mReadStatus == ItemReadStaus.ReadIdValue)
            {
                if (stream.TryReadLine(out line))
                {
                    if (line == "*-1")
                    {
                        StreamDataItem<T> item = new StreamDataItem<T>();
                        item.ID = mID;
                        item.Data = default(T);
                        Items.Add(item);
                        if (mCount != null && Items.Count >= mCount.Value)
                            return true;
                        mID = null;
                        mValueLength = null;
                        mFields = null;
                        mReadStatus = ItemReadStaus.None;
                        goto ReRead;
                    }
                    else
                    {
                        mFields = int.Parse(line.Substring(1))/2;
                    }
                    mReadStatus = ItemReadStaus.ReadBody;
                }
            }
        ReReadProperty:
            if (mReadStatus == ItemReadStaus.ReadBody)
            {
                if (stream.TryReadLine(out line))
                {
                    mValueLength = int.Parse(line.Substring(1));
                    mReadStatus = ItemReadStaus.ReadFieldLength;
                }
            }
            if (mReadStatus == ItemReadStaus.ReadFieldLength)
            {
                if (stream.TryReadLine(out line))
                {
                    mFieldName = line;
                    mReadStatus = ItemReadStaus.ReadField;
                }
            }
            if (mReadStatus == ItemReadStaus.ReadField)
            {
                if (stream.TryReadLine(out line))
                {
                    mValueLength = int.Parse(line.Substring(1));
                    mReadStatus = ItemReadStaus.ReadValueLength;
                }
            }
            if (mReadStatus == ItemReadStaus.ReadValueLength)
            {
                if (stream.Length >= (mValueLength.Value + 2))
                {
                    if (typeof(T) == typeof(Dictionary<string, string>))
                    {
                        if (mProperty == null)
                        {
                            mProperty = new Dictionary<string, string>();
                        }
                        mProperty[mFieldName] = stream.ReadLine();
                        if (mProperty.Count >= mFields)
                        {
                            StreamDataItem<T> item = new StreamDataItem<T>();
                            item.ID = mID;
                            item.Data = (T)(object)mProperty;
                            Items.Add(item);
                            mProperty = null;
                            mFieldName = null;
                            mFields = null;
                        }
                        else
                        {
                            mReadStatus = ItemReadStaus.ReadBody;
                            goto ReReadProperty;
                        }
                    }
                    else
                    {
                        var data = request.Command.DataFormater.DeserializeObject(typeof(T), request.Client, stream, mValueLength.Value);
                        stream.ReadFree(2);
                        StreamDataItem<T> item = new StreamDataItem<T>();
                        item.ID = mID;
                        item.Data = (T)data;
                        Items.Add(item);
                    }
                    if (mCount != null && Items.Count >= mCount.Value)
                        return true;
                    mID = null;
                    mValueLength = null;
                    mFields = null;
                    mReadStatus = ItemReadStaus.None;
                    goto ReRead;
                }
            }
            return false;
        }

        public enum ItemReadStaus
        {
            None,
            ReadMessage,
            ReadIdLength,
            ReadIdValue,
            ReadBody,
            ReadFieldLength,
            ReadField,
            ReadValueLength,
            ReadValue
        }
    }

    public class StreamData<T>
    {
        public string Name { get; set; }

        public List<StreamDataItem<T>> Items { get; set; } = new List<StreamDataItem<T>>();
    }

    public class StreamDataItem<T>
    {
        public string ID { get; internal set; }

        public T Data { get; internal set; }

        public string StreamName { get; set; }

        internal RedisStream<T> Stream { get; set; }

        internal RedisStreamGroup<T> Group { get; set; }

        public async ValueTask Delete()
        {
            if (Group != null)
            {
                await Group.Stream.Del(ID);
                return;
            }
            if (Stream != null)
                await Stream.Del(ID);
        }

        public async ValueTask Ack()
        {
            if (Group != null)
                await Group.Ack(ID);
        }
    }

}
