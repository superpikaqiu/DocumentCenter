using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Domain.Helper
{
    public class TimeHelper
    {
        public static long GetTimeStamp(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long timeStamp = (long)(time - startTime).TotalMilliseconds;

            return timeStamp;
        }

        public static DateTime GetTime(long timestamp)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            DateTime dt = startTime.AddMilliseconds(timestamp);

            return dt;
        }
    }
}