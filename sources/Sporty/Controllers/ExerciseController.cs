using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using Sporty.Business;
using Sporty.Business.IO;
using Sporty.Business.IO.Tcx;
using Sporty.Business.Interfaces;
using Sporty.Business.Series;
using Sporty.Common;
using Sporty.Infrastructure;
using Sporty.ViewModel;

namespace Sporty.Controllers
{
    public class ExerciseController : BaseController
    {
        private ICalendarService calendarService;
        private IExerciseRepository exerciseRepository;
        private ISportTypeRepository sportTypeRepository;
        private ITrainingTypeRepository trainingTypeRepository;
        private IWeekPlanRepository weekPlanRepository;
        private IZoneRepository zoneRepository;
        private IMaterialRepository materialRepository;
        
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //if (UserId.HasValue)
            //{
            exerciseRepository = ServiceFactory.Current.Resolve<IExerciseRepository>();
            sportTypeRepository = ServiceFactory.Current.Resolve<ISportTypeRepository>();
            //}
            zoneRepository = ServiceFactory.Current.Resolve<IZoneRepository>();
            trainingTypeRepository = ServiceFactory.Current.Resolve<ITrainingTypeRepository>();
            weekPlanRepository = ServiceFactory.Current.Resolve<IWeekPlanRepository>();
            calendarService = ServiceFactory.Current.Resolve<ICalendarService>();
            materialRepository = ServiceFactory.Current.Resolve<IMaterialRepository>();
        }


        public JsonResult GetExerciseData(int id, string authKey)
        {
            Guid? userId = null;

            if (!string.IsNullOrEmpty(authKey))
            {
                Guid? localUserId = UserRepository.FindUserIdByToken(authKey);
                if (!localUserId.HasValue)
                {
                    return null;
                }
                else
                {
                    userId = localUserId.Value;
                }
            }
            if (!userId.HasValue)
            {
                userId = GetUserId();
            }
            ExerciseDetails exercise = exerciseRepository.GetElement(userId, id);
            var data = new ExerciseDataView();

            if (exercise != null && exercise.Attachments != null)
            {
                foreach (AttachmentView attachmentView in exercise.Attachments)
                {
                    string filePathAndName = GetAttachmentFilePathAndName(attachmentView.Filename, userId.Value);

                    IEnumerable<Activity> activities = null;
                    if (attachmentView.Filename.Contains(".tur"))
                    {
                        var parser = new TurParser();
                        parser.ParseExercise(filePathAndName);

                        var heartrateSeries = new HeartrateDataSeriesTur(parser.TimeInSecList, parser.HeartrateList);
                        data.ChartSeries.Add(heartrateSeries);
                    }
                    else if (attachmentView.Filename.Contains(".gpx"))
                    {
                        var parser = new GpxParser();

                        activities = parser.LoadTracks(filePathAndName);
                    }
                    else if (attachmentView.Filename.Contains(".tcx"))
                    {
                        var parser = new TcxParser();
                        activities = parser.LoadTcxTracks(filePathAndName);
                    }
                    if (activities != null && (attachmentView.Filename.Contains(".tcx") || attachmentView.Filename.Contains(".gpx")))
                    {
                        data.MapPoints = MapTcxTracksToView(activities);

                        //TODO alle Rundendaten einlesen (erstmal Lap auslesen)
                        data.LapData = GetLapData(activities);

                        var speedSeries = new SpeedDataSeries(activities);
                        data.ChartSeries.Add(speedSeries);

                        var paceDataSeries = new PaceDataSeries(activities);
                        data.ChartSeries.Add(paceDataSeries);

                        var elevationDataSeries = new ElevationDataSeries(activities);
                        data.ChartSeries.Add(elevationDataSeries);

                        var heartrateDataSeries = new HeartrateDataSeries(activities);
                        data.ChartSeries.Add(heartrateDataSeries);

                        var cadenceDataSeries = new CadenceDataSeries(activities);
                        data.ChartSeries.Add(cadenceDataSeries); 
                    }
                }
            }
            return Json(data,
                        JsonRequestBehavior.AllowGet);
        }

