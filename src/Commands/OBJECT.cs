using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class OBJECT : Command
    {
        public OBJECT(SubCommand cmd, params string[] arguments)
        {
            Command = cmd;
            Arguments = arguments;
        }

        public SubCommand Command { get; set; }

        public string[] Arguments { get; set; }

        public override bool Read => true;

        public override string Name => "OBJECT";

        public enum SubCommand
        {
            REFCOUNT,
            ENCODING,
            IDLETIME
        }
        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Command);
            for (int i = 0; i < Arguments.Length; i++)
            {
                AddText(Arguments[i]);
            }
        }
    }
}
