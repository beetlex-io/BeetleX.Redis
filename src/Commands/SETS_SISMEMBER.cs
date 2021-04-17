using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SISMEMBER : Command
    {
        public SETS_SISMEMBER(string key,string member)
        {
            Key = key;
            Member = member;
        }
        public override bool Read => true;

        public override string Name => "SISMEMBER";

        public string Key { get; set; }

        public string Member { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Member);
        }
    }
}
