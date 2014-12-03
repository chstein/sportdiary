using System;

namespace Sporty.ViewModel
{
    public class ExerciseView
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string SportTypeName { get; set; }
        public string ZoneName { get; set; }
        public TimeSpan? Duration { get; set; }
        public double? Distance { get; set; }
        public string TrainingTypeName { get; set; }
        public string Description { get; set; }
        public int? Heartrate { get; set; }
        public bool HasAttachment { get; set; }

        public double? Speed { get; set; }

        public int? Trimp { get; set; }

        public int? Cadence { get; set; }

        public int? HeartrateMax { get; set; }

        public int? CadenceMax { get; set; }

        public double? SpeedMax { get; set; }
    }
}