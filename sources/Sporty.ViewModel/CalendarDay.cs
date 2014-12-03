using System;
using System.Collections.Generic;

namespace Sporty.ViewModel
{
    public class CalendarDay
    {
        public DateTime Date;
        public int Day
        {
            get
            {
                return Date.Day;
            }
        }
        public string ClientId { get; private set; }
        public CalendarDay(DateTime date)
        {
            Date = date;

            ClientId = String.Format("{0:00}_{1:00}_{2:00}", date.Day, date.Month, date.Year);
            Sessions = new List<SessionCalendarView>();
        }

        public List<SessionCalendarView> Sessions { get; set; }
        public bool IsDayInMonth { get; set; }
        public bool IsDayToday { get; set; }
        public bool IsGoalToday { get; set; }
        public string GoalName { get; set; }
    }
}
