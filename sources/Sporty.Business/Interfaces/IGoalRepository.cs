using System;
using System.Collections.Generic;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Interfaces
{
    public interface IGoalRepository : IRepository<Goal>
    {
        IEnumerable<GoalView> GetGoals(Guid? userId);
        GoalView GetElement(Guid? userId, int id);
        void Save(Guid userId, GoalView element);
        void Delete(Guid userId, int id);
        IEnumerable<GoalView> GetGoalsBetweenAndNextGoal(Guid userId, DateTime startDate, DateTime endDate);
        IEnumerable<GoalView> GetGoals(Guid userId, DateTime fromDate, DateTime? toDate);
    }
}