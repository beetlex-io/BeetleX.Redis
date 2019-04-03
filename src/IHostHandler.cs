using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis
{
    public interface IHostHandler
    {


        RedisHost AddWriteHost(string host, int port = 6379);


        RedisHost AddReadHost(string host, int port = 6379);


        RedisHost GetWriteHost();


        RedisHost GetReadHost();

    }
}
