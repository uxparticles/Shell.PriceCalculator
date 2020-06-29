using Shell.PriceCalculator.Contracts;
using Shell.PriceCalculator.Engine.Mappers._interfaces;
using Shell.PriceCalculator.Engine.Repositories._Interfaces;
using Shell.PriceCalculator.Engine.Services.BasktetPricingOffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.PriceCalculator.Engine
{
    internal class PriceCalculatorService : IPriceCalculatorService
    {
        private readonly IBasketPricingEngine pricingEngine;
        private readonly IBasketPricingOfferRepository pricingOffersRepository;
        private readonly IItemPriceRepository itemPriceRepository;
        private readonly IPricingResultsMapper pricingResultsMapper;

        public PriceCalculatorService(
            IBasketPricingEngine pricingEngine, 
            IBasketPricingOfferRepository pricingOffersRepository,
            IItemPriceRepository itemPriceRepository,
            IPricingResultsMapper pricingResultsMapper)
        {
            this.pricingEngine = pricingEngine ?? throw new ArgumentNullException(nameof(pricingEngine));
            this.pricingOffersRepository = pricingOffersRepository ?? throw new ArgumentNullException(nameof(pricingOffersRepository));
            this.itemPriceRepository = itemPriceRepository ?? throw new ArgumentNullException(nameof(itemPriceRepository));
            this.pricingResultsMapper = pricingResultsMapper ?? throw new ArgumentNullException(nameof(pricingResultsMapper));

            this.Setup();
        }

        private void Setup()
        {
            this.pricingOffersRepository.Setup(new IBasketPricingOffer[]
            {
                new DiscountPricingOffer("apple",0.1,true),
                new ConditionalOffer("beans", 2,"bread",.5m)
            });

            this.itemPriceRepository.AddOrUpdateItemPrice(new[]
            {
                new ItemPrice{ Name = "Beans", Price = 0.65m },
                new ItemPrice{ Name = "Bread", Price = 0.30m },
                new ItemPrice{ Name = "Milk", Price = 1.3m },
                new ItemPrice{ Name = "Apple", Price = 1m },
            });
        }

        public PricingResult PriceBasket(Basket basket)
        {
            var offers = this.pricingOffersRepository.GetCurrentOffers();
            var result = this.pricingEngine.PriceBasket(basket, offers);
            return this.pricingResultsMapper.Map(result);
        }        
    }
}
