using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HMGET : Command
    {
        public HMGET(string key, IDataFormater dataFormater, params string[] fields)
        {
            Key = key;
            DataFormater = dataFormater;
            Fields = fields;
        }

        public string Key { get; set; }

        public string[] Fields { get; set; }

        public override bool Read => true;

        public override string Name => "HMGET";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            for (int i = 0; i < Fields.Length; i++)
            {
                AddText(Fields[i]);
            }
        }
    }
}
