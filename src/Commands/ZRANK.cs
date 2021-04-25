using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZRANK : Command
    {

        public ZRANK(string key, string member)
        {
            Key = key;
            Member = member;
        }

        public string Key { get; private set; }

        public string Member { get; private set; }

        public override bool Read => true;

        public override string Name => "ZRANK";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Member);
        }
    }
}
