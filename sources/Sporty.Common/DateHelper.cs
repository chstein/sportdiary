using System;
using System.Globalization;

namespace Sporty.Common
{
    public static class DateHelper
    {
        public static DateTime GetFirstDayInWeekMonth(int month, int year)
        {
            var date = new DateTime(year, month, 1);
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.Subtract(new TimeSpan(1, 0, 0, 0));
            }
            return date;
        }

        public static DateTime GetFirstDayInWeek(DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.Subtract(new TimeSpan(1, 0, 0, 0));
            }
            return date;
        }

        public static DateTime GetLastDayInWeekMonth(int month, int year)
        {
            var date = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            while (date.DayOfWeek != DayOfWeek.Sunday)
            {
                date = date.DayOfWeek == DayOfWeek.Monday
                           ? date.AddDays(6)
                           : date.AddDays(1);
            }
            return date;
        }

        public static DateTime GetLastDayInWeek(DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Sunday)
            {
                date = date.DayOfWeek == DayOfWeek.Monday
                           ? date.AddDays(6)
                           : date.AddDays(1);
            }
            return date;
        }

        public static int GetWeekNumber(DateTime day)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            return cal.GetWeekOfYear(day, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }
    }
}