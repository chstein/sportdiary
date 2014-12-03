using System;
using System.Collections.Generic;
using System.Linq;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;

namespace Sporty.Business.Series
{
    public class MetricSeries
    {
        public const string Weight = "Weight";
        public const string RestingPulse = "RestingPulse";
        public const string SleepDuration = "SleepDuration";
        public const string SleepQuality = "SleepQuality";
        public const string StressLevel = "StressLevel";
        public const string Motivation = "Motivation";
        public const string Mood = "Mood";
        public const string Sick = "Sick";
        public const string YesterdaysTraining = "YesterdaysTraining";

        public List<ExercisesPerTimeUnit> GetSeries(List<ExercisesPerTimeUnit> metricsPerDay,
                                                    DateTime currentDate, int daysDiff,
                                                    IEnumerable<MetricView> metricViews)
        {
            var preValues = new Dictionary<string, double>
                                {
                                    {Weight, 0.0},
                                    {RestingPulse, 0.0},
                                    {SleepDuration, 0.0},
                                    {SleepQuality, 0.0},
                                    {StressLevel, 0.0},
                                    {Motivation, 0.0},
                                    {Mood, 0.0},
                                    {Sick, 0.0},
                                    {YesterdaysTraining, 0.0}
                                };


            //var metricNames = new List<string> { "Weight", "RestingPulse", "SleepDuration", "SleepQuality", "StressLevel", "Motivation", "Mood", "Sick", "Yesterdays Training" };
            for (int i = 0; i < daysDiff; i++)
            {
                var perTimeUnit = new ExercisesPerTimeUnit(ReportTypeName.MetricsSingle);
                perTimeUnit.DataPoints.Add(new DataPoint<double>{SportTypeName= Weight});
                perTimeUnit.DataPoints.Add(new DataPoint<double>{SportTypeName= RestingPulse});
                perTimeUnit.DataPoints.Add(new DataPoint<double>{SportTypeName= SleepDuration});
                perTimeUnit.DataPoints.Add(new DataPoint<double> { SportTypeName = SleepQuality });
                perTimeUnit.DataPoints.Add(new DataPoint<double> { SportTypeName = StressLevel });
                perTimeUnit.DataPoints.Add(new DataPoint<double> { SportTypeName = Motivation });
                perTimeUnit.DataPoints.Add(new DataPoint<double> { SportTypeName = Mood });
                perTimeUnit.DataPoints.Add(new DataPoint<double> { SportTypeName = Sick });
                perTimeUnit.DataPoints.Add(new DataPoint<double> { SportTypeName = YesterdaysTraining });

                perTimeUnit.TimeUnitValue = String.Format("{0}.{1}.", currentDate.Day, currentDate.Month);
                IEnumerable<MetricView> metricPerTimeUnit =
                    metricViews.Where(
                        m => m.Date.Day == currentDate.Date.Day && m.Date.Month == currentDate.Date.Month &&
                             m.Date.Year == currentDate.Date.Year);

                if (metricPerTimeUnit.Count() > 0)
                {
                    for (int k = 0; k < metricPerTimeUnit.Count(); k++)
                    {
                        MetricView metric = metricPerTimeUnit.ElementAt(k);
                        //Ziel --> Recovery Index ermitteln.
                        // maximale Werte der Enums = 100%
                        //Mood (3 = 100%, 2= 66, 1=33, 0 = 0, ...) 0.1
                        //Weight (angegebenes Gewicht = 100%) 0.05
                        //Sleep (8h = 100%) mit SQ 0.25
                        //Sleep Qualitity
                        //Puls (angebener Ruhepuls = 100%) 0.15
                        //Krank (6 = 100%, 0 = 0%) 0.15
                        //Stress (1 = 100%, 2 = .. 7 = 0%) 0.1
                        //Motivation 0.1
                        //gestriges Training 0.1

                        DataPoint<double> dps = perTimeUnit.DataPoints.Single(s => s.SportTypeName == Weight);
                        dps.Value = metric.Weight.HasValue ? metric.Weight.Value : 0;
                        if (dps.Value > 0)
                        {
                            dps.Label = dps.Value.ToString("N1");
                            preValues[Weight] = dps.Value;// 76kg = 105%, 80 = 100%, 84kg =95%
                            dps.Value = Math.Round((1 + (1 - (dps.Value/80)))*100*0.05, 1);
                        }
                        else
                        {
                            dps.Label = preValues[Weight].ToString("N1");
                            dps.Value = Math.Round((1 + (1 - (preValues[Weight] / 80))) * 100 * 0.05, 1); 
                        }

                        dps = perTimeUnit.DataPoints.Single(s => s.SportTypeName == RestingPulse);
                        dps.Value = metric.RestingPulse.HasValue ? metric.RestingPulse.Value : 0;
                        if (dps.Value > 0)
                        {
                            //50 = 100%, 45 = 110%, 55% = 90%
                            dps.Label = dps.Value.ToString("N0");
                            dps.Value = Math.Round((1 + (1 - ((dps.Value)/50)))*100*0.15, 1);
                            preValues[RestingPulse] = dps.Value;
                        }
                        else
                        {
                            dps.Label = preValues[RestingPulse].ToString("N0");
                            dps.Value = preValues[RestingPulse];
                        }

                        dps = perTimeUnit.DataPoints.Single(s => s.SportTypeName == SleepDuration);
                        dps.Value = metric.SleepDuration.HasValue ? Convert.ToInt32(metric.SleepDuration.Value) : 0;
                        if (dps.Value > 0)
                        {
                            dps.Label = dps.Value.ToString("N0");
                            dps.Value = Math.Round((dps.Value/8)*100*0.2, 1);
                            preValues[SleepDuration] = dps.Value;
                        }
                        else
                        {
                            dps.Label = preValues[SleepDuration].ToString("N0");
                            dps.Value = preValues[SleepDuration];
                        }

                        dps = perTimeUnit.DataPoints.Single(s => s.SportTypeName == SleepQuality);
                        dps.Label = metric.SleepQuality.HasValue
                                        ? metric.SleepQuality.Value.ToString("N0")
                                        : String.Empty;
                        dps.Value = metric.SleepQuality.HasValue ? metric.SleepQuality.Value : 0;


                        dps = perTimeUnit.DataPoints.Single(s => s.SportTypeName == StressLevel);
                        dps.Value = metric.StressLevel.HasValue ? Convert.ToInt32(metric.StressLevel.Value) : 0;
                        if (dps.Value > 0)
                        {
                            dps.Label = dps.Value.ToString("N0");
                            dps.Value = Math.Round(((8 - dps.Value)/7)*100*0.1, 1);
                            preValues[StressLevel] = dps.Value;
                        }
                        else
                        {
                            dps.Label = preValues[StressLevel].ToString("N0");
                            dps.Value = preValues[StressLevel];
                        }

                        dps = perTimeUnit.DataPoints.Single(s => s.SportTypeName == Motivation);
                        dps.Value = metric.Motivation.HasValue ? Convert.ToInt32(metric.Motivation.Value) : 0;
                        if (dps.Value > 0)
                        {
                            dps.Label = dps.Value.ToString("N0");
                            dps.Value = Math.Round((dps.Value/11)*100*0.1, 1);
                            preValues[Motivation] = dps.Value;
                        }
                        else
                        {
                            dps.Label = preValues[Motivation].ToString("N0");
                            dps.Value = preValues[Motivation];
                        }

                        dps = perTimeUnit.DataPoints.Single(s => s.SportTypeName == Mood);
                        dps.Value = metric.Mood.HasValue ? Convert.ToInt32(metric.Mood.Value) : 0;
                        if (dps.Value > 0)
                        {
                            dps.Label = dps.Value.ToString("N0");
                            dps.Value = Math.Round((dps.Value/3)*100*0.1, 1);
                            preValues[Mood] = dps.Value;
                        }
                        else
                        {
                            dps.Label = preValues[Mood].ToString("N0");
                            dps.Value = preValues[Mood];
                        }

                        dps = perTimeUnit.DataPoints.Single(s => s.SportTypeName == Sick);
                        dps.Value = metric.Sick.HasValue ? Convert.ToInt32(metric.Sick.Value) : 0;
                        if (dps.Value > 0)
                        {
                            dps.Label = dps.Value.ToString("N0");
                            dps.Value = Math.Round((dps.Value/6)*100*0.15, 1);
                            preValues[Sick] = dps.Value;
                        }
                        else
                        {
                            dps.Label = preValues[Sick].ToString("N0");
                            dps.Value = preValues[Sick];
                        }

                        dps = perTimeUnit.DataPoints.Single(s => s.SportTypeName == YesterdaysTraining);
                        dps.Value = metric.YesterdaysTraining.HasValue
                                        ? Convert.ToInt32(metric.YesterdaysTraining.Value)
                                        : 0;
                        if (dps.Value > 0)
                        {
                            dps.Label = dps.Value.ToString("N0");
                            dps.Value = Math.Round((dps.Value/4)*100*0.1, 1);
                            preValues[YesterdaysTraining] = dps.Value;
                        }
                        else
                        {
                            dps.Label = preValues[YesterdaysTraining].ToString("N0");
                            dps.Value = preValues[YesterdaysTraining];
                        }

                        metricsPerDay.Add(perTimeUnit);
                    }
                }
                //AddValueIfIsNotEmpty(metricsPerDay, perTimeUnit);

                currentDate = currentDate.AddDays(1);
            }
            return metricsPerDay;
        }
    }
}