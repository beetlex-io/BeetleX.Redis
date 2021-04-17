using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SETS_SREM : Command
    {

        public SETS_SREM(string key, string[] member)
        {
            Key = key;
            Members = member;
        }

        public override bool Read => false;

        public override string Name => "SREM";

        public string Key { get; set; }

        public string[] Members { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            foreach (var item in Members)
            {
                AddText(item);
            }
        }
    }
}
