using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ExpressionParser.MarketPrices.RandomPricesProvider;
using ExpressionParser.MarketPrices.Repository;
using ExpressionParser.Pricing;
using ExpressionParser.ReferentialData.Product;
using ExpressionParser.ReferentialData.UoM;

namespace ExpressionParser
{
    internal class Program
    {
        private static async Task Main()
        {
            var ns = typeof(Program).Namespace;

            var sampleProductsDb =
                await Assembly.GetExecutingAssembly().GetResourceContentsAsync($"{ns}.products.json");
             
            var sampleProductRepository = new SampleProductRepository(sampleProductsDb);
            var unitConverter = new UnitConverter();
            var fxRepo = new FxRateRepository(new RandomPriceSource());

            var pricingService = new ProductPricingService(unitConverter, fxRepo, sampleProductRepository);

            // wait for OnComplete
            var products = (await sampleProductRepository.GetAll()).ToList();

            var randomProductIndex = new Random();

            var randomProduct = (await sampleProductRepository.GetAll().Take(1)).ToList()[randomProductIndex.Next(products.Count)];
            Console.WriteLine($"Pricing product {randomProduct}");

            var priceStream = pricingService.Price(randomProduct.Name);

            var subscription = priceStream.Subscribe(price =>
            {
                Console.WriteLine($"Got product {randomProduct.Name}'s computed price {price}");
            });

            Console.ReadKey(true);

            subscription.Dispose();

            Console.WriteLine("Exited. Press any key to quit");
            Console.ReadKey(false);
        }
    }
}
