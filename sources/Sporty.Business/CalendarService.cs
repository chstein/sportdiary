using System;
using System.Collections.Generic;
using System.Linq;
using Sporty.Business.Interfaces;
using Sporty.Common;
using Sporty.DataModel;
using Sporty.ViewModel;
using Sporty.Business.Helper;

namespace Sporty.Business
{
    public class CalendarService : ICalendarService
    {
        private readonly IExerciseRepository exerciseRepository;
        private readonly IGoalRepository goalRepository;
        private readonly IPhaseRepository phaseRepository;
        private readonly IPlanRepository planRepository;
        private readonly IWeekPlanRepository weekPlanRepository;


        public CalendarService(IExerciseRepository exerciseRepository, IPlanRepository planRepository,
                               IGoalRepository goalRepository, IWeekPlanRepository weekPlanRepository,
                               IPhaseRepository phaseRepository)
        {
            this.exerciseRepository = exerciseRepository;
            this.planRepository = planRepository;
            this.goalRepository = goalRepository;
            this.weekPlanRepository = weekPlanRepository;
            this.phaseRepository = phaseRepository;
        }

        #region ICalendarService Members

        public CalendarViewModel GetViewModel(int monthValue, int yearValue, Guid userId,
                                              CalendarContentType calendarType)
        {
            DateTime startDate = DateHelper.GetFirstDayInWeekMonth(monthValue, yearValue);
            DateTime endDate = DateHelper.GetLastDayInWeekMonth(monthValue, yearValue);

            IEnumerable<Exercise> exercises =
                exerciseRepository.GetExercises(
                    e => e.UserId == userId && e.Date >= startDate.Date && e.Date <= endDate.Date).ToList();

            IEnumerable<Plan> plans = planRepository.GetPlans(
                e => e.UserId == userId && e.Date >= startDate.Date && e.Date <= endDate.Date).ToList();


            IEnumerable<GoalView> nextGoals = goalRepository.GetGoalsBetweenAndNextGoal(userId, startDate, endDate);

            var model = new CalendarViewModel(monthValue, yearValue);

            model.AllPhases = phaseRepository.GetAll(userId);

            foreach (CalendarWeek week in model.Weeks)
            {
                GoalView nextGoal = nextGoals.FirstOrDefault(g => g.Date >= week.FirstDayInWeek);

                if (nextGoal != null)
                {
                    week.WeekNumberToNextGoal = nextGoal.Date.Subtract(week.FirstDayInWeek).Days / 7;
                }

                week.Phase = weekPlanRepository.GetPhaseForWeek(userId, week.Number, yearValue);

                foreach (CalendarDay day in week.Days)
                {
                    if (day == null)
                        throw new NullReferenceException("Calendar can't be null.");
                    if (nextGoal != null)
                    {
                        day.IsGoalToday = nextGoal.Date == day.Date;
                        if (day.IsGoalToday) day.GoalName = nextGoal.Name;
                    }
                    day.IsDayInMonth = day.Date.Month == monthValue;
                    day.IsDayToday = day.Date.Date == DateTime.Now.Date;
                    var date = day.Date;
                    //Daten für einen Tag holen
                    var exerc = exercises.Where(e => e.DateLocal.Date == date.Date);
                    var plan = plans.Where(p => p.Date.Date == date).ToList();

                    IEnumerable<SessionCalendarView> exSessionCalendarView = MapExercisesToCalendarModel(exerc);
                    IEnumerable<SessionCalendarView> planSessionCalendarView = MapPlansToCalendarModel(plan);


                    if (calendarType == CalendarContentType.Exercise)
                    {
                        day.Sessions.AddRange(exSessionCalendarView);
                    }
                    if (calendarType == CalendarContentType.Plan)
                    {
                        day.Sessions.AddRange(planSessionCalendarView);
                    }

                    week.UpdateSummaryForCurrentWeek(exSessionCalendarView);
                    week.UpdateSummaryForCurrentWeek(planSessionCalendarView);
                }
            }
            return model;
        }

        #endregion

        private IEnumerable<SessionCalendarView> MapExercisesToCalendarModel(IEnumerable<Exercise> exc)
        {
            var list = new List<SessionCalendarView>();
            if (exc.Any())
            {
                //alle Einheiten des Tages müssen zusammengefasst werden
                foreach (Exercise item in exc)
                {
                    var session = new SessionCalendarView
                                      {
                                          SportTypeId = item.SportType.Id,
                                          SportTypeName = item.SportType.Name,
                                          Discipline = Enum.GetName(typeof(Disciplines), item.SportType.Type),
                                          ZoneId = item.ZoneId,
                                          ZoneName =  StringHelper.GetShortname(item.Zone),
                                          Distance = item.Distance,
                                          SessionId = item.Id,
                                          IsExercise = true,
                                          Heartrate = item.Heartrate
                                      };
                   
                    
                    if (item.Duration.HasValue)
                    {
                        session.Duration = item.Duration.Value.Hours * 60 + item.Duration.Value.Minutes;
                    }

                    list.Add(session);
                }
            }
            return list;
        }

        

        private IEnumerable<SessionCalendarView> MapPlansToCalendarModel(IEnumerable<Plan> plan)
        {
            var list = new List<SessionCalendarView>();
            if (plan.Any())
            {
                //alle Einheiten des Tages müssen zusammengefasst werden
                foreach (Plan item in plan)
                {
                    var session = new SessionCalendarView
                                      {
                                          SportTypeId = item.SportType.Id,
                                          SportTypeName = item.SportType.Name,
                                          Discipline = Enum.GetName(typeof(Disciplines), item.SportType.Type),
                                          ZoneId = item.ZoneId,
                                          ZoneName = StringHelper.GetShortname(item.Zone),
                                          SessionId = item.Id,
                                          IsExercise = false
                                      };

                    if (item.Duration.HasValue)
                    {
                        session.Duration = item.Duration.Value.Hours * 60 + item.Duration.Value.Minutes;
                        session.PlannedDuration = (int)item.Duration.Value.TotalMinutes;
                    }

                    if (item.Distance.HasValue)
                    {
                        session.PlannedDistance = item.Distance.Value;
                    }
                    
                    list.Add(session);
                }
            }
            return list;
        }
    }
}