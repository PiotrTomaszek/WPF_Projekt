using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;

namespace P4_Projekt_2.Converters
{
    /// <summary>
    /// Konwerter pobierający cały DataContext i zmieniający wyświetlone ceny zakupu i sprzedaży.
    /// Obsługuje on zarówno pobieranie "Ask" jak i "Bid" przez warunek sprawdzający przesłany parametr.
    /// </summary>
    public class DataContextToFindPricesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is theBankVievModel))
                return null;

            if (!(values[1] is string))
                return null;

            if (targetType != typeof(string))
                return null;

            var theBankModel = values[0] as theBankVievModel;
            var selectedCode = theBankModel.SelectedCurrency;
            var helper = default(CurrencyPrice);


            if (theBankModel.CurrencyRatesCHF.Code == selectedCode.Code)
                helper = (theBankModel.CurrencyPricesCHF.Rates as IEnumerable<CurrencyPrice>).FirstOrDefault();
            else if (theBankModel.CurrencyRatesEURO.Code == selectedCode.Code)
                helper = (theBankModel.CurrencyPricesEURO.Rates as IEnumerable<CurrencyPrice>).FirstOrDefault();
            else if (theBankModel.CurrencyRatesUSD.Code == selectedCode.Code)
                helper = (theBankModel.CurrencyPricesUSD.Rates as IEnumerable<CurrencyPrice>).FirstOrDefault();
            else if (theBankModel.CurrencyRatesGBP.Code == selectedCode.Code)
                helper = (theBankModel.CurrencyPricesGBP.Rates as IEnumerable<CurrencyPrice>).FirstOrDefault();

            if (int.Parse(parameter.ToString()) == 1)
                return Math.Round(helper.Ask, 4).ToString();
            else
                return Math.Round(helper.Bid, 4).ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
