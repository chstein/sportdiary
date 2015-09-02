using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sporty.Common;

namespace Sporty.ViewModel
{
    public class CalendarWeek
    {
        public CalendarWeek(DateTime firstDayInWeek)
        {
            FirstDayInWeek = firstDayInWeek;
            WeekSummary = new List<SportWeekSummary>();
            Days = new List<CalendarDay>();
            for (int i = 0; i < 7; i++)
            {
                Days.Add(new CalendarDay(firstDayInWeek.AddDays(i)));
            }

            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            Number = currentCulture.Calendar.GetWeekOfYear(firstDayInWeek, CalendarWeekRule.FirstFourDayWeek,
                                                           DayOfWeek.Monday);
        }

        public DateTime FirstDayInWeek { get; private set; }


        public List<CalendarDay> Days { get; set; }

        public int Number { get; set; }

        public int? WeekNumberToNextGoal { get; set; }

        public List<SportWeekSummary> WeekSummary { get; set; }

        public PhaseView Phase { get; set; }

        public void AddSessionToSummary(IEnumerable<SessionCalendarView> sessionViewList,
                                        CalendarContentType calendarType)
        {
            foreach (SessionCalendarView session in sessionViewList)
            {
                if (WeekSummary.Count(sw => sw.SportTypeId == session.SportTypeId) == 0)
                {
                    var sportWeekSummary = new SportWeekSummary
                                               {
                                                   SportTypeId = session.SportTypeId,
                                                   Discipline = Enum.GetName(typeof(Disciplines), session.SportTypeId)
                                               };
                    if (calendarType == CalendarContentType.Plan)
                    {
                        sportWeekSummary.PlannedDistance = session.PlannedDistance;
                        sportWeekSummary.PlannedDuration = session.PlannedDuration;
                    }
                    else if (calendarType == CalendarContentType.Exercise)
                    {
                        sportWeekSummary.Duration = session.Duration.HasValue ? session.Duration.Value : 0;
                        sportWeekSummary.Distance = session.Distance.HasValue ? session.Distance.Value : 0.0;
                    }
                    WeekSummary.Add(sportWeekSummary);
                }
                else
                {
                    SportWeekSummary ws = WeekSummary.Single(sw => sw.SportTypeId == session.SportTypeId);

                    if (calendarType == CalendarContentType.Plan)
                    {
                        if (session.PlannedDistance > 0)
                        {
                            ws.PlannedDistance += session.PlannedDistance;
                        }
                        if (session.PlannedDuration > 0)
                        {
                            ws.PlannedDuration += session.PlannedDuration;
                        }
                    }
                    else if (calendarType == CalendarContentType.Exercise)
                    {
                        if (session.Distance > 0)
                        {
                            ws.Distance += session.Distance.Value;
                        }
                        if (session.Duration > 0)
                        {
                            ws.Duration += session.Duration.Value;
                        }
                    }
                }
            }
        }

        public void UpdateSummaryForCurrentWeek(IEnumerable<SessionCalendarView> sessions)
        {
            foreach (SessionCalendarView session in sessions)
            {
                if (WeekSummary.Count(sw => sw.SportTypeId == session.SportTypeId) == 0)
                {
                    var sportWeekSummary = new SportWeekSummary
                                               {
                                                   SportTypeId = session.SportTypeId,
                                                   Discipline = session.Discipline
                                               };
                    if (session.IsExercise)
                    {
                        sportWeekSummary.Duration = session.Duration.HasValue ? session.Duration.Value : 0;
                        sportWeekSummary.Distance = session.Distance.HasValue ? session.Distance.Value : 0.0;
                    }
                    else
                    {
                        sportWeekSummary.PlannedDistance = session.PlannedDistance;
                        sportWeekSummary.PlannedDuration = session.PlannedDuration;
                    }
                    WeekSummary.Add(sportWeekSummary);
                }
                else
                {
                    SportWeekSummary ws = WeekSummary.Single(sw => sw.SportTypeId == session.SportTypeId);

                    if (session.IsExercise)
                    {
                        if (session.Distance > 0)
                        {
                            ws.Distance += session.Distance.Value;
                        }
                        if (session.Duration > 0)
                        {
                            ws.Duration += session.Duration.Value;
                        }
                    }
                    else
                    {
                        if (session.PlannedDistance > 0)
                        {
                            ws.PlannedDistance += session.PlannedDistance;
                        }
                        if (session.PlannedDuration > 0)
                        {
                            ws.PlannedDuration += session.PlannedDuration;
                        }
                    }
                }
            }
        }
    }
}