using System;
using DDay.iCal;
using Sporty.ViewModel;

namespace Sporty.Helper
{
    public class CalendarHelpers
    {
        public static Event GoalToEvent(GoalView goal, iCalendar iCal)
        {
            //string eventLink = "http://nrddnr.com/" + dinner.DinnerID;
            var evt = iCal.Create<Event>();
            evt.Start = new iCalDateTime(goal.Date);
            //evt.Duration = goal.;
            //evt.Location = dinner.Address;
            evt.Summary = goal.Name;
            //evt.AddContact(dinner.ContactPhone);
            //evt.Geo = new Geo(dinner.Latitude, dinner.Longitude);
            //evt.Url = eventLink;
            evt.Description = goal.Description;
            return evt;
        }

        public static Event ExerciseToEvent(ExerciseView ev, iCalendar iCal)
        {
            //string eventLink = "http://nrddnr.com/" + dinner.DinnerID;
            var evt = iCal.Create<Event>();
            evt.Start = new iCalDateTime(ev.Date);
            evt.Duration = ev.Duration.Value;
            //evt.Location = dinner.Address;
            evt.Summary = ev.Description;
            //evt.AddContact(dinner.ContactPhone);
            //evt.Geo = new Geo(dinner.Latitude, dinner.Longitude);
            //evt.Url = eventLink;
            evt.Description = ev.Description;
            return evt;
        }

        public static Event PlaneToEvent(PlanView pv, iCalendar iCal)
        {
            //string eventLink = "http://nrddnr.com/" + dinner.DinnerID;
            var evt = iCal.Create<Event>();
            evt.Start = new iCalDateTime(pv.Date);
            evt.Duration = TimeSpan.FromMinutes(pv.PlannedDuration.Value);
            //evt.Location = dinner.Address;
            evt.Summary = pv.Description;
            //evt.AddContact(dinner.ContactPhone);
            //evt.Geo = new Geo(dinner.Latitude, dinner.Longitude);
            //evt.Url = eventLink;
            evt.Description = pv.Description;
            return evt;
        }
    }
}