using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class PING : Command
    {
        public PING(string msg)
        {
            Message = msg;
        }

        public string Message { get; set; }

        public override bool Read => false;

        public override string Name => "PING";

        public override void OnExecute()
        {
            base.OnExecute();
            if (!string.IsNullOrEmpty(Message))
                AddText(Message);
        }
    }
}
