using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HGET : Command
    {

        public HGET(string key, string field, IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
            Key = key;
            Field = field;
        }

        public string Key { get; set; }

        public string Field { get; set; }

        public override bool Read => true;

        public override string Name => "HGET";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddText(Field);
        }
    }
}
