using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HLEN : Command
    {
        public HLEN(string key)
        {
            Key = key;
        }

        public string Key { get; set; }

        public override bool Read => true;

        public override string Name => "HLEN";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
        }
    }
}
