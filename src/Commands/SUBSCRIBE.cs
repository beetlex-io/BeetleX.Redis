using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SUBSCRIBE : Command
    {
        public SUBSCRIBE(IDataFormater dataFormater, params string[] channels)
        {
            Changes = channels;
            DataFormater = dataFormater;
        }

        public string[] Changes { get; set; }

        public override bool Read => false;

        public override string Name => "SUBSCRIBE";

        public override void OnExecute()
        {
            base.OnExecute();
            for (int i = 0; i < Changes.Length; i++)
                AddText(Changes[i]);
        }
    }
}
