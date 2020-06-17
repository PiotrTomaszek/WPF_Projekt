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
    /// Konwerter służący do znalezienia ostatniej wartości z pobranych kursów (aktualnej ceny) i wyświetlenie jej.
    /// </summary>
    public class RatesToLastElementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is IEnumerable<CurrencyRates>))
                return null;

            if (targetType != typeof(string))
                return null;

            // Zwraca aktulną cenę. CultureInfor wymagane, ponieważ separatorem była "." a wymagany był ",".
            return ((IEnumerable<CurrencyRates>)value).LastOrDefault().Mid.ToString(CultureInfo.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
