using Sporty.Business.Interfaces;
using Sporty.Common;
using Sporty.Infrastructure;
using Sporty.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Sporty.Controllers
{
    public class PlanCalendarController : BaseCalendarController
    {
        protected IPlanRepository planRepository;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            CalendarMode = CalendarContentType.Plan;
            planRepository = ServiceFactory.Current.Resolve<IPlanRepository>();
            base.Initialize(controllerContext);
        }

        //get favs
        public override CalendarViewModel GetCalendar(int? month, int? year)
        {
            var model = base.GetCalendar(month, year);
            model.Favorites = planRepository.GetFavoritePlans(UserId);
            return model;
        }

        // DELETE api/calendar/5
        public HttpResponseMessage Delete(int id)
        {
            var plan = planRepository.GetElement(UserId, id);
            string resultMsg;
            if (plan != null)
            {
                planRepository.Delete(UserId, id);
                resultMsg =
                    String.Format("<span style='color: red'>{0} Exercise from {1} would have been deleted.</span>",
                                  plan.SportTypeName, plan.Date);
            }
            else
            {
                resultMsg = "<span style='color: red'>Exercise was not found.</span>";
            }
            return Request.CreateResponse(HttpStatusCode.OK, plan);
        }

        [Authorize]
        public HttpResponseMessage UpdateSessionDate(string dayId, int sessionId, bool shouldCopy)
        {
            //string[] idTemp = sessionId.Split("_".ToCharArray());
            string statusMessage = String.Empty;
            int newExerciseId = 0;
            //if (idTemp.Count() > 1 && Int32.TryParse(idTemp[1], out exerciseId))
            //{
            var plan = planRepository.GetElement(GetUserId(), sessionId);
            if (plan != null)
            {
                if (shouldCopy)
                {
                    //TODO Copy all data
                    plan.Id = 0;

                }
                DateTime newDate;
                if (DateTime.TryParseExact(dayId, "dd_MM_yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                                           out newDate))
                {
                    plan.Date = new DateTime(newDate.Year, newDate.Month, newDate.Day, plan.Date.Hour,
                                                 plan.Date.Minute, plan.Date.Second);
                    newExerciseId = planRepository.Save(GetUserId(), plan);
                    statusMessage = "Die Einheit wurde gespeichert.";
                    //isSuccess = true;
                    return Request.CreateResponse(HttpStatusCode.OK, newExerciseId);

                }
            }
            //}
            return Request.CreateResponse(HttpStatusCode.Conflict);

            // return Json(new { success = isSuccess, message = statusMessage, exerciseId = newExerciseId });
        }

        public HttpResponseMessage UpdateFavorite(int id)
        {
            PlanDetailsView plan = planRepository.GetElement(UserId, id);

            if (plan == null) 
                return Request.CreateResponse(HttpStatusCode.Conflict);

            plan.IsFavorite = !plan.IsFavorite;
            planRepository.Save(UserId, plan);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
