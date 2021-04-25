using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HEXISTS : Command
    {
        public HEXISTS(string key, string field)
        {
            Key = key;
            Field = field;
        }

        public string Key { get; set; }

        public string Field { get; set; }

        public override bool Read => true;

        public override string Name => "HEXISTS";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Field);
        }
    }
}
