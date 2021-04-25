using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class SET : Command
    {
        public SET(string key, object data, IDataFormater formater)
        {
            Key = key;
            Data = data;
            this.DataFormater = formater;
        }

        public int TimeOut { get; set; }

        public ExpireTimeType ExpireTimeType { get; set; } = ExpireTimeType.EX;

        public override string Name => "SET";

        public string Key { get; set; }

        public object Data { get; set; }

        public bool? NX { get; set; }

        public override bool Read => false;

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            AddData(Data);
            if (TimeOut > 0)
            {
                AddText(ExpireTimeType);
                AddText(TimeOut);
            }
            if (NX != null)
            {
                if (NX == true)
                {
                    AddText("NX");
                }
                else
                {
                    AddText("XX");
                }
            }
        }
    }
}
