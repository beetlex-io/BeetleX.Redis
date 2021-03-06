﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class UNLINK : Command
    {

        public UNLINK(params string[] keys)
        {
            Keys = keys;
        }

        public string[] Keys { get; set; }

        public override bool Read => false;

        public override string Name => "UNLINK";

        public override void OnExecute()
        {
            base.OnExecute();
            for (int i = 0; i < Keys.Length; i++)
            {
                OnWriteKey(Keys[i]);
            }
        }
    }
}
