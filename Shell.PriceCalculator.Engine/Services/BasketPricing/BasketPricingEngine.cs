using Shell.PriceCalculator.Contracts;
using Shell.PriceCalculator.Engine.Repositories._Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shell.PriceCalculator.Engine
{
    internal class BasketPricingEngine : IBasketPricingEngine
    {
        private readonly IItemPriceRepository priceRepository;

        public class Errors
        {
            public const string BasketIsNull = "The basket is null";

            public const string BasketIsEmpty = "Basket is empty";

            public const string PriceUnavailable = "Price for item {0} is unavailable";
            
            public const string NoOffersAvailable = "(No offers available)";
        }

        public BasketPricingEngine(IItemPriceRepository priceRepository)
        {
            this.priceRepository = priceRepository ?? throw new ArgumentNullException(nameof(priceRepository));
        }

        public BasketPricingResults PriceBasket(Basket basket, IEnumerable<IBasketPricingOffer> offers = null)
        {
            var results = new BasketPricingResults { ItemCount = 0 };

            var isValidationFailed = this.ValidateBasket(basket, results);
            if (isValidationFailed)
            {
                return results;
            }

            var isPricingFailed = this.TryToPriceBasket(basket, results);
            this.CalculateTotalPrice(results);
            if (isPricingFailed)
            {
                return results;
            }

            this.TryToApplySpecialOffers(basket, results, offers);
            this.CalculateDiscountedPrices(results);

            if (results.FullPrice == results.DiscountedPrice)
            {
                results.Messages.Add(Errors.NoOffersAvailable);
            }

            return results;
        }

        private bool TryToPriceBasket(Basket basket, BasketPricingResults results)
        {
            var pricesToAsk = basket.Items.Select(x => x.Name).Distinct(StringComparer.OrdinalIgnoreCase);
            var prices = this.priceRepository.GetItemPrices(pricesToAsk);
            var pricesByName = prices.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            results.ItemPrices = new List<BasketItemPrice>();
            var isFailed = false;

            var itemCount = 0;
            foreach (var item in basket.Items.GroupBy(x => x.Name))
            {
                var itemName = item.Key;
                var quantity = item.Count();
                var itemPrice = pricesByName.TryGetValue(itemName, out var price) ? price.Price : default(decimal?);
                var baskItemPrice =
               new BasketItemPrice
               {
                   Name = item.Key,
                   UnitPrice = itemPrice,
                   Quantity = quantity
               };

                itemCount += quantity;
                results.ItemPrices.Add(baskItemPrice);

                if (!baskItemPrice.UnitPrice.HasValue)
                {
                    results.Messages.Add(string.Format(Errors.PriceUnavailable, baskItemPrice.Name));
                    isFailed = true;
                }
            }

            results.ItemCount = itemCount;
            
            return isFailed;
        }

        private bool ValidateBasket(Basket basket, BasketPricingResults results)
        {
            var isFailed = false;
            
            if (results.Messages == null)
            {
                results.Messages = new List<string>();
            }

            if (basket is null)
            {
                results.Messages.Add(Errors.BasketIsNull);
                return true;
            }

            if (basket.Items == null || !basket.Items.Any())
            {
                results.Messages.Add(Errors.BasketIsEmpty);
                isFailed = true;
            }

            return isFailed;
        }

        private void TryToApplySpecialOffers(Basket basket, BasketPricingResults results, IEnumerable<IBasketPricingOffer> offers)
        {
            if (offers  != null && offers.Any())
            {
                var itemsByName = results.ItemPrices.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

                foreach (var item in offers)
                {
                    item.ApplyOffers(itemsByName, results);
                }
            }            
        }

        private void CalculateTotalPrice(BasketPricingResults results)
        {
            var totalPrice = results.ItemPrices.Where(x => x.UnitPrice.HasValue).Select(this.CalculatePrice).Sum();            
            results.FullPrice = totalPrice;
        }

        private decimal CalculatePrice(BasketItemPrice basketItemPrice)
        {
            var price = basketItemPrice.UnitPrice.Value * basketItemPrice.Quantity;
            return price;
        }

        private void CalculateDiscountedPrices(BasketPricingResults results)
        {
            var discount = 0m;
            foreach (var item in results.ItemPrices)
            {
                discount += item.Discount??0;
            }

            results.DiscountedPrice = results.FullPrice - discount;
        }          
    }
}

