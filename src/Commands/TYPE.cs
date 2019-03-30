using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class TYPE : Command
    {
        public TYPE(string key)
        {
            Key = key;
        }

        public string Key { get; set; }

        public override bool Read => true;

        public override string Name => "TYPE";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
        }
    }
}
