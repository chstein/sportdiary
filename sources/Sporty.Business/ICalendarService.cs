using System;
using Sporty.Common;
using Sporty.ViewModel;

namespace Sporty.Business
{
    public interface ICalendarService
    {
        CalendarViewModel GetViewModel(int monthValue, int yearValue, Guid userId, CalendarContentType calendarType);
    }
}