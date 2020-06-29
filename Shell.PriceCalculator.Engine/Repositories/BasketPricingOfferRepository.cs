using Shell.PriceCalculator.Engine.Repositories._Interfaces;
using System;
using System.Collections.Generic;

namespace Shell.PriceCalculator.Engine.Repositories
{
    internal class BasketPricingOfferRepository : IBasketPricingOfferRepository
    {
        private IEnumerable<IBasketPricingOffer> currentOffers;

        public IEnumerable<IBasketPricingOffer> GetCurrentOffers()
        {
            return this.currentOffers;
        }

        public void Setup(IEnumerable<IBasketPricingOffer> currentOffers)
        {
            if (currentOffers is null)
            {
                throw new ArgumentNullException(nameof(currentOffers));
            }

            this.currentOffers = currentOffers;
        }
    }
}


