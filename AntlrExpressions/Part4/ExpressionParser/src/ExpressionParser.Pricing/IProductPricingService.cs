using System;

namespace ExpressionParser.Pricing
{
    public interface IProductPricingService
    {
        IObservable<decimal> Price(string product);
    }
}