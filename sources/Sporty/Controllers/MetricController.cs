using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using Sporty.Business.Interfaces;
using Sporty.Common;
using Sporty.Infrastructure;
using Sporty.ViewModel;

namespace Sporty.Controllers
{
    [Authorize]
    public class MetricController : BaseController
    {
        private IMetricRepository metricRepository;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (UserId.HasValue)
            {
                metricRepository = ServiceFactory.Current.Resolve<IMetricRepository>();
            }
        }

        //
        // GET: /Metric/

        public ActionResult List(int? page, GridSortOptions sort, string search, DateTime? from, DateTime? to)
        {
            IEnumerable<MetricListView> filteredMetrics = null;

            if (from.HasValue && from > DateTime.MinValue && from.Value.Date < DateTime.MaxValue)
            {
                filteredMetrics = metricRepository.GetMetrics(UserId.Value, from.Value, to);
            }

            if (!String.IsNullOrEmpty(search))
            {
                filteredMetrics = filteredMetrics == null
                                      ? metricRepository.GetMetrics(UserId).Where(
                                          g => g.Description != null && g.Description.Contains(search))
                                      : filteredMetrics.Where(
                                          g => g.Description != null && g.Description.Contains(search));
            }
            else
            {
                if (filteredMetrics == null)
                {
                    filteredMetrics = metricRepository.GetMetrics(UserId);
                }
            }
            filteredMetrics = sort.Column != null
                                  ? filteredMetrics.OrderBy(sort.Column, sort.Direction)
                                  : filteredMetrics.OrderBy("Date", SortDirection.Descending);

            if (sort.Column != null)
            {
                filteredMetrics = filteredMetrics.OrderBy(sort.Column, sort.Direction);
            }

            ViewData["sort"] = sort;
            IPagination<MetricListView> pagedMetrics = filteredMetrics.AsPagination(page ?? 1, 25);
            return View(pagedMetrics);
        }

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Goal/Create

        public ActionResult Create()
        {
            var metric = new MetricView {Date = DateTime.Today.Date};
            metric.IsNew = true;

            if (Request != null && Request.IsAjaxRequest())
                return PartialView("_Edit", metric);
            return View("_Edit", metric);
        }

        //
        // GET: /Goal/Edit/5

        public ActionResult Edit(int id)
        {
            MetricView metric = metricRepository.GetElement(UserId, id);

            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView("_Edit", metric);
            }

            return View("_Edit", metric);
        }

        //
        // POST: /Goal/Edit/5

        [HttpPost]
        [OutputCache(CacheProfile = "ZeroCacheProfile")]
        public ActionResult Edit(MetricView metricView)
        {
            int statusCode = 0;
            StringBuilder errorMessage = null;
            string resultMsg = string.Empty;


            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return PartialView("_Edit", metricView);
                }
                return PartialView("_Edit", metricView);
            }

            try
            {
                metricRepository.Save(UserId.Value, metricView);
                resultMsg = "<span style='color: blue'>Der Eintrag wurde erfolgreich gespeichert.</span>";
            }
            catch (Exception exp)
            {
                errorMessage = new StringBuilder(200);
                errorMessage.AppendFormat("<div class=\"validation-summary-errors\" title=\"Server Error\">{0}</div>",
                                          exp.GetBaseException().Message);
                statusCode = (int) HttpStatusCode.InternalServerError;
            }
            if (Request.IsAjaxRequest())
            {
                if (statusCode > 0)
                {
                    Response.StatusCode = statusCode;
                    return Content(errorMessage.ToString());
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
            MetricView metric = metricRepository.GetElement(UserId, id);
            string resultMsg;
            if (metric != null)
            {
                metricRepository.Delete(UserId.Value, id);
                resultMsg = String.Format("<span style='color: red'>Der Tageswert vom {0} wurde gelöscht.</span>",
                                          metric.Date.ToString("d", CultureHelper.DefaultCulture));
            }
            else
            {
                resultMsg = "<span style='color: red'>Der Tageswert wurde nicht gefunden.</span>";
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
    }
}