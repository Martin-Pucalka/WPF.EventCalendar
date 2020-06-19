using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF.EventCalendar
{
    /// <summary>
    ///     Interaction logic for CustomCalendar.xaml
    /// </summary>
    public partial class CalendarMonth : INotifyPropertyChanged
    {
        public IEnumerable<object> Events
        {
            get { return (IEnumerable<object>)GetValue(EventsProperty); }
            set { SetValue(EventsProperty, value); }
        }

        public static readonly DependencyProperty EventsProperty =
            DependencyProperty.Register("Events", typeof(IEnumerable<object>), typeof(CalendarMonth),
                 new PropertyMetadata(OnEventsChangedCallBack));


        public SolidColorBrush GridBrush
        {
            get { return (SolidColorBrush)GetValue(GridBrushProperty); }
            set { SetValue(GridBrushProperty, value); }
        }

        public static readonly DependencyProperty GridBrushProperty =
            DependencyProperty.Register("GridBrush", typeof(SolidColorBrush), typeof(CalendarMonth),
                new PropertyMetadata(Brushes.Black));


        public SolidColorBrush Color0
        {
            get { return (SolidColorBrush)GetValue(Color0Property); }
            set { SetValue(Color0Property, value); }
        }

        public static readonly DependencyProperty Color0Property =
            DependencyProperty.Register("Color0", typeof(SolidColorBrush), typeof(CalendarMonth),
                new PropertyMetadata(Brushes.LightCyan));


        public SolidColorBrush Color1
        {
            get { return (SolidColorBrush)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }

        public static readonly DependencyProperty Color1Property =
            DependencyProperty.Register("Color1", typeof(SolidColorBrush), typeof(CalendarMonth),
                new PropertyMetadata(Brushes.PaleTurquoise));

        public SolidColorBrush Color2
        {
            get { return (SolidColorBrush)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }

        public static readonly DependencyProperty Color2Property =
            DependencyProperty.Register("Color2", typeof(SolidColorBrush), typeof(CalendarMonth),
                new PropertyMetadata(Brushes.SkyBlue));

        public SolidColorBrush HighlightColor
        {
            get { return (SolidColorBrush)GetValue(HighlightColorProperty); }
            set { SetValue(HighlightColorProperty, value); }
        }

        public static readonly DependencyProperty HighlightColorProperty =
            DependencyProperty.Register("HighlightColor", typeof(SolidColorBrush), typeof(CalendarMonth),
                new PropertyMetadata(Brushes.DodgerBlue));


        public double GridBorderThickness
        {
            get { return (double)GetValue(GridBorderThicknessProperty); }
            set { SetValue(GridBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty GridBorderThicknessProperty =
            DependencyProperty.Register("GridBorderThickness", typeof(double), typeof(CalendarMonth),
                new PropertyMetadata(0.5));


        private static void OnEventsChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CalendarMonth calendar = sender as CalendarMonth;
            if (calendar != null)
            {
                calendar.DrawDays();
            }
        }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                    OnPropertyChanged(() => CurrentDate);
                    DrawDays();
                    SetDateSelectionComboBoxesByCurrentDate();
                }
            }
        }

        private void SetDateSelectionComboBoxesByCurrentDate()
        {
            MonthsComboBox.SelectedValue = CurrentDate.Month;
            YearsComboBox.SelectedValue = CurrentDate.Year;
        }

        private void SetCurrentDateByDateSelectionComboBoxes()
        {
            if (YearsComboBox?.SelectedValue != null && MonthsComboBox?.SelectedValue != null)
            {
                CurrentDate = new DateTime((int)YearsComboBox.SelectedValue, (int)MonthsComboBox.SelectedValue, 1);
            }
        }

        public event EventHandler<CalendarEventView> CalendarEventDoubleClickedEvent;

        public ObservableCollection<CalendarDay> DaysInCurrentMonth { get; set; }

        public CalendarMonth()
        {
            InitializeComponent();
            DaysInCurrentMonth = new ObservableCollection<CalendarDay>();
            InitializeDayLabels();
            InitializeDateSelectionComboBoxes();
        }

        private void InitializeDayLabels()
        {
            for (int i = 0; i < 7; i++)
            {
                Label dayLabel = new Label();
                dayLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                dayLabel.SetValue(Grid.ColumnProperty, i);
                // Days.Sunday == 0, so i+1 will make Monday as first day
                dayLabel.Content = CultureInfo.InstalledUICulture.DateTimeFormat.DayNames[(i + 1) % 7];
                dayLabel.FontWeight = FontWeights.Bold;
                DayLabelsGrid.Children.Add(dayLabel);
            }
        }

        private void InitializeDateSelectionComboBoxes()
        {
            for (int i = 1; i <= 12; i++)
            {
                MonthsComboBox.Items.Add(i);
            }

            for (int i = 1950; i <= 2100; i++)
            {
                YearsComboBox.Items.Add(i);
            }
            CurrentDate = DateTime.Now;
        }

        public void CalendarEventDoubleClicked(CalendarEventView calendarEventView)
        {
            CalendarEventDoubleClickedEvent?.Invoke(this, calendarEventView);
        }

        internal void CalendarEventClicked(CalendarEventView eventToSelect)
        {
            foreach (CalendarDay day in DaysInCurrentMonth)
            {
                foreach (CalendarEventView e in day.Events.Children)
                {
                    if (e.DataContext == eventToSelect.DataContext)
                    {
                        e.BackgroundColor = HighlightColor;
                    }
                    else
                    {
                        e.BackgroundColor = e.DefaultBackfoundColor;
                    }
                }
            }
        }

        public void DrawDays()
        {
            DaysGrid.Children.Clear();
            DaysInCurrentMonth.Clear();

            DateTime firstDayOfMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            for (DateTime date = firstDayOfMonth; date.Date <= lastDayOfMonth; date = date.AddDays(1))
            {
                CalendarDay newDay = new CalendarDay();
                newDay.BorderThickness = new Thickness((double)GridBorderThickness / 2); // /2 because neighbor day has border too, so two half borders next to each other will create final border
                newDay.BorderBrush = GridBrush;
                newDay.Date = date;
                DaysInCurrentMonth.Add(newDay);
            }

            int row = 0;
            int column = 0;

            for (int i = 0; i < DaysInCurrentMonth.Count; i++)
            {
                switch (DaysInCurrentMonth[i].Date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        column = 0;
                        break;
                    case DayOfWeek.Tuesday:
                        column = 1;
                        break;
                    case DayOfWeek.Wednesday:
                        column = 2;
                        break;
                    case DayOfWeek.Thursday:
                        column = 3;
                        break;
                    case DayOfWeek.Friday:
                        column = 4;
                        break;
                    case DayOfWeek.Saturday:
                        column = 5;
                        break;
                    case DayOfWeek.Sunday:
                        column = 6;
                        break;
                }

                Grid.SetRow(DaysInCurrentMonth[i], row);
                Grid.SetColumn(DaysInCurrentMonth[i], column);
                DaysGrid.Children.Add(DaysInCurrentMonth[i]);

                if (column == 6)
                {
                    row++;
                }
            }

            DrawTopBorder();
            DrawBottomBorder();
            DrawRightBorder();
            DrawLeftBorder();

            // set some background today
            CalendarDay today = DaysInCurrentMonth.Where(d => d.Date == DateTime.Today).FirstOrDefault();
            if (today != null)
            {
                today.DateTextBlock.Background = Color0;
            }

            DrawEvents();
        }

        private void DrawTopBorder()
        {
            // draw top border line to be the same as inner lines in calendar
            for (int i = 0; i < 7; i++)
            {
                DaysInCurrentMonth[i].BorderThickness = new Thickness(DaysInCurrentMonth[i].BorderThickness.Left, GridBorderThickness, DaysInCurrentMonth[i].BorderThickness.Right, DaysInCurrentMonth[i].BorderThickness.Bottom);
            }
        }

        private void DrawBottomBorder()
        {
            // draw bottom border line to be the same as inner lines in calendar
            int daysInCurrentMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
            for (int i = daysInCurrentMonth - 1; i >= daysInCurrentMonth - 7; i--)
            {
                DaysInCurrentMonth[i].BorderThickness = new Thickness(DaysInCurrentMonth[i].BorderThickness.Left, DaysInCurrentMonth[i].BorderThickness.Top, DaysInCurrentMonth[i].BorderThickness.Right, GridBorderThickness);
            }
        }

        private void DrawRightBorder()
        {
            // draw right border line to be the same as inner lines in calendar
            IEnumerable<CalendarDay> sundays = DaysInCurrentMonth.Where(d => d.Date.DayOfWeek == DayOfWeek.Sunday);
            foreach (var sunday in sundays)
            {
                sunday.BorderThickness = new Thickness(sunday.BorderThickness.Left, sunday.BorderThickness.Top, GridBorderThickness, sunday.BorderThickness.Bottom);
            }
            // right border for last day in month
            var lastDay = DaysInCurrentMonth.Last();
            lastDay.BorderThickness = new Thickness(lastDay.BorderThickness.Left, lastDay.BorderThickness.Top, GridBorderThickness, lastDay.BorderThickness.Bottom);

        }

        private void DrawLeftBorder()
        {
            // draw left border line to be the same as inner lines in calendar
            IEnumerable<CalendarDay> mondays = DaysInCurrentMonth.Where(d => d.Date.DayOfWeek == DayOfWeek.Monday);
            foreach (var monday in mondays)
            {
                monday.BorderThickness = new Thickness(GridBorderThickness, monday.BorderThickness.Top, monday.BorderThickness.Right, monday.BorderThickness.Bottom);
            }
            // left border for first day in month
            var firstDay = DaysInCurrentMonth.First();
            firstDay.BorderThickness = new Thickness(GridBorderThickness, firstDay.BorderThickness.Top, firstDay.BorderThickness.Right, firstDay.BorderThickness.Bottom);
        }

        private void DrawEvents()
        {
            // this method can be called when Events is not binded yet. So check that case and return
            if (Events == null)
            {
                return;
            }

            // when Events is binded, check if it is collection of ICalendarEvent (events must have DateFrom and DateTo)
            if (Events is IEnumerable<ICalendarEvent> events)
            {
                // add colors of events to array, to pick up them using number
                SolidColorBrush[] colors = { Color0, Color1, Color2 };

                // index to array of colors
                int accentColor = 0;

                // loop all events
                foreach (var e in events.OrderBy(e => e.DateFrom))
                {
                    if (!e.DateFrom.HasValue || !e.DateTo.HasValue)
                    {
                        continue;
                    }

                    // number of row in day, in which event should be displayed
                    int eventRow = 0;

                    var dateFrom = (DateTime)e.DateFrom;
                    var dateTo = (DateTime)e.DateTo;

                    // loop all days of current event
                    for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
                    {
                        // get DayOfWeek for current day of current event
                        CalendarDay day = DaysInCurrentMonth.Where(d => d.Date.Day == date.Day).FirstOrDefault();

                        // day is in another mont, so skip it
                        if (day == null)
                        {
                            continue;
                        }

                        // if the DayOfWeek is Monday, event shloud be displayed on first row
                        if (day.Date.DayOfWeek == DayOfWeek.Monday)
                        {
                            eventRow = 0;
                        }

                        // but if there are some events before, event won't be on the first row, but after previous events
                        if (day.Events.Children.Count > eventRow)
                        {
                            eventRow = Grid.GetRow(day.Events.Children[day.Events.Children.Count - 1]) + 1;
                        }

                        // get color for event
                        int accentColorIndex = accentColor % colors.Count();
                        CalendarEventView calendarEventView = new CalendarEventView(colors[accentColorIndex], this);

                        calendarEventView.DataContext = e;
                        Grid.SetRow(calendarEventView, eventRow);
                        day.Events.Children.Add(calendarEventView);
                    }
                    accentColor++;
                }
            }
            else
            {
                throw new ArgumentException("Events must be IEnumerable<ICalendarEvent>");
            }
        }

        private void PreviousMonthButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrentDate.Month == 1)
            {
                CurrentDate = CurrentDate.AddYears(-1);
            }
            CurrentDate = CurrentDate.AddMonths(-1);
        }

        private void NextMonthButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrentDate.Month == 12)
            {
                CurrentDate = CurrentDate.AddYears(1);
            }
            CurrentDate = CurrentDate.AddMonths(1);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged<T>(Expression<Func<T>> exp)
        {
            //the cast will always succeed
            var memberExpression = (MemberExpression)exp.Body;
            var propertyName = memberExpression.Member.Name;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MonthsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCurrentDateByDateSelectionComboBoxes();
        }

        private void YearsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCurrentDateByDateSelectionComboBoxes();
        }
    }
}