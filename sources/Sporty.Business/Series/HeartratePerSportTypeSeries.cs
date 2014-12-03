using System;
using System.Collections.Generic;
using System.Linq;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;

namespace Sporty.Business.Series
{
    public class HeartratePerSportTypeSeries : ReportSeries
    {
        public override List<ExercisesPerTimeUnit> GetSeries(TimeUnit timeUnit,
                                                             List<ExercisesPerTimeUnit> exercisesPerDay,
                                                             IQueryable<string> sportTypeNames, DateTime currentDate,
                                                             int daysDiff, IEnumerable<ExerciseView> exercises)
        {
            for (int i = 0; i < daysDiff; i++)
            {
                if (timeUnit == TimeUnit.Week) i += 6;

                var perTimeUnit = new ExercisesPerTimeUnit(ReportTypeName.HeartratePerSportType)
                                      {
                                          DataPoints =
                                              sportTypeNames.Select(
                                                  sportTypeName => new DataPoint<double>{SportTypeName= sportTypeName}).
                                              ToList(),
                                          TimeUnitValue = GetTimeUnitText(timeUnit, currentDate),
                                      };
                IEnumerable<ExerciseView> excPerTimeUnit = GetExercisesPerTimeUnit(timeUnit, currentDate, exercises);

                if (excPerTimeUnit.Count() > 0)
                {
                    foreach (ExerciseView exercise in excPerTimeUnit)
                    {
                        DataPoint<double> dps =
                            perTimeUnit.DataPoints.Single(s => s.SportTypeName == exercise.SportTypeName);
                        if (exercise.Heartrate.HasValue)
                        {
                            if (dps.Value == 0)
                            {
                                dps.Value = exercise.Heartrate.Value;
                            }
                            else
                            {
                                dps.Value = (dps.Value + exercise.Heartrate.Value)/2;
                            }
                        }
                        else
                        {
                            dps.Value = 0;
                        }
                    }
                }
                AddValueIfIsNotEmpty(exercisesPerDay, perTimeUnit);

                currentDate = AddTimeUnitToCurrentDate(timeUnit, currentDate);
            }
            return exercisesPerDay;
        }
    }
}