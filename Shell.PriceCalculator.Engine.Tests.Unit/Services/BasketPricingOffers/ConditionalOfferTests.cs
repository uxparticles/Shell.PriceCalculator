using NUnit.Framework;
using Shell.PriceCalculator.Engine.Services.BasktetPricingOffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.PriceCalculator.Engine.Tests.Unit.Services.BasketPricingOffers
{
    [TestFixture]
    public class ConditionalOfferTests
    {
        [Test]
        public void ThatCanSkipItemsThatAreNotUnderOffer()
        {
            var offer = new ConditionalOffer("aaa", 3, "bbb", .05m);

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

        }

        [Test]
        public void ThatCanApplyOfferOnce()
        {
            // 10% offer on bb if i take 3 aaa
            var offer = new ConditionalOffer("aaa", 3, "bbb", .10m);

            var basketItemsByName = new Dictionary<string, BasketItemPrice>()
            {
                {
                    "aaa",
                    new BasketItemPrice {
                                Name="aaa",
                                UnitPrice = 150,
                                Quantity = 3 }
                },
                {
                    "bbb",
                    new BasketItemPrice {
                                Name="bbb",
                                UnitPrice = 100,
                                Quantity = 1 }
                }
            };

            var results = new BasketPricingResults() { Messages = new List<string>() };

            offer.ApplyOffers(basketItemsByName, results);

            var discount = basketItemsByName["bbb"].Discount;
            Assert.That(discount, Is.EqualTo(10));
        }

        [Test]
        public void ThatCanApplyOfferOnceOnly()
        {
            // 10% offer on bb if i take 3 aaa
            var offer = new ConditionalOffer("aaa", 3, "bbb", .10m);

            var basketItemsByName = new Dictionary<string, BasketItemPrice>()
            {
                {
                    "aaa",
                    new BasketItemPrice {
                                Name="aaa",
                                UnitPrice = 150,
                                Quantity = 3 }
                },
                {
                    "bbb",
                    new BasketItemPrice {
                                Name="bbb",
                                UnitPrice = 100,
                                Quantity = 2 }
                }
            };

            var results = new BasketPricingResults() { Messages = new List<string>() };

            offer.ApplyOffers(basketItemsByName, results);

            var discount = basketItemsByName["bbb"].Discount;
            Assert.That(discount, Is.EqualTo(10));
        }

        [Test]
        public void ThatCanApplyOfferMoreThanOnce()
        {
            // 10% offer on bb if i take 3 aaa
            var offer = new ConditionalOffer("aaa", 3, "bbb", .10m);

            var basketItemsByName = new Dictionary<string, BasketItemPrice>()
            {
                {
                    "aaa",
                    new BasketItemPrice {
                                Name="aaa",
                                UnitPrice = 150,
                                Quantity = 6 }
                },
                {
                    "bbb",
                    new BasketItemPrice {
                                Name="bbb",
                                UnitPrice = 100,
                                Quantity = 2 }
                }
            };

            var results = new BasketPricingResults() { Messages = new List<string>() };

            offer.ApplyOffers(basketItemsByName, results);

            var discount = basketItemsByName["bbb"].Discount;
            Assert.That(discount, Is.EqualTo(20));
        }
    }
    }
