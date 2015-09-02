using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Sporty.ViewModel;

namespace Sporty.ViewModel
{
    public class PlanDetailsView
    {
        public int Id { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Eine Dauer muss sein")]
        //[RegularExpression(@"^((0?[1-9]|1[012])(:[0-5]\d){0,2}(\ [AP]M))$|^([01]\d|2[0-3])(:[0-5]\d){0,2}$")]
        public TimeSpan? Duration { get; set; }

        public double? Distance { get; set; }
        
        [Required(ErrorMessage = "Ein Datum fehlt.")]
        public DateTime Date { get; set; }

        public string SportTypeName { get; set; }
        public string ZoneName { get; set; }
        public int? Heartrate { get; set; }

        public string TrainingTypeName { get; set; }

        public int SportTypeId { get; set; }
        public int? ZoneId { get; set; }
        public int? TrainingTypeId { get; set; }

        public SelectList SportTypes { get; set; }
        public SelectList Zones { get; set; }
        public SelectList TrainingTypes { get; set; }

        public bool IsCopy { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsNew { get; set; }

        public PlanDetailsView()
        {
            
        }

        public PlanDetailsView(IEnumerable<SportTypeView> allSportTypes, 
            IEnumerable<ZoneView> allZones, IEnumerable<TrainingTypeView> allTrainingTypes)
        {
            SportTypes = SportTypeId > 0 ? new SelectList(allSportTypes, "Id", "Name", SportTypeId) : new SelectList(allSportTypes, "Id", "Name");

            Zones = ZoneId.HasValue ? new SelectList(allZones, "Id", "Name", ZoneId) : new SelectList(allZones, "Id", "Name");

            TrainingTypes = TrainingTypeId.HasValue ? new SelectList(allTrainingTypes, "Id", "Name", TrainingTypeId) : new SelectList(allTrainingTypes, "Id", "Name");
        }
    }
}
