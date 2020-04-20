using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class PFCount : Command
    {
        public PFCount(params string[] keys)
        {
            Keys = keys;
        }

        public override bool Read => true;

        public override string Name => "PFCOUNT";

        public string[] Keys { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            foreach(var item in Keys)
            {
                AddText(item);
            }
        }
    }
}
