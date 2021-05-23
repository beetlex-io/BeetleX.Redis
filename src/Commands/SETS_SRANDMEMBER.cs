using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SRANDMEMBER : Command
    {
        public SETS_SRANDMEMBER(string key, int count = 1)
        {
            Key = key;
            Count = count;
        }

        public override bool Read => true;

        public override string Name => "SRANDMEMBER";

        public string Key { get; set; }

        public int Count { get; set; } = 1;

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Count);
        }
    }
}
