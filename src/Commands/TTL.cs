using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class TTL : Command
    {
        public TTL(string key)
        {
            Key = key;
        }

        public string Key { get; set; }

        public override bool Read => false;

        public override string Name => "TTL";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
        }
    }
}
