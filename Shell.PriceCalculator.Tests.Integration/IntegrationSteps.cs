using Autofac;
using NUnit.Framework;
using Shell.PriceCalculator.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Shell.PriceCalculator.Tests.Integration
{
   [Binding]
   public class IntegrationSteps
    {
        private readonly TestContext testContext;

        public IntegrationSteps(TestContext testContext)
        {
            this.testContext = testContext;
            this.Setup();
        }

        private void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<Shell.PriceCalculator.Engine.IoC.Registrations>();
            var container = builder.Build();
            var scope = container.BeginLifetimeScope();                           
            this.testContext.PriceCalculatorService = scope.Resolve<IPriceCalculatorService>();
        }

        [Given(@"I have an empty basket to price")]
        public void GivenIHaveAnEmptyBasketToPrice()
        {
            this.testContext.Basket = new Basket { };
        }

        [Given(@"I have a basket to price with the following items")]
        public void GivenIHaveABasketToPriceWithTheFollowingItems(Table table)
        {
            var basketItems = table.CreateSet<BasketItem>();
            var basket = new Basket { Items = basketItems };

            this.testContext.Basket = basket;
        }


        [When(@"I send the basket to the pricing Engine")]
        public void WhenISendTheBasketToThePricingEngine()
        {
            this.testContext.Results = this.testContext.PriceCalculatorService.PriceBasket(this.testContext.Basket);
        }

        [Then(@"the result should contain the following messages")]
        public void ThenTheResultShouldContainTheFollowingMessages(Table table)
        {
            table.CompareToSet(this.testContext.Results.Messages.Select(x => new { Message = x }));
        }

        [Then(@"The subtotal should be equal to (.*)")]
        public void ThenTheSubtotalShouldBeEqualTo(Decimal p0)
        {
            Assert.That(this.testContext.Results.SubTotal, Is.EqualTo(p0));
        }
        [Then(@"the total should be equal to (.*)")]
        public void ThenTheTotalShouldBeEqualTo(Decimal p0)
        {
            Assert.That(this.testContext.Results.Total, Is.EqualTo(p0));
        }

    }
}
