using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class EXPIREAT : Command
    {
        public EXPIREAT(string key, long timestamp)
        {
            Key = key;
            Timestamp = timestamp;
        }

        public string Key { get; set; }

        public long Timestamp { get; set; }


        public override bool Read => false;

        public override string Name => "EXPIREAT";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Timestamp);
        }
    }
}
