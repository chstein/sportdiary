using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;
using Sporty.Business.Interfaces;
using Sporty.Infrastructure;
using Sporty.ViewModel;
using Sporty.ViewModel.Reports;

namespace Sporty.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        private IExerciseRepository exerciseRepository;
        private IMetricRepository metricsRepository;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            exerciseRepository = ServiceFactory.Current.Resolve<IExerciseRepository>();
            metricsRepository = ServiceFactory.Current.Resolve<IMetricRepository>();
        }

        public ActionResult Index()
        {
            DateTime today = DateTime.Today;
            var rh = new ReportHeader(today.Month, today.Year);
            return View(rh);
        }

        /// <summary>
        /// Reports the specified from month.
        /// </summary>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="timeUnit">day, week or year</param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult Report(DateTime? from, DateTime? to, string timeUnit,
                                   int? reportType)
        {
            DateTime today = DateTime.Today;
            //int fromMonthValue = fromMonth.HasValue ? fromMonth.Value : today.Month;
            //int fromYearValue = fromYear.HasValue ? fromYear.Value : today.Year;
            DateTime fromDate = from.HasValue ? from.Value : new DateTime(today.Year, today.Month, 1);
            //int toMonthValue = toMonth.HasValue ? toMonth.Value : today.Month;
            //int toYearValue = toYear.HasValue ? toYear.Value : today.Year;
            DateTime toDate = to.HasValue ? to.Value : new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            ReportTypeName reportTypeName;
            if (!reportType.HasValue || !Enum.IsDefined(typeof (ReportTypeName), reportType.Value))
            {
                //falscher Typ
                reportTypeName = ReportTypeName.DurationPerSportType;
            }
            else
            {
                reportTypeName = (ReportTypeName) reportType;
            }

            TimeUnit timeUnitValue;
            if (!Enum.TryParse(timeUnit, true, out timeUnitValue))
            {
                //falscher Typ
                timeUnitValue = TimeUnit.Day;
            }

            List<ExercisesPerTimeUnit> exerciseViewList;


            if (reportTypeName == ReportTypeName.MetricsSingle)
            {
                exerciseViewList = metricsRepository.GetReportMetrics(GetUserId(), fromDate, toDate);
            }
            else //(reportTypeName != ReportTypeName.MetricsSingle)
            {
                exerciseViewList = exerciseRepository.GetReportExercises(GetUserId(), fromDate, toDate, timeUnitValue,
                                                                         reportTypeName);
            }

            return Json(exerciseViewList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MonthName(int? id)
        {
            string monthName = String.Empty;
            if (id.HasValue && id > 0 && id < 13)
                monthName = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[id.Value - 1];

            return Json(monthName, JsonRequestBehavior.AllowGet);
        }
    }
}