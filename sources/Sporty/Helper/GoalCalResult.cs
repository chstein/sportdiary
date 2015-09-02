using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;
using Sporty.ViewModel;

namespace Sporty.Helper
{
    public class GoalCalResult : FileResult
    {
        public GoalCalResult(string filename)
            : base("text/calendar")
        {
            FileDownloadName = filename;
        }

        public GoalCalResult(List<GoalView> goals, string filename)
            : this(filename)
        {
            Goals = goals;
        }

        public GoalCalResult(GoalView goal, string filename)
            : this(filename)
        {
            Goals = new List<GoalView>();
            Goals.Add(goal);
        }

        public List<GoalView> Goals { get; set; }

        protected override void WriteFile(HttpResponseBase response)
        {
            var iCal = new iCalendar();
            foreach (GoalView d in Goals)
            {
                try
                {
                    Event e = CalendarHelpers.GoalToEvent(d, iCal);
                    iCal.Events.Add(e);
                }
                catch (ArgumentOutOfRangeException)
                {
                    //Swallow folks that have dinners in 9999. 
                }
            }

            var serializer = new iCalendarSerializer(iCal);
            string result = serializer.SerializeToString();
            response.ContentEncoding = Encoding.UTF8;
            response.Write(result);
        }
    }
}