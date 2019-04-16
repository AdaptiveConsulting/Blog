using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExpressionParser.ReferentialData.Product
{
    public class SampleProductRepository : IProductRepository
    {
        private readonly IDictionary<string, Product> _productsCache;

        public SampleProductRepository(string productsJson)
        {
            var json = JObject.Parse(productsJson); //validate
            var products = JsonConvert.DeserializeObject<Product[]>(json["Products"].ToString());
            _productsCache = products.ToDictionary(x => x.Name);
        }
        
        public IObservable<IEnumerable<Product>> GetAll()
        {
            return Observable.Create<IEnumerable<Product>>(obs =>
            {
                obs.OnNext(_productsCache.Values.AsEnumerable());
                obs.OnCompleted();
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