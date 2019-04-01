using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTest
{
    public class TestBase
    {
        public TestBase()
        {
            mProcess = System.Diagnostics.Process.GetCurrentProcess();
        }

        private System.Diagnostics.Process mProcess;

        private System.Diagnostics.Stopwatch mWatch = new System.Diagnostics.Stopwatch();

        private int mIncrement;

        private int mThreads;

        private int mCompletedQuantity;

        public double Seconds
        {
            get
            {
                return mWatch.Elapsed.TotalSeconds;
            }
        }

        private TaskCompletionSource<TestResult> taskCompletionSource;

        public int Count { get; set; }

        private double mStartProcessTime;

        internal Task<TestResult> Run(int threads, int count)
        {
            mThreads = threads;
            mCompletedQuantity = 0;
            Count = count;
            mIncrement = 0;
            mWatch.Restart();
            taskCompletionSource = new TaskCompletionSource<TestResult>();
            mStartProcessTime = mProcess.TotalProcessorTime.TotalMilliseconds;
            for (int i = 0; i < threads; i++)
            {
                Task.Run(() => { OnTest(); });
            }
            return taskCompletionSource.Task;
        }

        private void Completed()
        {
            int value = System.Threading.Interlocked.Increment(ref mCompletedQuantity);
            if (value == mThreads)
            {
                mWatch.Stop();
                TestResult result = new TestResult
                {
                    Name = GetType().Name,
                    Count = Count,
                    Seconds = mWatch.Elapsed.TotalSeconds,
                    Threads = mThreads
                };
                Task.Run(() => { taskCompletionSource.SetResult(result); });
            }
        }

        protected virtual void OnTest()
        {

        }

        public bool Increment()
        {
            int count = System.Threading.Interlocked.Increment(ref mIncrement);
            if (count >= Count)
            {
                Completed();
                return false;
            }
            return true;
        }

        public class TestResult
        {
            public int Count { get; set; }

            public int Round { get; set; }

            public double Seconds { get; set; }

            public string Name { get; set; }

            public int Threads { get; set; }

            public double Cpu { get; set; }



        }

        public static async Task<TestResult> Run<T>(int threads, int count) where T : TestBase, new()
        {
            T item = new T();
            return await item.Run(threads, count);
        }
    }

    public class TestCenter
    {
        public TestCenter()
        {


        }

        private List<Type> mCases = new List<Type>();

        private List<(int, int, int)> mTests = new List<(int, int, int)>();

        public TestCenter AddCases<T>() where T : TestBase, new()
        {
            mCases.Add(typeof(T));
            return this;
        }

        public TestCenter AddTest(int threads, int count, ushort round = 1)
        {
            mTests.Add((threads, count, round));
            return this;
        }


        public async Task<bool> Run()
        {
            Console.BufferWidth = 100;

            string evt = $"|{"Name".PadRight(30)}| Round| Threads|{" Count".PadLeft(10)}| Use time(s)|{" Sec".PadLeft(6)}|";
            Console.WriteLine("".PadLeft(evt.Length, '-'));
            Console.WriteLine(evt);
            Console.WriteLine("".PadLeft(evt.Length, '-'));
            foreach (var t in mTests)
            {
                for (int i = 0; i < t.Item3; i++)
                {
                    List<TestBase> items = new List<TestBase>();
                    foreach (var item in mCases)
                    {
                        items.Add((TestBase)Activator.CreateInstance(item));
                    }
                    foreach (var test in items)
                    {

                        var tresult = await test.Run(t.Item1, t.Item2);
                        tresult.Round = t.Item3;
                        evt = $"|{tresult.Name.PadRight(30)}|{(i + 1).ToString().PadLeft(6)}|{tresult.Threads.ToString().PadLeft(8)}|{tresult.Count.ToString().PadLeft(10)}|{tresult.Seconds.ToString("######.##").PadLeft(12)}|{(tresult.Count / tresult.Seconds).ToString("######").PadLeft(6)}|";
                        Console.WriteLine(evt);
                        Console.WriteLine("".PadLeft(evt.Length, '-'));
                    }
                }
            }
            Console.WriteLine("Test completed!");
            return true;

        }
    }

}
