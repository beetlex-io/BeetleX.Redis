using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class XLEN:Command
    {
        public XLEN(string stream)
        {
            mStream = stream;
        }

        private string mStream;

        public override bool Read => true;

        public override string Name => "XLEN";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(mStream);
        }
    }
}
