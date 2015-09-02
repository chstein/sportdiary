using System;
using System.Collections.Generic;
using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Interfaces
{
    public interface ITrainingTypeRepository : IRepository<TrainingType>
    {
        IEnumerable<TrainingTypeView> GetAll(Guid userId);

        void CheckAndUpdateTrainingTypes(Guid userId, List<TrainingTypeView> trainingTypesViews);
    }
}