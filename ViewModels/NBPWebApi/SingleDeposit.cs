using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace P4_Projekt_2
{
    public class SingleDeposit : ISingleParameters, IDataErrorInfo
    {
        public string Cash { get; set; }
        public double WhatPercent { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string this[string columnName]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(columnName) || columnName == nameof(Cash))
                {
                    if (string.IsNullOrEmpty(Cash))
                    {
                        return "Pole kwota jest wymagane.";
                    }

                    bool canConvert = double.TryParse(Cash, out double temporary);

                    if (!canConvert)
                    {
                        return ("Wprowadź tylko cyfry.");
                    }
                }

                return null;
            }
        }

        [JsonIgnore]
        public string Error => this[string.Empty];
    }
}
