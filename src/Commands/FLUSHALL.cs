using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class FLUSHALL : Command
    {
        public override bool Read => false;

        public override string Name => "FLUSHALL";
    }
}
