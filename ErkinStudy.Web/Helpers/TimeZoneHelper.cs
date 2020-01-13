using System;

namespace ErkinStudy.Web.Helpers
{
    public  static class TimeZoneHelper
    {
        public static DateTime ConvertLocalDateTime()
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            var localServerTime = DateTimeOffset.Now;
            var usersTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            return usersTime.DateTime;
        }
    }
}
