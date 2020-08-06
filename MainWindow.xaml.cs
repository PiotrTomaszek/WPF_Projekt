using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace P4_Projekt_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private theBankVievModel _theBankVievModel;
        private Dictionary<string, CurrencyClass<CurrencyRates>> _courses;
        private Dictionary<string, UserControl> _myControls;

        public MainWindow(theBankVievModel theBankVievModel)
        {
            InitializeComponent();

            // Dodanie wszystkich kursów do słownika
            _courses = new Dictionary<string, CurrencyClass<CurrencyRates>>();
            _courses.Add("btnEuro", theBankVievModel.CurrencyRatesEURO);
            _courses.Add("btnUSD", theBankVievModel.CurrencyRatesUSD);
            _courses.Add("btnCHF", theBankVievModel.CurrencyRatesCHF);
            _courses.Add("btnGBP", theBankVievModel.CurrencyRatesGBP);

            _myControls = new Dictionary<string, UserControl>();

            _theBankVievModel = theBankVievModel;
            DataContext = _theBankVievModel;

            InitializeAllControls();
            InitializeAllButtonsClicks();
            ChangeControl("btnMojBank");
        }

        /// <summary>
        /// Słownik który wiąże wszystkie kontrolki z odpowiadającymi im stringami.
        /// </summary>
        private void InitializeAllControls()
        {
            _myControls.Add("btnMojBank", uControlMojBank);
            _myControls.Add("btnLokaty", uControlLokaty);
            _myControls.Add("btnSzczegolyLokaty", uControlLokaty);
            _myControls.Add("btnHistoria", uControlHistoria);
            _myControls.Add("btnHistoriaPrzelewow", uControlHistoria);
            _myControls.Add("btnPrzelew", uControlPrzelew);
            _myControls.Add("btnWykonajPrzelew", uControlPrzelew);
            _myControls.Add("btnPomoc", uControlPomoc);
            _myControls.Add("btnKredyty", uControlKredyty);
            _myControls.Add("btnSzczegolyKredyty", uControlKredyty);
            _myControls.Add("btnKursy", uControlKursy);
            _myControls.Add("btnKontakt", uControlKontakt);
        }

        /// <summary>
        /// Metoda która wiąże wszystkie przyciski występujące we wszystkich kontrolkach i 
        /// dodajemy im odpowiednie do kontekstu zdarzenie.
        /// </summary>
        private void InitializeAllButtonsClicks()
        {
            var szczegoly = uControlMojBank.FindName("btnSzczegolyLokaty") as Button;
            szczegoly.Click += ClickHandler;

            var wykonajPrzelewBank = uControlMojBank.FindName("btnWykonajPrzelew") as Button;
            wykonajPrzelewBank.Click += ClickHandler;

            var historiaPrzelewow = uControlMojBank.FindName("btnHistoriaPrzelewow") as Button;
            historiaPrzelewow.Click += ClickHandler;

            var szczegolyKredyty = uControlMojBank.FindName("btnSzczegolyKredyty") as Button;
            szczegolyKredyty.Click += ClickHandler;

            var wykonajPrzelew = uControlPrzelew.FindName("btnWykonajPrzelew") as Button;
            wykonajPrzelew.Click += HandlerForButtonInPrzelew;

            var kursEuro = uControlKursy.FindName("btnEuro") as Button;
            kursEuro.Click += HandlerForButtonsInRates;

            var kursUSD = uControlKursy.FindName("btnUSD") as Button;
            kursUSD.Click += HandlerForButtonsInRates;

            var kursCHF = uControlKursy.FindName("btnCHF") as Button;
            kursCHF.Click += HandlerForButtonsInRates;

            var kursGBP = uControlKursy.FindName("btnGBP") as Button;
            kursGBP.Click += HandlerForButtonsInRates;

            var zalozLokate = uControlLokaty.FindName("btnZalozLokate") as Button;
            zalozLokate.Click += HandlerForButtonInLokaty;

            var wezKredyt = uControlKredyty.FindName("btnWezKredyt") as Button;
            wezKredyt.Click += HandlerForButtonInKredyty;
        }

        /// <summary>
        /// Handler dla operacji dodawania nowego kredytu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlerForButtonInKredyty(object sender, RoutedEventArgs e)
        {
            // Zainicjalizowanie danych niezbędnych do stworzenia kredytu.
            var percent = 18.89;
            var date = ((ComboBox)uControlKredyty.FindName("DateComboBox")).SelectionBoxItem.ToString();
            var temp = ((ComboBox)uControlKredyty.FindName("PriceComboBox")).SelectionBoxItem.ToString();
            var price = double.Parse(temp.Substring(0, temp.Length - 2));

            // Dodanie śródków na konto klienta.
            _theBankVievModel.ClientMoney += price;

            // Tworzenie nowej instacji Kredytu.
            var newCredit = new SingleCredit
            {
                WhatPercent = percent,
                Cash = price.ToString(CultureInfo.CreateSpecificCulture("en-US")),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddMonths(int.Parse(date.Split(' ').FirstOrDefault()))
            };

            // Tworzy nową zmienną która zastępuje dotychczasową listę inną listą rozszerzoną o nową instacje kredytu.
            var listOfCredits = _theBankVievModel.Credits.ToList<SingleCredit>();
            listOfCredits.Add(newCredit);
            _theBankVievModel.Credits = listOfCredits;

            // Zapis do JSONa.
            SaveJSONFile(_theBankVievModel);

            // Zmiana kontrolki.
            ChangeControl("btnKredyty");
        }

        /// <summary>
        /// Handler do obsługu operacji dodawania nowej lokaty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlerForButtonInLokaty(object sender, RoutedEventArgs e)
        {
            // Pobranie danych wymaganych dla przelewu z kontrolki.
            var percent = ((ComboBox)uControlLokaty.FindName("PercentComboBox")).SelectionBoxItem.ToString();
            var date = ((ComboBox)uControlLokaty.FindName("DateComboBox")).SelectionBoxItem.ToString();
            var price = ((TextBox)uControlLokaty.FindName("KwotaLokaty")).Text;

            // Sprawdzanie czy klient posiada odpowiednia sumę pieniędzy by traksakcja się wykonała.
            if (double.TryParse(price, NumberStyles.Any, new CultureInfo("pl-PL"), out double result))
            {
                if (_theBankVievModel.ClientMoney < result)
                {
                    MessageBox.Show("Brak środków na koncie bankowym.", "theBank - brak środków");
                    return;
                }
                // Odejmij tę sumę z konta klienta.
                _theBankVievModel.ClientMoney -= result;
            }

            // Tworzenie nowego obiektu Lokaty.
            var newDeposit = new SingleDeposit
            {
                WhatPercent = double.Parse(percent.Substring(0, percent.Length - 1)),
                Cash = price.ToString(CultureInfo.CreateSpecificCulture("de-DE")),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddMonths(int.Parse(date.Split(' ').FirstOrDefault()))
            };

            // Tworzy nową zmienną która zastępuje dotychczasową listę inną listą rozszerzoną o nową instacje lokaty.
            var listOfDeposits = _theBankVievModel.Deposits.ToList<SingleDeposit>();
            listOfDeposits.Add(newDeposit);
            _theBankVievModel.Deposits = listOfDeposits;

            // Zapisz JSONa.
            SaveJSONFile(_theBankVievModel);

            // Zmiana kontrolki.
            ChangeControl("btnLokaty");
        }

        /// <summary>
        /// Metoda odpowiedzialna za zmiane wybranego kursu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlerForButtonsInRates(object sender, RoutedEventArgs e)
        {
            var whichRate = ((Button)sender).Name;

            // Znajdź kurs który klient chce wyświetlić.
            var temp = _courses.Where(x => x.Key.Equals(whichRate)).FirstOrDefault();

            // Jeżeli to nie jest ten sam kurs to nie zmieniaj go.
            if (_theBankVievModel.SelectedCurrency.Code != temp.Value.Code)
                _theBankVievModel.SelectedCurrency = temp.Value;

        }

        /// <summary>
        /// Asyncowa metoda która odpowiedzialna jest za operację wykonywania przelewu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HandlerForButtonInPrzelew(object sender, RoutedEventArgs e)
        {
            // Pobieranie wszystkich wprowadzonych danych potrzebnych do przelewu.
            var person = ((TextBox)uControlPrzelew.FindName("PrzelewOdbiorca")).Text;
            var price = ((TextBox)uControlPrzelew.FindName("PrzelewKwota")).Text;
            var accountNumber = ((TextBox)uControlPrzelew.FindName("PrzelewNumerKonta")).Text;
            var adress = ((TextBox)uControlPrzelew.FindName("PrzelewAdres")).Text;

            // Sprawdzanie czy klient posiada odpowiednia sumę pieniędzy by traksakcja się wykonała.
            if (double.TryParse(price, out double result))
            {
                if (_theBankVievModel.ClientMoney < result)
                {
                    MessageBox.Show("Brak środków na koncie bankowym.", "theBank - brak środków");
                    return;
                }
                // Odejmij tę sumę z konta klienta.
                _theBankVievModel.ClientMoney -= result;
            }

            // Tworzenie nowego obiektu przelewu.
            var newTransfer = new Transfer
            {
                NameOfTransfer = person,
                AccountNumberOfTransfer = accountNumber,
                AddressOfTransfer = adress,
                DateOfTransfer = DateTime.Now,
                PriceTransfer = price
            };

            // Tworzy nową zmienną która zastępuje dotychczasową listę inną listą rozszerzoną o nową instacje transakcji.
            var listOfTransfers = _theBankVievModel.Transfers.ToList<Transfer>();
            listOfTransfers.Add(newTransfer);
            _theBankVievModel.Transfers = listOfTransfers;

            // Zapisywanie do pliku JSON.
            SaveJSONFile(_theBankVievModel);

            // Task który stwarza iluzję pracy programu.
            await Task.Run(() =>
            {
                Thread.Sleep(500);
                _theBankVievModel.newTransfer = new Transfer();
            });

            // Zmiana kontrolki.
            ChangeControl("btnMojBank");
        }

        /// <summary>
        /// Aktualizacja JSONa na którym działa bank, żeby miał zawsze zapisane aktualne dane.
        /// </summary>
        /// <param name="client"></param>
        private void SaveJSONFile(theBankVievModel client)
        {
            var saveFile = JsonConvert.SerializeObject(client, Formatting.Indented);
            File.WriteAllText("ClientAllData.json", saveFile);
        }

        /// <summary>
        /// Handler które zadaniem jest wywołanie metody do zmiany wyświetlanej kontrolki.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClickHandler(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ChangeControl(button.Name);
        }

        /// <summary>
        /// Metoda która jest odpowiedzialnia za zmienianie wyświetlanych kontrolek,
        /// parametrem jest string za pomocą którego jest szukanie odpowiedniej kontrolki i jej wyświetlenie.
        /// </summary>
        /// <param name="nameOfControl"></param>
        public void ChangeControl(string nameOfControl)
        {
            // Ustaw wszystkie kontrolki na Hidden po to żeby tylko wybrana była wyświetlona.
            uControlKontakt.Visibility = Visibility.Hidden;
            uControlPomoc.Visibility = Visibility.Hidden;
            uControlHistoria.Visibility = Visibility.Hidden;
            uControlLokaty.Visibility = Visibility.Hidden;
            uControlMojBank.Visibility = Visibility.Hidden;
            uControlKredyty.Visibility = Visibility.Hidden;
            uControlKursy.Visibility = Visibility.Hidden;
            uControlPrzelew.Visibility = Visibility.Hidden;

            if (nameOfControl.Equals("btnWyloguj"))
                Application.Current.Shutdown();

            if (nameOfControl != null && !nameOfControl.Equals("btnWyloguj"))
            {
                var temp = _myControls.Where(key => key.Key.Equals(nameOfControl)).FirstOrDefault().Value;
                temp.Visibility = Visibility.Visible;
            }
        }
    }
}
