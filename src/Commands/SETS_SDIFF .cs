using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SDIFF : Command
    {
        public SETS_SDIFF(string[] keys)
        {
            Keys = keys;
        }

        public override bool Read => true;

        public string[] Keys { get; set; }

        public override string Name => "SDIFF";

        public override void OnExecute()
        {
            base.OnExecute();
            foreach (var item in Keys)
                AddText(item);
        }
    }
}
