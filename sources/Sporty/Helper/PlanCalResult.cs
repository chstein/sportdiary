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
    public class PlanCalResult : FileResult
    {
        public PlanCalResult(string filename)
            : base("text/calendar")
        {
            FileDownloadName = filename;
        }

        public PlanCalResult(List<PlanView> planViews, string filename)
            : this(filename)
        {
            PlanViews = planViews;
        }

        public PlanCalResult(PlanView planView, string filename)
            : this(filename)
        {
            PlanViews = new List<PlanView>();
            PlanViews.Add(planView);
        }

        public List<PlanView> PlanViews { get; set; }

        protected override void WriteFile(HttpResponseBase response)
        {
            var iCal = new iCalendar();
            foreach (PlanView ev in PlanViews)
            {
                try
                {
                    Event e = CalendarHelpers.PlaneToEvent(ev, iCal);
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