using System;
using System.Reactive.Linq;
using ExpressionParser.ReferentialData.Product;

namespace ExpressionParser.Pricing
{
    public class PricingRepository : IPricingRepository
    {
        private readonly IProductRepository _productRepository =
            new SampleProductRepository();
        
        
        public IObservable<decimal> Get(string marketProduct)
        {
            return Observable.Empty<decimal>();
        }
    }
}