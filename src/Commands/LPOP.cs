using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class LPOP : Command
    {
        public LPOP(string key, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
        }

        public string Key { get; set; }

        public override bool Read => false;

        public override string Name => "LPOP";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
        }
    }
}
