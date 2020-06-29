namespace Shell.PriceCalculator.Engine
{
    internal class BasketItemPrice
    {
        public int Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public string Name { get; set; }
        
        public decimal? Discount { get; internal set; }
    }
}

