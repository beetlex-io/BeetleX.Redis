using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class XACK : Command
    {

        public XACK(string stream,string group,params string[] id)
        {
            mStream = stream;
            mGroup = group;
            mID = id;
        }

        private string mStream;

        private string mGroup;

        private string[] mID;

        public override bool Read =>false;

        public override string Name => "XACK";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(mStream);
            AddText(mGroup);
            foreach (var item in mID)
                AddText(item);

        }
    }
}
