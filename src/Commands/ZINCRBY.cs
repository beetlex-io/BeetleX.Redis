using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZINCRBY : Command
    {
        public ZINCRBY(string key, double increment,string member)
        {
            Key = key;
            mIncrement = increment;
            mMember = member;
        }

        private double mIncrement;

        private string mMember;

        public string Key { get; private set; }

        public override bool Read => false;

        public override string Name => "ZINCRBY";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(mIncrement);
            AddText(mMember);
        }
    }
}
