using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZREM : Command
    {
        public ZREM(string key,string[] members)
        {
            Key = key;
            Members = members;
        }

        public string Key { get; private set; }

        public string[] Members { get; private set; }

        public override bool Read =>false;

        public override string Name => "ZREM";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            foreach (var item in Members)
                AddText(item);
        }
    }
}
