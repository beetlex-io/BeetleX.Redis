using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZREMRANGEBYLEX : Command
    {
        public override bool Read => false;

        public override string Name => "ZREMRANGEBYLEX";

        public ZREMRANGEBYLEX(string key, string min, string max)
        {
            Key = key;
            Min = min;
            Max = max;
        }

        public string Key { get; private set; }

        public string Min { get; private set; }

        public string Max { get; private set; }

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Min);
            if (!string.IsNullOrEmpty(Max))
                AddText(Max);
        }
    }
}
