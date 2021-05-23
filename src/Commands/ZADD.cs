using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class ZADD : Command
    {
        public ZADD(string key, params (double, string)[] items)
        {
            mItems = items;
            Key = key;
        }

        public string Key { get; private set; }

        private (double, string)[] mItems;

        public override bool Read => false;

        public override string Name => "ZADD";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            foreach (var item in mItems)
            {
                AddText($"{item.Item1}");
                AddText($"{item.Item2}");
            }
        }
    }
}
