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

        public int ExTime { get; set; }

        public int PXTime { get; set; }

        public int EXATTime { get; set; }

        public override string Name => "SET";

        public string Key { get; set; }

        public object Data { get; set; }

        public bool? NX { get; set; }

        public override bool Read => false;

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
            if (Data == null)
                Data = string.Empty;
            AddData(Data);
            if (ExTime > 0)
            {
                AddText("EX");
                AddText(ExTime);
            }
            if (PXTime > 0)
            {
                AddText("PX");
                AddText(PXTime);
            }
            if (EXATTime > 0)
            {
                AddText("EXAT");
                AddText(EXATTime);
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
