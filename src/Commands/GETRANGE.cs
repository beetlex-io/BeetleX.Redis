using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class GETRANGE : Command
    {
        public GETRANGE(string key, int start, int end)
        {
            Key = key;
            Start = start;
            End = end;
        }

        public string Key { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public override bool Read => true;

        public override string Name => "GETRANGE";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Start);
            AddText(End);
        }
    }
}
