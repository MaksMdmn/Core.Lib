using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Utils
{
    public static class TimeSpanHelper
    {
        public static bool IsTimeoutPassed(DateTime timeStart, TimeSpan timeout)
        {
            return timeout == Timeout.InfiniteTimeSpan
                ? false
                : DateTime.Now.Subtract(timeStart) < timeout;
        }
    }
}
