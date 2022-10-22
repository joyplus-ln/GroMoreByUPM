using System;

namespace ByteDance.Union
{
    public static class TimeUtil
    {
        // 获取系统时间戳
        public static string GetTimeStamp()
        {
            return GetTimeStampLong().ToString();
        }

        public static long GetTimeStampLong()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
    }
}