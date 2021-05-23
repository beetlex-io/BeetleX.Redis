using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SDIFFSTORE : Command
    {

        public SETS_SDIFFSTORE(string dest,params string[] keys)
        {
            this.Destination = dest;
            Keys = keys;
        }

        public override bool Read => false;

        public override string Name => "SDIFFSTORE";

        public string[] Keys { get; set; }

        public string Destination { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Destination);
            foreach (var item in Keys)
                OnWriteKey(item);
        }
    }
}
