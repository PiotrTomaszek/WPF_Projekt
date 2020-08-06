using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace P4_Projekt_2.Converters
{
    /// <summary>
    /// Konwerter do poprawnego wyświetlania kwoty przelewów. 
    /// </summary>
    public class PriceToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string || value is double))
                return null;

            string[] arr = value.ToString().Split(new[] { '.', ',' });
            string temp = string.Empty;

            if (arr.Length == 1)
                temp = "00";
            else
                temp = arr[1];

            return $"{arr[0]}{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}{double.Parse(temp) % 100}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
