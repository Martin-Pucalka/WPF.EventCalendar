using System;

namespace WPF.EventCalendar
{
    public interface ICalendarEvent
    {
        DateTime? DateFrom { get; set; }
        DateTime? DateTo { get; set; }
        string Label { get; set; }
    }
}
