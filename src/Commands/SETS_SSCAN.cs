using BeetleX.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    class SETS_SSCAN:Command
    {
        public SETS_SSCAN()
        {
            this.NetworkReceive = OnReceive;
        }

        public string Key { get; set; }

        public String Pattern { get; set; }

        public int Cursor { get; set; } = 0;

        public int Count { get; set; } = 20;

        public override bool Read => true;

        public override string Name => "SSCAN";

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
            AddText(Count);
        }

        private ScanResult mScanResult = new ScanResult();

        private Result OnReceive(RedisRequest request, PipeStream stream)
        {
            if (mScanResult.Read(stream,this))
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
}
