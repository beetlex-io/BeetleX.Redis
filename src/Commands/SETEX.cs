using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETEX : Command
    {
        public SETEX(string key, int seconds, object value, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
            Seconds = seconds;
            Value = value;
        }

        public string Key { get; set; }

        public int Seconds { get; set; }

        public object Value { get; set; }

        public override bool Read => false;

        public override string Name => "SETEX";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Seconds);
            AddData(Value);
        }
    }
}
