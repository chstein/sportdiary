using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using Sporty.Business;
using Sporty.Business.Interfaces;
using Sporty.Common;
using Sporty.Infrastructure;
using Sporty.ViewModel;

namespace Sporty.Controllers
{
    [Authorize]
    public class PlanController : BaseController
    {
        private ICalendarService calendarService;
        private IPlanRepository planRepository;
        private ISportTypeRepository sportTypeRepository;
        private ITrainingTypeRepository trainingTypeRepository;
        private IWeekPlanRepository weekPlanRepository;
        private IZoneRepository zoneRepository;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (UserId.HasValue)
            {
                planRepository = ServiceFactory.Current.Resolve<IPlanRepository>();
                sportTypeRepository = ServiceFactory.Current.Resolve<ISportTypeRepository>();
            }
            zoneRepository = ServiceFactory.Current.Resolve<IZoneRepository>();
            trainingTypeRepository = ServiceFactory.Current.Resolve<ITrainingTypeRepository>();
            weekPlanRepository = ServiceFactory.Current.Resolve<IWeekPlanRepository>();
            calendarService = ServiceFactory.Current.Resolve<ICalendarService>();
        }

        private string GetCurrentUserId()
        {
            string name = HttpContext.User.Identity.Name;
            return name;
        }

        //
        // GET: /Plan/

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

        public ActionResult List(int? page, GridSortOptions sort, string search, DateTime? from, DateTime? to)
        {
            IEnumerable<PlanView> filteredPlans = null;

            if (from.HasValue && from > DateTime.MinValue && from.Value.Date < DateTime.MaxValue)
            {
                filteredPlans = planRepository.GetPlans(UserId, from.Value, to);
            }
            if (!String.IsNullOrEmpty(search))
            {
                if (filteredPlans == null)
                {
                    filteredPlans =
                        planRepository.GetPlans(UserId).Where(
                            g => g.SportTypeName.Contains(search) || g.TrainingTypeName.Contains(search) ||
                                 g.ZoneName.Contains(search) || g.Description.Contains(search));
                }
                else
                {
                    filteredPlans =
                        filteredPlans.Where(
                            g => g.SportTypeName.Contains(search) || g.TrainingTypeName.Contains(search) ||
                                 g.ZoneName.Contains(search) || g.Description.Contains(search));
                }
            }
            else
            {
                if (filteredPlans == null)
                {
                    filteredPlans = planRepository.GetPlans(UserId);
                }
            }
            filteredPlans = sort.Column != null
                                ? filteredPlans.OrderBy(sort.Column, sort.Direction)
                                : filteredPlans.OrderBy("Date", SortDirection.Descending);

            if (sort.Column != null)
            {
                filteredPlans = filteredPlans.OrderBy(sort.Column, sort.Direction);
            }

            ViewData["sort"] = sort;
            IPagination<PlanView> pagedMetrics = filteredPlans.AsPagination(page ?? 1, 25);
            return View(pagedMetrics);
        }

        public ActionResult Calendar(int? month, int? year)
        {
            DateTime currentDate = GetDateFromSessionOrToday();

            int monthValue = month.HasValue ? month.Value : currentDate.Month;

            int yearValue = year.HasValue ? year.Value : currentDate.Year;

            if (month.HasValue && year.HasValue)
            {
                Session["startDate"] = new DateTime(year.Value, month.Value, 1);
            }

            CalendarViewModel model = calendarService.GetViewModel(monthValue, yearValue, UserId.Value, CalendarContentType.Plan);
            model.PublicICalUrl = UserRepository.GetSsoToken(UserId);

            return View(model);
        }

        //
        // GET: /Plan/Create

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

