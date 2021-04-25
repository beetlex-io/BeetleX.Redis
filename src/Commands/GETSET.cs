﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class GETSET : Command
    {
        public GETSET(string key, object value, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public object Value { get; set; }

        public override bool Read => false;

        public override string Name => "GETSET";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddData(Value);
        }
    }
}
