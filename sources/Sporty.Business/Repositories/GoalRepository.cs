using System;
using System.Collections.Generic;
using System.Linq;
using Sporty.Business.Helper;
using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.ViewModel;
using Sporty.Common;

namespace Sporty.Business.Repositories
{
    public class GoalRepository : BaseRepository<Goal>, IGoalRepository
    {
        public GoalRepository(SportyEntities context)
            : base(context)
        {
        }

        #region IGoalRepository Members

        public IEnumerable<GoalView> GetGoals(Guid? userId)
        {
            IQueryable<Goal> goalList = context.Goal.Where(g => g.UserId == userId);
            return goalList.Select(item => new GoalView
                                               {
                                                   Id = item.Id,
                                                   Description = item.Description,
                                                   Name = item.Name,
                                                   Date = item.DateLocal
                                               }).ToList();
        }

        public GoalView GetElement(Guid? userId, int id)
        {
            Goal goal = this.context.Goal.SingleOrDefault(g => g.Id == id && g.UserId == userId);
            if (goal == null) return null;

            var goalView = new GoalView
                               {
                                   Id = goal.Id,
                                   Name = goal.Name,
                                   Description = goal.Description,
                                   Date = goal.DateLocal
                               };

            return goalView;
        }

        public void Save(Guid userId, GoalView element)
        {
            Goal goal = element.Id > 0
                            ? this.context.Goal.SingleOrDefault(e => e.Id == element.Id && e.UserId == userId)
                            : new Goal { Id = element.Id };

            goal.Name = element.Name;
            goal.Description = element.Description;
            goal.DateLocal = element.Date;
            goal.UserId = userId;

            if (goal.Id > 0)
                base.Update();
            else
                this.Add(goal);
        }

        public void Delete(Guid userId, int id)
        {
            this.Delete(g => g.Id == id && g.User.UserId == userId);
        }

        public IEnumerable<GoalView> GetGoalsBetweenAndNextGoal(Guid userId, DateTime startLocalDate,
                                                                DateTime endLocalDate)
        {
            var startUtcDate = DateTimeConverter.GetUtcDateTime(startLocalDate, User.LocalTimeZone);
            IOrderedQueryable<Goal> goals = context.Goal.Where(g => g.UserId == userId &&
                                                g.Date >= startUtcDate).OrderBy(d => d.Date);
            var relevantGoals = new List<GoalView>();
            foreach (Goal goal in goals)
            {
                GoalView goalView = GetGoalView(goal);
                if (goal.DateLocal <= endLocalDate)
                {
                    relevantGoals.Add(goalView);
                }
                else if (goal.DateLocal > endLocalDate)
                {
                    //nur das nächste Ziel außerhalb des Zeitraums soll mit rein, alle weiteren können ignoriert werden
                    relevantGoals.Add(goalView);
                    break;
                }
            }

            return relevantGoals;
        }

        public IEnumerable<GoalView> GetGoals(Guid userId, DateTime fromLocalDate, DateTime? toLocalDate)
        {
            DateTime fromUtc = DateTimeConverter.GetUtcDateTime(fromLocalDate, User.LocalTimeZone);
            DateTime? toUtc = null;
            if (toLocalDate.HasValue)
            {
                toUtc = DateTimeConverter.GetUtcDateTime(toLocalDate.Value, User.LocalTimeZone);
            }
            IQueryable<Goal> goals = toUtc.HasValue
                                         ? context.Goal.Where(
                                             e => e.Date >= fromUtc && e.Date <= toUtc && e.UserId == userId)
                                         : context.Goal.Where(e => e.Date >= fromUtc && e.UserId == userId);

            return GetGoalsViewList(goals);
        }

        #endregion

        private GoalView GetGoalView(Goal goal)
        {
            return new GoalView
                       {
                           Id = goal.Id,
                           Description = goal.Description,
                           Name = goal.Name,
                           Date = DateTimeConverter.GetLocalDateTime(goal.Date, User.LocalTimeZone)
                       };
        }

        private IEnumerable<GoalView> GetGoalsViewList(IEnumerable<Goal> goalList)
        {
            return goalList.Select(GetGoalView).ToList();
        }
    }
}