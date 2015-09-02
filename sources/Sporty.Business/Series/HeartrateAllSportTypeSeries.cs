using System;
using System.Collections.Generic;
using System.Linq;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;

namespace Sporty.Business.Series
{
    public class HeartrateAllSportTypeSeries : ReportSeries
    {
        public override List<ExercisesPerTimeUnit> GetSeries(TimeUnit timeUnit,
                                                             List<ExercisesPerTimeUnit> exercisesPerDay,
                                                             IQueryable<string> sportTypeNames, DateTime currentDate,
                                                             int daysDiff, IEnumerable<ExerciseView> exercises)
        {
            for (int i = 0; i < daysDiff; i++)
            {
                if (timeUnit == TimeUnit.Week) i += 6;

                var perTimeUnit = new ExercisesPerTimeUnit(ReportTypeName.HeartrateSummary)
                                      {
                                          DataPoints = new List<DataPoint<double>> {new DataPoint<double>{ SportTypeName = "All"}},
                                          TimeUnitValue = GetTimeUnitText(timeUnit, currentDate),
                                      };
                IEnumerable<ExerciseView> excPerTimeUnit = GetExercisesPerTimeUnit(timeUnit, currentDate, exercises);

                if (excPerTimeUnit.Count() > 0)
                {
                    var valueList = new List<int>();
                    foreach (ExerciseView exercise in excPerTimeUnit)
                    {
                        if (exercise.Heartrate.HasValue && exercise.Heartrate.Value != 0)
                        {
                            valueList.Add(exercise.Heartrate.Value);
                        }
                    }
                    if (valueList.Count > 0)
                    {
                        perTimeUnit.DataPoints[0].Value = (int) valueList.Average();
                    }
                }
                AddValueIfIsNotEmpty(exercisesPerDay, perTimeUnit);

                currentDate = AddTimeUnitToCurrentDate(timeUnit, currentDate);
            }
            return exercisesPerDay;
        }
    }
}