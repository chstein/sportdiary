using System;
using System.Collections.Generic;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Interfaces
{
    public interface ISportTypeRepository : IRepository<SportType>
    {
        IEnumerable<SportTypeView> GetAll(Guid userId);
        void CheckAndUpdateSportTypes(Guid userId, List<SportTypeView> sportTypesNames);
    }
}