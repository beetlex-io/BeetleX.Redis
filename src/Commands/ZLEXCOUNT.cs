using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZLEXCOUNT:Command
    {
        public ZLEXCOUNT(string key, string min, string max, bool negative)
        {
            Negative = negative;
            Key = key;
            Min = min;
            Max = max;
        }

        public string Key { get; private set; }

        public string Min { get; private set; }

        public string Max { get; private set; }

        public bool Negative { get; private set; }

        public override bool Read => true;

        public override string Name => "ZLEXCOUNT";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            if (Negative)
                AddText("-");
            else
                AddText("+");
            AddText(Min);
            if (!string.IsNullOrEmpty(Max))
                AddText(Max);
        }
    }
}
