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

    public abstract class FreeRedisBase : IExample
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
            if (cli == null)
            {
                cli = new FreeRedis.RedisClient($"{Program.Host}:6379,min poolsize=10");
            }
        }

        public static FreeRedis.RedisClient cli;
    }

    public abstract class NewLifeRedisBase : IExample
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
            if (cli == null)
            {
                cli = new NewLife.Caching.Redis($"{Program.Host}:6379", null, 0);
            }
        }

        public static NewLife.Caching.Redis cli;

    }

}
