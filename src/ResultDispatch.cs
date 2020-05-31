using BeetleX.Dispatchs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.Redis
{
    public class ResultDispatch
    {
        static ResultDispatch()
        {
            DispatchCenter = new DispatchCenter<RedisRequest>(OnExecute);
        }

        public static void SetThreads(int threads)
        {
            DispatchCenter = new DispatchCenter<RedisRequest>(OnExecute, threads);
        }

        public static DispatchCenter<RedisRequest> DispatchCenter { get; set; }

        private static void OnExecute(RedisRequest request)
        {
            request.TaskCompletion();
        }

    }
}
