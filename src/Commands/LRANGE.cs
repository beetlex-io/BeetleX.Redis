using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class LRANGE : Command
    {
        public LRANGE(string key, int start, int stop, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
            Start = start;
            Stop = stop;
        }


        public string Key { get; set; }

        public int Start { get; set; }

        public int Stop { get; set; }

        public override bool Read => true;

        public override string Name => "LRANGE";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Start);
            AddText(Stop);
        }
    }
}
