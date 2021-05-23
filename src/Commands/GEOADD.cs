using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class GEOADD : Command
    {

        public GEOADD(string key, (double, double, string)[] data)
        {
            Key = key;
            Data = data;
        }

        public string Key { get; set; }

        public (double, double, string)[] Data { get;private set; }

        public override bool Read => false;

        public override string Name => "GEOADD";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            foreach (var item in Data)
            {
                AddText(item.Item1);
                AddText(item.Item2);
                AddText(item.Item3);
            }
        }
    }
}
