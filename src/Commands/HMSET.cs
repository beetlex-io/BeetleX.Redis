using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class HMSET : Command
    {

        public HMSET(string key, IDataFormater dataFormater)
        {
            this.DataFormater = dataFormater;
            Key = key;
        }

        public string Key { get; set; }

        public override bool Read => false;

        public override string Name => "HMSET";

        private List<Tuple<string, object>> mFields = new List<Tuple<string, object>>();

        public HMSET this[string field, object value]
        {
            get
            {
                mFields.Add(new Tuple<string, object>(field, value));
                return this;
            }
        }
        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            for (int i = 0; i < mFields.Count; i++)
            {
                var item = mFields[i];
                AddText(item.Item1);
                AddData(item.Item2);
            }
        }
    }
}
