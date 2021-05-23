using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SUNION : Command
    {

        public SETS_SUNION(string[] keys)
        {
            Keys = keys;
        }

        public override bool Read => false;

        public override string Name => "SUNION";

        public string[] Keys { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            foreach (var item in Keys)
                OnWriteKey(item);
        }
    }
}
