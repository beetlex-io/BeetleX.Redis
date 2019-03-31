using System;
using System.Threading.Tasks;
namespace PerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.Read();
        }

        private static async void Test()
        {
            Console.WriteLine(await TestBase.Run<SET_JSON>(1, 100000));
            Console.WriteLine(await TestBase.Run<GET_JSON>(1, 100000));

            Console.WriteLine(await TestBase.Run<SET_JSON>(4, 100000));
            Console.WriteLine(await TestBase.Run<GET_JSON>(4, 100000));

            Console.WriteLine(await TestBase.Run<SET_JSON>(8, 100000));
            Console.WriteLine(await TestBase.Run<GET_JSON>(8, 100000));
        }
    }
}
