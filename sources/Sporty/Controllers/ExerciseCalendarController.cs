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
    [Authorize]
    
    public class ExerciseCalendarController : BaseCalendarController
    {
        protected IExerciseRepository exerciseRepository;
        
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            CalendarMode = CalendarContentType.Exercise;
            exerciseRepository = ServiceFactory.Current.Resolve<IExerciseRepository>();
            base.Initialize(controllerContext);
        }
        
        // DELETE api/calendar/5
        public HttpResponseMessage Delete(int id)
        {
            ExerciseDetails exercise = exerciseRepository.GetElement(UserId, id);
            string resultMsg;
            if (exercise != null)
            {
                if (exercise.Attachments != null)
                {
                    foreach (AttachmentView attachmentView in exercise.Attachments)
                    {
                        //if filename exists for other exercise, don't delete
                        if (exerciseRepository.CanDeleteAttachment(attachmentView.Filename))
                        {
                            DeleteFile(attachmentView.Filename);
                        }
                    }
                }
                exerciseRepository.Delete(UserId, id);
                resultMsg =
                    String.Format("<span style='color: red'>{0} Exercise from {1} would have been deleted.</span>",
                                  exercise.SportTypeName, exercise.Date);
            }
            else
            {
                resultMsg = "<span style='color: red'>Exercise was not found.</span>";
            }
            return Request.CreateResponse(HttpStatusCode.OK, exercise);
        }

        [Authorize]
        public HttpResponseMessage UpdateSessionDate(string dayId, int sessionId, bool shouldCopy)
        {
            //string[] idTemp = sessionId.Split("_".ToCharArray());
            string statusMessage = String.Empty;
            int newExerciseId = 0;
            //if (idTemp.Count() > 1 && Int32.TryParse(idTemp[1], out exerciseId))
            //{
            ExerciseDetails exercise = exerciseRepository.GetElement(GetUserId(), sessionId);
            if (exercise != null)
            {
                if (shouldCopy)
                {
                    //TODO Copy all data
                    exercise.Id = 0;

                }
                DateTime newDate;
                if (DateTime.TryParseExact(dayId, "dd_MM_yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                                           out newDate))
                {
                    exercise.Date = new DateTime(newDate.Year, newDate.Month, newDate.Day, exercise.Date.Hour,
                                                 exercise.Date.Minute, exercise.Date.Second);
                    newExerciseId = exerciseRepository.Save(GetUserId(), exercise);
                    statusMessage = "Die Einheit wurde gespeichert.";
                    //isSuccess = true;
                    return Request.CreateResponse(HttpStatusCode.OK, newExerciseId);

                }
            }
            //}
            return Request.CreateResponse(HttpStatusCode.Conflict);

            // return Json(new { success = isSuccess, message = statusMessage, exerciseId = newExerciseId });
        }
    }
}
