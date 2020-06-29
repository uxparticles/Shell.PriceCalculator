using Autofac;
using Shell.PriceCalculator.Contracts;
using Shell.PriceCalculator.Engine.Mappers;
using Shell.PriceCalculator.Engine.Repositories;
using Shell.PriceCalculator.Engine.Repositories._Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.PriceCalculator.Engine.IoC
{

    public class Registrations : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<PricingResultMapper>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ItemPriceRepository>().As<IItemPriceRepository>().SingleInstance();
            builder.RegisterType<BasketPricingOfferRepository>().As<IBasketPricingOfferRepository>().SingleInstance();
            
            builder.RegisterType<BasketPricingEngine>().As<IBasketPricingEngine>().SingleInstance();
            builder.RegisterType<PriceCalculatorService>().As<IPriceCalculatorService>().SingleInstance();
        }
    }
}
