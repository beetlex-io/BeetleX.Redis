using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class EXPIRE : Command
    {

        public EXPIRE(string key, int seconds)
        {
            Key = key;
            Seconds = seconds;
        }

        public string Key { get; set; }

        public int Seconds { get; set; }

        public EXPIREType? Type { get; set; }

        public override bool Read => false;

        public override string Name => "EXPIRE";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Seconds);
            if (Type != null)
            {
                AddText(Enum.GetName(typeof(EXPIREType), Type.Value));
            }
        }
    }

    public enum EXPIREType
    {
        NX,
        XX,
        GT,
        LT
    }
}
