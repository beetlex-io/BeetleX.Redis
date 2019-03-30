using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class LTRIM : Command
    {
        public LTRIM(string key, int start, int stop)
        {
            Key = key;
            Start = start;
            Stop = stop;
        }

        public string Key { get; set; }

        public int Start { get; set; }

        public int Stop { get; set; }

        public override bool Read => false;

        public override string Name => "LTRIM";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Start);
            AddText(Stop);
        }
    }
}
