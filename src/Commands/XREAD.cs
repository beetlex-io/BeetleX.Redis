using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class XREAD : Command
    {
        public XREAD(string stream)
        {
            mStream = stream;
        }

        private string mStream;

        public int? Count { get; set; }

        public int? Block { get; set; }

        public string Start { get; set; }

        public override bool Read => true;

        public override string Name => "XREAD";

        public override void OnExecute()
        {
            base.OnExecute();
            if(Block !=null)
            {
                AddText("BLOCK");
                AddText(Block.Value);
            }
            if(Count !=null)
            {
                AddText("COUNT");
                AddText(Count.Value);
            }
            AddText("STREAMS");
            OnWriteKey(mStream);
            if (string.IsNullOrEmpty(Start))
            {
                AddText("$");
            }
            else
            {
                AddText(Start);
            }
        }
    }
}
