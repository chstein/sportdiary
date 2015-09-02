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
using Sporty.Infrastructure;
using Sporty.ViewModel;

namespace Sporty.Controllers
{
    [Authorize]
    public class GoalController : BaseController
    {
        private IGoalRepository goalRepository;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (UserId.HasValue)
            {
                goalRepository = ServiceFactory.Current.Resolve<IGoalRepository>();
            }
        }

        //
        // GET: /Goal/

        public ActionResult Index()
        {
            ViewBag.PublicICalUrl = UserRepository.GetSsoToken(UserId);
            return View();
        }

        public ActionResult List(int? page, GridSortOptions sort, string search, DateTime? from, DateTime? to)
        {
            IEnumerable<GoalView> filteredGoals = null;

            if (from.HasValue && from > DateTime.MinValue && from.Value.Date < DateTime.MaxValue)
            {
                filteredGoals = goalRepository.GetGoals(UserId.Value, from.Value, to);
            }
            if (!String.IsNullOrEmpty(search))
            {
                if (filteredGoals == null)
                {
                    filteredGoals =
                        goalRepository.GetGoals(UserId).Where(
                            g => g.Description != null && (g.Description.Contains(search))
                                 || (g.Name != null && g.Name.Contains(search)));
                }
                else
                {
                    filteredGoals = filteredGoals.Where(g => g.Description != null && (g.Description.Contains(search))
                                                             || (g.Name != null && g.Name.Contains(search)));
                }
            }
            else
            {
                if (filteredGoals == null)
                {
                    filteredGoals = goalRepository.GetGoals(UserId);
                }
            }
            filteredGoals = sort.Column != null
                                ? filteredGoals.OrderBy(sort.Column, sort.Direction)
                                : filteredGoals.OrderBy("Date", SortDirection.Descending);

            if (sort.Column != null)
            {
                filteredGoals = filteredGoals.OrderBy(sort.Column, sort.Direction);
            }

            ViewData["sort"] = sort;
            IPagination<GoalView> pagedMetrics = filteredGoals.AsPagination(page ?? 1, 25);
            return View(pagedMetrics);
        }

        //
        // GET: /Goal/Create

        public ActionResult Create()
        {
            var goal = new GoalView {Date = DateTime.Today.Date};
            goal.IsNew = true;

            if (Request != null && Request.IsAjaxRequest())
                return PartialView("_Edit", goal);
            return View("_Edit", goal);
        }

        //
        // GET: /Goal/Edit/5

        public ActionResult Edit(int id)
        {
            GoalView goal = goalRepository.GetElement(UserId, id);

            if (Request != null && Request.IsAjaxRequest())
            {
                return PartialView("_Edit", goal);
            }

            return View("_Edit", goal);
        }

        //
        // POST: /Goal/Edit/5

        [HttpPost]
        [OutputCache(CacheProfile = "ZeroCacheProfile")]
        public ActionResult Edit(GoalView goalView)
        {
            int statusCode = 0;
            StringBuilder errorMessage = null;
            string resultMsg = string.Empty;


            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return PartialView("_Edit", goalView);
                }
                return PartialView("_Edit", goalView);
            }

            try
            {
                goalRepository.Save(UserId.Value, goalView);
                resultMsg = "<span style='color: blue'>Der Eintrag wurde erfolgreich gespeichert.</span>";
            }
            catch (Exception exp)
            {
                errorMessage = new StringBuilder(200);
                errorMessage.AppendFormat("<div class=\"validation-summary-errors\" title=\"Server Error\">{0}</div>",
                                          exp.GetBaseException().Message);
                statusCode = (int) HttpStatusCode.InternalServerError;
                ;
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
            GoalView goal = goalRepository.GetElement(UserId, id);
            string resultMsg;
            if (goal != null)
            {
                goalRepository.Delete(UserId.Value, id);
                resultMsg = String.Format("<span style='color: red'>{0} would have been deleted.</span>", goal.Name);
            }
            else
            {
                resultMsg = "<span style='color: red'>Goal was not found.</span>";
            }
            if (Request.IsAjaxRequest())
            {
                return Content(resultMsg);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}