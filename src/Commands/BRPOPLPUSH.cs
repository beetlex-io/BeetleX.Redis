using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class BRPOPLPUSH : Command
    {
        public BRPOPLPUSH(string source, string dest, int timeout, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Source = source;
            Dest = dest;
            TimeOut = timeout;
        }

        public string Source { get; set; }

        public string Dest { get; set; }

        public int TimeOut { get; set; }

        public override bool Read => false;

        public override string Name => "BRPOPLPUSH";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Source);
            OnWriteKey(Dest);
            AddText(TimeOut);
        }
    }
}
