using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class XDEL : Command
    {
        public XDEL(string stream,params string[] id)
        {
            mStream = stream;
            mIDs = id;
        }

        private string mStream;

        private string[] mIDs;

        public override bool Read => false;

        public override string Name => "XDEL";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(mStream);
            foreach(var item in mIDs)
            {
                AddText(item);
            }
        }
    }
}
