using System;
using System.Collections.Generic;
using System.Linq;
using Sporty.Common;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;

namespace Sporty.Business.Series
{
    public abstract class ReportSeries
    {
        public abstract List<ExercisesPerTimeUnit> GetSeries(TimeUnit timeUnit,
                                                             List<ExercisesPerTimeUnit> exercisesPerDay,
                                                             IQueryable<string> sportTypeNames, DateTime currentDate,
                                                             int daysDiff, IEnumerable<ExerciseView> exercises);

        protected string GetTimeUnitText(TimeUnit timeUnit, DateTime currentDate)
        {
            if (timeUnit == TimeUnit.Week)
            {
                return String.Format("KW {0}", DateHelper.GetWeekNumber(currentDate));
            }
            return String.Format("{0}.{1}.", currentDate.Day, currentDate.Month);
        }

        protected IEnumerable<ExerciseView> GetExercisesPerTimeUnit(TimeUnit timeUnit, DateTime currentDate,
                                                                    IEnumerable<ExerciseView> exercises)
        {
            IEnumerable<ExerciseView> excPerTimeUnit;
            if (timeUnit == TimeUnit.Week)
            {
                DateTime startDate = DateHelper.GetFirstDayInWeek(currentDate);
                DateTime endDate = DateHelper.GetLastDayInWeek(currentDate);
                excPerTimeUnit =
                    exercises.Where(
                        e =>
                        e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date);
            }
            else
            {
                excPerTimeUnit =
                    exercises.Where(
                        e =>
                        e.Date.Day == currentDate.Date.Day && e.Date.Month == currentDate.Date.Month &&
                        e.Date.Year == currentDate.Date.Year);
            }
            return excPerTimeUnit;
        }

        protected DateTime AddTimeUnitToCurrentDate(TimeUnit timeUnit, DateTime currentDate)
        {
            if (timeUnit == TimeUnit.Week)
                currentDate = currentDate.AddDays(7);
            else
                currentDate = currentDate.AddDays(1);
            return currentDate;
        }

        protected void AddValueIfIsNotEmpty(List<ExercisesPerTimeUnit> exercisesPerDay, ExercisesPerTimeUnit perTimeUnit)
        {
            if (perTimeUnit.DataPoints.Sum(s => s.Value) > 0)
            {
                exercisesPerDay.Add(perTimeUnit);
            }
        }
    }
}