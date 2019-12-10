using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF.EventCalendar
{
    /// <summary>
    ///    Interaction logic for CalendarEvent.xaml
    /// </summary>
    public partial class CalendarEventView
    {
        private CalendarMonth _calendar;

        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(CalendarEventView));

        public SolidColorBrush DefaultBackfoundColor;

        public CalendarEventView()
        {
            InitializeComponent();
        }

        public CalendarEventView(SolidColorBrush color, CalendarMonth calendar) : this()
        {
            _calendar = calendar;
            DefaultBackfoundColor = BackgroundColor = color;
        }

        private void EventMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                _calendar.CalendarEventDoubleClicked(this);
            }
            else if (e.ChangedButton == MouseButton.Left && e.ClickCount == 1)
            {
                _calendar.CalendarEventClicked(this);
            }
        }
    }
}
