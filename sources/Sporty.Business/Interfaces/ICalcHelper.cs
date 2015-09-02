using Sporty.DataModel;
using Sporty.ViewModel;

namespace Sporty.Business.Interfaces
{
    public interface ICalcHelper
    {
        int CalculateTrimp(Exercise exercise);
        int CalculateTrimp(ExerciseView exerciseView);
    }
}