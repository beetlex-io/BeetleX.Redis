using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class LINDEX : Command
    {
        public LINDEX(string key, int index, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
            Index = index;
        }

        public string Key { get; set; }

        public int Index { get; set; }

        public override bool Read => false;

        public override string Name => "LINDEX";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Index);
        }

    }
}
