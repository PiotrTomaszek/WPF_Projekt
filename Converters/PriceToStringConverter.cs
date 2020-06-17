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

            if (!(double.TryParse(value.ToString(), out double temp)))
                return null;

            var tempStr = string.Empty;

            tempStr = Math.Round(double.Parse(temp.ToString()), 2).ToString();

            if (temp % 1 == 0)
                tempStr = $"{temp},00";

            return $"{tempStr.ToString(CultureInfo.CurrentCulture)}zł";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
