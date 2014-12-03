using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sporty.ViewModel
{
    public class PlanView
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string SportTypeName { get; set; }
        public string ZoneName { get; set; }
        public int ZoneId { get; set; }
        public double? Distance { get; set; }
        public string TrainingTypeName { get; set; }
        public string Description { get; set; }

        public string Discipline { get; set; }

        public int SportTypeId { get; set; }

        public int? PlannedDuration { get; set; }
    }
}
