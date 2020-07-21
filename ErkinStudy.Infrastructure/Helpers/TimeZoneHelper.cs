using System;

namespace ErkinStudy.Infrastructure.Helpers
{
    public  static class TimeZoneHelper
    {
        public static DateTime GetLocalDateTime()
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            var localServerTime = DateTimeOffset.Now;
            var usersTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            return usersTime.DateTime;
        }
    }
}
