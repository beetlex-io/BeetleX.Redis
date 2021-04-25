using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class LINSERT : Command
    {
        public LINSERT(string key, object value, bool before, object addValue, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
            Value = value;
            Before = before;
            AddValue = addValue;
        }

        public string Key { get; set; }

        public object Value { get; set; }

        public bool Before { get; set; }

        public object AddValue { get; set; }

        public override bool Read => false;

        public override string Name => "LINSERT";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            if (Before)
                AddText("BEFORE");
            else
                AddText("AFTER");
            AddData(Value);
            AddData(AddValue);

        }


    }
}