        private List<LapDataView> GetLapData(IEnumerable<Activity> activities)
        {
            return (from activity in activities
                    from lap in activity.Laps
                    where lap != null
                    select new LapDataView
                               {
                                   DistanceMeters = lap.DistanceMeters,
                                   AverageHeartRateBpm = lap.AverageHeartRateBpm,
                                   TotalTimeSeconds = lap.TotalTimeSeconds,
                                   Cadence = lap.Cadence,
                                   Calories = lap.Calories,
                                   Intensity = lap.Intensity,
                                   MaximumHeartRateBpm = lap.MaximumHeartRateBpm,
                                   MaximumSpeed = lap.MaximumSpeed
                               }).ToList();
        }

        //private List<MapPointsView> MapTracksToView(IEnumerable<GpxTrack> tracks)
        //{
        //    var points = (from track in tracks
        //                  from seg in track.Segs
        //                  select new MapPointsView
        //                             {
        //                                 Elevation = seg.Elevation,
        //                                 Latitude = seg.Latitude,
        //                                 Longitude = seg.Longitude,
        //                                 Time = seg.Time
        //                             }).ToList();
        //    return points;
        //}

        private List<MapPointsView> MapTcxTracksToView(IEnumerable<Activity> activities)
        {
            return (from activity in activities
                    from lap in activity.Laps
                    from track in lap.Tracks
                    from trackpoint in track.TrackPoints
                    where trackpoint.Positionx.Count > 0
                    select new MapPointsView
                               {
                                   Elevation = trackpoint.AltitudeMeters,
                                   Latitude = trackpoint.Positionx.First().LatitudeDegrees,
                                   Longitude = trackpoint.Positionx.First().LongitudeDegrees,
                                   Time = trackpoint.Time
                               }).ToList();
        }

        //
        // GET: /Exercise/
        [Authorize]

        public ActionResult Index(bool? listDefaultView)
        {
            //var listDefaultViewFromSession = Session["ListDefaultView"] as bool?;
            if ((listDefaultView.HasValue && listDefaultView.Value))// || Session["ListDefaultView"] != null)
            {
                Session["ListDefaultView"] = true;
                return View("Index");
            }
            return RedirectToAction("Calendar");
        }
        [Authorize]
        public ActionResult List(int? page, GridSortOptions sort, string search, DateTime? from, DateTime? to)
        {
            IEnumerable<ExerciseView> filteredExercises = null;
            var now = DateTime.Now;
            if (!from.HasValue) from = new DateTime(now.Year, now.Month, 1);
            if (!to.HasValue) to = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

            if (from.HasValue && from > DateTime.MinValue && from.Value.Date < DateTime.MaxValue)
            {
                filteredExercises = exerciseRepository.GetExercises(UserId, from.Value, to);
            }
            if (!String.IsNullOrEmpty(search))
            {
                if (filteredExercises == null)
                {
                    filteredExercises =
                        exerciseRepository.GetExercises(UserId).Where(
                            g =>
                            (g.SportTypeName != null && g.SportTypeName.Contains(search)) ||
                            (g.TrainingTypeName != null && g.TrainingTypeName.Contains(search)) ||
                            (g.ZoneName != null && g.ZoneName.Contains(search)) ||
                            (g.Description != null && g.Description.Contains(search)));
                }
                else
                {
                    filteredExercises =
                        filteredExercises.Where(
                            g =>
                            (g.SportTypeName != null && g.SportTypeName.Contains(search)) ||
                            (g.TrainingTypeName != null && g.TrainingTypeName.Contains(search)) ||
                            (g.ZoneName != null && g.ZoneName.Contains(search)) ||
                            (g.Description != null && g.Description.Contains(search)));
                }
            }
            else
            {
                if (filteredExercises == null)
                {
                    filteredExercises = exerciseRepository.GetExercises(UserId);
                }
            }
            filteredExercises = sort.Column != null
                                    ? filteredExercises.OrderBy(sort.Column, sort.Direction)
                                    : filteredExercises.OrderBy("Date", SortDirection.Descending);

            if (sort.Column != null)
            {
                filteredExercises = filteredExercises.OrderBy(sort.Column, sort.Direction);
            }

            ViewData["sort"] = sort;
            IPagination<ExerciseView> pagedMetrics = filteredExercises.AsPagination(page ?? 1, 25);
            return View(pagedMetrics);
        }

        //
        // GET: /Exercise/Calendar/05/2010
        //[Authorize]
        //public ActionResult Calendar(int? month, int? year)
        //{
        //    DateTime currentDate = GetDateFromSessionOrToday();

        //    int monthValue = month.HasValue ? month.Value : currentDate.Month;

