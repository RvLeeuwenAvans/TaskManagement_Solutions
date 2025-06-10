using System.Globalization;

namespace TaskManagement.MobileApp.Helpers.Converters;

public class DateToBackgroundColorConverter : IValueConverter
{
    public required Color PastDueColor { get; set; }
    public required Color TodayColor { get; set; }
    public required Color FutureColor { get; set; }
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DateTime dueDate) return FutureColor;
        
        var today = DateTime.Today;
        var dueDateOnly = dueDate.Date;

        return (dueDateOnly.CompareTo(today)) switch
        {
            < 0 => PastDueColor,
            0 => TodayColor,
            _ => FutureColor
        };
    }


    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}