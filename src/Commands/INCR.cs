using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class INCR : Command
    {

        public INCR(string key)
        {
            Key = key;
        }

        public override bool Read => false;

        public override string Name => "INCR";

        public string Key { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
        }
    }
}
