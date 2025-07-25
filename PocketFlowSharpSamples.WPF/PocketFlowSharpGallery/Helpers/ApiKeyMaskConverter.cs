using System;
using System.Globalization;
using System.Windows.Data;

namespace PocketFlowSharpGallery.Helpers;

public class ApiKeyMaskConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string apiKey && !string.IsNullOrEmpty(apiKey))
        {
            // Mask all characters except the first 3 and last 4
            if (apiKey.Length <= 7)
            {
                return new string('*', apiKey.Length);
            }
            
            string prefix = apiKey.Substring(0, 3);
            string suffix = apiKey.Substring(apiKey.Length - 4);
            string masked = new string('*', apiKey.Length - 7);
            
            return $"{prefix}{masked}{suffix}";
        }
        
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // We don't need to convert back
        return value;
    }
}