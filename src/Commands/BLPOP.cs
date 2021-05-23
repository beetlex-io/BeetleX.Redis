using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class BLPOP : Command
    {
        public BLPOP(string[] key, int timeout, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
            TimeOut = timeout;
        }

        public string[] Key { get; set; }

        public int TimeOut { get; set; }

        public override bool Read => false;

        public override string Name => "BLPOP";

        public override void OnExecute()
        {
            base.OnExecute();
            for (int i = 0; i < Key.Length; i++)
                OnWriteKey(Key[i]);
            AddText(TimeOut);
        }
    }
}
