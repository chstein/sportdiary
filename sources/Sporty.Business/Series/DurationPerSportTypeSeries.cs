using System;
using System.Collections.Generic;
using System.Linq;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;

namespace Sporty.Business.Series
{
    public class DurationPerSportTypeSeries : ReportSeries
    {
        public override List<ExercisesPerTimeUnit> GetSeries(TimeUnit timeUnit,
                                                             List<ExercisesPerTimeUnit> exercisesPerDay,
                                                             IQueryable<string> sportTypeNames, DateTime currentDate,
                                                             int daysDiff, IEnumerable<ExerciseView> exercises)
        {
            for (int i = 0; i < daysDiff; i++)
            {
                if (timeUnit == TimeUnit.Week) i += 6;

                var perTimeUnit = new ExercisesPerTimeUnit(ReportTypeName.DurationPerSportType)
                                      {
                                          DataPoints =
                                              sportTypeNames.Select(
                                                  sportTypeName => new DataPoint<double> {SportTypeName = sportTypeName}).
                                              ToList(),
                                          TimeUnitValue = GetTimeUnitText(timeUnit, currentDate),
                                      };
                IEnumerable<ExerciseView> excPerTimeUnit = GetExercisesPerTimeUnit(timeUnit, currentDate, exercises);

                if (excPerTimeUnit.Any())
                {
                    foreach (ExerciseView exercise in excPerTimeUnit)
                    {
                        DataPoint<double> dps =
                            perTimeUnit.DataPoints.Single(s => s.SportTypeName == exercise.SportTypeName);
                        dps.Value += exercise.Duration.HasValue
                                         ? Convert.ToInt32(exercise.Duration.Value.TotalMinutes)
                                         : 0;
                    }
                }
                AddValueIfIsNotEmpty(exercisesPerDay, perTimeUnit);

                currentDate = AddTimeUnitToCurrentDate(timeUnit, currentDate);
            }
            return exercisesPerDay;
        }
    }
}