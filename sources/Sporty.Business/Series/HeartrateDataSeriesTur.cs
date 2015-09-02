using System.Collections.Generic;
using System.Linq;

namespace Sporty.Business.Series
{
    public class HeartrateDataSeriesTur : ExerciseDataSeries
    {
        private readonly int[] heartrateList;
        private readonly long[] timeInSecList;

        public HeartrateDataSeriesTur(long[] timeInSecList, int[] heartrateList)
        {
            this.timeInSecList = timeInSecList;
            this.heartrateList = heartrateList;
            Type = "HEARTRATE";
            UnitY = "bpm";
            UnitX = "Min";
            CalculatePoints();
        }

        private void CalculatePoints()
        {
            Points = new List<object[]>();
            for (int i = 0; i < timeInSecList.Count(); i++)
            {
                Points.Add(new object[] {((double) timeInSecList[i])/60, heartrateList[i]});
            }
        }
    }
}