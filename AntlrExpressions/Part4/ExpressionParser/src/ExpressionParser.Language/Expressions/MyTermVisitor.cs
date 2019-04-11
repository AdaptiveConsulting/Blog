using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using ExpressionParser.MarketPrices.Repository;
using ExpressionParser.ReferentialData;

namespace ExpressionParser.Language.Expressions
{
    public class MyTermVisitor : ITermVisitor
    {
        private List<IGrammarTerm> _terms = new List<IGrammarTerm>();
        private readonly IUnitConverter _unitConverter = new UnitConverter();
        private readonly IFxRateRepository _fxRateRepository = new FxRateRepository();
        
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