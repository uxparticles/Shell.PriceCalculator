using Shell.PriceCalculator.Contracts;
using System.Collections.Generic;

namespace Shell.PriceCalculator.Engine
{
    internal interface IBasketPricingEngine
    {
        /// <summary>
        /// Crerate a <see cref="BasketPricingResults"/> for the input <see cref="Basket"/>.
        /// </summary>
        /// <returns></returns>
        /// <param name="basket">A valid basket to price. If the basket is null or empy, error messages will be returned.</param>
        /// <param name="offers">Prices the basket given a set of feers. If null, no offers are provided, the basket will only provide the non discounted prices</param>
        BasketPricingResults PriceBasket(Basket basket, IEnumerable<IBasketPricingOffer> offers);
    }
}