using Newtonsoft.Json.Linq;
using Sporty.Business;
using Sporty.Business.Interfaces;
using Sporty.Common;
using Sporty.Helper;
using Sporty.Infrastructure;
using Sporty.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Sporty.Controllers
{
    //[HandleErrorWithElmah]
    [Authorize]
    public abstract class BaseCalendarController : ApiController
    {
        protected Guid? UserId;
        protected IUserRepository UserRepository;

        protected ICalendarService calendarService;
        protected ISportTypeRepository sportTypeRepository;
        protected ITrainingTypeRepository trainingTypeRepository;
        protected IWeekPlanRepository weekPlanRepository;
        protected IZoneRepository zoneRepository;
        protected IMaterialRepository materialRepository;

        public CalendarContentType CalendarMode;

        protected Guid? GetUserId()
        {
            if (!UserId.HasValue)
            {
                UserId = UserRepository.FindUserId(User.Identity.Name);
            }
            return UserId;
        }

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            UserRepository = ServiceFactory.Current.Resolve<IUserRepository>();
            UserId = GetUserId();

            sportTypeRepository = ServiceFactory.Current.Resolve<ISportTypeRepository>();
            //}
            zoneRepository = ServiceFactory.Current.Resolve<IZoneRepository>();
            trainingTypeRepository = ServiceFactory.Current.Resolve<ITrainingTypeRepository>();
            weekPlanRepository = ServiceFactory.Current.Resolve<IWeekPlanRepository>();
            calendarService = ServiceFactory.Current.Resolve<ICalendarService>();
            materialRepository = ServiceFactory.Current.Resolve<IMaterialRepository>();

            base.Initialize(controllerContext);
        }
        public CalendarViewModel GetCalendar()
        {
            return GetCalendar(null, null);
        }

        // GET api/calendar
        public virtual CalendarViewModel GetCalendar(int? month, int? year)
        {
            DateTime currentDate = GetDateFromSessionOrToday();

            int monthValue = month.HasValue ? month.Value : currentDate.Month;

            int yearValue = year.HasValue ? year.Value : currentDate.Year;

            if (month.HasValue && year.HasValue)
            {
                //Http["startDate"] = new DateTime(year.Value, month.Value, 1);
            }

            CalendarViewModel model = calendarService.GetViewModel(monthValue, yearValue, UserId.Value, CalendarMode );

            model.PublicICalUrl = UserRepository.GetSsoToken(UserId);
            return model;
        }

        // PUT api/calendar/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }



        public void DeleteFile(string filename)
        {
            string filePathAndName = GetAttachmentFilePathAndName(filename, GetUserId().Value);

            if (System.IO.File.Exists(filePathAndName))
            {
                try
                {
                    System.IO.File.Delete(filePathAndName);
                }
                catch (Exception)
                {
                    //TODO log 
                }
            }
        }

        public string GetAttachmentFilePathAndName(string filename, Guid userId)
        {
            string rootFolder = AppConfigHelper.GetWebConfigValue(Constants.RootUploadFolder);

            string fileName = Path.GetFileName(filename).ToLower();
            string filePathAndName = Path.Combine(rootFolder, userId.ToString(), fileName);
            return filePathAndName;
        }

        protected DateTime GetDateFromSessionOrToday()
        {
            DateTime currentDate = DateTime.Today;
            //var dateInSession = Session["startDate"] as DateTime?;
            //if (dateInSession.HasValue)
            //{
            //    currentDate = dateInSession.Value;
            //}
            return currentDate;
        }
    }
}
