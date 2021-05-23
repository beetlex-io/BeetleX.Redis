using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class XREVRANGE : Command
    {

        public XREVRANGE(string stream, string start, string stop)
        {
            mStartID = string.IsNullOrEmpty(start) ? "+" : start;
            mEndID = string.IsNullOrEmpty(stop) ? "-" : stop;
            mStream = stream;
        }

        public int? Count { get; set; }

        private string mStream;

        private string mStartID;

        private string mEndID;

        public override bool Read => true;

        public override string Name => "XREVRANGE";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(mStream);
            AddText(mStartID);
            AddText(mEndID);
            if (Count != null)
            {
                AddText("COUNT");
                AddText(Count.Value);
            }

        }
    }
}
