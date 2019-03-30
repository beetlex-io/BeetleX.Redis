using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class DECR : Command
    {
        public DECR(string key)
        {
            Key = key;
        }

        public string Key { get; set; }

        public override bool Read => false;

        public override string Name => "DECR";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
        }
    }
}
