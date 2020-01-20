using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZREVRANGEBYSCORE:Command
    {
        public ZREVRANGEBYSCORE(string key, string max, string min, bool withscores = false)
        {

            Key = key;
            Min = min;
            Max = max;
            Withscores = withscores;
        }

        public string Key { get; private set; }

        public override bool Read => true;

        public bool Withscores { get; private set; }

        public string Min { get; private set; }

        public string Max { get; private set; }

        public override string Name => "ZREVRANGEBYSCORE";

        public int? Offset { get; set; }

        public int? Count { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            if (!string.IsNullOrEmpty(Max))
                AddText(Max);
            if (!string.IsNullOrEmpty(Min))
                AddText(Min);
            if (Withscores)
                AddText("WITHSCORES");
            if (Offset != null && Count != null)
            {
                AddText("LIMIT").AddText(Offset.Value).AddText(Count.Value);
            }
        }
    }
}
