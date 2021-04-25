using BeetleX.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis.Commands
{
    public class GET : Command
    {
        public GET(string key, IDataFormater formater)
        {
            Key = key;
            DataFormater = formater;
        }

        public string Key { get; set; }

        public override bool Read => true;

        public override string Name => "GET";

        public override void OnExecute()
        {
            base.OnExecute();
            OnWriteKey(Key);
        }
    }

    public class GetBytes : GET
    {
        public GetBytes(string key) : base(key, null)
        {
            this.NetworkReceive = OnReceive;
        }

        protected Result OnReceive(RedisRequest request, PipeStream pipeStream)
        {
            Result result = null;
            return result;
        }
    }

}
