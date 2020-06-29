using System.Collections.Generic;

namespace Shell.PriceCalculator.Engine.Repositories._Interfaces
{
    internal interface IItemPriceRepository
    {
        /// <summary>
        /// Return, when available, prices for all the products.
        /// </summary>
        /// <param name="pricesToAsk">A collection of items to return the prices for. If multiple values are passed, only one price will be returned for that value</param>
        /// <returns>A collection of <see cref="ItemPrice"/> object that represent the name and the price of the product</returns>
        IEnumerable<ItemPrice> GetItemPrices(IEnumerable<string> items);

        /// <summary>
        /// Adds, update or removes a price from storage.
        /// </summary>
        /// <param name="prices">A non null colelction of items that need to be added to the repository. If the <see cref="ItemPrice"/> has a null price value, the price will be removed.</param>
        void AddOrUpdateItemPrice(IEnumerable<ItemPrice> prices);
    }
}

