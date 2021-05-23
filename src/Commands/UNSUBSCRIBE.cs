using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class UNSUBSCRIBE : Command
    {
        public UNSUBSCRIBE(params string[] channels)
        {
            Changes = channels;
        }

        public string[] Changes { get; set; }

        public override bool Read => false;

        public override string Name => "UNSUBSCRIBE";

        public override void OnExecute()
        {
            base.OnExecute();
            if (Changes != null)
                for (int i = 0; i < Changes.Length; i++)
                    OnWriteKey(Changes[i]);
        }
    }
}
