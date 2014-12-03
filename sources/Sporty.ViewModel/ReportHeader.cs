using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web.Mvc;
using Sporty.Common;

namespace Sporty.ViewModel
{
    public class ReportHeader
    {
        public SelectList MonthNames { get; set; }
        public SelectList Years { get; set; }
        //public string MonthName { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
        public string Data { get; set; }

        public ReportHeader(int month, int year)
        {
            Month = month;
            Year = year;
            
            var names = new Dictionary<int, string>();
            for (int i = 1; i < 13; i++)
            {
                names.Add(i, CultureHelper.DefaultCulture.DateTimeFormat.MonthNames[i - 1]);
            }
            MonthNames = new SelectList(names, "Key", "Value", month);
            //TODO nur Jahre anzeigen, die auch Einheiten haben

            Years = new SelectList(new[] { 2009, 2010, 2011, 2012 });
            //MonthName = MonthNames.Items[month - 1];
        }
    }
}
