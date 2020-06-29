using Moq;
using NUnit.Framework;
using Shell.PriceCalculator.Contracts;
using Shell.PriceCalculator.Engine.Repositories._Interfaces;
using System.Collections.Generic;

namespace Shell.PriceCalculator.Engine.Tests.Unit.Services.BasketPricing
{
    [TestFixture]
    public class BasketPricingEngineTests
    {
        internal class BasketPricingEngineContext
        {
            public BasketPricingEngine BasketPricingEngine { get; set; }
            internal Mock<IItemPriceRepository> MockPriceRepository { get; set; }
        }

        private static BasketPricingEngineContext GetBasketPricingEngine()
        {
            var priceRepository = new Mock<IItemPriceRepository>();

            var bpe = new BasketPricingEngine(priceRepository.Object);

            return new BasketPricingEngineContext
            {
                BasketPricingEngine = bpe,
                MockPriceRepository = priceRepository
            };
        }

        public class ComputePrice
        {
            [Test]
            public void ThatCanComputePriceOfEmptyBasket()
            {
                var pricingEngine = GetBasketPricingEngine().BasketPricingEngine;
                var pricingResult = pricingEngine.PriceBasket(new Basket());


                Assert.That(pricingResult.ItemCount, Is.EqualTo(0));
            }

            [Test]
            public void ThatCanComputePriceOfSimpleBasket()
            {
                var pricingEngineContext = GetBasketPricingEngine();
                pricingEngineContext.MockPriceRepository.Setup(x => x.GetItemPrices(It.IsAny<IEnumerable<string>>())).Returns(new[] { new ItemPrice { Price = 123.0m, Name = "item1" } });

                var pricingEngine = pricingEngineContext.BasketPricingEngine;
                var basket = new Basket()
                {
                    Items = new[] { new BasketItem { Name = "item1" } }
                };

                var pricingResult = pricingEngine.PriceBasket(basket);

                Assert.That(pricingResult.ItemCount, Is.EqualTo(1));
                Assert.That(pricingResult.FullPrice, Is.EqualTo(123));
            }

            [Test]
            public void ThatCanComputeRepetitions()
            {
                var pricingEngineContext = GetBasketPricingEngine();
                pricingEngineContext.MockPriceRepository.Setup(x => x.GetItemPrices(It.IsAny<IEnumerable<string>>())).Returns(new[] { new ItemPrice { Price = 5m, Name = "item1" } });

                var pricingEngine = pricingEngineContext.BasketPricingEngine;
                var basket = new Basket()
                {
                    Items = new[] { new BasketItem { Name = "item1" }, new BasketItem { Name = "item1" }, new BasketItem { Name = "item1" } }
                };

                var pricingResult = pricingEngine.PriceBasket(basket);

                Assert.That(pricingResult.ItemCount, Is.EqualTo(3));
                Assert.That(pricingResult.FullPrice, Is.EqualTo(15));
            }

            [Test]
            public void ThatBasketMatchingIsCaseInsensitive()
            {
                var pricingEngineContext = GetBasketPricingEngine();
                pricingEngineContext.MockPriceRepository.Setup(x => x.GetItemPrices(It.IsAny<IEnumerable<string>>())).Returns(new[] { new ItemPrice { Price = 5m, Name = "item1" } });

                var pricingEngine = pricingEngineContext.BasketPricingEngine;
                var basket = new Basket()
                {
                    Items = new[] { new BasketItem { Name = "item1" }, new BasketItem { Name = "ITEM1" } }
                };

                var pricingResult = pricingEngine.PriceBasket(basket);

                Assert.That(pricingResult.ItemCount, Is.EqualTo(2));
                Assert.That(pricingResult.FullPrice, Is.EqualTo(10));
            }

            [Test]
            public void ThatFailsForMissingPrice()
            {
                var pricingEngineContext = GetBasketPricingEngine();
                pricingEngineContext.MockPriceRepository.Setup(x => x.GetItemPrices(It.IsAny<IEnumerable<string>>())).Returns(new[] { new ItemPrice { Price = 5m, Name = "item1" } });

                var pricingEngine = pricingEngineContext.BasketPricingEngine;
                var basket = new Basket()
                {
                    Items = new[] { new BasketItem { Name = "item1" }, new BasketItem { Name = "item2" } }
                };

                var pricingResult = pricingEngine.PriceBasket(basket);

                Assert.That(pricingResult.ItemCount, Is.EqualTo(2));
                Assert.That(pricingResult.FullPrice, Is.EqualTo(5));

                CollectionAssert.Contains(pricingResult.Messages, string.Format(BasketPricingEngine.Errors.PriceUnavailable, "item2"));
            }
        }

        public class Discounts
        {
            [Test]
            public void ThatInvokesDiscountPipelineIfDiscountsAreAvailable()
            {
                var discountOffer = new Mock<IBasketPricingOffer>();
                var pricingEngineContext = GetBasketPricingEngine();
                pricingEngineContext.MockPriceRepository.Setup(x => x.GetItemPrices(It.IsAny<IEnumerable<string>>())).Returns(new[] { new ItemPrice { Price = 5m, Name = "item1" } });

                var pricingEngine = pricingEngineContext.BasketPricingEngine;
                var basket = new Basket()
                {
                    Items = new[] { new BasketItem { Name = "item1" }, new BasketItem { Name = "item1" }, new BasketItem { Name = "item1" } }
                };

                var pricingResult = pricingEngine.PriceBasket(basket, new[] { discountOffer.Object });
                discountOffer.Verify(x => x.ApplyOffers(It.IsAny<IDictionary<string, BasketItemPrice>>(), It.IsAny<BasketPricingResults>()), Times.Once);
            }

            [Test]
            public void ThatFinalPriceIsTakenEvenWhenNoDsicountsAreApplied()
            {
                var pricingEngineContext = GetBasketPricingEngine();
                pricingEngineContext.MockPriceRepository.Setup(x => x.GetItemPrices(It.IsAny<IEnumerable<string>>())).Returns(new[] { new ItemPrice { Price = 5m, Name = "item1" } });

                var pricingEngine = pricingEngineContext.BasketPricingEngine;
                var basket = new Basket()
                {
                    Items = new[] { new BasketItem { Name = "item1" } }
                };

                var pricingResult = pricingEngine.PriceBasket(basket);

                Assert.That(pricingResult.DiscountedPrice, Is.EqualTo(pricingResult.FullPrice));
            }
        }

        public class Validation
        {
            [Test]
            public void ThatWarnsForNullArgument()
            {
                var pricingEngine = GetBasketPricingEngine().BasketPricingEngine;
                var pricingResult = pricingEngine.PriceBasket(null);

                CollectionAssert.Contains(pricingResult.Messages, BasketPricingEngine.Errors.BasketIsNull);
            }

            [Test]
            public void ThatWarnsIfTheBasketIsEmpty()
            {
                var pricingEngine = GetBasketPricingEngine().BasketPricingEngine;
                var pricingResult = pricingEngine.PriceBasket(new Basket());

                CollectionAssert.Contains(pricingResult.Messages, BasketPricingEngine.Errors.BasketIsEmpty);
            }
        }
    }
}