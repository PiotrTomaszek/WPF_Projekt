using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace P4_Projekt_2
{
    public class theBankVievModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Pola dla pliku JSON.
        public string Login { get; set; }
        public string Alias { get; set; }
        public double ClientMoney { get; set; }
        public IEnumerable<SingleDeposit> Deposits { get; set; }
        public IEnumerable<SingleCredit> Credits { get; set; }
        public IEnumerable<Transfer> Transfers { get; set; }


        [JsonIgnore]
        public double DepositsMoney { get => Methods.LoadMoney(Deposits); }
        [JsonIgnore]
        public double CreditsMoney { get => Methods.LoadMoney(Credits); }


        // Pola do pobrania aktualnych danych z NBP.
        [JsonIgnore]
        public CurrencyClass<CurrencyRates> CurrencyRatesEURO { get; set; }
        [JsonIgnore]
        public CurrencyClass<CurrencyRates> CurrencyRatesUSD { get; set; }
        [JsonIgnore]
        public CurrencyClass<CurrencyRates> CurrencyRatesCHF { get; set; }
        [JsonIgnore]
        public CurrencyClass<CurrencyRates> CurrencyRatesGBP { get; set; }
        [JsonIgnore]
        public CurrencyClass<CurrencyPrice> CurrencyPricesEURO { get; set; }
        [JsonIgnore]
        public CurrencyClass<CurrencyPrice> CurrencyPricesUSD { get; set; }
        [JsonIgnore]
        public CurrencyClass<CurrencyPrice> CurrencyPricesCHF { get; set; }
        [JsonIgnore]
        public CurrencyClass<CurrencyPrice> CurrencyPricesGBP { get; set; }


        // Pomocnicze pola dla prawidłowej funkcjonalności aplikacji.
        [JsonIgnore]
        public CurrencyClass<CurrencyRates> SelectedCurrency { get; set; }
        [JsonIgnore]
        public Transfer newTransfer { get; set; }
        [JsonIgnore]
        public SingleDeposit newDeposit { get; set; }
    }
}
