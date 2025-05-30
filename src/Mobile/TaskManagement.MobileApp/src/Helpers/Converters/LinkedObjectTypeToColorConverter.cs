using System.Globalization;
using TaskManagement.MobileApp.Models;

namespace TaskManagement.MobileApp.Helpers.Converters;

public class LinkedObjectTypeToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is LinkedObjectType type)
        {
            return type switch
            {
                LinkedObjectType.Relation => Application.Current?.Resources["Primary"],
                LinkedObjectType.DamageClaim => Application.Current?.Resources["Red"],
                LinkedObjectType.InsurancePolicy => Application.Current?.Resources["PolicyBrown"],
                _ => Application.Current?.Resources["TextGray"]
            };
        }

        return Application.Current?.Resources["TextGray"];
    }

    /**
     * Must be implemented by the Interface; but doesnt have a use atm.
     */
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}