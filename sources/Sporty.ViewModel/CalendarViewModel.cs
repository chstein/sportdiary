using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Sporty.Common;

namespace Sporty.ViewModel
{
    public class CalendarViewModel
    {
        public string PublicICalUrl { get; set; }
        public DateTime FirstDayInWeek;

        public string[] MonthNames;

        public List<int> Years;

        public string MonthName { get; set; }
        public int Month { get; private set; }
        public int Year { get; private set; }

        public List<CalendarWeek> Weeks { get; set; }
        public IEnumerable<PhaseView> AllPhases { get; set; }

        public IEnumerable<PlanView> Favorites { get; set; }

        public CalendarViewModel(int month, int year)
        {
            Month = month;
            Year = year;
            Weeks = new List<CalendarWeek>();
            //calculate how many week we need in this month
            FirstDayInWeek = DateHelper.GetFirstDayInWeekMonth(month, year);

            while ((Month != 12 && Month != 1 && FirstDayInWeek.Month <= Month) ||
                (Month == 12 && (FirstDayInWeek.Month == 11 || FirstDayInWeek.Month == 12)) ||
                (Month == 1 && (FirstDayInWeek.Month == 12 || FirstDayInWeek.Month == 1)))
            {
                Weeks.Add(new CalendarWeek(FirstDayInWeek));

                FirstDayInWeek = FirstDayInWeek.AddDays(7);
            }


            MonthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            Years = new List<int>();
            for (int i = (DateTime.Now.Year + 5); i > 1910; i--)
            {
                Years.Add(i);
            }

            MonthName = MonthNames[month - 1];

        }

        
    }
}