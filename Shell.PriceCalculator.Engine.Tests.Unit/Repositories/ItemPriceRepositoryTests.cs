using NUnit.Framework;
using Shell.PriceCalculator.Engine.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shell.PriceCalculator.Engine.Tests.Unit.Repositories
{
    [TestFixture]
    public class ItemPriceRepositoryTests
    {
        private class ItemPriceRepositoryEx : ItemPriceRepository
        {
            public Dictionary<string, decimal?> PricesInternal => base.prices;
        }

        [Test]
        public void ThatCannotAddNullItems() 
        {
            var repo = new ItemPriceRepository();
            Assert.Throws<ArgumentNullException>(() => repo.AddOrUpdateItemPrice(null));
        }

        [Test]
        public void ThatCanAddAPrice()
        {
            var repo = new ItemPriceRepositoryEx();

            repo.AddOrUpdateItemPrice(new[] { new ItemPrice { Name = "abc", Price = 100 } });

            Assert.That(repo.PricesInternal.ContainsKey("abc"));
            Assert.That(repo.PricesInternal["abc"], Is.EqualTo(100));

        }

        [Test]
        public void ThatCanUpdateAPrice() {
            var repo = new ItemPriceRepositoryEx();

            repo.AddOrUpdateItemPrice(new[] { new ItemPrice { Name = "abc", Price = 100 } }); 
            repo.AddOrUpdateItemPrice(new[] { new ItemPrice { Name = "abc", Price = 200 } });

            Assert.That(repo.PricesInternal.ContainsKey("abc"));
            Assert.That(repo.PricesInternal["abc"], Is.EqualTo(200));
        }

       

        [Test]
        public void ThatANonExistentPriceYieldsNothing() {

            var repo = new ItemPriceRepositoryEx();

            Assert.That(repo.GetItemPrices(new[] { "abc" }), Is.Empty);
        }

        [Test]
        public void ThatReturnsAPriceCorrectly()
        {
            var repo = new ItemPriceRepositoryEx();
            repo.PricesInternal.Add("abc", 1000);

            var result = repo.GetItemPrices(new[] { "abc" }).First();
            Assert.That(result.Price, Is.EqualTo(1000));
            Assert.That(result.Name, Is.EqualTo("abc"));
        }

        [Test]
        public void ThatReturnsOnePriceForTwoEqualRequests()
        {
            var repo = new ItemPriceRepositoryEx();
            repo.PricesInternal.Add("abc", 1000);

            var result = repo.GetItemPrices(new[] { "abc" , "abc"}).Single();
            Assert.That(result.Price, Is.EqualTo(1000));
            Assert.That(result.Name, Is.EqualTo("abc"));
        }

        [Test]
        public void ThatIsCaseInsensitive()
        {
            var repo = new ItemPriceRepositoryEx();
            repo.PricesInternal.Add("abc", 1000);

            var result = repo.GetItemPrices(new[] { "ABC" }).First();
            Assert.That(result.Price, Is.EqualTo(1000));
            Assert.That(result.Name, Is.EqualTo("ABC"));
        }
    }
}
