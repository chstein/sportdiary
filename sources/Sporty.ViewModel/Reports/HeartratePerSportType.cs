using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sporty.ViewModel.Reports
{
    public class HeartratePerSportType
    {
        public string SportTypeName { get; private set; }
        public int Heartrate { get; set; }

        public HeartratePerSportType(string sportTypeName)
        {
            SportTypeName = sportTypeName;
        }
    }
}
