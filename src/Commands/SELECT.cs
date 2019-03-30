using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SELECT : Command
    {

        public SELECT(int index)
        {
            Index = index;
        }

        public int Index { get; set; }

        public override bool Read => false;

        public override string Name => "SELECT";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Index);
        }
    }
}
