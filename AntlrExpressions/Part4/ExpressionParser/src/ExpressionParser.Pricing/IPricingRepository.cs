using System;

namespace ExpressionParser.Pricing
{
    public interface IPricingRepository
    {
        IObservable<decimal> Get(string marketProduct);
    }
}