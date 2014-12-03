using System.Collections.Generic;
using Sporty.Business.Series;
using Sporty.ViewModel;

namespace Sporty.Controllers
{
    public class ExerciseDataView
    {
        public ExerciseDataView()
        {
            ChartSeries = new List<ExerciseDataSeries>();
            MapPoints = new List<MapPointsView>();
            LapData = new List<LapDataView>();
        }

        public List<MapPointsView> MapPoints { get; set; }

        public List<ExerciseDataSeries> ChartSeries { get; set; }

        public List<LapDataView> LapData { get; set; }
    }
}