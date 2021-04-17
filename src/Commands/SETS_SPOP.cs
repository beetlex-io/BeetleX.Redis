using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SPOP : Command
    {
        public SETS_SPOP(string key)
        {
            Key = key;
        }

        public override bool Read => false;

        public override string Name => "SPOP";

        public string Key { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
        }
    }
}
