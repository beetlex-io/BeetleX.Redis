using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZRANGE : Command
    {

        public ZRANGE(string key,int start,int stop,bool withscores)
        {
            Key = key;
            mStart = start;
            mStop = stop;
            Withscores = withscores;
        }

        public string Key { get; private set; }

        private int mStart;

        private int mStop;

        public bool Withscores { get; private set; }

        public override bool Read =>true;

        public override string Name => "ZRANGE";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(mStart);
            AddText(mStop);
            if (Withscores)
                AddText("WITHSCORES");
        }
    }
}
