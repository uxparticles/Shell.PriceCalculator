using Shell.PriceCalculator.Contracts;
using Shell.PriceCalculator.Engine.Mappers._interfaces;
using System.Collections.Generic;

namespace Shell.PriceCalculator.Engine.Mappers
{
    internal class PricingResultMapper : IPricingResultsMapper
    {
        public PricingResult Map(BasketPricingResults basketPricingResults)
        {
            if (basketPricingResults is null)
            {
                throw new System.ArgumentNullException(nameof(basketPricingResults));
            }

            return new PricingResult
            {
                SubTotal = basketPricingResults.FullPrice,
                Total = basketPricingResults.DiscountedPrice,
                Messages = basketPricingResults.Messages == null ?
                                    null :
                                    new List<string>(basketPricingResults.Messages)
            };
        }
    }
}
