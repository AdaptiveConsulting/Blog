using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Newtonsoft.Json;

namespace ExpressionParser.ReferentialData.Product
{
    public class SampleProductRepository : IProductRepository
    {
        private const string ProductsJson = "products.json";
        private readonly IDictionary<string, Product> _productsCache;

        public SampleProductRepository()
        {
            var allLines = File.ReadAllText(ProductsJson);
            var products = JsonConvert.DeserializeObject<List<Product>>(allLines).AsEnumerable();
            _productsCache = products.ToDictionary(x => x.Name);
        }
        
        public IObservable<IEnumerable<Product>> GetAll()
        {
            return Observable.Create<IEnumerable<Product>>(obs =>
            {
                obs.OnNext(_productsCache.Values.AsEnumerable());
                return Disposable.Empty;
            });
        }

        public IObservable<Product> GetProduct(string productName)
        {
            return Observable.Create<Product>(obs =>
            {
                if (_productsCache.ContainsKey(productName))
                {
                    obs.OnNext(_productsCache[productName]);
                }
                
                // do nothing for now. 
                return Disposable.Empty;
            });
        }
    }
}