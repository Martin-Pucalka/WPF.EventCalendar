using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF.EventCalendar
{
    public class MonthNumberToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int valueInt)
            {
                DateTimeFormatInfo mfi = CultureInfo.InstalledUICulture.DateTimeFormat;
                return mfi.GetMonthName(valueInt).ToString();
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
