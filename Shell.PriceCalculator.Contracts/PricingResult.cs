using System.Collections.Generic;

namespace Shell.PriceCalculator.Contracts
{
    public class PricingResult
    {
        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

       public ICollection<string> Messages { get; set; }
    }
}
