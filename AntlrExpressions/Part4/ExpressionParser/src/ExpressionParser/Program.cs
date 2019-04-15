using System;
using ExpressionParser.MarketPrices.RandomPricesProvider;
using ExpressionParser.MarketPrices.Repository;
using ExpressionParser.Pricing;
using ExpressionParser.ReferentialData.Product;
using ExpressionParser.ReferentialData.UoM;

namespace ExpressionParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var sampleProductRepository = new SampleProductRepository();
            var unitConverter = new UnitConverter();
            var fxRepo = new FxRateRepository(new RandomPriceSource());
                
            var pricingService = new PricingRepository(unitConverter, fxRepo, sampleProductRepository);
            
            Console.ReadKey(true);
        }
    }
}
