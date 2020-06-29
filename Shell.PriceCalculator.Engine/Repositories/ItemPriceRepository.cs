using Shell.PriceCalculator.Contracts;
using Shell.PriceCalculator.Engine.Repositories._Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Shell.PriceCalculator.Engine.Repositories
{
    internal class ItemPriceRepository : IItemPriceRepository
    {
        protected Dictionary<string, decimal?> prices = new Dictionary<string, decimal?>(StringComparer.OrdinalIgnoreCase);

        public void AddOrUpdateItemPrice(IEnumerable<ItemPrice> prices)
        {
            if (prices is null)
            {
                throw new ArgumentNullException(nameof(prices));
            }

            foreach (var item in prices)
            {
                this.prices[item.Name] = item.Price;
            }
        }

        public IEnumerable<ItemPrice> GetItemPrices(IEnumerable<string> items)
        {
            foreach (var item in items.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (prices.TryGetValue(item, out var value) && value.HasValue)
                    yield return new ItemPrice
                    {
                        Name = item,
                        Price = value.Value
                    };
            }
        }
    }
}


