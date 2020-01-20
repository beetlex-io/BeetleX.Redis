using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZREMRANGEBYRANK : Command
    {

        public ZREMRANGEBYRANK(string key,int start,int stop)
        {
            Key = key;
            Start = start;
            Stop = stop;
        }

        public string Key { get; private set; }

        public int Start { get; private set; }

        public int Stop { get; private set; }

        public override bool Read => false;

        public override string Name => "ZREMRANGEBYRANK";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key).AddText(Start).AddText(Stop);
        }
    }
}
