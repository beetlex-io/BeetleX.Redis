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

        public AUTH(string user, string password)
        {
	        User = user;
	        Password = password;
        }

        public string User { get; set; }

        public override bool Read => false;

        public override string Name => "AUTH";

        public string Password { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            if (!string.IsNullOrEmpty(User))
	            AddText(User);
            AddText(Password);
        }
    }
}
