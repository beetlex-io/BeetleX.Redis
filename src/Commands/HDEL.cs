using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HDEL : Command
    {
        public HDEL(string key, string[] fields)
        {
            Key = key;
            Fields = fields;
        }

        public string Key { get; set; }

        public string[] Fields { get; set; }

        public override bool Read => false;

        public override string Name => "HDEL";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            for (int i = 0; i < Fields.Length; i++)
                AddText(Fields[i]);
        }
    }
}
