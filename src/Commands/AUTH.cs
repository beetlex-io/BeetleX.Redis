using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class AUTH : Command
    {
        public AUTH(string password)
        {
            Password = password;
        }

        public override bool Read => false;

        public override string Name => "AUTH";

        public string Password { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Password);
        }
    }
}
