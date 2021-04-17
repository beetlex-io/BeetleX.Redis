using BeetleX.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SCAN : Command
    {
        public SCAN()
        {
            this.NetworkReceive = OnReceive;
        }

        public String Pattern { get; set; }

        public int Cursor { get; set; } = 0;

        public int Count { get; set; } = 20;

        public override bool Read => true;

        public override string Name => "SCAN";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Cursor);
            if (!string.IsNullOrEmpty(Pattern))
            {
                AddText("MATCH");
                AddText(Pattern);
            }
            AddText("COUNT");
            AddText(Count);
        }

        private ScanResult mScanResult = new ScanResult();

        private Result OnReceive(RedisRequest request, PipeStream stream)
        {
            if (mScanResult.Read(stream))
            {
                Result result = new Result();
                result.ResultType = ResultType.Object;
                result.Data.Add(new ResultItem { Type = ResultType.Object, Data = mScanResult });
                return result;
            }
            else
            {

                return null;
            }
        }
    }

    public class ScanResult
    {
        public int NextCursor { get; set; }

        public List<string> Keys { get; set; } = new List<string>();

        private int? mCount;

        private int? mItemLength;

        public bool Read(PipeStream stream)
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
                }
                else
                {
                    Keys.Add(line);
                    mItemLength = null;
                }
            }
            return mCount != null && Keys.Count == mCount.Value;
        }
    }

}
