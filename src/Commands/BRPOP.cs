using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class BRPOP : Command
    {
        public BRPOP(string[] key, int timeout, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
            Timeout = timeout;
        }

        public string[] Key { get; set; }

        public int Timeout { get; set; }

        public override bool Read => false;

        public override string Name => "BRPOP";

        public override void OnExecute()
        {
            base.OnExecute();
            for (int i = 0; i < Key.Length; i++)
                AddText(Key[i]);
            AddText(Timeout);
        }
    }
}
