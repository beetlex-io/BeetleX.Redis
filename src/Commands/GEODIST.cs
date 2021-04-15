using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class GEODIST : Command
    {
        public GEODIST(string key, string member1, string member2, GEODISTUnit unit = GEODISTUnit.m)
        {

            Key = key;
            Member1 = member1;
            Member2 = member2;
            Unit = unit;
        }

        public override bool Read => true;

        public override string Name => "GEODIST";

        public GEODISTUnit Unit { get; set; } = GEODISTUnit.m;

        public string Key { get; set; }

        public string Member1 { get; set; }

        public string Member2 { get; set; }

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(Key);
            AddText(Member1);
            AddText(Member2);
            AddText(Unit.ToString());
        }

    }

    public enum GEODISTUnit
    {
        m,
        km,
        mi,
        ft
    }

}
