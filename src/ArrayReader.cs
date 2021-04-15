using BeetleX.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis
{
    public class ArrayReader
    {
        public ArrayReader(Command cmd, int count, Func<RedisClient, Command, PipeStream, int, int, object> readBody, int level = 0)
        {
            mCommand = cmd;
            mCount = count;
            ReadBody = readBody;
            mLevel = level;

        }
        private Command mCommand;

        private int mCount;

        private int mLevel = 0;

        private List<ArrayDataItem> mItems = new List<ArrayDataItem>();

        public List<ArrayDataItem> Items => mItems;

        private int mContentLength;

        private Status mStatus = Status.ReadContentLength;

        private ArrayReader mSubReader;

        public Func<RedisClient, Command, PipeStream, int, int, object> ReadBody { get; private set; }

        public Status Read(PipeStream stream, RedisClient client)
        {
            while (true)
            {
                if (mStatus == Status.Completed)
                {
                    if (!stream.TryReadLine(out string line))
                        break;
                    else
                        mStatus = Status.ReadContentLength;
                }
                if (Items.Count >= mCount)
                    return Status.End;
                if (mSubReader != null)
                {
                    if (mSubReader.Read(stream, client) == Status.End)
                    {
                        mItems.Add(new ArrayDataItem { IsArray = true, Level = mLevel, Value = mSubReader.Items });
                        mSubReader = null;
                        mStatus = Status.ReadContentLength;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (mStatus == Status.ReadContentLength)
                    {
                        if (stream.TryReadLine(out string line))
                        {
                            var length = int.Parse(line.Substring(1));
                            if (line[0] == '*')
                            {
                                if (length == -1)
                                {
                                    mItems.Add(new ArrayDataItem { Value = new List<ArrayDataItem>(0), IsArray = false, Level = mLevel });
                                }
                                else
                                {
                                    mSubReader = new ArrayReader(this.mCommand, length, ReadBody, mLevel++);
                                }
                                continue;
                            }
                            mContentLength = length;
                            mStatus = Status.ReadContent;
                        }
                        else
                            break;
                    }
                    if (mStatus == Status.ReadContent)
                    {
                        if (mContentLength == -1)
                        {
                            mItems.Add(new ArrayDataItem { Value = null, IsArray = false, Level = mLevel }); ;
                            mStatus = Status.ReadContentLength;
                            continue;
                        }
                        if (stream.Length >= mContentLength)
                        {
                            var item = ReadBody(client, this.mCommand, stream, mContentLength, mLevel);
                            mItems.Add(new ArrayDataItem { Value = item, IsArray = false, Level = mLevel });
                            mStatus = Status.Completed;
                            continue;
                        }
                        else
                            break;
                    }
                }
            }
            return mStatus;
        }
        public enum Status
        {
            None,
            ReadContentLength,
            ReadContent,
            Completed,
            End
        }
    }

    public class ArrayDataItem
    {

        public object Value { get; set; }

        public int Level { get; set; }

        public bool IsArray { get; set; } = false;
    }
}
