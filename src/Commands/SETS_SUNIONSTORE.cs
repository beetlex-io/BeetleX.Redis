using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SUNIONSTORE : Command
    {
        public SETS_SUNIONSTORE(string dest, string[] keys)
        {
            Destination = dest;
            Keys = keys;
        }

        public override bool Read => false;

        public override string Name => "SUNIONSTORE";

        public string Destination { get; set; }

        public string[] Keys { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Destination);
            foreach (var item in Keys)
                AddText(item);
        }
    }
}
