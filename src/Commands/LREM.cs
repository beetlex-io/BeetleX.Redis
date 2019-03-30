using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class LREM : Command
    {
        public LREM(string key, int count, object value, IDataFormater dataFormater = null)
        {
            this.DataFormater = dataFormater;
            Key = key;
            Count = count;
            Value = value;
        }

        public string Key { get; set; }

        public int Count { get; set; }

        public object Value { get; set; }

        public override bool Read => false;

        public override string Name => "LREM";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Count);
            AddData(Value);
        }
    }
}
