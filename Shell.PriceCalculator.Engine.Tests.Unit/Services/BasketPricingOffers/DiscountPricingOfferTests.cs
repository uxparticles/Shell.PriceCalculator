using NUnit.Framework;
using Shell.PriceCalculator.Engine.Services.BasktetPricingOffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.PriceCalculator.Engine.Tests.Unit.Services.BasketPricingOffers
{
    [TestFixture]
    public class DiscountPricingOfferTests
    {
        [Test]
        public void ThatCanSkipItemsThatAreNotUnderOffer()
        {
            var offer = new DiscountPricingOffer("test", .1, true);

            var basketItemsByName = new Dictionary<string, BasketItemPrice>();
            var results = new BasketPricingResults() { Messages = new List<string>() };

            offer.ApplyOffers(basketItemsByName, results);

            Assert.That(results.Messages, Is.Empty);
        }

        [Test]
        public void ThatCanApplyDiscountsOnASingleItem()
        {
            var offer = new DiscountPricingOffer("test", .1, true);

            var basketItemsByName = new Dictionary<string, BasketItemPrice>()
            {
                {
                    "test",
                    new BasketItemPrice {
                                Name="test",
                                UnitPrice = 150,
                                Quantity = 1 }
                }
            };

            var results = new BasketPricingResults() { Messages = new List<string>() };

            offer.ApplyOffers(basketItemsByName, results);

            var appliedDiscount = basketItemsByName["test"].Discount;
            Assert.That(appliedDiscount, Is.EqualTo(15));
        }

        [Test]
        public void ThatCanApplyDiscountsOnAllMatchingItems()
        {
            var offer = new DiscountPricingOffer("test", .1, true);

            var basketItemsByName = new Dictionary<string, BasketItemPrice>()
            {
                {
                    "test",
                    new BasketItemPrice {
                                Name="test",
                                UnitPrice = 150,
                                Quantity = 10 }
                }
            };

            var results = new BasketPricingResults() { Messages = new List<string>() };

            offer.ApplyOffers(basketItemsByName, results);

            var appliedDiscount = basketItemsByName["test"].Discount;
            Assert.That(appliedDiscount, Is.EqualTo(150));
        }

        [Test]
        public void ThatSkipsUnpricedItems()
        {
            var offer = new DiscountPricingOffer("test", .1, true);

            var basketItemsByName = new Dictionary<string, BasketItemPrice>()
            {
                {
                    "test",
                    new BasketItemPrice {
                                Name="test",
                                Quantity = 10 }
                }
            };

            var results = new BasketPricingResults() { Messages = new List<string>() };

            offer.ApplyOffers(basketItemsByName, results);

            var appliedDiscount = basketItemsByName["test"].Discount;
            Assert.That(appliedDiscount, Is.Null);
        }
    }
}
