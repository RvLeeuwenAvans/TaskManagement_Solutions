using System.Globalization;

namespace TaskManagement.MobileApp.Helpers.Converters;

public class EnumToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        var states = parameter.ToString()?.Split(',');
        return states != null && states.Any(s => s.Trim().Equals(value.ToString(), StringComparison.OrdinalIgnoreCase));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b && b && parameter is string s)
        {
            return Enum.Parse(targetType, s);
        }

        return Binding.DoNothing;
    }
}