using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETNX : Command
    {
        public SETNX(string key, object value, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public object Value { get; set; }

        public override bool Read => false;

        public override string Name => "SETNX";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddData(Value);
        }
    }
}
