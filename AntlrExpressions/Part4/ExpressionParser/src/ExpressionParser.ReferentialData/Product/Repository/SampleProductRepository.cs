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
        private readonly IDictionary<string, Product> _products;

        public SampleProductRepository()
        {
            var allLines = File.ReadAllText("products.json ");
            var products = JsonConvert.DeserializeObject<List<Product>>(allLines).AsEnumerable();
            _products = products.ToDictionary(x => x.Name);
        }
        
        public IObservable<IEnumerable<Product>> GetAll()
        {
            return Observable.Create<IEnumerable<Product>>(obs =>
            {
                obs.OnNext(_products.Values.AsEnumerable());
                return Disposable.Empty;
            });
        }
    }
}