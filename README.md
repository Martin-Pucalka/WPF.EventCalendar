# WPF Calendar
Simple WPF month calendar with multi-day events similar to Outlook.

![WPF Calendar](https://github.com/Martin-Pucalka/WPF.EventCalendar/blob/master/WPF.EventCalendar/images/icon.png)

## Usage example:

In xaml:
```
<Window
    ...
    xmlns:calendar="clr-namespace:WPF.EventCalendar;assembly=WPF.EventCalendar"
    x:Name="mainWindow"
    ...>

<calendar:CalendarMonth x:Name="Calendar" Events="{Binding ElementName=mainWindow, Path=Events}" />
```

In code behind to display events (optionally): 
1. Create property containing Events:
```
    ...
    public List<ICalendarEvent> _events;
    public List<ICalendarEvent> Events
    {
        get { return _events; }
        set
        {
            if (_events != value)
            {
                _events = value;
                OnPropertyChanged(() => Events);

                // redraw days with events when Events property changes
                Calendar.DrawDays();
            }
        }
    }
    ...
```

2. Class containing binded property must implement INotifyPropertyChanged:
```
    public partial class MainWindow : Window, INotifyPropertyChanged
    ...
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged<T>(Expression<Func<T>> exp)
    {
        var memberExpression = (MemberExpression)exp.Body;
        var propertyName = memberExpression.Member.Name;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    ...
```

3. Class to be displayed as an Event in calendar must implement ICalendarEvent:
```
    public class MyCustomEvent : ICalendarEvent
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Label { get; set; } // will be displayed in calendar
    }
```