            var plan = new PlanDetailsView(sportTypesViewList, zonesViewList, trainingTypesViewList) { Date = defaultDate.Value };
            plan.IsNew = true;

            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView("_Edit", plan);
            }
            return View("_Edit", plan);
        }

        public ActionResult Edit(int id)
        {
            PlanDetailsView plan = planRepository.GetElement(UserId, id);
            IEnumerable<SportTypeView> sportTypesViewList = sportTypeRepository.GetAll(UserId.Value);

            IEnumerable<ZoneView> zonesViewList = zoneRepository.GetAll(UserId.Value);

            IEnumerable<TrainingTypeView> trainingTypesViewList = trainingTypeRepository.GetAll(UserId.Value);


            plan.SportTypes = plan.SportTypeId > 0
                                  ? new SelectList(sportTypesViewList, "Id", "Name", plan.SportTypeId)
                                  : new SelectList(sportTypesViewList, "Id", "Name");

            plan.Zones = plan.ZoneId.HasValue
                             ? new SelectList(zonesViewList, "Id", "Name", plan.ZoneId)
                             : new SelectList(zonesViewList, "Id", "Name");

            plan.TrainingTypes = plan.TrainingTypeId.HasValue
                                     ? new SelectList(trainingTypesViewList, "Id", "Name", plan.TrainingTypeId)
                                     : new SelectList(trainingTypesViewList, "Id", "Name");


            //if (Request != null && Request.IsAjaxRequest())
            //{
            return PartialView("_Edit", plan);
            //}

            //return View("_Edit", plan);
        }

        //
        // POST: /Goal/Edit/5

        [HttpPost]
        [OutputCache(CacheProfile = "ZeroCacheProfile")]
        public ActionResult Edit(PlanDetailsView plan)
        {
            int statusCode = 0;
            StringBuilder errorMessage = null;
            string resultMsg = string.Empty;


            if (!ModelState.IsValid)
            {
                IEnumerable<SportTypeView> sportTypesViewList = sportTypeRepository.GetAll(UserId.Value);

                IEnumerable<ZoneView> zonesViewList = zoneRepository.GetAll(UserId.Value);

                IEnumerable<TrainingTypeView> trainingTypesViewList = trainingTypeRepository.GetAll(UserId.Value);


                plan.SportTypes = plan.SportTypeId > 0
                                      ? new SelectList(sportTypesViewList, "Id", "Name", plan.SportTypeId)
                                      : new SelectList(sportTypesViewList, "Id", "Name");

                plan.Zones = plan.ZoneId.HasValue
                                 ? new SelectList(zonesViewList, "Id", "Name", plan.ZoneId)
                                 : new SelectList(zonesViewList, "Id", "Name");

                plan.TrainingTypes = plan.TrainingTypeId.HasValue
                                         ? new SelectList(trainingTypesViewList, "Id", "Name",
                                                          plan.TrainingTypeId)
                                         : new SelectList(trainingTypesViewList, "Id", "Name");

                //if (Request.IsAjaxRequest())
                //{
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Edit", plan);
                //}
                //else
                //    return PartialView("_Edit", plan);
            }

            try
            {
                if (plan.IsCopy)
                    plan.Id = 0;
                planRepository.Save(UserId, plan);
                resultMsg = "<span style='color: blue'>Der Eintrag wurde erfolgreich gespeichert.</span>";
            }
            catch (Exception exp)
            {
                errorMessage = new StringBuilder(200);
                errorMessage.AppendFormat("<div class=\"validation-summary-errors\" title=\"Server Error\">{0}</div>",
                                          exp.GetBaseException().Message);
                statusCode = (int)HttpStatusCode.InternalServerError;
            }
            if (Request.IsAjaxRequest())
            {
                if (statusCode > 0)
                {
                    Response.StatusCode = statusCode;
                    return Content(errorMessage.ToString());
                }

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
                                                                           CalendarContentType.Plan);

                    return PartialView("CalendarView", model);
                }
                TempData["message"] = resultMsg;

                return Content(resultMsg);
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Goal/Delete/5

        public ActionResult Delete(int id)
        {
            PlanDetailsView plan = planRepository.GetElement(UserId, id);
            string resultMsg;
            if (plan != null)
            {
                planRepository.Delete(UserId, id);
                resultMsg =
                    String.Format("<span style='color: red'>{0} Plan from {1} would have been deleted.</span>",
                                  plan.SportTypeName, plan.Date);
            }
            else
            {
                resultMsg = "<span style='color: red'>Plan not found.</span>";
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

        public void UpdateFavorite(int id)
        {
            PlanDetailsView plan = planRepository.GetElement(UserId, id);

            if (plan == null) return;

            plan.IsFavorite = !plan.IsFavorite;
            planRepository.Save(UserId, plan);
        }

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

        public JsonResult UpdateSessionDate(string dayId, string sessionId, bool shouldCopy)
        {
            string[] idTemp = sessionId.Split("_".ToCharArray());
            bool isSuccess = false;
            string statusMessage = String.Empty;
            int planId;
            int newPlanId = 0;
            
            if (idTemp.Count() > 1 && Int32.TryParse(idTemp[1], out planId))
            {
                PlanDetailsView plan = planRepository.GetElement(GetUserId(), planId);
                if (plan != null)
                {
                    if (shouldCopy)
                    {
                        plan.Id = 0;
                        plan.IsFavorite = false;
                    }

                    DateTime newDate;
                    if (DateTime.TryParseExact(dayId, "dd_MM_yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                                               out newDate))
                    {
                        plan.Date = new DateTime(newDate.Year, newDate.Month, newDate.Day, plan.Date.Hour,
                                                 plan.Date.Minute, plan.Date.Second);
                        newPlanId = planRepository.Save(GetUserId(), plan);
                        statusMessage = "Plan saved";
                        isSuccess = true;
                    }
                }
            }
            return Json(new { success = isSuccess, message = statusMessage, planid = newPlanId });
        }

        public ActionResult Favorites()
        {
            IEnumerable<PlanView> plans = planRepository.GetFavoritePlans(UserId);

            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView("Favorites", plans);
            }

            return View("Favorites", plans);
        }
    }
}