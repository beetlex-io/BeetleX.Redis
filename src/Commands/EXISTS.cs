using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class EXISTS : Command
    {

        public EXISTS(params string[] keys)
        {
            Keys = keys;
        }

        public string[] Keys { get; set; }

        public override bool Read => true;

        public override string Name => "EXISTS";

        public override void OnExecute()
        {
            base.OnExecute();
            for (int i = 0; i < Keys.Length; i++)
                AddText(Keys[i]);
        }
    }
}
