using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using Sporty.Business.Helper;
using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.ViewModel;
using Sporty.Common;

namespace Sporty.Business.Repositories
{
    public class PlanRepository : BaseRepository<Plan>, IPlanRepository
    {
        private readonly IGoalRepository goalRepository;
        private IPhaseRepository phaseRepository;
        private IWeekPlanRepository weekPlanRepository;

        public PlanRepository(SportyEntities context, IGoalRepository goalRepository,
                              IWeekPlanRepository weekPlanRepository, IPhaseRepository phaseRepository)
            : base(context)
        {
            this.goalRepository = goalRepository;
            this.weekPlanRepository = weekPlanRepository;
            this.phaseRepository = phaseRepository;
        }

        #region IPlanRepository Members

        public int Save(Guid? userId, PlanDetailsView element)
        {
            Plan plan = element.Id > 0
                            ? this.context.Plan.FirstOrDefault(e => e.Id == element.Id && e.UserId == userId.Value)
                            : new Plan {Id = element.Id};

            plan.SportType = context.SportType.First(s => s.Id == element.SportTypeId);

            plan.Date = element.Date;//für Pläne kein UTC notwendig, da nur das Datum gespeichert wird
            plan.Description = element.Description;
            plan.Distance = element.Distance;
            plan.Duration = element.Duration;
            plan.IsFavorite = element.IsFavorite;
            if (element.TrainingTypeId.HasValue)
                plan.TrainingType = context.TrainingType.First(tt => tt.Id == element.TrainingTypeId);
            if (element.ZoneId.HasValue)
                plan.Zone = context.Zone.First(z => z.Id == element.ZoneId);
            plan.UserId = userId.Value;

            if (plan.Id > 0)
                base.Update();
            else
                this.Add(plan);
            return plan.Id;
        }

        public IEnumerable<Plan> GetPlans(Expression<Func<Plan, bool>> expression)
        {
            return context.Plan.Where(expression);
        }

        public PlanDetailsView GetElement(Guid? userId, int id)
        {
            Plan plan = this.context.Plan.SingleOrDefault(e => e.Id == id && e.UserId == userId);
            if (plan == null) return null;

            var detailsView = new PlanDetailsView
                                  {
                                      SportTypeId = plan.SportTypeId,
                                      SportTypeName = plan.SportType != null ? plan.SportType.Name : String.Empty,
                                      Date = plan.Date,
                                      Description = plan.Description,
                                      Distance = plan.Distance,
                                      Duration = plan.Duration,
                                      IsFavorite = plan.IsFavorite,
                                      TrainingTypeId = plan.TrainingTypeId,
                                      TrainingTypeName =
                                          plan.TrainingType != null ? plan.TrainingType.Name : String.Empty,
                                      ZoneId = plan.ZoneId,
                                      ZoneName = plan.Zone != null ? plan.Zone.Name : String.Empty,
                                      Id = plan.Id
                                  };

            return detailsView;
            //return context.Plans.Where(e => e.Id == id && e.User.Id == userId).Single();
        }

        public void Delete(Guid? userId, int id)
        {
            Delete(e => e.Id == id && e.UserId == userId);
        }

        public IEnumerable<PlanView> GetPlans(Guid? userId)
        {
            if (!userId.HasValue) return null;
            IQueryable<Plan> planList = context.Plan.Where(e => e.UserId == userId);
            return GetPlanViewList(planList);
        }

        public IEnumerable<PlanView> GetPlans(Guid? userId, int month, int year)
        {
            if (!userId.HasValue) return null;
            IQueryable<Plan> planList =
                context.Plan.Where(e => e.Date.Month == month && e.Date.Year == year && e.UserId == userId);
            return GetPlanViewList(planList);
        }

        public IEnumerable<PlanView> GetPagedPlans(Guid? userId, int itemsPerPage, int? page, string sortColumnName,
                                                   SortDirection sortDirection)
        {
            if (!userId.HasValue) return null;
            if (!page.HasValue) page = 1;
            //var PlanList = base.GetPagedItems(userId, itemsPerPage, page.Value, sortColumnName, sortDirection);


            int start = (page.Value*itemsPerPage) - itemsPerPage;
            int end = page.Value*itemsPerPage;
            //context.GetTable<TEntity>().OrderBy()
            IEnumerable<Plan> table = context.Plan.Where(e => e.UserId == userId);
            if (!String.IsNullOrEmpty(sortColumnName))
                table = base.OrderBy<Plan>(table, sortColumnName, sortDirection);
            else
                table = base.OrderBy<Plan>(table, "Date", SortDirection.Descending);

            List<Plan> planList = table.Skip(start).Take(end).ToList();
            if (planList == null || planList.Count == 0)
                return null;

            return GetPlanViewList(planList);
        }

        public IEnumerable<PlanView> GetFavoritePlans(Guid? userId)
        {
            if (!userId.HasValue) return null;
            IQueryable<Plan> planList = context.Plan.Where(e => e.UserId == userId && e.IsFavorite);
            return GetPlanViewList(planList);
        }

        public IEnumerable<PlanView> GetPlans(Guid? userId, DateTime fromDate, DateTime? toDate)
        {
            if (!userId.HasValue) return null;

            if (toDate.HasValue)
            {
                toDate = DateTimeConverter.GetUtcDateTime(toDate.Value, User.LocalTimeZone);
            }
            IQueryable<Plan> plans = toDate.HasValue
                                         ? context.Plan.Where(
                                             e => e.Date >= fromDate && e.Date <= toDate && e.UserId == userId)
                                         : context.Plan.Where(e => e.Date >= fromDate && e.UserId == userId);

            return GetPlanViewList(plans);
        }

        #endregion

        private IEnumerable<PlanView> GetPlanViewList(IEnumerable<Plan> planList)
        {
            return planList.Select(GetPlanView).ToList();
        }

        private PlanView GetPlanView(Plan plan)
        {
            return new PlanView
                       {
                           Id = plan.Id,
                           Date = plan.Date,
                           Distance = plan.Distance,
                           PlannedDuration = plan.Duration.HasValue ? (int) plan.Duration.Value.TotalMinutes : 0,
                           Discipline = Enum.GetName(typeof(Disciplines), plan.SportType.Type),
                           SportTypeId = plan.SportTypeId,
                           SportTypeName =
                               plan.SportType != null ? plan.SportType.Name : String.Empty,
                           TrainingTypeName =
                               plan.TrainingType != null ? plan.TrainingType.Name : String.Empty,
                           ZoneName = StringHelper.GetShortname(plan.Zone),
                           ZoneId = plan.ZoneId,
                           Description = plan.Description
                       };
        }
    }
}