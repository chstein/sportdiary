using System;
using System.Collections.Generic;
using System.Linq;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;

namespace Sporty.Business.Series
{
    public class DurationAllSportTypeSeries : ReportSeries
    {
        public override List<ExercisesPerTimeUnit> GetSeries(TimeUnit timeUnit,
                                                             List<ExercisesPerTimeUnit> exercisesPerDay,
                                                             IQueryable<string> sportTypeNames, DateTime currentDate,
                                                             int daysDiff, IEnumerable<ExerciseView> exercises)
        {
            for (int i = 0; i < daysDiff; i++)
            {
                if (timeUnit == TimeUnit.Week) i += 6;

                var perTimeUnit = new ExercisesPerTimeUnit(ReportTypeName.DurationSummary)
                                      {
                                          DataPoints = new List<DataPoint<double>> { new DataPoint<double> { SportTypeName = "All" 
                                          } },
                                          TimeUnitValue = GetTimeUnitText(timeUnit, currentDate)
                                      };
                IEnumerable<ExerciseView> excPerTimeUnit = GetExercisesPerTimeUnit(timeUnit, currentDate, exercises);

                if (excPerTimeUnit.Count() > 0)
                {
                    foreach (ExerciseView exercise in excPerTimeUnit)
                    {
                        perTimeUnit.DataPoints[0].Value += exercise.Duration.HasValue
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