using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class PEXPIRE : Command
    {
        public PEXPIRE(string key, long milliseconds)
        {
            Key = key;
            Milliseconds = milliseconds;
        }

        public string Key { get; set; }

        public long Milliseconds { get; set; }

        public override bool Read => false;

        public override string Name => "PEXPIRE";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Milliseconds);
        }
    }
}
