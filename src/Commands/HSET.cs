using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HSET : Command
    {
        public HSET(string key, string field, object value, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
            Field = field;
            Value = value;
        }

        public string Key { get; set; }

        public string Field { get; set; }

        public object Value { get; set; }

        public override bool Read => false;

        public override string Name => "HSET";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Field);
            if (Value == null)
                Value = string.Empty;
            AddData(Value);
        }

    }
}
