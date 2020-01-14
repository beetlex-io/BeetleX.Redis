using BeetleX.Redis;
using CodeBenchmark;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTest
{
    public abstract class BeetleX_Base : IExample
    {
        public void Dispose()
        {

        }

        public virtual Task Execute()
        {
            return Task.CompletedTask;
        }

        public void Initialize(Benchmark benchmark)
        {
            if (RedisDB == null)
            {
                RedisDB = new RedisDB(0, new JsonFormater());
                RedisDB.Host.AddWriteHost(Program.Host);
            }
        }

        protected static RedisDB RedisDB;
    }

    public abstract class StackExchangeBase : IExample
    {
        public void Dispose()
        {

        }

        public virtual Task Execute()
        {
            return Task.CompletedTask;
        }

        public void Initialize(Benchmark benchmark)
        {
            if (RedisDB == null)
            {
                ConfigurationOptions options = ConfigurationOptions.Parse(Program.Host);
                options.AsyncTimeout = 20 * 1000;
                options.SyncTimeout = 20 * 1000;
                Redis = ConnectionMultiplexer.Connect(options);
                RedisDB = Redis.GetDatabase(0);
            }
        }

        protected static ConnectionMultiplexer Redis;

        protected static IDatabase RedisDB;
    }
}
