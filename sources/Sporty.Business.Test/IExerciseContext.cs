using System.Collections.Generic;
using Sporty.DataModel;

namespace SportBusinessTest
{
    public interface IExerciseContext
    {
        List<Exercise> GetAllExercises(string username);
    }
}