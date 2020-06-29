using System.Collections.Generic;

namespace Shell.PriceCalculator.Engine.Repositories._Interfaces
{
    internal interface IBasketPricingOfferRepository
    {
        /// <summary>
        /// Retrieves the current available offers
        /// </summary>
        /// <returns>The currently available offers. Null if there are no offers</returns>
        IEnumerable<IBasketPricingOffer> GetCurrentOffers();

        /// <summary>
        /// Setup the offers
        /// </summary>      
        void Setup(IEnumerable<IBasketPricingOffer> currentOffers);

    }
}


