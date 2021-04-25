using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class DECRBY : Command
    {
        public DECRBY(string key, int decrement)
        {
            Key = key;
            Decrement = decrement;
        }

        public string Key { get; set; }

        public int Decrement { get; set; }

        public override bool Read => false;

        public override string Name => "DECRBY";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Decrement);
        }
    }
}
