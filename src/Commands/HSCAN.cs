using BeetleX.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HSCAN : Command
    {


        public HSCAN(string key)
        {
            Key = key;
            this.NetworkReceive = OnReceive;
        }

        public string Key { get; set; }
        public override bool Read => true;
        public override string Name => "HSCAN";

        public int Cursor { get; set; }

        public string Pattern { get; set; }

        public int Count { get; set; } = 10;

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Cursor);
            if (!string.IsNullOrEmpty(Pattern))
            {
                AddText("MATCH");
                AddText(Pattern);
            }
            AddText("COUNT");
            AddText(Count.ToString());
        }
        private Result mScanResult = new Result();
        private Redis.Result OnReceive(RedisRequest request, PipeStream stream)
        {
            if (mScanResult.Read(stream, this))
            {
                Redis.Result result = new Redis.Result();
                result.ResultType = ResultType.Object;
                result.Data.Add(new ResultItem { Type = ResultType.Object, Data = mScanResult });
                return result;
            }
            else
            {

                return null;
            }
        }
        public class KeyValue
        {
            public string Key { get; set; }

            public byte[] Value { get; set; }

            internal BeetleX.Redis.IDataFormater FormatProvider { get; set; }
            public T ToObject<T>()
            {
                return (T)FormatProvider.DeserializeObject(typeof(T), Value);
            }
            public override string ToString()
            {
                if (Value.Length > 0)
                    return System.Text.Encoding.UTF8.GetString(Value, 0, Value.Length);
                else
                    return "";
            }
        }
        public class Result
        {
            public int NextCursor { get; set; }

            public List<KeyValue> Values { get; set; } = new List<KeyValue>();

            private int? mCount;

            private int mIndex = 0;

            private int? mItemLength;

            private KeyValue mKeyValue = new KeyValue();
            internal bool Read(PipeStream stream, Command cmd)
            {
                if (mCount == null)
                {
                    var status = stream.ReadLine();
                    if (status[0] == '-')
                        throw new RedisException(status.Substring(1));
                    stream.ReadLine();
                    var cursor = stream.ReadLine();
                    NextCursor = int.Parse(cursor);
                    var countline = stream.ReadLine();
                    mCount = int.Parse(countline.Substring(1));
                }
                while (stream.TryReadLine(out string line))
                {
                    if (mItemLength == null)
                    {
                        mItemLength = int.Parse(line.Substring(1));
                        mIndex++;
                    }
                    if (stream.Length >= mItemLength + 2)
                    {
                        if (mIndex % 2 == 0)
                        {
                            mKeyValue.Value = new byte[mItemLength.Value];
                            stream.Read(mKeyValue.Value, 0, mItemLength.Value);
                            mKeyValue.FormatProvider = cmd.DataFormater;
                            Values.Add(mKeyValue);
                            mKeyValue = new KeyValue();
                        }
                        else
                        {
                            mKeyValue.Key = stream.ReadString(mItemLength.Value);

                        }
                        stream.ReadLine();
                        mItemLength = null;
                    }
                }
                return mCount != null && Values.Count == mCount.Value / 2;
            }
        }
    }
}
