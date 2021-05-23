using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class DUMP : Command
    {
        public DUMP(string key)
        {
            Key = key;
        }

        public string Key { get; set; }

        public override bool Read => true;

        public override string Name => "DUMP";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
        }
    }
}
