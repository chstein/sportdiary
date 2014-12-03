using System;
using System.Collections.Generic;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Interfaces
{
    public interface IPhaseRepository : IRepository<Phase>
    {
        IEnumerable<PhaseView> GetAll(Guid userId);
        void CheckAndUpdatePhases(Guid userId, List<PhaseView> phases);
    }
}