using Autofac;
using Shell.PriceCalculator.Contracts;
using System;
using System.Linq;

namespace Shell.PriceCalculator
{
    class Program
    {
        private static IContainer Container;

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<Shell.PriceCalculator.Engine.IoC.Registrations>();
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                var calculator = scope.Resolve<IPriceCalculatorService>();
                var basket = new Basket
                {
                    Items = args.Where(x => x != null).Select(x => new BasketItem { Name = x.Trim() }).ToList()
                };

                var result = calculator.PriceBasket(basket);
                Console.WriteLine($"Subtotal: {result.SubTotal}");
                
                foreach (var message in result.Messages)
                {
                    Console.WriteLine(message);
                }

                Console.WriteLine($"Total   : {result.Total}");
                Console.WriteLine();

                Console.WriteLine("Press a key to exit");
                Console.ReadKey();
            }
        }
    }
}
