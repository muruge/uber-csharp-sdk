using System;

namespace Uber.Website.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime UnixToDateTime(int unix)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dtDateTime.AddSeconds(unix);
        }
    }
}