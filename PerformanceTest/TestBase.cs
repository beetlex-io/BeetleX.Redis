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

        }

        private System.Diagnostics.Stopwatch mWatch = new System.Diagnostics.Stopwatch();

        private int mIncrement;

        private int mThreads;

        public double Seconds
        {
            get
            {
                return mWatch.Elapsed.TotalSeconds;
            }
        }

        private TaskCompletionSource<TestResult> taskCompletionSource;

        public int Count { get; set; }

        internal Task<TestResult> Run(int threads, int count)
        {
            mThreads = threads;
            mCompleted = 0;
            Count = count;
            mIncrement = 0;
            mWatch.Restart();
            taskCompletionSource = new TaskCompletionSource<TestResult>();
            for (int i = 0; i < threads; i++)
            {
                Task.Run(() => { OnTest(); });
            }
            return taskCompletionSource.Task;
        }

        protected virtual void OnTest()
        {

        }

        private int mCompleted = 0;

        public bool Increment()
        {
            int count = System.Threading.Interlocked.Increment(ref mIncrement);
            if (count >= Count)
            {
                if (System.Threading.Interlocked.CompareExchange(ref mCompleted, 1, 0) == 0)
                {
                    mWatch.Stop();
                    TestResult result = new TestResult { Name = GetType().Name, Count = Count, Seconds = mWatch.Elapsed.TotalSeconds, Threads = mThreads };
                    Task.Run(() => { taskCompletionSource.SetResult(result); });
                }
                return false;
            }
            return true;

        }

        public class TestResult
        {
            public int Count { get; set; }

            public double Seconds { get; set; }

            public string Name { get; set; }

            public int Threads { get; set; }

            public override string ToString()
            {
                return $"{Name} using {Threads} threads run {Count} [use time {Seconds.ToString("###,###.##")}s {(Count / Seconds).ToString("###,###.##")}/sec]";
            }

        }

        public static async Task<TestResult> Run<T>(int threads, int count) where T : TestBase, new()
        {
            T item = new T();
            return await item.Run(threads, count);
        }
    }
}
