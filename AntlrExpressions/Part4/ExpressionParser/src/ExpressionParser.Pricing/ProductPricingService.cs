using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ExpressionParser.Language.Expressions;
using ExpressionParser.MarketPrices.Repository;
using ExpressionParser.ReferentialData.Product;
using ExpressionParser.ReferentialData.UoM;

namespace ExpressionParser.Pricing
{
    public class ProductPricingService : IProductPricingService
    {
        private readonly IUnitConverter _unitConverter;
        private readonly IFxRateRepository _fxRateRepository;
        private readonly IProductRepository _productRepository;

        public ProductPricingService(IUnitConverter unitConverter, IFxRateRepository fxRateRepository,
            IProductRepository productRepository)
        {
            _unitConverter = unitConverter;
            _fxRateRepository = fxRateRepository;
            _productRepository = productRepository;
        }

        public IObservable<decimal> Price(string product)
        {
            return Observable.Create<decimal>(obs =>
            {
                try
                {
                    var disposable = new CompositeDisposable();
                    var productStream =
                        _productRepository.GetProduct(product)
                            .Subscribe(p =>
                            {
                                var (priceExpression, rawTerms) =
                                    MyGrammarExpressionEvaluator.EvaluateExpression(p.PriceExpression);

                                var termVisitor = new MyTermVisitor(_unitConverter, _fxRateRepository);

                                rawTerms.ForEach(t => t.Accept(termVisitor));

                                var subscription = termVisitor.GetAllTerms().Subscribe(terms =>
                                {
                                    var price = priceExpression.Invoke(terms as IReadOnlyList<IGrammarTerm>);

                                    obs.OnNext(price);
                                });

                                disposable.Add(subscription);
                            });

                    disposable.Add(productStream);

                    return disposable;
                }
                catch (Exception e)
                {
                    obs.OnError(e);
                    throw;
                }
            });
        }
    }
}