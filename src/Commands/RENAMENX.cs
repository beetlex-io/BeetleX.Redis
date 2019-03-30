using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class RENAMENX : Command
    {

        public RENAMENX(string key, string newkey)
        {
            Key = key;
            NewKey = newkey;
        }

        public string Key { get; set; }

        public string NewKey { get; set; }

        public override bool Read => false;

        public override string Name => "RENAMENX";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(NewKey);
        }
    }
}
