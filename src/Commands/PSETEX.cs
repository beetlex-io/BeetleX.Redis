using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class PSETEX : Command
    {
        public PSETEX(string key, long milliseconds, object value, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
            Milliseconds = milliseconds;
            Value = value;
        }

        public string Key { get; set; }

        public object Value { get; set; }

        public long Milliseconds { get; set; }

        public override bool Read => false;

        public override string Name => "PSETEX";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Milliseconds);
            AddData(Value);
        }


    }
}
