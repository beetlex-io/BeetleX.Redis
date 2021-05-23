using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class RPOP : Command
    {
        public RPOP(string key, IDataFormater dataFormater = null)
        {
            DataFormater = dataFormater;
            Key = key;
        }

        public string Key { get; set; }

        public override bool Read => false;

        public override string Name => "RPOP";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
        }
    }
}
