using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZREMRANGEBYSCORE : Command
    {
        public ZREMRANGEBYSCORE(string key,double min,double max)
        {
            Key = key;
            Min = min;
            Max = max;
        }

        public double Min { get; private set; }

        public double Max { get; private set; }

        public string Key { get; private set; }

        public override bool Read => false;

        public override string Name => "ZREMRANGEBYSCORE";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key).AddText(Min).AddText(Max);
        }
    }
}
