using BeetleX.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class XADD : Command
    {
        public XADD(string name, string id)
        {
            mStreamName = name;
            mID = string.IsNullOrEmpty(id) ? "*" : id;
            NetworkReceive = OnRead;
        }

        private string mStreamName;

        private string mID;

        public int? MAXLEN { get; set; }

        public object Data { get; set; }

        public override bool Read => false;

        public override string Name => "XADD";

        public override void OnExecute()
        {
            base.OnExecute();
            AddText(mStreamName);
            if (MAXLEN != null)
            {
                AddText("MAXLEN");
                AddText(MAXLEN.Value);
            }
            AddText(mID);
            if (Data is Dictionary<string, string> kv)
            {
                foreach (var item in kv)
                {
                    AddText(item.Key);
                    AddText(item.Value);
                }
            }
            else
            {
                AddText("data");
                AddData(Data);
            }
        }

        private Result OnRead(RedisRequest request, PipeStream stream)
        {
            stream.ReadLine();
            Result result = new Result();
            result.ResultType = ResultType.Object;
            result.Data.Add(new ResultItem { Type = ResultType.String, Data = stream.ReadLine() });
            return result;
        }
    }
}
