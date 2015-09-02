using System;
using System.Collections;
using System.Collections.Generic;

namespace Sporty.ViewModel
{
    public class UserProfileView
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
        public double? BodyHeight { get; set; }

        public bool SendMetricsMail { get; set; } 
        public TimeSpan? DailyMetricsMailSendingTime { get; set;} 

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordRepeat { get; set; }

        public List<SportTypeView> SportTypes { get; set; }

        public List<TrainingTypeView> TrainingTypes { get; set; }
        public List<ZoneView> Zones { get; set; }

        public Dictionary<string, string> AllZones { get; set; }

        public Dictionary<Disciplines, string> AllDisciplines { get; set; }

        public List<PhaseView> Phases { get; set; }

        public int? MaxHeartrate { get; set; }

        public UserProfileView()
        {
            SportTypes = new List<SportTypeView>();
            TrainingTypes = new List<TrainingTypeView>();
            Zones = new List<ZoneView>();
            Phases = new List<PhaseView>();
        }
    }
}
