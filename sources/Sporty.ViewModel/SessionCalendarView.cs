using System;

namespace Sporty.ViewModel
{
    public class SessionCalendarView
    {
        public int SessionId { get; set; }
        public string SportTypeName { get; set; }
        public int SportTypeId { get; set; }
        public int? Duration { get; set; }
        public int PlannedDuration { get; set; }
        public double? Distance { get; set; }

        public int? ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int? Heartrate { get; set; }
        public bool IsFavorite { get; set; }

        public string Discipline { get; set; }

        public double PlannedDistance { get; set; }

        public bool IsExercise { get; set; }
    }
}
