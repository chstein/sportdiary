using System.Collections.Generic;

namespace Sporty.Business.IO.Tcx
{
    public class Lap
    {
        public double TotalTimeSeconds { set; get; }

        public double DistanceMeters { set; get; }

        public double MaximumSpeed { set; get; }

        public int Calories { set; get; }

        public string TriggerMethod { set; get; }

        public int AverageHeartRateBpm { set; get; }

        public int MaximumHeartRateBpm { set; get; }

        public string Intensity { set; get; }

        public int Cadence { set; get; }

        public string Notes { set; get; }

        public List<Track> Tracks { set; get; }
    }
}