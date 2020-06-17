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
    /// Konwerter do wyświetlenia tylko krótkiej daty dla każdego przelewu w historii przelewów.
    /// </summary>
    public class DateToShortDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Parse(value.ToString()).ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
