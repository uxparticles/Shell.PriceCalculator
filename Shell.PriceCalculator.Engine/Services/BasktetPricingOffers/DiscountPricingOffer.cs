using System.Collections.Generic;

namespace Shell.PriceCalculator.Engine.Services.BasktetPricingOffers
{
    internal class DiscountPricingOffer : IBasketPricingOffer
    {
        private readonly string basketItem;
        private readonly double percentDiscount;
        private readonly bool canBeCumulated;

        public DiscountPricingOffer(string basketItem, double percentDiscount, bool canBeCumulated)
        {
            this.basketItem = basketItem;
            this.percentDiscount = percentDiscount;
            this.canBeCumulated = canBeCumulated;
        }

        public void ApplyOffers(IDictionary<string, BasketItemPrice> basketItemsByItemName, BasketPricingResults results)
        {
            if (!basketItemsByItemName.TryGetValue(basketItem, out var item))
            {
                return;
            }

            if (!item.UnitPrice.HasValue)
            {
                results.Messages.Add(string.Format(IBasketPricingOffer.Errors.CannotDiscountUnpricedItem, this.basketItem));
                return;
            }

            var price = canBeCumulated ? item.Discount ?? item.UnitPrice : item.UnitPrice;
            var discount = price * (decimal)(percentDiscount) * item.Quantity;
            results.Messages.Add($"{basketItem} {percentDiscount:P2} off: -£{discount}");

            item.Discount = discount;
        }
    }
}

