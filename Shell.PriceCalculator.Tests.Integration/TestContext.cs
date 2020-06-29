using Shell.PriceCalculator.Contracts;

namespace Shell.PriceCalculator.Tests.Integration
{
    public class TestContext
    {
        public Basket Basket { get; set; }

        public IPriceCalculatorService PriceCalculatorService { get; set; }
        public PricingResult Results { get; internal set; }
    }
}
