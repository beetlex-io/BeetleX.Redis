using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class RENAME : Command
    {
        public RENAME(string key, string newkey)
        {
            Key = key;
            NewKey = newkey;
        }

        public string Key { get; set; }

        public string NewKey { get; set; }

        public override bool Read => false;

        public override string Name => "RENAME";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(NewKey);
        }
    }
}
