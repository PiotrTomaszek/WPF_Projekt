using Newtonsoft.Json;
using System.IO;

namespace P4_Projekt_2
{
    /// <summary>
    /// Klasa odpowiedzialna za tworzenie instacji theBankViewModel zawierającej już niezbędne dane dla wykresów.
    /// </summary>
    public class theBankLoader
    {
        private theBankVievModel _theBankVievModel;
        public theBankLoader()
        {
            var theBankVievModel = new theBankVievModel();
            theBankVievModel = JsonConvert.DeserializeObject<theBankVievModel>(File.ReadAllText("ClientAllData.json"));
            theBankVievModel.newTransfer = new Transfer();
            theBankVievModel.newDeposit = new SingleDeposit();
            theBankVievModel.CurrencyRatesEURO = Methods.LoadRates<CurrencyClass<CurrencyRates>>("http://api.nbp.pl/api/exchangerates/rates/a/eur/");
            theBankVievModel.CurrencyRatesUSD = Methods.LoadRates<CurrencyClass<CurrencyRates>>("http://api.nbp.pl/api/exchangerates/rates/a/usd/");
            theBankVievModel.CurrencyRatesCHF = Methods.LoadRates<CurrencyClass<CurrencyRates>>("http://api.nbp.pl/api/exchangerates/rates/a/chf/");
            theBankVievModel.CurrencyRatesGBP = Methods.LoadRates<CurrencyClass<CurrencyRates>>("http://api.nbp.pl/api/exchangerates/rates/a/gbp/");
            theBankVievModel.CurrencyPricesEURO = Methods.Load<CurrencyClass<CurrencyPrice>>("https://api.nbp.pl/api/exchangerates/rates/c/eur/?format=json");
            theBankVievModel.CurrencyPricesUSD = Methods.Load<CurrencyClass<CurrencyPrice>>("https://api.nbp.pl/api/exchangerates/rates/c/usd/?format=json");
            theBankVievModel.CurrencyPricesCHF = Methods.Load<CurrencyClass<CurrencyPrice>>("https://api.nbp.pl/api/exchangerates/rates/c/chf/?format=json");
            theBankVievModel.CurrencyPricesGBP = Methods.Load<CurrencyClass<CurrencyPrice>>("https://api.nbp.pl/api/exchangerates/rates/c/gbp/?format=json");
            theBankVievModel.SelectedCurrency = theBankVievModel.CurrencyRatesEURO;
            _theBankVievModel = theBankVievModel;
        }

        public theBankVievModel GetViewModel()
        {
            return _theBankVievModel;
        }
    }
}
