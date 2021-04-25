using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class PFMerge : Command
    {
        public PFMerge(string key,params string[] items)
        {
            Key = key;
            Items = items;
        }

        public override bool Read => false;

        public override string Name => "PFMERGE";

        public string Key { get; set; }

        public string[] Items { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            foreach (var item in Items)
                OnWriteKey(item);
        }
    }
}
