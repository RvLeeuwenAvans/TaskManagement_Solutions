using System.Globalization;

namespace TaskManagement.MobileApp;

public class EnumToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value?.ToString() == parameter?.ToString();

    /**
     * Must be implemented by the Interface; but doesnt have a use; this converter is mainly used to for the viewstate
     * enum; in the xaml binding logic.. so reverse conversion isn't nescessary.
     *
     * In future; if it becomes necessary here to implement; maybe make this class proprietary and create a new converter
     */
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}