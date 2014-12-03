using System;
using System.Collections.Generic;
using Sporty.DataModel;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;

namespace Sporty.Business.Interfaces
{
    public interface IMetricRepository : IRepository<Metrics>
    {
        IEnumerable<MetricListView> GetMetrics(Guid? userId);
        MetricView GetElement(Guid? userId, int id);
        void Save(Guid userId, MetricView metricView);
        void Delete(Guid userId, int id);
        List<ExercisesPerTimeUnit> GetReportMetrics(Guid? userId, DateTime fromDate, DateTime toDate);
        IEnumerable<MetricListView> GetMetrics(Guid userId, DateTime fromDate, DateTime? toDate);
        Metrics GetLatestMetric(Guid userId);
    }
}