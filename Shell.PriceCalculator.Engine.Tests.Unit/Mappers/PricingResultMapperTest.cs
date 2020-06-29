using NUnit.Framework;
using Shell.PriceCalculator.Engine.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.PriceCalculator.Engine.Tests.Unit.Mappers
{
    [TestFixture]
    public class PricingResultMapperTest
    {
        [Test]
        public void ThatThrowsForNullItem()
        {
            var mapper = new PricingResultMapper();
            Assert.Throws<ArgumentNullException>(() => mapper.Map(null));
        }

        [Test]
        public void ThatMapsProperties()
        {
            var mapper = new PricingResultMapper();
            var resultToMapFrom = new BasketPricingResults()
            {
                FullPrice = 100,
                DiscountedPrice = 50
            };

            var mappedResult = mapper.Map(resultToMapFrom);

            Assert.That(mappedResult.SubTotal, Is.EqualTo(resultToMapFrom.FullPrice));
            Assert.That(mappedResult.Total, Is.EqualTo(resultToMapFrom.DiscountedPrice));
            CollectionAssert.AreEqual(resultToMapFrom.Messages, mappedResult.Messages);
        }

        [Test]
        public void ThatMapsCollection()
        {
            var mapper = new PricingResultMapper();
            var resultToMapFrom = new BasketPricingResults()
            {                
                Messages = new[] {"test"}
            };

            var mappedResult = mapper.Map(resultToMapFrom);

            CollectionAssert.AreEqual(resultToMapFrom.Messages, mappedResult.Messages);
            Assert.That(mappedResult.Messages, Is.Not.SameAs(resultToMapFrom.Messages));
        }
    }
}
