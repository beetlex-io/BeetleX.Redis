using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class RPOPLPUSH : Command
    {
        public RPOPLPUSH(string source, string dest, IDataFormater dataFormater = null)
        {
            DataFormater = dataFormater;
            Source = source;
            Dest = dest;
        }

        public string Source { get; set; }

        public string Dest { get; set; }

        public override bool Read => false;

        public override string Name => "RPOPLPUSH";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Source);
            AddText(Dest);
        }
    }
}
