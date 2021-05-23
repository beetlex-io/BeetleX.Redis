using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZCARD : Command
    {
        public ZCARD(string key)
        {
            Key = key;
        }

        public string Key { get; private set; }

        public override bool Read => true;

        public override string Name => "ZCARD";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
        }
    }
}
