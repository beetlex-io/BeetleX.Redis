using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SADD : Command
    {

        public SETS_SADD(string key, params string[] members)
        {
            Key = key;
            Members = members;
        }

        public string Key { get; set; }

        public string[] Members { get; set; }

        public override bool Read => false;

        public override string Name => "SADD";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            foreach (var item in Members)
                AddText(item);
        }
    }
}
