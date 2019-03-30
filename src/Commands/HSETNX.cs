using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HSETNX : Command
    {
        public HSETNX(string key, string field, object value, IDataFormater dataFormater)
        {
            Key = key;
            Field = field;
            Value = value;
            DataFormater = dataFormater;
        }

        public string Key { get; set; }

        public string Field { get; set; }

        public object Value { get; set; }

        public override bool Read => false;

        public override string Name => "HSETNX";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Field);
            AddData(Value);
        }
    }
}
