using System;
using System.Collections.Generic;

namespace Sporty.ViewModel.Reports
{
    public class ExercisesPerTimeUnit
    {
        public string TimeUnitValue { get; set; }
        public List<DataPoint<double>> DataPoints { get; set; }
        public List<HeartratePerSportType> HeartratePerSportType { get; set; }

        public ReportTypeName ReportTypeName { get; set; }

        public ExercisesPerTimeUnit(ReportTypeName reportTypeName)
        {
            ReportTypeName = reportTypeName;
            DataPoints = new List<DataPoint<double>>();
        }
    }

    public class DataPoint<T>
    {
        public string SportTypeName { get; set; }

        public DataPoint()
        {
            //SportTypeName = sportTypeName;
        }

        public string Label { get; set; }
        public T Value { get; set; }
    }
}
