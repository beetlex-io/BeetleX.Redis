using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class GET : Command
    {
        public GET(string key, IDataFormater formater)
        {
            Key = key;
            DataFormater = formater;
        }

        public string Key { get; set; }

        public override bool Read => true;

        public override string Name => "GET";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
        }
    }
}
