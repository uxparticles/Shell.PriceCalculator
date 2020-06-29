using System;
using System.Collections.Generic;

namespace Shell.PriceCalculator.Engine.Services.BasktetPricingOffers
{
    internal class ConditionalOffer : IBasketPricingOffer
    {
        private readonly string conditionalItem;
        private readonly int quantity;
        private readonly string offer;
        private readonly decimal discount;

        public ConditionalOffer(string conditionalItem, int quantity, string offer, decimal discount)
        {
            this.conditionalItem = conditionalItem;
            this.quantity = quantity;
            this.offer = offer;
            this.discount = discount;
        }

        public void ApplyOffers(IDictionary<string, BasketItemPrice> basketItemsByItemName, BasketPricingResults results)
        {
            var conditionalItemFound = basketItemsByItemName.TryGetValue(conditionalItem, out var basketItem) && basketItem.Quantity >= quantity;
            if (!conditionalItemFound)
            {
                return;
            }

            var canApplyOffers = basketItemsByItemName.TryGetValue(offer, out var basketItemToDiscount);
            if (!canApplyOffers)
            {
                return;
            }

            if (!basketItemToDiscount.UnitPrice.HasValue)
            {
                results.Messages.Add(string.Format(IBasketPricingOffer.Errors.CannotDiscountUnpricedItem));
                return;
            }


            var itemsICanDiscount = basketItem.Quantity / quantity;
            if (itemsICanDiscount >= basketItemToDiscount.Quantity)
            {
                // all
                basketItemToDiscount.Discount = basketItemToDiscount.Quantity * basketItemToDiscount.UnitPrice.Value * discount;
                results.Messages.Add($"All {offer} received {discount:P2} discount: -£{basketItemToDiscount.Discount}");
                return;
            }

            // only a smaller number
            basketItemToDiscount.Discount = itemsICanDiscount * basketItemToDiscount.UnitPrice * discount;
            results.Messages.Add($"{itemsICanDiscount} {offer}s received {discount:P2} discount: -£{basketItemToDiscount.Discount}");
            return;
        }
    }
}