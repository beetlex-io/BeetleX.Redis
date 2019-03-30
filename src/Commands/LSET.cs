using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class LSET : Command
    {
        public LSET(string key, int index, object value, IDataFormater dataFormater)
        {
            Key = key;
            Index = index;
            Value = value;
            DataFormater = dataFormater;
        }

        public string Key { get; set; }

        public int Index { get; set; }

        public object Value { get; set; }

        public override bool Read => false;

        public override string Name => "LSET";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Index);
            AddData(Value);
        }
    }
}
