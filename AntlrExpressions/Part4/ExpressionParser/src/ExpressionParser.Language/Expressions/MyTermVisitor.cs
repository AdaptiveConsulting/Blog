using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ExpressionParser.MarketPrices.RandomPricesProvider;
using ExpressionParser.MarketPrices.Repository;
using ExpressionParser.ReferentialData;
using ExpressionParser.ReferentialData.UoM;

namespace ExpressionParser.Language.Expressions
{
    public class MyTermVisitor : ITermVisitor
    {
        private List<IGrammarTerm> _terms = new List<IGrammarTerm>();
        private readonly IUnitConverter _unitConverter = new UnitConverter();
        
        // TODO use a container for this type of thing.
        private readonly IFxRateRepository _fxRateRepository = new FxRateRepository(new RandomPriceSource());
        
        public IReadOnlyList<IGrammarTerm> GetAllTerms() => _terms.AsReadOnly();
        
        public void Visit(UomConvertTerm uomConversionTerm)
        {
            var conversionFactor =
                _unitConverter.GetConversionFactor(uomConversionTerm.FromUom, uomConversionTerm.ToUom);
            uomConversionTerm.SetValue(conversionFactor);
            _terms.Add(uomConversionTerm);
        }

        public void Visit(FxRateTerm fxRateTerm)
        {
            var priceStream = _fxRateRepository.GetPricesFor(fxRateTerm.Identifier).Take(1);
            var latestPriceSubscription =
                priceStream
                    .Take(1)
                    .Subscribe(rate => { fxRateTerm.SetValue(rate.Rate); });

            latestPriceSubscription.Dispose();
            
            _terms.Add(fxRateTerm);
        }
    }
}