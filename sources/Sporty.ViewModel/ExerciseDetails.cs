using Sporty.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Sporty.ViewModel
{
    public class ExerciseDetails
    {
        public int Id { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Eine Dauer muss sein")]
        [RegularExpression(@"^((0?[1-9]|1[012])(:[0-5]\d){0,2}(\ [AP]M))$|^([01]\d|2[0-3])(:[0-5]\d){0,2}$")]
        public TimeSpan? Duration { get; set; }

        public double? Distance { get; set; }
        public double? Speed { get; set; }

        [Required(ErrorMessage = "Ein Datum fehlt.")]
        public DateTime Date { get; set; }

        public string Time { get; set; }
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

        public IEnumerable<AttachmentView> Attachments { get; set; }

        public IEnumerable<MaterialView> Materials { get; set; }
        public IEnumerable<int> SelectedMaterialIds { get; set; }

        public int? Trimp { get; set; }

        public int? Cadence { get; set; }

        public int? HeartrateMax { get; set; }

        public double? SpeedMax { get; set; }

        public int? CadenceMax { get; set; }

        public bool IsNew { get; set; }

        public string PublicLink { get; set; }

        public double? Temperature { get; set; }

        public List<WeatherCondition> WeatherCondition { get; set; }

        public string SelectedWeatherCondition { get; set; }

        public string WeatherNote { get; set; }

        public ExerciseDetails()
        {
            
        }

        public ExerciseDetails(IEnumerable<SportTypeView> allSportTypes,
            IEnumerable<ZoneView> allZones, IEnumerable<TrainingTypeView> allTrainingTypes)
        {
            SportTypes = SportTypeId > 0 ? new SelectList(allSportTypes, "Id", "Name", SportTypeId) : new SelectList(allSportTypes, "Id", "Name");

            Zones = ZoneId.HasValue ? new SelectList(allZones, "Id", "Name", ZoneId) : new SelectList(allZones, "Id", "Name");

            TrainingTypes = TrainingTypeId.HasValue ? new SelectList(allTrainingTypes, "Id", "Name", TrainingTypeId) : new SelectList(allTrainingTypes, "Id", "Name");

            WeatherCondition = Constants.AllWeatherConditions;
            if (!string.IsNullOrEmpty(SelectedWeatherCondition))
            {
                var selected = WeatherCondition.SingleOrDefault(wc => wc.SelectValue.Equals(SelectedWeatherCondition, StringComparison.CurrentCultureIgnoreCase));
                if (selected != null)
                {
                    selected.IsSelectedWeatherCondition = true;
                }
            }
            //!string.IsNullOrEmpty(SelectedWeatherCondition) ?
            //    new List<WeatherCondition>(Constants.AllWeatherConditions, "Key", "Value", SelectedWeatherCondition) :
            //    new SelectList(Constants.AllWeatherConditions, "Key", "Value");

        }










        public double Pace { get; set; }
    }
}
