using System.Collections.Generic;

namespace Shell.PriceCalculator.Contracts
{
    public class Basket
    {
        public IEnumerable<BasketItem> Items { get; set; }
    }
}
