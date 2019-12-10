using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WPF.EventCalendar
{
    public partial class CalendarDay
    {
        public DateTime Date { get; set; }

        public CalendarDay()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}