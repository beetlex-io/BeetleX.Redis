using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class XREADGROUP : Command
    {

        public XREADGROUP(string stream, string group, string consumer, string id)
        {
            mStream = stream;
            mGroup = group;
            mConsumer = consumer;
            if (!string.IsNullOrEmpty(id))
                ID = id;
        }

        private string mStream;

        private string mGroup;

        private string mConsumer;

        public override bool Read => true;

        public string ID { get; set; } = ">";

        public int? Count { get; set; }

        public int? Block { get; set; }

        public override string Name => "XREADGROUP";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText("GROUP");
            AddText(mGroup);
            AddText(mConsumer);
            if (Count != null)
            {
                AddText("COUNT");
                AddText(Count.Value);
            }
            if (Block != null)
            {
                AddText("BLOCK");
                AddText(Block.Value);
            }
            AddText("STREAMS");
            AddText(mStream);
            AddText(ID);
        }
    }
}
