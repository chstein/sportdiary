using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Sporty.ViewModel
{
    public class ExerciseDetailsView
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public TimeSpan? Duration { get; set; }

        public double? Distance { get; set; }
        public double? Speed { get; set; }

        public DateTime Date { get; set; }

        public string Time { get; set; }
        public string SportTypeName { get; set; }
        public string ZoneName { get; set; }
        public int? Heartrate { get; set; }

        public string TrainingTypeName { get; set; }

        public int SportTypeId { get; set; }
        public int? ZoneId { get; set; }
        public int? TrainingTypeId { get; set; }

        public IEnumerable<AttachmentView> Attachments { get; set; }

        public List<MaterialView> UsedMaterials { get; set; }

        public int? Trimp { get; set; }

        public IEnumerable<int> SelectedMaterials { get; set; }

        public double Pace { get; set; }

        public int? Cadence { get; set; }

        public int? HeartrateMax { get; set; }

        public double? SpeedMax { get; set; }

        public int? CadenceMax { get; set; }
    }
}
