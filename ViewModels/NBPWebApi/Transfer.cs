using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;

namespace P4_Projekt_2
{
    public class Transfer : IDataErrorInfo
    {
        public string NameOfTransfer { get; set; }
        public string AddressOfTransfer { get; set; }
        public string AccountNumberOfTransfer { get; set; }
        public string PriceTransfer { get; set; }
        public DateTime DateOfTransfer { get; set; }


        [JsonIgnore]
        public string Error => this[string.Empty];

        /// <summary>
        /// Walidacja wprowadzanych danych dla wykonywania operacji przelewu.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {

                if (string.IsNullOrWhiteSpace(columnName) || columnName == nameof(NameOfTransfer))
                {
                    if (string.IsNullOrEmpty(NameOfTransfer))
                        return ("Pole nazwa jest wymagane.");

                    if (NameOfTransfer.Length < 7)
                        return "Tytuł przelewu za krótki";

                    if (NameOfTransfer.Length > 20)
                        return "Tytuł przelewu może mieć maksimum 20 znaków";
                }

                if (string.IsNullOrWhiteSpace(columnName) || columnName == nameof(AddressOfTransfer))
                {
                    if (string.IsNullOrEmpty(AddressOfTransfer))
                        return ("Pole adres jest wymagane.");

                }

                if (string.IsNullOrWhiteSpace(columnName) || columnName == nameof(AccountNumberOfTransfer))
                {
                    if (string.IsNullOrEmpty(AccountNumberOfTransfer))
                    {
                        return ("Pole numer konta jest wymagane.");
                    }
                    else if (!AccountNumberOfTransfer.All(char.IsDigit))
                    {
                        return ("Nie wolno podawać liter w numerze konta.");
                    }
                    else if (AccountNumberOfTransfer.Length != 12)
                    {
                        return "Konto bankowe musi posiadać 12 cyfr.";
                    }

                }

                if (string.IsNullOrWhiteSpace(columnName) || columnName == nameof(PriceTransfer))
                {
                    if (string.IsNullOrEmpty(PriceTransfer))
                    {
                        return "Pole kwota jest wymagane.";
                    }

                    bool canConvert = double.TryParse(PriceTransfer.ToString(), out double temporary);

                    if (!canConvert)
                    {
                        return ("Wprowadź tylko cyfry.");
                    }

                    if (temporary < 1.00)
                        return "Minimalna Kwota przelewu to 1 złoty.";

                    if (string.IsNullOrEmpty(PriceTransfer.ToString()))
                    {
                        return ("Pole kwota jest wymagane.");
                    }
                }
                return null;
            }
        }

    }
}
