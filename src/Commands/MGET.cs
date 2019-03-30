using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class MGET : Command
    {

        public MGET(IDataFormater dataFormater, params string[] keys)
        {
            Keys = keys;
            DataFormater = dataFormater;
        }

        public string[] Keys { get; set; }

        public override bool Read => true;

        public override string Name => "MGET";

        public override void OnExecute()
        {
            base.OnExecute();
            for (int i = 0; i < Keys.Length; i++)
            {
                AddText(Keys[i]);
            }
        }
    }
}
