using System;

namespace Sporty.Common
{
    public static class DateTimeConverter
    {
        public static DateTime GetUtcDateTime(DateTime localDateTime, string timeZoneId)
        {
            DateTime localDate = DateTime.SpecifyKind(localDateTime, DateTimeKind.Local);
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime utcDate = DateTime.SpecifyKind(localDate.Subtract(tzi.BaseUtcOffset), DateTimeKind.Utc);
            return utcDate;
        }

        public static DateTime GetLocalDateTime(DateTime utcDateTime, string timeZoneId)
        {
            DateTime utcDate = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime localDate = DateTime.SpecifyKind(utcDate.Add(tzi.BaseUtcOffset), DateTimeKind.Local);
            return localDate;
        }
    }
}