using System.Collections.Generic;

namespace P4_Projekt_2
{
    public class CurrencyClass<T>
    {
        public string Currency { get; set; }
        public string Code { get; set; }
        public IEnumerable<T> Rates { get; set; }
    }
}
