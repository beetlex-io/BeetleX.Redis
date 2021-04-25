using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZSCORE : Command
    {

        public ZSCORE(string key, string member)
        {
            Key = key;
            mMember = member;
        }

        private string mMember;

        public string Key { get; private set; }

        public override bool Read =>true;

        public override string Name => "ZSCORE";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(mMember);
        }
    }
}
