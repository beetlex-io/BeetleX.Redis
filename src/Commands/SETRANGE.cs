using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETRANGE : Command
    {
        public SETRANGE(string key, int offset, string value)
        {
            Key = key;
            Offset = offset;
            Value = value;
        }

        public string Key { get; set; }

        public int Offset { get; set; }

        public string Value { get; set; }

        public override bool Read => false;

        public override string Name => "SETRANGE";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Offset);
            AddText(Value);
        }
    }
}
