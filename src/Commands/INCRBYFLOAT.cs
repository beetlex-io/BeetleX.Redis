using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class INCRBYFLOAT : Command
    {

        public INCRBYFLOAT(string key, float increment)
        {
            Key = key;
            Increment = increment;
        }

        public string Key { get; set; }

        public float Increment { get; set; }

        public override bool Read => false;

        public override string Name => "INCRBYFLOAT";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Increment);
        }
    }
}
