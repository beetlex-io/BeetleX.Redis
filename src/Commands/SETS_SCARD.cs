using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SCARD : Command
    {
        public SETS_SCARD(string key)
        {
            Key = key;
        }

        public override bool Read => true;

        public string Key { get; set; }

        public override string Name => "SCARD";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
        }
    }
}
