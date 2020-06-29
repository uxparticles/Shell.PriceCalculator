namespace Shell.PriceCalculator.Contracts
{
    public interface IPriceCalculatorService
    {
        public PricingResult PriceBasket(Basket basket);
    }
}
