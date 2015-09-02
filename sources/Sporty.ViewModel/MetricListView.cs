using System;

namespace Sporty.ViewModel
{
    public class MetricListView
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public double? Weight { get; set; }

        public int? RestingPulse { get; set; }

        public double? SleepDuration { get; set; }

        public string SleepQuality { get; set; }

        public string StressLevel { get; set; }

        public string Motivation { get; set; }

        public string Mood { get; set; }

        public string Sick { get; set; }

        public string Description { get; set; }

        public string YesterdaysTraining { get; set; }

        
    }
}