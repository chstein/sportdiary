using System.Collections.Generic;

namespace Sporty.Business.Series
{
    public abstract class ExerciseDataSeries
    {
        public string Type { get; protected set; }
        public string UnitX { get; protected set; }
        public string UnitY { get; protected set; }
        public List<object[]> Points { get; protected set; }
    }
}