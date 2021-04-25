using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class KEYS : Command
    {

        public KEYS(string pattern)
        {
            Pattern = pattern;
        }

        public String Pattern { get; set; }


        public override bool Read => true;

        public override string Name => "KEYS";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Pattern);
        }
    }
}
