using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class INCRBY : Command
    {
        public INCRBY(string key, int value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public int Value { get; set; }

        public override bool Read => false;

        public override string Name => "INCRBY";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Value.ToString());
        }
    }
}
