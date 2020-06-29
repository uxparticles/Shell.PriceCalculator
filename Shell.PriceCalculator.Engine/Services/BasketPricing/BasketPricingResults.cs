using System.Collections.Generic;

namespace Shell.PriceCalculator.Engine
{
    internal class BasketPricingResults
    {
        public ICollection<string> Messages { get; set; }

        public ICollection<BasketItemPrice> ItemPrices { get; set; }

        internal int ItemCount { get; set; }

        internal decimal FullPrice { get; set; }

        internal decimal DiscountedPrice { get; set; }
    }
}

