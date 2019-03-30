using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class RANDOMKEY : Command
    {
        public override bool Read => true;

        public override string Name => "RANDOMKEY";
    }
}
