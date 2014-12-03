using System;
using System.Collections.Generic;
using System.Linq;
using Sporty.Business.Helper;
using Sporty.Business.Series;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;

namespace Sporty.Business.Series
{
    public class TrainingAllSportTypeSeries : ReportSeries
    {
        private readonly CalcHelper calcHelper;
        private const int na = 7;
        public const int LongTerm = 42;

        public TrainingAllSportTypeSeries(CalcHelper calcHelper)
        {
            this.calcHelper = calcHelper;
        }

        public override List<ExercisesPerTimeUnit> GetSeries(TimeUnit timeUnit,
                                                             List<ExercisesPerTimeUnit> exercisesPerDay,
                                                             IQueryable<string> sportTypeNames, DateTime currentDate,
                                                             int daysDiff, IEnumerable<ExerciseView> exercises)
        {
            var preAtl = 0.0;
            double deltaA = 2 / (double)(na + 1);
            var preCtl = 0.0;
            double deltaC = 2 / (double)(LongTerm + 1);

            var calculationStart = 0 - LongTerm;
            for (int i = calculationStart; i < daysDiff; i++)
            {
                if (timeUnit == TimeUnit.Week) i += 6;

                var perTimeUnit = new ExercisesPerTimeUnit(ReportTypeName.Training)
                                      {
                                          DataPoints = new List<DataPoint<double>> { new DataPoint<double>{SportTypeName="Trimp"}, 
                                              new DataPoint<double>{SportTypeName="ATL"}, 
                                              new DataPoint<double>{SportTypeName="CTL"},
                                          new DataPoint<double>{SportTypeName="TSB"}},
                                          TimeUnitValue = GetTimeUnitText(timeUnit, currentDate),
                                      };
                IEnumerable<ExerciseView> excPerTimeUnit = GetExercisesPerTimeUnit(timeUnit, currentDate, exercises);
                var trimpPerDay = 0.0;

                if (excPerTimeUnit.Any())
                {
                    foreach (ExerciseView exercise in excPerTimeUnit)
                    {
                        perTimeUnit.DataPoints[0].Value += exercise.Trimp.HasValue
                                                               ? Convert.ToInt32(exercise.Trimp.Value)
                                                               : calcHelper.CalculateTrimp(exercise);
                        trimpPerDay += perTimeUnit.DataPoints[0].Value;
                    }

                }

                //ATLtoday = TRIMP * λa * ka + ((1 – λa) * ATLyesterday
                //λa = 2/(Na+1)
                var atl = trimpPerDay * deltaA + ((1 - deltaA) * preAtl);
                preAtl = atl;
                perTimeUnit.DataPoints[1].Value = Math.Round(atl, 1);

                var ctl = trimpPerDay * deltaC + ((1 - deltaC) * preCtl);
                preCtl = ctl;
                perTimeUnit.DataPoints[2].Value = Math.Round(ctl, 1);

                perTimeUnit.DataPoints[3].Value = Math.Round(ctl - atl, 1);


                //AddValueIfIsNotEmpty(exercisesPerDay, perTimeUnit);
                if (i >= 0)
                {
                    exercisesPerDay.Add(perTimeUnit);
                }

                currentDate = AddTimeUnitToCurrentDate(timeUnit, currentDate);
            }
            return exercisesPerDay;
        }
    }
}