        //    int yearValue = year.HasValue ? year.Value : currentDate.Year;

        //    if (month.HasValue && year.HasValue)
        //    {
        //        Session["startDate"] = new DateTime(year.Value, month.Value, 1);
        //    }

        //    CalendarViewModel model = calendarService.GetViewModel(monthValue, yearValue, UserId.Value, CalendarContentType.Exercise);

        //    model.PublicICalUrl = UserRepository.GetSsoToken(UserId);

        //    return View(model);
        //}

        [Authorize]
        public ActionResult Calendar(int? month, int? year)
        {
            DateTime currentDate = GetDateFromSessionOrToday();

            int monthValue = month.HasValue ? month.Value : currentDate.Month;

            int yearValue = year.HasValue ? year.Value : currentDate.Year;

            if (month.HasValue && year.HasValue)
            {
                Session["startDate"] = new DateTime(year.Value, month.Value, 1);
            }

            CalendarViewModel model = calendarService.GetViewModel(monthValue, yearValue, UserId.Value, CalendarContentType.Exercise);

            model.PublicICalUrl = UserRepository.GetSsoToken(UserId);
            //return model;
            return View(model);
        }




        //
        // GET: /Exercise/Create

        [Authorize]
        public ActionResult Create(DateTime? defaultDate)
        {
            IEnumerable<SportTypeView> sportTypesViewList = sportTypeRepository.GetAll(UserId.Value);

            IEnumerable<ZoneView> zonesViewList = zoneRepository.GetAll(UserId.Value);

            IEnumerable<TrainingTypeView> trainingTypesViewList = trainingTypeRepository.GetAll(UserId.Value);

            if (!defaultDate.HasValue)
            {
                defaultDate = DateTime.Now;
            }
            else
            {
                //add default hours / time
                defaultDate = defaultDate.Value.AddHours(8);
            }
            var exercise = new ExerciseDetails(sportTypesViewList, zonesViewList, trainingTypesViewList) { Date = defaultDate.Value };

            var allMaterials = materialRepository.GetMaterials(UserId);

            exercise.Materials = new List<MaterialView>(allMaterials);
            exercise.IsNew = true;

            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView("_Edit", exercise);
            }
            return View("_Edit", exercise);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var exercise = exerciseRepository.GetElement(UserId, id);
            var sportTypesViewList = sportTypeRepository.GetAll(UserId.Value);

            var zonesViewList = zoneRepository.GetAll(UserId.Value);

            var trainingTypesViewList = trainingTypeRepository.GetAll(UserId.Value);

            var allMaterials = materialRepository.GetMaterials(UserId);

            exercise.Materials = new List<MaterialView>(allMaterials);

            exercise.SportTypes = exercise.SportTypeId > 0
                                      ? new SelectList(sportTypesViewList, "Id", "Name", exercise.SportTypeId)
                                      : new SelectList(sportTypesViewList, "Id", "Name");

            exercise.Zones = exercise.ZoneId.HasValue
                                 ? new SelectList(zonesViewList, "Id", "Name", exercise.ZoneId)
                                 : new SelectList(zonesViewList, "Id", "Name");

            exercise.TrainingTypes = exercise.TrainingTypeId.HasValue
                                         ? new SelectList(trainingTypesViewList, "Id", "Name", exercise.TrainingTypeId)
                                         : new SelectList(trainingTypesViewList, "Id", "Name");
            exercise.WeatherCondition = Constants.AllWeatherConditions;

            var values = new Dictionary<string, object>();
            values.Add("id", id);
            values.Add("authkey", UserRepository.GetSsoToken(UserId));
            var link = Url.Action("Details", new RouteValueDictionary(values));
            var baseLink = Request.Url.OriginalString.Substring(0, Request.Url.OriginalString.ToLower().IndexOf("/exercise/edit"));

