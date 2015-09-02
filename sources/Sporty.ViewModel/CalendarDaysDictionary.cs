using System;
using System.Collections.Generic;

namespace Sporty.ViewModel
{
    public class CalendarDaysDictionary
    {
        private Dictionary<int, CalendarDay> days;

        public CalendarDaysDictionary()
        {
            days = new Dictionary<int, CalendarDay>();
        }

        public void AddExercise(ExerciseDetails Exercise)
        {
            int dayId = GetDayId(Exercise.Date);
            
            CalendarDay day = null;
            if (!days.ContainsKey(dayId))
                day = new CalendarDay(Exercise.Date);
            else
                day = days[dayId];

            var session = new SessionCalendarView();
            session.SessionId = Exercise.Id;
            session.SportTypeId = Exercise.SportTypeId;
            session.SportTypeName = Exercise.SportTypeName;
            if (Exercise.Duration.HasValue)
                session.Duration = Exercise.Duration.Value.Minutes;
            if (Exercise.Distance.HasValue)
                session.Distance = Exercise.Distance;
            if (Exercise.ZoneId.HasValue)
                session.ZoneId = Exercise.ZoneId.Value;

            day.Sessions.Add(session);
        }

        private int GetDayId(DateTime date)
        {
            string idTemp = String.Concat(date.Date.Year, date.Date.Month.ToString("d:2"), date.Date.Day.ToString("d:2")) ;
            return Int32.Parse(idTemp);
        }
    }
}