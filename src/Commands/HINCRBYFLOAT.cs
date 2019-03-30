using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HINCRBYFLOAT : Command
    {
        public HINCRBYFLOAT(string key, string field, float value)
        {
            Key = key;
            Field = field;
            Value = value;
        }

        public string Key { get; set; }

        public string Field { get; set; }

        public float Value { get; set; }

        public override bool Read => false;

        public override string Name => "HINCRBYFLOAT";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Field);
            AddText(Value);
        }
    }
}
