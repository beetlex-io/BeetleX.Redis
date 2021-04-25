using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZREVRANK : Command
    {
        public ZREVRANK(string key, string member)
        {
            Key = key;
            Member = member;
        }

        public string Key { get; private set; }

        public string Member { get; private set; }

        public override bool Read => true;

        public override string Name => "ZREVRANK";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Member);
        }
    }
}
