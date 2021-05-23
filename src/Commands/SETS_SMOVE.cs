using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SMOVE : Command
    {

        public SETS_SMOVE(string source, string dest, string member)
        {
            Source = source;
            Destination = dest;
            Member = member;
        }

        public string Source { get; set; }

        public string Destination { get; set; }

        public string Member { get; set; }

        public override bool Read => false;

        public override string Name => "SMOVE";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Source);
            OnWriteKey(Destination);
            AddText(Member);
        }
    }
}
