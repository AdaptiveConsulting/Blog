using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ExpressionParser.Language.Expressions;
using ExpressionParser.ReferentialData.Product;

namespace ExpressionParser.Pricing
{
    public class PricingRepository : IPricingRepository
    {
        private readonly IProductRepository _productRepository;

        public PricingRepository(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IObservable<decimal> Get(string marketProduct)
        {
            return Observable.Create<decimal>(obs =>
            {
                var disposable = new CompositeDisposable();
                var productStream =
                    _productRepository.GetProduct(marketProduct);

                var subscription = productStream.Subscribe(product =>
                {
                    var (calculatorFunction, rawTerms) = MyGrammarExpressionEvaluator.EvaluateExpression(product.PriceExpression);

                    var visitor = new MyTermVisitor();
                    foreach (var term in rawTerms)
                    {
                        term.Accept(visitor);
                    }

                    var price = calculatorFunction(visitor.GetAllTerms());
                    
                    obs.OnNext(price);
                });
                
                disposable.Add(subscription);

                return disposable;
            });
        }
    }
}