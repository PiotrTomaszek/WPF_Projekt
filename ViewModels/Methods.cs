using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace P4_Projekt_2
{
    public static class Methods
    {
        /// <summary>
        /// Metoda Która odpowiedzialna jest za ładowanie danych o cenie zakupu i cenie sprzedaży.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="urlToLoad"></param>
        /// <returns></returns>
        public static T Load<T>(string urlToLoad)
        {
            var currData = new RestClient(urlToLoad);
            var request = new RestRequest();
            var response = currData.Execute<T>(request);

            if (response.IsSuccessful)
                return response.Data;
            else
                return default(T);
        }

        /// <summary>
        /// Metoda która ładuje cały kurs danej waluty przez rok trwania.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static T LoadRates<T>(string prefix)
        {
            //Ustawia wartości podstawowe dla jednego roku.
            var dateNow = DateTime.Now.AddDays(-1);
            var datePast = DateTime.Now.AddDays(-1).AddYears(-1);

            // Domyślne wartości dla dnia i miesiąca.
            var monthPast = datePast.Month.ToString();
            var monthNow = dateNow.Month.ToString();
            var dayPast = datePast.Day.ToString();
            var dayNow = dateNow.Day.ToString();

            // Sprawdza czy zmienna posiada wartośc np 4 i zmienia na wartość 04.
            if (dateNow.Month < 10)
                monthNow = $"0{dateNow.Month}";
            if (datePast.Month < 10)
                monthPast = $"0{datePast.Month}";
            if (datePast.Day < 10)
                dayPast = $"0{datePast.Day}";
            if (dateNow.Day < 10)
                dayNow = $"0{dateNow.Day}";

            // Skleja linka i pobiera z NBP.
            var linkApi = prefix + $"{datePast.Year}-{monthPast}-{dayPast}/{dateNow.Year}-{monthNow}-{dayNow}/?format=json";
            var loadedData = Methods.Load<T>(linkApi);

            return loadedData;
        }

        // Załaduje całkowitą kwotę dla wybranego pola.
        public static double LoadMoney(IEnumerable<ISingleParameters> single)
        {
            var money = 0.0;
            foreach (var item in single)
            {
                double.TryParse(item.Cash, NumberStyles.Any, new CultureInfo("pl-PL"), out double result);
                money += result;
            }
            return money;
        }
    }
}

