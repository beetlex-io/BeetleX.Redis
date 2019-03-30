using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis
{
    public class RedisException : Exception
    {
        public RedisException(string msg) : base(msg)
        {

        }

        public RedisException(string msg, Exception innerError) : base(msg, innerError) { }
    }
}
