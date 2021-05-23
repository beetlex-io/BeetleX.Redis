using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class XGROUP_CREATE : Command
    {
        public XGROUP_CREATE(string stream, string group)
        {
            mStream = stream;
            mGroup = group;
        }

        public string Start { get; set; } = "$";

        private string mStream;

        private string mGroup;

        public override bool Read => false;

        public override string Name => "XGROUP";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText("CREATE");
            OnWriteKey(mStream);
            AddText(mGroup);
            AddText(Start);
            AddText("mkstream");
        }
    }

    public class XGROUP_DESTROY : Command
    {

        public XGROUP_DESTROY(string stream, string group)
        {
            mStream = stream;
            mGroup = group;
        }

        private string mStream;

        private string mGroup;

        public override bool Read => false;

        public override string Name => "XGROUP";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText("DESTROY");
            OnWriteKey(mStream);
            AddText(mGroup);
        }
    }

    public class XGROUP_SETID : Command
    {
        public override bool Read => false;

        public override string Name => "XGROUP";

        public XGROUP_SETID(string stream, string group, string id)
        {
            mStream = stream;
            mGroup = group;
            mID = string.IsNullOrEmpty(id) ? "0" : id;
        }

        private string mID;

        private string mStream;

        private string mGroup;

        public override void OnExecute()
        {
            base.OnExecute();
            AddText("SETID");
            OnWriteKey(mStream);
            AddText(mGroup);
            AddText(mID);
        }
    }



}
