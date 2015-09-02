namespace Sporty.ViewModel
{
    public class LapDataView
    {
        public double TotalTimeSeconds { set; get; }

        public double DistanceMeters { set; get; }

        public double MaximumSpeed { set; get; }

        public double AveragePace { get; set; }

        public double MaximumPace { get; set; }

        public int Calories { set; get; }

        public int AverageHeartRateBpm { set; get; }

        public int MaximumHeartRateBpm { set; get; }

        public string Intensity { set; get; }

        public int Cadence { set; get; }
    }
}