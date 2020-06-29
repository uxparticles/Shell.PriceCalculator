using System.Collections.Generic;

namespace Shell.PriceCalculator.Engine
{
    internal interface IBasketPricingOffer
    {
        public class Errors
        {
            public const string CannotDiscountUnpricedItem = "Item '{0}' cannot be discounted because it has no price";
        }
        void ApplyOffers(IDictionary<string, BasketItemPrice> basketItemsByItemName, BasketPricingResults results);
    }
}

