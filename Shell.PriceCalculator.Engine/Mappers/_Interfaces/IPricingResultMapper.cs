using Shell.PriceCalculator.Contracts;

namespace Shell.PriceCalculator.Engine.Mappers._interfaces
{
    internal interface IPricingResultsMapper
    {
        /// <summary>
        /// Maps the <see cref="BasketPricingResults"/> into the <see cref="PricingResult"/> contract
        /// </summary>
        /// <param name="basketPricingResults"></param>
        /// <returns></returns>
        PricingResult Map(BasketPricingResults basketPricingResults);
    }
}
