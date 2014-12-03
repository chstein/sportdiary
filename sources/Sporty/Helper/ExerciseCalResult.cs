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
    public class ExerciseCalResult : FileResult
    {
        public ExerciseCalResult(string filename)
            : base("text/calendar")
        {
            FileDownloadName = filename;
        }

        public ExerciseCalResult(List<ExerciseView> exerciseViews, string filename)
            : this(filename)
        {
            ExerciseViews = exerciseViews;
        }

        public ExerciseCalResult(ExerciseView exerciseView, string filename)
            : this(filename)
        {
            ExerciseViews = new List<ExerciseView>();
            ExerciseViews.Add(exerciseView);
        }

        public List<ExerciseView> ExerciseViews { get; set; }

        protected override void WriteFile(HttpResponseBase response)
        {
            var iCal = new iCalendar();
            foreach (ExerciseView ev in ExerciseViews)
            {
                try
                {
                    Event e = CalendarHelpers.ExerciseToEvent(ev, iCal);
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