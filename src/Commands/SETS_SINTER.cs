using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SINTER : Command
    {
        
        public SETS_SINTER(string[] keys)
        {
            Keys = keys;
        }

        public string[] Keys { get; set; }

        public override bool Read => false;

        public override string Name => "SINTER";

        public override void OnExecute()
        {
            base.OnExecute();
            foreach (var item in Keys)
                AddText(item);
        }
    }
}
