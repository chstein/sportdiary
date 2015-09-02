using System;
using System.Collections.Generic;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Interfaces
{
    public interface IWeekPlanRepository : IRepository<WeekPlan>
    {
        IEnumerable<WeekPlanView> GetAll(Guid userId);
        //void CheckAndUpdateWeekPlans(Guid userId, List<WeekPlanView> sportTypesNames);
        PhaseView GetPhaseForWeek(Guid userId, int number, int currentYear);
        void UpdateWeekPlan(Guid userId, int phaseId, int weekNumber, int currentYear);

        void DeleteWeekPlan(Guid userId, int weekNumber, int currentYear);
    }
}