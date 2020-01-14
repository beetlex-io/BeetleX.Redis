using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceTest
{
    public class OrderHelper
    {

        private static long mID;

        public static int GetOrderID()
        {
            var id = System.Threading.Interlocked.Increment(ref mID);
            id = id % (11077 - 10248);
            id += 10248;
            return (int)id;
        }

        public static Northwind.Data.Order GetOrder()
        {
            var id = System.Threading.Interlocked.Increment(ref mID);
            return Northwind.Data.DataHelper.Defalut.Orders[(int)(id % Northwind.Data.DataHelper.Defalut.Orders.Count)];
        }
    }
}
