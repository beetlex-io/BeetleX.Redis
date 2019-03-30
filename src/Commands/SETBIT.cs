using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETBIT : Command
    {
        public SETBIT(string key, int offset, bool value)
        {
            Key = key;
            Offset = offset;
            Value = value;
        }

        public string Key { get; set; }

        public int Offset { get; set; }

        public bool Value { get; set; }

        public override bool Read => false;

        public override string Name => "SETBIT";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Offset);
            AddText(Value ? 1 : 0);
        }
    }
}
