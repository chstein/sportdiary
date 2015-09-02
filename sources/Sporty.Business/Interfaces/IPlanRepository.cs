using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Interfaces
{
    public interface IPlanRepository : IRepository<Plan>
    {
        IEnumerable<PlanView> GetPlans(Guid? userId, int month, int year);

        IEnumerable<PlanView> GetPagedPlans(Guid? userId, int itemsPerPage, int? page, string sortColumnName,
                                            SortDirection sortDirection);

        PlanDetailsView GetElement(Guid? userId, int id);
        int Save(Guid? userId, PlanDetailsView element);
        void Delete(Guid? userId, int id);
        IEnumerable<PlanView> GetPlans(Guid? userId);
        IEnumerable<Plan> GetPlans(Expression<Func<Plan, bool>> expression);

        IEnumerable<PlanView> GetFavoritePlans(Guid? userId);
        IEnumerable<PlanView> GetPlans(Guid? userId, DateTime fromDate, DateTime? toDate);
    }
}