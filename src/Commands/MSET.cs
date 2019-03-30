using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class MSET : Command
    {
        public MSET(IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
        }

        public override bool Read => false;

        public override string Name => "MSET";

        private List<Tuple<String, Object>> mValues { get; set; } = new List<Tuple<string, object>>();

        public MSET this[string name, object data]
        {
            get
            {
                mValues.Add(new Tuple<string, object>(name, data));
                return this;
            }
        }

        public override void OnExecute()
        {
            base.OnExecute();

            for (int i = 0; i < mValues.Count; i++)
            {
                var item = mValues[i];
                AddText(item.Item1);
                AddData(item.Item2);
            }
        }
    }
}
