using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class WAIT : Command
    {
        public WAIT(int numreplicas, int timeout)
        {
            Numreplicas = numreplicas;
            Timeout = timeout;
        }

        public int Numreplicas { get; set; }

        public int Timeout { get; set; }

        public override bool Read => false;

        public override string Name => "WAIT";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Numreplicas);
            AddText(Timeout);
        }
    }
}
