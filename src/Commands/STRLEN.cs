using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class STRLEN : Command
    {
        public STRLEN(string key)
        {
            Key = key;
        }
        public string Key { get; set; }

        public override bool Read => true;

        public override string Name => "STRLEN";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
        }
    }
}
