using System;
using System.Collections.Generic;
using System.Linq;
using Sporty.Business.Interfaces;
using Sporty.Business.Series;
using Sporty.DataModel;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;

namespace Sporty.Business.Repositories
{
    public class MetricRepository : BaseRepository<Metrics>, IMetricRepository
    {
        public MetricRepository(SportyEntities context)
            : base(context)
        {
        }

        #region IMetricRepository Members

        public IEnumerable<MetricListView> GetMetrics(Guid? userId)
        {
            IQueryable<Metrics> metricList = context.Metrics.Where(m => m.UserId == userId);
            return metricList.Select(item => GetMetricListView(item)).ToList();
        }

        public MetricView GetElement(Guid? userId, int id)
        {
            var item = this.context.Metrics.FirstOrDefault(m => m.Id == id && m.User.UserId == userId);
            return item != null ? GetMetricView(item) : null;
        }

        public void Save(Guid userId, MetricView item)
        {
            var metric = item.Id > 0
                                ? this.context.Metrics.FirstOrDefault(e => e.Id == item.Id && e.User.UserId == userId)
                                : new Metrics {Id = item.Id};

            metric.Description = item.Description;
            metric.Weight = item.Weight;
            metric.Mood = item.Mood > 0 ? item.Mood : null;
            metric.Motivation = item.Motivation > 0 ? item.Motivation : null;
            metric.RestingPulse = item.RestingPulse;
            metric.Sick = item.Sick > 0 ? item.Sick : null;
            metric.SleepDuration = item.SleepDuration;
            metric.SleepQuality = item.SleepQuality > 0 ? item.SleepQuality : null;
            metric.StressLevel = item.StressLevel > 0 ? item.StressLevel : null;
            metric.YesterdaysTraining = item.YesterdaysTraining > 0 ? item.YesterdaysTraining : null;
            metric.Date = item.Date;
            metric.UserId = userId;

            if (metric.Id > 0)
                Update();
            else
                this.Add(metric);
        }

        public void Delete(Guid userId, int id)
        {
           Delete(g => g.Id == id && g.User.UserId == userId);
        }

        public List<ExercisesPerTimeUnit> GetReportMetrics(Guid? userId, DateTime fromDate, DateTime toDate)
        {
            if (!userId.HasValue) return null;
            IQueryable<Metrics> metricList =
                context.Metrics.Where(e => e.Date >= fromDate.Date && e.Date <= toDate.Date && e.UserId == userId);


            IEnumerable<MetricView> metrics = GetMetricViewList(metricList);
            var exercisesPerDay = new List<ExercisesPerTimeUnit>();
            int daysDiff = toDate.Subtract(fromDate).Days;
            DateTime currentDate = fromDate;

            var metricseries = new MetricSeries();

            return metricseries.GetSeries(exercisesPerDay, currentDate, daysDiff, metrics);
        }

        public IEnumerable<MetricListView> GetMetrics(Guid userId, DateTime fromDate, DateTime? toDate)
        {
            var metrics = toDate.HasValue
                                             ? context.Metrics.Where(
                                                 e =>
                                                 e.Date >= fromDate && e.Date <= toDate.Value &&
                                                 e.UserId == userId).ToList()
                                             : context.Metrics.Where(
                                                 e => e.Date >= fromDate && e.UserId == userId).ToList();
            return metrics.Select(item => GetMetricListView(item));
            ;
        }

        public Metrics GetLatestMetric(Guid userId)
        {
            var metric = new Metrics();
            Metrics latestRestingPulse = context.Metrics.OrderBy(m => m.Date).FirstOrDefault(n => n.RestingPulse.HasValue);
            if (latestRestingPulse != null)
            {
                metric.RestingPulse = latestRestingPulse.RestingPulse;
            }
            return metric;
        }

        #endregion

        private MetricListView GetMetricListView(Metrics item)
        {
            return new MetricListView
                       {
                           Id = item.Id,
                           Description = item.Description,
                           Weight = item.Weight,
                           Mood = item.Mood != null ? MetricNameBag.MoodData[(int) item.Mood] : null,
                           Motivation =
                               item.Motivation != null ? MetricNameBag.MotivationData[(int) item.Motivation] : null,
                           RestingPulse = item.RestingPulse,
                           Sick = item.Sick != null ? MetricNameBag.SickData[(int) item.Sick] : null,
                           SleepDuration = item.SleepDuration,
                           SleepQuality =
                               item.SleepQuality != null
                                   ? MetricNameBag.SleepQualityData[(int) item.SleepQuality]
                                   : null,
                           StressLevel =
                               item.StressLevel != null ? MetricNameBag.StressLevelData[(int) item.StressLevel] : null,
                           YesterdaysTraining =
                               item.YesterdaysTraining != null
                                   ? MetricNameBag.YesterdaysTrainingData[(int) item.YesterdaysTraining]
                                   : null,
                           Date = item.Date
                       };
        }

        private IEnumerable<MetricView> GetMetricViewList(IEnumerable<Metrics> exerciseList)
        {
            return exerciseList.Select(GetMetricView).ToList();
        }

        private MetricView GetMetricView(Metrics item)
        {
            return new MetricView
                       {
                           Id = item.Id,
                           Description = item.Description,
                           Weight = item.Weight,
                           Mood = item.Mood,
                           Motivation = item.Motivation,
                           RestingPulse = item.RestingPulse,
                           Sick = item.Sick,
                           SleepDuration = item.SleepDuration,
                           SleepQuality = item.SleepQuality,
                           StressLevel = item.StressLevel,
                           YesterdaysTraining = item.YesterdaysTraining,
                           Date = item.Date
                       };
        }
    }
}