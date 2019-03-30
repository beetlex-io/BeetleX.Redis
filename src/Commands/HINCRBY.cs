using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HINCRBY : Command
    {
        public HINCRBY(string key, string field, int value)
        {
            Key = key;
            Field = field;
            Value = value;
        }

        public string Key { get; set; }

        public string Field { get; set; }

        public int Value { get; set; }

        public override bool Read => false;

        public override string Name => "HINCRBY";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Field);
            AddText(Value);
        }
    }
}
