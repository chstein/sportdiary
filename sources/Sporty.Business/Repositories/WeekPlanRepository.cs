using System;
using System.Collections.Generic;
using System.Linq;
using Sporty.Business.Interfaces;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Repositories
{
    public class WeekPlanRepository : BaseRepository<WeekPlan>, IWeekPlanRepository
    {
        public WeekPlanRepository(SportyEntities context)
            : base(context)
        {
        }

        #region IWeekPlanRepository Members

        public IEnumerable<WeekPlanView> GetAll(Guid userId)
        {
            IQueryable<WeekPlan> wpList = context.WeekPlan.Where(s => s.UserId == userId);
            return wpList.Select(phase => new WeekPlanView()
                ).ToList();
        }

        public PhaseView GetPhaseForWeek(Guid userId, int number, int currentYear)
        {
            WeekPlan weekPlan = context.WeekPlan.SingleOrDefault(w => w.UserId == userId && w.WeekNumber == number && w.Year == currentYear);
            if (weekPlan != null)
            {
                return new PhaseView
                           {
                               ShortName = weekPlan.Phase.ShortName,
                               Id = weekPlan.Phase.Id,
                               Description = weekPlan.Phase.Description,
                               Order = weekPlan.Phase.Order
                           };
            }
            return null;
        }

        public void UpdateWeekPlan(Guid userId, int phaseId, int weekNumber, int currentYear)
        {
            WeekPlan weekPlan =
                context.WeekPlan.SingleOrDefault(w => w.UserId == userId && w.WeekNumber == weekNumber && w.Year == currentYear) ??
                new WeekPlan { UserId = userId, WeekNumber = weekNumber };
            weekPlan.PhaseId = phaseId;
            weekPlan.Year = currentYear;
            if (weekPlan.Id == 0)
            {
                this.Add(weekPlan);
            }
            else
            {
                Update();
            }
        }

        public void DeleteWeekPlan(Guid userId, int weekNumber, int currentYear)
        {
            Delete(w => w.UserId == userId && w.WeekNumber == weekNumber && w.Year == currentYear);
        }
        #endregion
    }
}