            exercise.PublicLink = string.Concat(baseLink, link);

            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView("_Edit", exercise);
            }
            return View("_Edit", exercise);
        }

        public ActionResult Details(int id, string authKey)
        {
            //get user by token
            Guid? userId = UserRepository.FindUserIdByToken(authKey);
            if (!userId.HasValue)
            {
                return View("NoRights");
            }
            var exercise = exerciseRepository.GetDetailsView(userId, id);
            
            var sportTypesViewList = sportTypeRepository.GetAll(userId.Value);

            var zonesViewList = zoneRepository.GetAll(userId.Value);

            var trainingTypesViewList = trainingTypeRepository.GetAll(userId.Value);

            var allMaterials = materialRepository.GetMaterials(userId);
            exercise.UsedMaterials = new List<MaterialView>();
            foreach (var materialId in exercise.SelectedMaterials)
            {
                exercise.UsedMaterials.AddRange(allMaterials.Where(am => am.Id == materialId));
            }

            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView("Details", exercise);
            }
            return View("Details", exercise);
        }


        [HttpPost]
        [OutputCache(CacheProfile = "ZeroCacheProfile")]
        [Authorize]
        public ActionResult Edit(ExerciseDetails exercise)
        {
            int statusCode = 0;
            StringBuilder errorMessage = null;
            string resultMsg = string.Empty;

            //parse time
            if (!String.IsNullOrEmpty(exercise.Time))
            {
                DateTime time;
                if (DateTime.TryParse(exercise.Time, out time))
                    exercise.Date = new DateTime(exercise.Date.Year, exercise.Date.Month, exercise.Date.Day, time.Hour,
                                                 time.Minute, 0);
            }
            if (!ModelState.IsValid)
            {
                IEnumerable<SportTypeView> sportTypesViewList = sportTypeRepository.GetAll(UserId.Value);

                IEnumerable<ZoneView> zonesViewList = zoneRepository.GetAll(UserId.Value);

                IEnumerable<TrainingTypeView> trainingTypesViewList = trainingTypeRepository.GetAll(UserId.Value);


                exercise.SportTypes = exercise.SportTypeId > 0
                                          ? new SelectList(sportTypesViewList, "Id", "Name", exercise.SportTypeId)
                                          : new SelectList(sportTypesViewList, "Id", "Name");

                exercise.Zones = exercise.ZoneId.HasValue
                                     ? new SelectList(zonesViewList, "Id", "Name", exercise.ZoneId)
                                     : new SelectList(zonesViewList, "Id", "Name");

                exercise.TrainingTypes = exercise.TrainingTypeId.HasValue
                                             ? new SelectList(trainingTypesViewList, "Id", "Name",
                                                              exercise.TrainingTypeId)
                                             : new SelectList(trainingTypesViewList, "Id", "Name");

                //exercise.WeatherCondition = !string.IsNullOrEmpty(exercise.SelectedWeatherCondition) ?
                //new SelectList(Constants.AllWeatherConditions, "Key", "Value", exercise.SelectedWeatherCondition) :
                //new SelectList(Constants.AllWeatherConditions, "Key", "Value");

                exercise.WeatherCondition = Constants.AllWeatherConditions;
                if (!string.IsNullOrEmpty(exercise.SelectedWeatherCondition))
                {
                    var selected = exercise.WeatherCondition.SingleOrDefault(wc => wc.SelectValue.Equals(exercise.SelectedWeatherCondition, StringComparison.CurrentCultureIgnoreCase));
                    if (selected != null)
                    {
                        selected.IsSelectedWeatherCondition = true;
                    }
                }

                if (Request.IsAjaxRequest())
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return PartialView("_Edit", exercise);
                }
            }

            try
            {
                if (exercise.IsCopy)
                    exercise.Id = 0;
                exerciseRepository.Save(UserId, exercise);
                //materialRepository.SaveMaterialPerExercise(UserId, exercise.Id, exercise.SelectedMaterialIds);
                resultMsg = "Updated exercise information.";
            }
            catch (Exception exp)
            {
                errorMessage = new StringBuilder(200);
                errorMessage.AppendFormat("<div class=\"validation-summary-errors\" title=\"Server Error\">{0}</div>",
                                          exp.GetBaseException().Message);
                statusCode = (int)HttpStatusCode.InternalServerError;
                ;
            }
            if (Request.IsAjaxRequest())
            {
                if (statusCode > 0)
                {
                    Response.StatusCode = statusCode;
                    return Content(errorMessage.ToString());
                }
                else
                {
                    string path = HttpContext.Request.UrlReferrer.LocalPath.ToLower();
                    if (path.Contains("calendar"))
                    {
                        object monthTemp = Session["CalendarMonth"];
                        object yearTemp = Session["CalendarYear"];
                        int? month = null;
                        int? year = null;
                        if (monthTemp != null)
                            month = (int)monthTemp;

                        if (yearTemp != null)
                            year = (int)yearTemp;

                        DateTime today = DateTime.Today;

                        int monthValue = month.HasValue ? month.Value : today.Month;

                        int yearValue = year.HasValue ? year.Value : today.Year;

                        CalendarViewModel model = calendarService.GetViewModel(monthValue, yearValue, UserId.Value,
                                                                               CalendarContentType.Exercise);

                        return PartialView("CalendarView", model);
                    }
                    IEnumerable<ExerciseView> exercises = exerciseRepository.GetExercises(UserId);
                    TempData["message"] = resultMsg;

                    return PartialView("List", exercises);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Exercise/Delete/5

        [Authorize]
        public ActionResult Delete(int id)
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
            if (Request.IsAjaxRequest())
            {
                return Content(resultMsg);
            }
            else
            {
                return RedirectToAction("Calendar");
            }
        }

        [Authorize]
        public JsonResult UpdateWeekPhase(int phaseId, int weekNumber, int currentYear)
        {
            string statusMessage = String.Empty;
            bool isSuccess = false;
            if (phaseId >= -1 && weekNumber > 0 && weekNumber < 54 && currentYear > 2000 && currentYear < 2100)
            {
                try
                {
                    if (phaseId == -1)
                    {
                        weekPlanRepository.DeleteWeekPlan(GetUserId().Value, weekNumber, currentYear);
                    }
                    else
                    {
                        weekPlanRepository.UpdateWeekPlan(GetUserId().Value, phaseId, weekNumber, currentYear);
                    }
                    statusMessage = "Exercise saved";
                    isSuccess = true;
                }
                catch (Exception exc)
                {
                    statusMessage = "Es ist ein Fehler aufgetreten. Meldung: " + exc.Message;
                    isSuccess = false;
                }
            }
            return Json(new { success = isSuccess, message = statusMessage });
        }

        [Authorize]
        public JsonResult UpdateSessionDate(string dayId, string sessionId, bool shouldCopy)
        {
            string[] idTemp = sessionId.Split("_".ToCharArray());
            bool isSuccess = false;
            string statusMessage = String.Empty;
            int exerciseId;
            int newExerciseId = 0;
            if (idTemp.Count() > 1 && Int32.TryParse(idTemp[1], out exerciseId))
            {
                ExerciseDetails exercise = exerciseRepository.GetElement(GetUserId(), exerciseId);
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
                        isSuccess = true;
                    }
                }
            }
            return Json(new { success = isSuccess, message = statusMessage, exerciseId = newExerciseId });
        }

        [Authorize]
        public ActionResult Upload()
        {
            return View();
        }

        [Authorize]
        public ActionResult RefreshAttachment(int exerciseId, int attachmenId)
        {
            AttachmentView attachment = exerciseRepository.GetAttachment(UserId, attachmenId);
            string resultMsg = String.Empty;

            if (attachment != null)
            {
                string rootFolder = AppConfigHelper.GetWebConfigValue(Constants.RootUploadFolder);
                ExerciseView view = exerciseRepository.RefreshFromAttachment(exerciseId, attachmenId, rootFolder, UserId);
                //var eview = exerciseRepository.GetElement(userId, exerciseId);
                return Json(new
                                {
                                    Duration = view.Duration.Value.ToString(),
                                    view.Heartrate,
                                    view.Speed,
                                    view.Distance
                                });
            }
            resultMsg = "Attachment was not found.";
            return Json(null);
        }

        [Authorize]
        public ActionResult DeleteAttachment(int id)
        {
            AttachmentView attachment = exerciseRepository.GetAttachment(UserId, id);
            string resultMsg = String.Empty;
            if (attachment != null)
            {
                //check if attachment has more exercise
                //if (exerciseRepository.CanDeleteAttachment(UserId, id))
                //{
                exerciseRepository.DeleteAttachment(UserId, id);
                DeleteFile(attachment.Filename);
                // }
            }
            else
            {
                resultMsg = "Attachment was not found.";
            }
            return Json(resultMsg);
        }

        private void DeleteFile(string filename)
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

        private string GetAttachmentFilePathAndName(string filename, Guid userId)
        {
            string rootFolder = AppConfigHelper.GetWebConfigValue(Constants.RootUploadFolder);

            string fileName = Path.GetFileName(filename).ToLower();
            string filePathAndName = Path.Combine(rootFolder, userId.ToString(), fileName);
            return filePathAndName;
        }
    }
}