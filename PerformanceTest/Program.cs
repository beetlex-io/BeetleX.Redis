using StackExchange.Redis;
using System;
using System.Threading.Tasks;
namespace PerformanceTest
{
    class Program
    {
        public const string Host = "localhost";

        static void Main(string[] args)
        {

            Test();
            Console.Read();
        }

        private static async void Test()
        {
            TestCenter testCenter = new TestCenter();
            await testCenter
            .AddCases<BeetleX_SET>()
            .AddCases<StackExchange_SET>()
            .AddCases<StackExchange_Sync_SET>()

            .AddCases<BeetleX_GET>()
            .AddCases<StackExchange_GET>()
            .AddCases<StackExchange_Sync_GET>()

            .AddCases<BeetleX_MGET>()
            .AddCases<StackExchange_MGET>()
            .AddCases<StackExchange_Sync_MGET>()

            .AddTest(1, 100000)
            .AddTest(2, 100000)
            .AddTest(4, 100000)
            //.AddTest(8, 200000)
            //.AddTest(16, 500000)
            //.AddTest(24, 500000)
            .Run();
        }
    }
}
