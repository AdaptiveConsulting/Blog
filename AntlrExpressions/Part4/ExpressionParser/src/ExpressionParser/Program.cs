using System;
using ExpressionParser.Pricing;
using ExpressionParser.ReferentialData.Product;

namespace ExpressionParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var sampleProductRepository = new SampleProductRepository();
            var pricingService = new PricingRepository(sampleProductRepository);
            
            
            Console.ReadKey(true);
        }
    }
}
