using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SINTERSTORE : Command
    {
        public SETS_SINTERSTORE(string  dest,string[] keys)
        {
            Destination = dest;
            Keys = keys;
        }

        public override bool Read => false;

        public string Destination { get; set; }

        public string[] Keys { get; set; }

        public override string Name => "SINTERSTORE";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Destination);
            foreach (var item in Keys)
                OnWriteKey(item);
        }
    }
}
