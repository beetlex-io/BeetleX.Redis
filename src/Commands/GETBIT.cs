using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class GETBIT : Command
    {
        public GETBIT(string key, int offset)
        {
            Key = key;
            Offset = offset;
        }


        public string Key { get; set; }

        public int Offset { get; set; }

        public override bool Read => true;

        public override string Name => "GETBIT";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Offset);
        }
    }
}
