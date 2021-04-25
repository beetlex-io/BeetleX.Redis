using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class MSETNX : Command
    {
        public MSETNX(IDataFormater dataFormater)
        {
            DataFormater = dataFormater;
        }


        public MSETNX this[string name, object data]
        {
            get
            {
                mValues.Add(new Tuple<string, object>(name, data));
                return this;
            }
        }

        public override bool Read => false;

        public override string Name => "MSETNX";

        private List<Tuple<string, object>> mValues = new List<Tuple<string, object>>();


        public override void OnExecute()
        {
            base.OnExecute();
            for (int i = 0; i < mValues.Count; i++)
            {
                var item = mValues[i];
                OnWriteKey(item.Item1);
                AddData(item.Item2);
            }
        }
    }
}
