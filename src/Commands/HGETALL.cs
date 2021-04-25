using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HGETALL : Command
    {

        public HGETALL(string key, IDataFormater dataFormater)
        {
            this.DataFormater = dataFormater;
            Key = key;
        }

        public string Key { get; set; }

        public override bool Read => true;

        public override string Name => "HGETALL";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
        }
    }
}
