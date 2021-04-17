using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SMEMBERS : Command
    {

        public SETS_SMEMBERS(string key)
        {
            Key = key;
        }

        public override bool Read => true;

        public override string Name => "SMEMBERS";

        public string Key { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
        }
    }
}
