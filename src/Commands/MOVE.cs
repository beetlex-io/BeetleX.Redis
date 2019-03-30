using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class MOVE : Command
    {

        public MOVE(string key, int db)
        {
            Key = key;
            DB = db;
        }


        public string Key { get; set; }

        public int DB { get; set; }

        public override bool Read => false;

        public override string Name => "MOVE";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(DB);
        }
    }
}
