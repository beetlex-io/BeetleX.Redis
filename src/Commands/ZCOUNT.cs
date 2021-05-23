using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZCOUNT : Command
    {

        public ZCOUNT(string key,double min,double max)
        {
            Key = key;
            mMin = min;
            mMax = max;
        }

        private double mMin, mMax;

        public string Key { get; private set; }

        public override bool Read => true;

        public override string Name => "ZCOUNT";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(mMin);
            AddText(mMax);
        }
    }
}
